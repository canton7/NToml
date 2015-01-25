using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class IntegerValue : IValue
    {
        private readonly long value;
        public TomlValueType Type
        {
            get { return TomlValueType.Integer; }
        }

        public IntegerValue(long value)
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
