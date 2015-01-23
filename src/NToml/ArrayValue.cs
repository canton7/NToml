using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class ArrayValue<T> : ITableValue where T : ITableValue
    {
        private readonly T[] values;

        public ArrayValue(T[] values)
        {
            this.values = values;
        }

        public override string ToString()
        {
            return String.Format("[{0}]", String.Join(", ", values));
        }
    }
}
