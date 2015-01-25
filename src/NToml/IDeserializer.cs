using NToml.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public interface IDeserializer
    {
        void Deserialize(IValue rootTable);
    }
}
