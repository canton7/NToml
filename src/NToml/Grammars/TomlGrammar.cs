using NToml.Values;
using Sprache;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml.Grammars
{
    internal class TomlGrammar
    {
        static readonly Parser<char> Whitespace = Parse.Chars(' ', '\t').Named("whitespace");
        static readonly Parser<char> Newline = Parse.Char('\n').Or(Parse.String("\r\n").Return('\n')).Named("newline");
        static readonly Parser<char> WhitespaceOrNewline = Whitespace.Or(Newline).Named("whitespace or newline");
        static readonly Parser<char> Backslash = Parse.Char('\\');
        static readonly Parser<char> BasicStringDelimeter = Parse.Char('"');
        static readonly Parser<string> MultilineBasicStringDelimeter = Parse.String("\"\"\"").Text();
        static readonly Parser<char> LiteralStringDelimeter = Parse.Char('\'');
        static readonly Parser<string> MultilineLiteralStringDelimeter = Parse.String("'''").Text();

        static readonly Parser<char> Escape = Parse.Char('\\');
        static Parser<T> Escaped<T>(Parser<T> following)
        {
            return from escape in Escape
                   from f in following
                   select f;
        }
        static Parser<T> Tokenize<T>(Parser<T> parser, Parser<char> whitespace)
        {
            return (from before in whitespace.Many()
                   from item in parser
                   from after in whitespace.Many()
                   select item);
        }
        static Parser<T> TokenizeBefore<T>(Parser<T> parser, Parser<char> whitespace)
        {
            return from before in whitespace.Many()
                   from item in parser
                   select item;
        }
        static Parser<T> TokenizeAfter<T>(Parser<T> parser, Parser<char> whitespace)
        {
            return from item in parser
                   from after in whitespace.Many()
                   select item;
        }

        static char ParseUnicodeHexChars(string hexChars)
        {
            return Convert.ToChar(Int64.Parse(hexChars, NumberStyles.HexNumber));
        }

        static readonly Parser<char> EscapedBackspace = Escaped(Parse.Char('b')).Return('\u0008');
        static readonly Parser<char> EscapedTab = Escaped(Parse.Char('t')).Return('\t');
        static readonly Parser<char> EscapedLinefeed = Escaped(Parse.Char('n')).Return('\n');
        static readonly Parser<char> EscapedFormFeed = Escaped(Parse.Char('f')).Return('\f');
        static readonly Parser<char> EscapedCarriageReturn = Escaped(Parse.Char('r')).Return('\r');
        static readonly Parser<char> EscapedQuote = Escaped(BasicStringDelimeter).Return('"');
        static readonly Parser<char> EscapedBackslash = Escaped(Parse.Char('\\')).Return('\\');
        static readonly Parser<char> HexChars = Parse.Chars("0123456789abcdefABCDEF").Named("hex character");
        static readonly Parser<char> ShortUnicode =
            from u in Escaped(Parse.Char('u'))
            from rest in HexChars.Repeat(4).Text()
            select ParseUnicodeHexChars(rest);
        static readonly Parser<char> LongUnicode =
            from u in Escaped(Parse.Char('U'))
            from rest in HexChars.Repeat(8).Text()
            select ParseUnicodeHexChars(rest);

        static readonly Parser<char> ControlCharacter = Parse.Char(x => x <= 0x1F, "Unicode Control Characters");

        static readonly Parser<char> StringContent =
            EscapedBackspace.Or(EscapedTab).Or(EscapedLinefeed).Or(EscapedFormFeed).Or(EscapedCarriageReturn).Or(EscapedQuote)
            .Or(EscapedBackslash).Or(HexChars).Or(ShortUnicode).Or(LongUnicode)
            .Or(Parse.AnyChar.Except(ControlCharacter).Except(Parse.Char('\\')));

        static readonly Parser<char> SingleLineBasicStringContent =
            StringContent.Except(BasicStringDelimeter);

        static readonly Parser<string> SingleLineBasicString =
            from open in BasicStringDelimeter
            from content in SingleLineBasicStringContent.Many().Text()
            from close in BasicStringDelimeter
            select content;

        static readonly Parser<IEnumerable<char>> MultilineBasicStringNewlineEscape =
            from backslash in Parse.Char('\\')
            // Force a newline in there - otherwise 'a \ b' is allowed
            from tailingSpace in Whitespace.Many()
            from newline in Newline
            from rest in WhitespaceOrNewline.Many()
            select Enumerable.Empty<char>();

        // Try and match newline first, as it gets normalized to \n
        static readonly Parser<char> MultilineBasicStringContent =
            Newline.Or(StringContent.Except(MultilineBasicStringDelimeter).Except(MultilineBasicStringNewlineEscape));
        static readonly Parser<string> MultilineBasicString =
            from open in MultilineBasicStringDelimeter
            from firstNewline in MultilineBasicStringNewlineEscape.Or(Newline.Once()).Or(Parse.Return(Enumerable.Empty<char>()))
            from content in (MultilineBasicStringNewlineEscape.Text().Or(MultilineBasicStringContent.Many().Text())).Many()
            from close in MultilineBasicStringDelimeter
            select String.Join("", content);

        static readonly Parser<char> SingleLineLiteralStringContent =
            Parse.AnyChar.Except(LiteralStringDelimeter).Except(Newline);

        static readonly Parser<string> SingleLineLiteralString =
            from open in LiteralStringDelimeter
            from content in SingleLineLiteralStringContent.Many().Text()
            from close in LiteralStringDelimeter
            select content;

        // Try and match newline first, as it gets normalized to \n
        static readonly Parser<char> MultilineLiteralStringContent = Newline.Or(Parse.AnyChar.Except(MultilineLiteralStringDelimeter));
        static readonly Parser<string> MultilineLiteralString =
            from open in MultilineLiteralStringDelimeter
            from firstNewline in Newline.Optional()
            from content in MultilineLiteralStringContent.Many().Text()
            from close in MultilineLiteralStringDelimeter
            select content;

        static readonly Parser<IntegerValue> Integer =
            (from sign in Parse.Chars("+-").Optional()
            from leading in Parse.Digit.Except(Parse.Char('0'))
            from rest in Parse.Digit.Many().Text()
            select new IntegerValue(Int64.Parse(String.Format("{0}{1}{2}", sign.GetOrElse('0'), leading, rest)))).Named("integer");

        static readonly Parser<string> FloatIntegerPart =
            from sign in Parse.Chars("+-").Optional()
            from integer in Parse.Digit.AtLeastOnce().Text()
            select sign.GetOrElse('+') + integer;

        static readonly Parser<string> FloatFractionalPart =
            from point in Parse.Char('.')
            from rest in Parse.Digit.AtLeastOnce().Text()
            select "." + rest;

        static readonly Parser<string> FloatExponentialPart =
            from exp in Parse.Chars("eE")
            from sign in Parse.Chars("+-").Optional()
            from rest in Parse.Digit.AtLeastOnce().Text()
            select "E" + sign.GetOrElse('+') + rest;

        static readonly Parser<FloatValue> Float =
            (from integer in FloatIntegerPart
            from rest in FloatFractionalPart.Or(FloatExponentialPart).Or(FloatFractionalPart.Then(frac => FloatExponentialPart.Select(exp => frac + exp)))
            select new FloatValue(Double.Parse(integer + rest))).Named("float");

        static readonly Parser<IValue> NumericValue =
            Integer.Or<IValue>(Float);

        static readonly Parser<BooleanValue> Boolean =
            (Parse.String("true").Return(true).Or(Parse.String("false").Return(false)).Select(x => new BooleanValue(x))).Named("boolean (true or false)");

        static readonly Parser<char> BareKeyChars = Parse.Chars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-").Named("a-zA-Z_-");
        static readonly Parser<string> BareKey = BareKeyChars.AtLeastOnce().Text();
        static readonly Parser<string> QuotedKey = SingleLineBasicString;
        static readonly Parser<string> Key = BareKey.Or(QuotedKey);

        // Multi-line variants have to go before single-line variants, or the opening """/''' gets incorrectly parsed as a string in its own right
        // We have to allow zero-length strings, so we can't insist that all strings contain content
        static readonly Parser<StringValue> StringValue =
            MultilineBasicString.Or(SingleLineBasicString).Or(MultilineLiteralString).Or(SingleLineLiteralString).Select(x => new StringValue(x));

        static readonly Parser<DateTimeValue> DateTime =
            Rfc3339Grammar.Rfc3339.Select(x => new DateTimeValue(x)).Named("datetime");

        static readonly Parser<char> CommentChar = Parse.Char('#');
        static readonly Parser<string> Comment =
            (from open in TokenizeBefore(CommentChar, Whitespace)
             from rest in Parse.AnyChar.Except(Newline).Many().Text()
             select rest).Named("comment");

        static Parser<ArrayValue<T>> ArrayOf<T>(Parser<T> elementType) where T : IValue
        {
            return from open in Parse.Char('[')
                   from comment1 in Tokenize(Comment, Newline).Many()
                   from items in
                   (
                       from first in Tokenize(elementType.Once(), WhitespaceOrNewline)
                       from rest in
                           (
                            from comment2 in Tokenize(Comment, Newline).Many()
                            from comma in Tokenize(Parse.Char(','), WhitespaceOrNewline)
                            from comment3 in Tokenize(Comment, Newline).Many()
                            from element in Tokenize(elementType, WhitespaceOrNewline)
                            select element
                           ).Many()
                       from comment4 in Tokenize(Comment, Newline).Many()
                       from trailingComma in Tokenize(Parse.Char(','), WhitespaceOrNewline).Optional()
                       from comment5 in Tokenize(Comment, Newline).Many()
                       select new ArrayValue<T>(first.Concat(rest).ToArray())
                   ).Optional()
                   from close in TokenizeBefore(Parse.Char(']'), WhitespaceOrNewline)
                   select items.GetOrElse(new ArrayValue<T>(new T[0]));
        }

        static readonly Parser<IValue> ArrayValue =
            ArrayOf(DateTime).Or<IValue>(ArrayOf(Integer)).Or(ArrayOf(Float)).Or(ArrayOf(Boolean))
            .Or(ArrayOf(StringValue)).Or(ArrayOf(Parse.Ref(() => ArrayValue)));

        static readonly Parser<IValue> InlineTableValue =
            from open in Parse.Char('{')
            from content in KeyValuePair.XDelimitedBy(Parse.Char(',')).Optional()
            from close in Tokenize(Parse.Char('}'), Whitespace)
            from comment in Comment.Optional()
            select new TableValue(null, content.GetOrElse(Enumerable.Empty<KeyValuePair>()));

        static readonly Parser<IEnumerable<string>> TableName =
            from open in Tokenize(Parse.Char('['), Whitespace)
            from content in Key.DelimitedBy(Tokenize(Parse.Char('.'), Whitespace))
            from close in Tokenize(Parse.Char(']'), Whitespace)
            select content;

        static readonly Parser<IEnumerable<string>> ArrayTableName =
            from open in Tokenize(Parse.String("[["), Whitespace)
            from content in Key.DelimitedBy(Tokenize(Parse.Char('.'), Whitespace))
            from close in Tokenize(Parse.String("]]"), Whitespace)
            select content;

        static readonly Parser<IValue> TableValue =
            from value in DateTime.XOr<IValue>(NumericValue).XOr(Boolean).XOr(StringValue).XOr(ArrayValue).XOr(InlineTableValue)
            from trailingWhitespace in Whitespace.Many()
            from rest in Comment.Optional()
            select value;

        static readonly Parser<KeyValuePair> KeyValuePair =
            from key in TokenizeBefore(BareKey.Or(QuotedKey), Whitespace)
            from eq in Tokenize(Parse.Char('='), Whitespace)
            from value in TableValue
            select new KeyValuePair(key, value);

        static readonly Parser<KeyValuePair> KeyValuePairWithComment =
            from keyValuePair in KeyValuePair
            from comment in Comment.Optional()
            select keyValuePair;

        static readonly Parser<string> TableLineTerminator =
            Parse.Return("").End().XOr(Newline.Once().Text().End()).Or(Newline.Once().Text());

        static readonly Parser<KeyValuePair> TableLine =
            from item in KeyValuePairWithComment.XOr(Comment.Select(_ => (KeyValuePair)null)).XOr(Whitespace.Many().Select(_ => (KeyValuePair)null))
            from newline in TableLineTerminator
            select item;

        static readonly Parser<IEnumerable<KeyValuePair>> TableLines = TableLine.AtLeastOnce().Select(x => x.Where(line => line != null));

        static readonly Parser<Table> Table =
            from name in TableName
            from comment in Comment.Optional()
            from lines in
            (
                from newline in Newline
                from lines in TableLines
                select lines
            )
            select new Table(name.ToArray(), lines, false);

        static readonly Parser<Table> ArrayTable =
            from name in ArrayTableName
            from comment in Comment.Optional()
            from newline in Newline
            from lines in TableLines
            select new Table(name.ToArray(), lines, true);

        static readonly Parser<Table> AnyTable = Table.XOr(ArrayTable);

        static readonly Parser<Table> FirstTable =
            TableLine.XAtLeastOnce().Select(x => new Table(new string[0], x.Where(line => line != null), false));

        public static readonly Parser<IEnumerable<Table>> Document =
            from firstTable in FirstTable.XOr(AnyTable)
            from rest in AnyTable.XMany().End()
            select new[] { firstTable }.Concat(rest);
    }
}