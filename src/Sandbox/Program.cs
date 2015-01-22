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
            string str = @"[hello]
this = ""is""
a = true
";
            var result = TomlParser.ParseInput(str);
        }
    }
}
