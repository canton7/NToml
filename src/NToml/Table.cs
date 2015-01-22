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
        private readonly KeyValuePair[] values;

        public Table(string[] title, KeyValuePair[] values)
        {
            this.title = title;
            this.values = values;
        }
    }
}
