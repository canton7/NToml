using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class DateTimeValue : ITableValue
    {
        private readonly DateTime value;

        public DateTimeValue(DateTime value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
