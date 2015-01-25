using NToml.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml.Values
{
    public interface IValueVisitor
    {
        void Deserialize(bool value);
        void Deserialize(DateTime value);
        void Deserialize(double value);
        void Deserialize(long value);
        void Deserialize(string value);
        void Deserialize(IEnumerable<IValue> values);
        void Deserialize(IDictionary<string, IValue> values);
    }
}
