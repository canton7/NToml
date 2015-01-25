using NToml.Dynamic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public static class TomlConvert
    {
        public static dynamic Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                return Deserialize(sr.ReadToEnd());
            }
        }

        public static dynamic Deserialize(string input)
        {
            var deserializer = new DynamicDeserializer();
            TomlParser.ParseInput(input, deserializer);
            return deserializer.Value;
        }
    }
}
