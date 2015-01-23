using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public class TomlParser
    {
        public void ParseInput(string input, IDeserializer deserializer)
        {
            var tables = TomlGrammar.Document.Parse(input).ToArray();
            var tableValueMap = tables.ToDictionary(x => x, x => new TableValue(x.Title, x.KeyValuePairs));
            // The dictionary holds all non-array tables, and the most recently processed instance of each array table (per the spec)
            var tableLookup = new Dictionary<string[], TableValue>(new TableKeyComparer());

            // This could probably be refactored out into its own method... But I want to keep the parser stateless for now,
            // and I could create an inner class... but the compiler will do that for me this way
            Action<Table> ensureTableDeclaredAndWiredInLookup = null;
            ensureTableDeclaredAndWiredInLookup = table =>
            {
                TableValue parent;
                // The case for tables that imply the existence of their parent, eifi [foo.bar] where [foo] isn't defined
                if (!tableLookup.TryGetValue(table.ParentTitle, out parent))
                {
                    var parentTable = new Table(table.ParentTitle, Enumerable.Empty<KeyValuePair>(), false);
                    parent = new TableValue(parentTable.Title);
                    tableValueMap.Add(parentTable, parent);
                    //tableLookup.Add(parentTable.Title, parent);
                    ensureTableDeclaredAndWiredInLookup(parentTable);
                }

                var tableValue = tableValueMap[table];
                if (table.IsArrayTable)
                    parent.AddChildArrayTable(table.ActualTitle, tableValue);
                else
                    parent.AddKeyValuePair(table.ActualTitle, tableValue);
            };

            // Assign child tables to their parents
            foreach (var table in tables)
            {
                tableLookup[table.Title] = tableValueMap[table];

                if (table.ParentTitle != null)
                    ensureTableDeclaredAndWiredInLookup(table);
            }

            // Tables doesn't contain the newly-created intermediate tables
            foreach (var table in tableValueMap.Values)
            {
                table.FlattenChildArrayTables();
            }

            deserializer.Deserialize(tableLookup[new string[0]]);
        }
    }
}
