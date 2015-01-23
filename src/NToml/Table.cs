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
        private readonly string[] parentTitle;
        private readonly string actualTitle;
        private readonly List<KeyValuePair> values;
        private readonly bool isArrayTable;

        public string[] ParentTitle { get { return this.parentTitle; } }
        public string ActualTitle { get { return this.actualTitle; } }
        public string[] Title { get { return this.title; } }
        public bool IsArrayTable { get { return this.isArrayTable; } }


        public IEnumerable<KeyValuePair> KeyValuePairs { get { return this.values; } }

        public Table(string[] title, IEnumerable<KeyValuePair> values, bool isArrayTable)
        {
            this.title = title;
            this.parentTitle = this.title.Length == 0 ? null : this.title.Take(this.title.Length - 1).ToArray();
            this.actualTitle = this.title.Length == 0 ? null : this.title[this.title.Length - 1];
            this.values = new List<KeyValuePair>(values);
            this.isArrayTable = isArrayTable;
        }
    }
}
