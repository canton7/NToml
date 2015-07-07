
using Xunit;
using Newtonsoft.Json;
using System.Collections.Generic;
using NTomlUnitTests;

namespace NTomlUnitTests
{
	public class ValidParsingTests
	{

		[Fact]
		public void ArrayEmpty()
		{
			var inputString = @"thevoid = [[[[[]]]]]";
			var expectedOutputString = @"{
    ""thevoid"": { ""type"": ""array"", ""value"": [
        {""type"": ""array"", ""value"": [
            {""type"": ""array"", ""value"": [
                {""type"": ""array"", ""value"": [
                    {""type"": ""array"", ""value"": []}
                ]}
            ]}
        ]}
    ]}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void ArrayNospaces()
		{
			var inputString = @"ints = [1,2,3]";
			var expectedOutputString = @"{
    ""ints"": {
        ""type"": ""array"",
        ""value"": [
            {""type"": ""integer"", ""value"": ""1""},
            {""type"": ""integer"", ""value"": ""2""},
            {""type"": ""integer"", ""value"": ""3""}
        ]
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void ArraysHetergeneous()
		{
			var inputString = @"mixed = [[1, 2], [""a"", ""b""], [1.1, 2.1]]";
			var expectedOutputString = @"{
    ""mixed"": {
        ""type"": ""array"",
        ""value"": [
            {""type"": ""array"", ""value"": [
                {""type"": ""integer"", ""value"": ""1""},
                {""type"": ""integer"", ""value"": ""2""}
            ]},
            {""type"": ""array"", ""value"": [
                {""type"": ""string"", ""value"": ""a""},
                {""type"": ""string"", ""value"": ""b""}
            ]},
            {""type"": ""array"", ""value"": [
                {""type"": ""float"", ""value"": ""1.1""},
                {""type"": ""float"", ""value"": ""2.1""}
            ]}
        ]
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void ArraysNested()
		{
			var inputString = @"nest = [[""a""], [""b""]]";
			var expectedOutputString = @"{
    ""nest"": {
        ""type"": ""array"",
        ""value"": [
            {""type"": ""array"", ""value"": [
                {""type"": ""string"", ""value"": ""a""}
            ]},
            {""type"": ""array"", ""value"": [
                {""type"": ""string"", ""value"": ""b""}
            ]}
        ]
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void Arrays()
		{
			var inputString = @"ints = [1, 2, 3]
floats = [1.1, 2.1, 3.1]
strings = [""a"", ""b"", ""c""]
dates = [
  1987-07-05T17:45:00Z,
  1979-05-27T07:32:00Z,
  2006-06-01T11:00:00Z,
]";
			var expectedOutputString = @"{
    ""ints"": {
        ""type"": ""array"",
        ""value"": [
            {""type"": ""integer"", ""value"": ""1""},
            {""type"": ""integer"", ""value"": ""2""},
            {""type"": ""integer"", ""value"": ""3""}
        ]
    },
    ""floats"": {
        ""type"": ""array"",
        ""value"": [
            {""type"": ""float"", ""value"": ""1.1""},
            {""type"": ""float"", ""value"": ""2.1""},
            {""type"": ""float"", ""value"": ""3.1""}
        ]
    },
    ""strings"": {
        ""type"": ""array"",
        ""value"": [
            {""type"": ""string"", ""value"": ""a""},
            {""type"": ""string"", ""value"": ""b""},
            {""type"": ""string"", ""value"": ""c""}
        ]
    },
    ""dates"": {
        ""type"": ""array"",
        ""value"": [
            {""type"": ""datetime"", ""value"": ""1987-07-05T17:45:00Z""},
            {""type"": ""datetime"", ""value"": ""1979-05-27T07:32:00Z""},
            {""type"": ""datetime"", ""value"": ""2006-06-01T11:00:00Z""}
        ]
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void Bool()
		{
			var inputString = @"t = true
f = false";
			var expectedOutputString = @"{
    ""f"": {""type"": ""bool"", ""value"": ""false""},
    ""t"": {""type"": ""bool"", ""value"": ""true""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void CommentsEverywhere()
		{
			var inputString = @"# Top comment.
  # Top comment.
# Top comment.

# [no-extraneous-groups-please]

[group] # Comment
answer = 42 # Comment
# no-extraneous-keys-please = 999
# Inbetween comment.
more = [ # Comment
  # What about multiple # comments?
  # Can you handle it?
  #
          # Evil.
# Evil.
  42, 42, # Comments within arrays are fun.
  # What about multiple # comments?
  # Can you handle it?
  #
          # Evil.
# Evil.
# ] Did I fool you?
] # Hopefully not.";
			var expectedOutputString = @"{
    ""group"": {
        ""answer"": {""type"": ""integer"", ""value"": ""42""},
        ""more"": {
            ""type"": ""array"",
            ""value"": [
                {""type"": ""integer"", ""value"": ""42""},
                {""type"": ""integer"", ""value"": ""42""}
            ]
        }
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void Datetime()
		{
			var inputString = @"bestdayever = 1987-07-05T17:45:00Z";
			var expectedOutputString = @"{
    ""bestdayever"": {""type"": ""datetime"", ""value"": ""1987-07-05T17:45:00Z""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void Empty()
		{
			var inputString = @"";
			var expectedOutputString = @"{}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void Example()
		{
			var inputString = @"best-day-ever = 1987-07-05T17:45:00Z

[numtheory]
boring = false
perfection = [6, 28, 496]";
			var expectedOutputString = @"{
  ""best-day-ever"": {""type"": ""datetime"", ""value"": ""1987-07-05T17:45:00Z""},
  ""numtheory"": {
    ""boring"": {""type"": ""bool"", ""value"": ""false""},
    ""perfection"": {
      ""type"": ""array"",
      ""value"": [
        {""type"": ""integer"", ""value"": ""6""},
        {""type"": ""integer"", ""value"": ""28""},
        {""type"": ""integer"", ""value"": ""496""}
      ]
    }
  }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void Float()
		{
			var inputString = @"pi = 3.14
negpi = -3.14";
			var expectedOutputString = @"{
    ""pi"": {""type"": ""float"", ""value"": ""3.14""},
    ""negpi"": {""type"": ""float"", ""value"": ""-3.14""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void ImplicitAndExplicitAfter()
		{
			var inputString = @"[a.b.c]
answer = 42

[a]
better = 43";
			var expectedOutputString = @"{
    ""a"": {
        ""better"": {""type"": ""integer"", ""value"": ""43""},
        ""b"": {
            ""c"": {
                ""answer"": {""type"": ""integer"", ""value"": ""42""}
            }
        }
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void ImplicitAndExplicitBefore()
		{
			var inputString = @"[a]
better = 43

[a.b.c]
answer = 42";
			var expectedOutputString = @"{
    ""a"": {
        ""better"": {""type"": ""integer"", ""value"": ""43""},
        ""b"": {
            ""c"": {
                ""answer"": {""type"": ""integer"", ""value"": ""42""}
            }
        }
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void ImplicitGroups()
		{
			var inputString = @"[a.b.c]
answer = 42";
			var expectedOutputString = @"{
    ""a"": {
        ""b"": {
            ""c"": {
                ""answer"": {""type"": ""integer"", ""value"": ""42""}
            }
        }
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void Integer()
		{
			var inputString = @"answer = 42
neganswer = -42";
			var expectedOutputString = @"{
    ""answer"": {""type"": ""integer"", ""value"": ""42""},
    ""neganswer"": {""type"": ""integer"", ""value"": ""-42""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void KeyEqualsNospace()
		{
			var inputString = @"answer=42";
			var expectedOutputString = @"{
    ""answer"": {""type"": ""integer"", ""value"": ""42""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void KeySpace()
		{
			var inputString = @"""a b"" = 1";
			var expectedOutputString = @"{
    ""a b"": {""type"": ""integer"", ""value"": ""1""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void KeySpecialChars()
		{
			var inputString = @"""~!@$^&*()_+-`1234567890[]|/?><.,;:'"" = 1";
			var expectedOutputString = @"{
    ""~!@$^&*()_+-`1234567890[]|/?><.,;:'"": {
        ""type"": ""integer"", ""value"": ""1""
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void LongFloat()
		{
			var inputString = @"longpi = 3.141592653589793
neglongpi = -3.141592653589793";
			var expectedOutputString = @"{
    ""longpi"": {""type"": ""float"", ""value"": ""3.141592653589793""},
    ""neglongpi"": {""type"": ""float"", ""value"": ""-3.141592653589793""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void LongInteger()
		{
			var inputString = @"answer = 9223372036854775807
neganswer = -9223372036854775808";
			var expectedOutputString = @"{
    ""answer"": {""type"": ""integer"", ""value"": ""9223372036854775807""},
    ""neganswer"": {""type"": ""integer"", ""value"": ""-9223372036854775808""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void MultilineString()
		{
			var inputString = @"multiline_empty_one = """"""""""""
multiline_empty_two = """"""
""""""
multiline_empty_three = """"""\
    """"""
multiline_empty_four = """"""\
   \
   \
   """"""

equivalent_one = ""The quick brown fox jumps over the lazy dog.""
equivalent_two = """"""
The quick brown \


  fox jumps over \
    the lazy dog.""""""

equivalent_three = """"""\
       The quick brown \
       fox jumps over \
       the lazy dog.\
       """"""";
			var expectedOutputString = @"{
    ""multiline_empty_one"": {
        ""type"": ""string"",
        ""value"": """"
    },
    ""multiline_empty_two"": {
        ""type"": ""string"",
        ""value"": """"
    },
    ""multiline_empty_three"": {
        ""type"": ""string"",
        ""value"": """"
    },
    ""multiline_empty_four"": {
        ""type"": ""string"",
        ""value"": """"
    },
    ""equivalent_one"": {
        ""type"": ""string"",
        ""value"": ""The quick brown fox jumps over the lazy dog.""
    },
    ""equivalent_two"": {
        ""type"": ""string"",
        ""value"": ""The quick brown fox jumps over the lazy dog.""
    },
    ""equivalent_three"": {
        ""type"": ""string"",
        ""value"": ""The quick brown fox jumps over the lazy dog.""
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void RawMultilineString()
		{
			var inputString = @"oneline = '''This string has a ' quote character.'''
firstnl = '''
This string has a ' quote character.'''
multiline = '''
This string
has ' a quote character
and more than
one newline
in it.'''";
			var expectedOutputString = @"{
    ""oneline"": {
        ""type"": ""string"",
        ""value"": ""This string has a ' quote character.""
    },
    ""firstnl"": {
        ""type"": ""string"",
        ""value"": ""This string has a ' quote character.""
    },
    ""multiline"": {
        ""type"": ""string"",
        ""value"": ""This string\nhas ' a quote character\nand more than\none newline\nin it.""
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void RawString()
		{
			var inputString = @"backspace = 'This string has a \b backspace character.'
tab = 'This string has a \t tab character.'
newline = 'This string has a \n new line character.'
formfeed = 'This string has a \f form feed character.'
carriage = 'This string has a \r carriage return character.'
slash = 'This string has a \/ slash character.'
backslash = 'This string has a \\ backslash character.'";
			var expectedOutputString = @"{
    ""backspace"": {
        ""type"": ""string"",
        ""value"": ""This string has a \\b backspace character.""
    },
    ""tab"": {
        ""type"": ""string"",
        ""value"": ""This string has a \\t tab character.""
    },
    ""newline"": {
        ""type"": ""string"",
        ""value"": ""This string has a \\n new line character.""
    },
    ""formfeed"": {
        ""type"": ""string"",
        ""value"": ""This string has a \\f form feed character.""
    },
    ""carriage"": {
        ""type"": ""string"",
        ""value"": ""This string has a \\r carriage return character.""
    },
    ""slash"": {
        ""type"": ""string"",
        ""value"": ""This string has a \\/ slash character.""
    },
    ""backslash"": {
        ""type"": ""string"",
        ""value"": ""This string has a \\\\ backslash character.""
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void StringEmpty()
		{
			var inputString = @"answer = """"";
			var expectedOutputString = @"{
    ""answer"": {
        ""type"": ""string"",
        ""value"": """"
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void StringEscapes()
		{
			var inputString = @"backspace = ""This string has a \b backspace character.""
tab = ""This string has a \t tab character.""
newline = ""This string has a \n new line character.""
formfeed = ""This string has a \f form feed character.""
carriage = ""This string has a \r carriage return character.""
quote = ""This string has a \"" quote character.""
backslash = ""This string has a \\ backslash character.""
notunicode1 = ""This string does not have a unicode \\u escape.""
notunicode2 = ""This string does not have a unicode \u005Cu escape.""
notunicode3 = ""This string does not have a unicode \\u0075 escape.""
notunicode4 = ""This string does not have a unicode \\\u0075 escape.""";
			var expectedOutputString = @"{
    ""backspace"": {
        ""type"": ""string"",
        ""value"": ""This string has a \u0008 backspace character.""
    },
    ""tab"": {
        ""type"": ""string"",
        ""value"": ""This string has a \u0009 tab character.""
    },
    ""newline"": {
        ""type"": ""string"",
        ""value"": ""This string has a \u000A new line character.""
    },
    ""formfeed"": {
        ""type"": ""string"",
        ""value"": ""This string has a \u000C form feed character.""
    },
    ""carriage"": {
        ""type"": ""string"",
        ""value"": ""This string has a \u000D carriage return character.""
    },
    ""quote"": {
        ""type"": ""string"",
        ""value"": ""This string has a \u0022 quote character.""
    },
    ""backslash"": {
        ""type"": ""string"",
        ""value"": ""This string has a \u005C backslash character.""
    },
    ""notunicode1"": {
        ""type"": ""string"",
        ""value"": ""This string does not have a unicode \\u escape.""
    },
    ""notunicode2"": {
        ""type"": ""string"",
        ""value"": ""This string does not have a unicode \u005Cu escape.""
    },
    ""notunicode3"": {
        ""type"": ""string"",
        ""value"": ""This string does not have a unicode \\u0075 escape.""
    },
    ""notunicode4"": {
        ""type"": ""string"",
        ""value"": ""This string does not have a unicode \\\u0075 escape.""
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void StringSimple()
		{
			var inputString = @"answer = ""You are not drinking enough whisky.""";
			var expectedOutputString = @"{
    ""answer"": {
        ""type"": ""string"",
        ""value"": ""You are not drinking enough whisky.""
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void StringWithPound()
		{
			var inputString = @"pound = ""We see no # comments here.""
poundcomment = ""But there are # some comments here."" # Did I # mess you up?";
			var expectedOutputString = @"{
    ""pound"": {""type"": ""string"", ""value"": ""We see no # comments here.""},
    ""poundcomment"": {
        ""type"": ""string"",
        ""value"": ""But there are # some comments here.""
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableArrayImplicit()
		{
			var inputString = @"[[albums.songs]]
name = ""Glory Days""";
			var expectedOutputString = @"{
    ""albums"": {
       ""songs"": [
           {""name"": {""type"": ""string"", ""value"": ""Glory Days""}}
       ]
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableArrayMany()
		{
			var inputString = @"[[people]]
first_name = ""Bruce""
last_name = ""Springsteen""

[[people]]
first_name = ""Eric""
last_name = ""Clapton""

[[people]]
first_name = ""Bob""
last_name = ""Seger""";
			var expectedOutputString = @"{
    ""people"": [
        {
            ""first_name"": {""type"": ""string"", ""value"": ""Bruce""},
            ""last_name"": {""type"": ""string"", ""value"": ""Springsteen""}
        },
        {
            ""first_name"": {""type"": ""string"", ""value"": ""Eric""},
            ""last_name"": {""type"": ""string"", ""value"": ""Clapton""}
        },
        {
            ""first_name"": {""type"": ""string"", ""value"": ""Bob""},
            ""last_name"": {""type"": ""string"", ""value"": ""Seger""}
        }
    ]
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableArrayNest()
		{
			var inputString = @"[[albums]]
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
  name = ""Dancing in the Dark""";
			var expectedOutputString = @"{
    ""albums"": [
        {
            ""name"": {""type"": ""string"", ""value"": ""Born to Run""},
            ""songs"": [
                {""name"": {""type"": ""string"", ""value"": ""Jungleland""}},
                {""name"": {""type"": ""string"", ""value"": ""Meeting Across the River""}}
            ]
        },
        {
            ""name"": {""type"": ""string"", ""value"": ""Born in the USA""},
            ""songs"": [
                {""name"": {""type"": ""string"", ""value"": ""Glory Days""}},
                {""name"": {""type"": ""string"", ""value"": ""Dancing in the Dark""}}
            ]
        }
    ]
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableArrayOne()
		{
			var inputString = @"[[people]]
first_name = ""Bruce""
last_name = ""Springsteen""";
			var expectedOutputString = @"{
    ""people"": [
        {
            ""first_name"": {""type"": ""string"", ""value"": ""Bruce""},
            ""last_name"": {""type"": ""string"", ""value"": ""Springsteen""}
        }
    ]
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableEmpty()
		{
			var inputString = @"[a]";
			var expectedOutputString = @"{
    ""a"": {}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableSubEmpty()
		{
			var inputString = @"[a]
[a.b]";
			var expectedOutputString = @"{
    ""a"": { ""b"": {} }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableWhitespace()
		{
			var inputString = @"[""valid key""]";
			var expectedOutputString = @"{
    ""valid key"": {}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void TableWithPound()
		{
			var inputString = @"[""key#group""]
answer = 42";
			var expectedOutputString = @"{
    ""key#group"": {
        ""answer"": {""type"": ""integer"", ""value"": ""42""}
    }
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void UnicodeEscape()
		{
			var inputString = @"answer4 = ""\u03B4""
answer8 = ""\U000003B4""";
			var expectedOutputString = @"{
    ""answer4"": {""type"": ""string"", ""value"": ""\u03B4""},
    ""answer8"": {""type"": ""string"", ""value"": ""\u03B4""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

		[Fact]
		public void UnicodeLiteral()
		{
			var inputString = @"answer = ""δ""";
			var expectedOutputString = @"{
    ""answer"": {""type"": ""string"", ""value"": ""δ""}
}";
			var output = (Dictionary<string, object>)TestDeserializer.Deserialize(inputString);
			var expectedOutput = (Dictionary<string, object>)JsonHelper.Deserialize(expectedOutputString);
			AssertHelpers.ObjectsEqual(expectedOutput, output);
		}

	}
}