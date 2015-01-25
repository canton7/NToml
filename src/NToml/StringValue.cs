using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class StringValue : IValue
    {
        private readonly string value;
        public TomlValueType Type
        {
            get { return TomlValueType.String; }
        }

        public StringValue(string value)
        {
            this.value = value;
        }

        public void Visit(IValueVisitor visitor)
        {
            visitor.Deserialize(this.value);
        }

        public override string ToString()
        {
            return String.Format("\"{0}\"", this.value);
        }
    }
}
