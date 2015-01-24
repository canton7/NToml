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
[[albums]]
name = ""Born to Run""
[[albums.songs]]
name = ""Jungleland""
[[albums.songs]]
name = ""Meeting Across the River""
[[albums]]
name = ""Born in the USA""
[[albums.songs]]
name = ""Glory Days""
[[albums.songs]]
name = ""Dancing in the Dark""
";

            var parser = new TomlParser();

            try
            {
                var deserializer = new TestDeserializer();
                parser.ParseInput(toml, deserializer);
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