using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class BooleanValue : ITableValue
    {
        private readonly bool value;

        public BooleanValue(bool value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value ? "true" : "false";
        }
    }
}
