using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class ArrayValue<T> : IValue where T : IValue
    {
        private readonly T[] values;
        public TomlValueType Type
        {
            get { return TomlValueType.Array; }
        }

        public ArrayValue(T[] values)
        {
            this.values = values;
        }

        public void Visit(IValueVisitor visitor)
        {
            visitor.Deserialize(this.values.Cast<IValue>());
        }

        public override string ToString()
        {
            return String.Format("[{0}]", String.Join(", ", values));
        }
    }
}
