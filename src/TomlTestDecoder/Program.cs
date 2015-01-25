using NToml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomlTestDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            //var toml = Console.In.ReadToEnd();

            var toml = @"
[[albums.songs]]
name = ""Glory Days""
";

            try
            {
                var deserializer = new TestDeserializer();
                TomlParser.ParseInput(toml, deserializer);
                Console.Write(deserializer.Value);
                Environment.Exit(0);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(1);
            }
        }
    }
}