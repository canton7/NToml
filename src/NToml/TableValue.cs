using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class TableValue : ITableValue
    {
        private readonly string[] fullName;
        private readonly string name;
        private readonly Dictionary<string, ITableValue> keyValuePairs = new Dictionary<string, ITableValue>();

        // Temporary stoage for child tables - this is flattened into keyValuePairs on Finalize()
        private readonly List<Tuple<string, TableValue>> childArrayTables = new List<Tuple<string, TableValue>>();

        public TableValue(string[] fullName, IEnumerable<KeyValuePair> initialKeyValuePairs = null)
        {
            this.fullName = fullName;
            this.name = this.fullName.Length == 0 ? null : this.fullName[this.fullName.Length - 1];
            if (initialKeyValuePairs != null)
            {
                foreach (var kvp in initialKeyValuePairs)
                {
                    this.AddKeyValuePair(kvp);
                }
            }
        }

        public void AddKeyValuePair(string key, ITableValue value)
        {
            if (this.keyValuePairs.ContainsKey(key))
                throw new DuplicateTableKeyException(String.Join(".", this.fullName.Select(x => String.Format("\"{0}\"", x))), key);
            this.keyValuePairs.Add(key, value);
        }

        public void AddKeyValuePair(KeyValuePair keyValuePair)
        {
            this.AddKeyValuePair(keyValuePair.Key, keyValuePair.Value);
        }

        public void AddChildArrayTable(string key, TableValue table)
        {
            this.childArrayTables.Add(Tuple.Create(key, table));
        }

        public void FlattenChildArrayTables()
        {
            foreach (var group in this.childArrayTables.GroupBy(x => x.Item1))
            {
                var array = new ArrayValue<TableValue>(group.Select(x => x.Item2).ToArray());
                this.AddKeyValuePair(group.Key, array);
            }

            this.childArrayTables.Clear();
        }

        public override string ToString()
        {
            return String.Format("|{0}|", String.Join(", ", this.keyValuePairs.Select(x => String.Format("{0}: {1}", x.Key, x.Value))));
        }
    }
}
