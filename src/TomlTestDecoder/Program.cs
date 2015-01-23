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
best-day-ever = 1987-07-05T17:45:00Z

[numtheory]
boring = false
perfection = [6, 28, 496]
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