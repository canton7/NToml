using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class Table
    {
        private readonly string[] title;
        private readonly List<KeyValuePair> values;
        private readonly bool isArrayTable;
        private readonly Dictionary<string[], Table> childTables = new Dictionary<string[], Table>();

        public string[] Title { get { return this.title; } }

        public Table(string[] title, IEnumerable<KeyValuePair> values, bool isArrayTable)
        {
            this.title = title;
            this.values = new List<KeyValuePair>(values);
            this.isArrayTable = isArrayTable;
        }

        public void AddChildTable(Table childTable)
        {
            if (!childTable.isArrayTable)
                this.childTables.Add(childTable.Title, childTable);
            // ELSE TODO
        }
    }
}
