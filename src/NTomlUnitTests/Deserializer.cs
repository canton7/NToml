using Newtonsoft.Json;
using NToml;
using NToml.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTomlUnitTests
{
    public class TestDeserializer : IDeserializer
    {
        private object value;

        void IDeserializer.Deserialize(IValue rootTable)
        {
            object retVal = null;
            var visitor = new ObjectVisitor(value =>
            {
                retVal = value;
            });
            rootTable.Visit(visitor);

            this.value = retVal;
        }

        public static object Deserialize(string input)
        {
            var deserializer = new TestDeserializer();
            TomlParser.ParseInput(input, deserializer);
            return deserializer.value;
        }
    }
}
