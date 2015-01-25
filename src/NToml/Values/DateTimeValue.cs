using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml.Values
{
    internal class DateTimeValue : IValue
    {
        private readonly DateTime value;
        public TomlValueType Type
        {
            get { return TomlValueType.DateTime; }
        }

        public DateTimeValue(DateTime value)
        {
            this.value = value;
        }

        public void Visit(IValueVisitor visitor)
        {
            visitor.Deserialize(this.value);
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
