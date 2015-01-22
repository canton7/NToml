using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class IntegerValue : ITableValue
    {
        private readonly long value;

        public IntegerValue(long value)
        {
            this.value = value;
        }
    }
}
