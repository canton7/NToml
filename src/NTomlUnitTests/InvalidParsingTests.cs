
using Xunit;
using System;
using NTomlUnitTests;
using NToml;

public class InvalidValidParsingTests
{

	[Fact]
	public void ArrayMixedTypesArraysAndInts()
	{
		var inputString = @"arrays-and-ints =  [1, [""Arrays are not integers.""]]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void ArrayMixedTypesIntsAndFloats()
	{
		var inputString = @"ints-and-floats = [1, 1.1]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void ArrayMixedTypesStringsAndInts()
	{
		var inputString = @"strings-and-ints = [""hi"", 42]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DatetimeMalformedNoLeads()
	{
		var inputString = @"no-leads = 1987-7-05T17:45:00Z";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DatetimeMalformedNoSecs()
	{
		var inputString = @"no-secs = 1987-07-05T17:45Z";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DatetimeMalformedNoT()
	{
		var inputString = @"no-t = 1987-07-0517:45:00Z";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DatetimeMalformedNoZ()
	{
		var inputString = @"no-z = 1987-07-05T17:45:00";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DatetimeMalformedWithMilli()
	{
		var inputString = @"with-milli = 1987-07-5T17:45:00.12Z";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DuplicateKeyTable()
	{
		var inputString = @"[fruit]
type = ""apple""

[fruit.type]
apple = ""yes""";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DuplicateKeys()
	{
		var inputString = @"dupe = false
dupe = true";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void DuplicateTables()
	{
		var inputString = @"[a]
[a]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void EmptyImplicitTable()
	{
		var inputString = @"[naughty..naughty]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void EmptyTable()
	{
		var inputString = @"[]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void FloatNoLeadingZero()
	{
		var inputString = @"answer = .12345
neganswer = -.12345";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void FloatNoTrailingDigits()
	{
		var inputString = @"answer = 1.
neganswer = -1.";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void KeyEmpty()
	{
		var inputString = @" = 1";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void KeyHash()
	{
		var inputString = @"a# = 1";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void KeyNewline()
	{
		var inputString = @"a
= 1";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void KeyOpenBracket()
	{
		var inputString = @"[abc = 1";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void KeySingleOpenBracket()
	{
		var inputString = @"[";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void KeyStartBracket()
	{
		var inputString = @"[a]
[xyz = 5
[b]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void KeyTwoEquals()
	{
		var inputString = @"key= = 1";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void StringBadByteEscape()
	{
		var inputString = @"naughty = ""\xAg""";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void StringBadEscape()
	{
		var inputString = @"invalid-escape = ""This string has a bad \a escape character.""";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void StringByteEscapes()
	{
		var inputString = @"answer = ""\x33""";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void StringNoClose()
	{
		var inputString = @"no-ending-quote = ""One time, at band camp";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TableArrayImplicit()
	{
		var inputString = @"# This test is a bit tricky. It should fail because the first use of
# `[[albums.songs]]` without first declaring `albums` implies that `albums`
# must be a table. The alternative would be quite weird. Namely, it wouldn't
# comply with the TOML spec: ""Each double-bracketed sub-table will belong to 
# the most *recently* defined table element *above* it.""
#
# This is in contrast to the *valid* test, table-array-implicit where
# `[[albums.songs]]` works by itself, so long as `[[albums]]` isn't declared
# later. (Although, `[albums]` could be.)
[[albums.songs]]
name = ""Glory Days""

[[albums]]
name = ""Born in the USA""";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TableArrayMalformedBracket()
	{
		var inputString = @"[[albums]
name = ""Born to Run""";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TableArrayMalformedEmpty()
	{
		var inputString = @"[[]]
name = ""Born to Run""";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TableEmpty()
	{
		var inputString = @"[]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TableNestedBracketsClose()
	{
		var inputString = @"[a]b]
zyx = 42";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TableNestedBracketsOpen()
	{
		var inputString = @"[a[b]
zyx = 42";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TextAfterArrayEntries()
	{
		var inputString = @"array = [
  ""Is there life after an array separator?"", No
  ""Entry""
]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TextAfterInteger()
	{
		var inputString = @"answer = 42 the ultimate answer?";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TextAfterString()
	{
		var inputString = @"string = ""Is there life after strings?"" No.";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TextAfterTable()
	{
		var inputString = @"[error] this shouldn't be here";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TextBeforeArraySeparator()
	{
		var inputString = @"array = [
  ""Is there life before an array separator?"" No,
  ""Entry""
]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


	[Fact]
	public void TextInArray()
	{
		var inputString = @"array = [
  ""Entry 1"",
  I don't belong,
  ""Entry 2"",
]";
		AssertHelpers.ThrowsDerived<ParseException>(() => TestDeserializer.Deserialize(inputString));
	}


}