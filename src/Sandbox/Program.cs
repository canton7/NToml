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
            string str = @"SomeKey = ""Some Value\t.\u00E9\"" \\ Yay!"" # This is a comment";
            var result = Parser.KeyValuePair(new Input(str));
        }
    }
}
