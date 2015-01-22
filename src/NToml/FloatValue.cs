using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class FloatValue : ITableValue
    {
        private readonly double value;

        public FloatValue(double value)
        {
            this.value = value;
        }
    }
}
