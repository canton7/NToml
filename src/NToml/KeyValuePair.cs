using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class KeyValuePair
    {
        public string Key { get; private set; }
        public ITableValue Value { get; private set; }

        public KeyValuePair(string key, ITableValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
