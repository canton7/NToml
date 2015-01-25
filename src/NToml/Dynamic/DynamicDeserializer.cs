using NToml.Values;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml.Dynamic
{
    internal class DynamicDeserializer : IDeserializer
    {
        public dynamic Value { get; private set; }

        public void Deserialize(IValue rootTable)
        {
            var visitor = new DynamicVisitor(value =>
            {
                this.Value = value;
            });
            rootTable.Visit(visitor);
        }
    }
}
