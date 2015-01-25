using NToml.Values;
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
        public IValue Value { get; private set; }

        public KeyValuePair(string key, IValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}
