using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class FloatValue : IValue
    {
        private readonly double value;

        public FloatValue(double value)
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
