using NToml;
using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = @"
[hello]
foo = 4

[hello.""god""]
bar = 4";
            var result = TomlParser.ParseInput(str);
        }
    }
}
