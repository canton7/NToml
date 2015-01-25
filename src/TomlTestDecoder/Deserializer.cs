using Newtonsoft.Json;
using NToml;
using NToml.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomlTestDecoder
{
    public class TestDeserializer : IDeserializer
    {
        public string Value { get; private set; }

        public void Deserialize(IValue rootTable)
        {
            object retVal = null;
            var visitor = new ObjectVisitor(value =>
            {
                retVal = value;
            });
            rootTable.Visit(visitor);

            this.Value = JsonConvert.SerializeObject(retVal, Formatting.Indented);
        }
    }
}
