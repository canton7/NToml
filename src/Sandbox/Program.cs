using NToml;
using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = @"


foo = [2, 2]


";
            var result = TomlConvert.Deserialize(str);

            Console.WriteLine(result);
        }
    }
}
