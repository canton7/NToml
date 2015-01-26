using NToml.Grammars;
using NToml.Values;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public static class TomlParser
    {
        private struct TableAndValue
        {
            public Table Table { get; private set; }
            public TableValue TableValue { get; private set; }

            public TableAndValue(Table table, TableValue tableValue)
                : this()
            {
                this.Table = table;
                this.TableValue = tableValue;
            }
        }

        public static void ParseInput(string input, IDeserializer deserializer)
        {
            var result = TomlGrammar.Document.TryParse(input);
            if (!result.WasSuccessful)
                throw new GrammarException(result.Message);

            var tables = result.Value;
            var tableValueMap = tables.ToDictionary(x => x, x => new TableValue(x.Title, x.KeyValuePairs));
            // The dictionary holds all non-array tables, and most recently processed array table element
            var tableLookup = new Dictionary<string[], TableAndValue>(new TableKeyComparer());
            foreach (var item in tableValueMap.Where(x => !x.Key.IsArrayTable))
            {
                if (tableLookup.ContainsKey(item.Key.Title))
                    throw new DuplicateTableKeyException(item.Key.ParentTitle, item.Key.ActualTitle);
                tableLookup.Add(item.Key.Title, new TableAndValue(item.Key, item.Value));
            }
            // A list of all array tables
            var arrayTableTitles = new HashSet<string[]>(tables.Where(x => x.IsArrayTable).Select(x => x.Title), new TableKeyComparer());

            // This could probably be refactored out into its own method... But I want to keep the parser stateless for now,
            // and I could create an inner class... but the compiler will do that for me this way
            Action<Table> ensureTableDeclaredAndWiredInLookup = null;
            ensureTableDeclaredAndWiredInLookup = table =>
            {
                TableAndValue parent;
                // The case for tables that imply the existence of their parent, eifi [foo.bar] where [foo] isn't defined
                if (!tableLookup.TryGetValue(table.ParentTitle, out parent))
                {
                    if (arrayTableTitles.Contains(table.ParentTitle))
                        throw new ParseException("Whoops"); // TODO better message

                    var parentTable = new Table(table.ParentTitle, Enumerable.Empty<KeyValuePair>(), false);
                    parent = new TableAndValue(parentTable, new TableValue(parentTable.Title));
                    tableValueMap.Add(parentTable, parent.TableValue);
                    ensureTableDeclaredAndWiredInLookup(parentTable);
                }

                var tableValue = tableValueMap[table];
                if (table.IsArrayTable)
                    parent.TableValue.AddChildArrayTable(table.ActualTitle, tableValue);
                else
                    parent.TableValue.AddKeyValuePair(table.ActualTitle, tableValue);
            };

            Table mostRecentlyDefinedTableElement = null;

            // Assign child tables to their parents
            foreach (var table in tables)
            {
                if (table.IsArrayTable)
                    tableLookup[table.Title] = new TableAndValue(table, tableValueMap[table]);

                if (table.ParentTitle != null)
                    ensureTableDeclaredAndWiredInLookup(table);

                mostRecentlyDefinedTableElement = table;
            }

            // Tables doesn't contain the newly-created intermediate tables
            foreach (var table in tableValueMap.Values)
            {
                table.FlattenChildArrayTables();
            }

            deserializer.Deserialize(tableLookup[new string[0]].TableValue);
        }
    }
}
