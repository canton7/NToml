using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class BooleanValue : IValue
    {
        private readonly bool value;
        public TomlValueType Type
        {
            get { return TomlValueType.Boolean; }
        }

        public BooleanValue(bool value)
        {
            this.value = value;
        }

        public void Visit(IValueVisitor visitor)
        {
            visitor.Deserialize(this.value);
        }

        public override string ToString()
        {
            return this.value ? "true" : "false";
        }
    }
}
