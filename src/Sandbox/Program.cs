﻿using NToml;
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
[the]
test_string = ""You'll hate me after this - #"" # "" Annoying, isn't it?
[the.hard]
test_array = [ ""] "", "" # ""] # ] There you go, parse this!
test_array2 = [ ""Test #11 ]proved that"", ""Experiment #9 was a success"" ]
# You didn't think it'd as easy as chucking out the last #, did you?
another_test_string = "" Same thing, but with a string #""
harder_test_string = "" And when \""'s are in the string, along with # \"""" # ""and comments are there too""
# Things will get harder
[the.hard.""bit#""]
""what?"" = ""You don't think some user won't do that?""
multi_line_array = [
""]"",
# ] Oh yes I did
]
";
            var result = TomlParser.ParseInput(str);
        }
    }
}
