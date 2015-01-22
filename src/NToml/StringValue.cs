using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class StringValue : ITableValue
    {
        private readonly string value;

        public StringValue(string value)
        {
            this.value = value;
        }
    }
}
