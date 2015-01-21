using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public class Parser
    {
        public static readonly Parser<char> Whitespace = Parse.Chars(' ', '\t');
        public static readonly Parser<char> Backslash = Parse.Char('\\');
        public static readonly Parser<char> Newline = Parse.Char('\n').Or(Parse.String("\r\n").Return('\n'));
        public static readonly Parser<char> QuotedStringDelimeter = Parse.Char('"');

        public static readonly Parser<char> Escape = Parse.Char('\\');
        public static Parser<T> Escaped<T>(Parser<T> following)
        {
            return from escape in Escape
                   from f in following
                   select f;
        }
        public static readonly Parser<char> EscapedBackspace = Escaped(Parse.Char('b')).Return('\u0008');
        public static readonly Parser<char> EscapedTab = Escaped(Parse.Char('t')).Return('\t');
        public static readonly Parser<char> EscapedLinefeed = Escaped(Parse.Char('n')).Return('\n');
        public static readonly Parser<char> EscapedFormFeed = Escaped(Parse.Char('f')).Return('\f');
        public static readonly Parser<char> EscapedCarriageReturn = Escaped(Parse.Char('r')).Return('\r');
        public static readonly Parser<char> EscapedQuote = Escaped(QuotedStringDelimeter).Return('"');
        public static readonly Parser<char> EscapedBackslash = Escaped(Parse.Char('\\')).Return('\\');
        public static readonly Parser<char> HexChars = Parse.Chars("0123456789abcdefABCDEF");
        public static readonly Parser<char> ShortUnicode =
            from u in Escaped(Parse.Char('u'))
            from rest in HexChars.Repeat(4)
            select 'T'; // TODO
        public static readonly Parser<char> LongUnicode =
            from u in Escaped(Parse.Char('U'))
            from rest in HexChars.Repeat(8)
            select 'U'; // TODO

        public static readonly Parser<char> ControlCharacter = Parse.Char(x => x <= 0x1F, "Unicode Control Characters");

        public static readonly Parser<char> BasicStringContent =
            EscapedBackspace.Or(EscapedTab).Or(EscapedLinefeed).Or(EscapedFormFeed).Or(EscapedCarriageReturn).Or(EscapedQuote)
            .Or(EscapedBackslash).Or(HexChars).Or(ShortUnicode).Or(LongUnicode)
            .Or(Parse.AnyChar.Except(ControlCharacter).Except(Parse.Char('\\')).Except(QuotedStringDelimeter));

        public static readonly Parser<string> BasicString =
            from open in QuotedStringDelimeter
            from content in BasicStringContent.Many().Text()
            from close in QuotedStringDelimeter
            select content;

        public static Parser<string> MultilineStringDelimeter = QuotedStringDelimeter.Repeat(3).Text();
        public static Parser<char> MultilineStringContent = Parse.AnyChar.Except(MultilineStringDelimeter);
        public static readonly Parser<string> MultilineBasicString =
            from open in MultilineStringDelimeter
            from firstNewline in Newline.Optional()
            from content in MultilineStringContent.Many().Text()
            from close in MultilineStringDelimeter
            select content;

        public static Parser<long> Integer =
            from sign in Parse.Chars("+-").Optional()
            from leading in Parse.Digit.Except(Parse.Char('0'))
            from rest in Parse.Digit.Many().Text()
            select Int64.Parse(sign.GetOrDefault() + leading + rest);

        public static Parser<bool> Boolean = Parse.String("true").Return(true).Or(Parse.String("false").Return(false));

        public static readonly Parser<char> BareKeyChars = Parse.Chars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-");
        public static readonly Parser<string> BareKey = BareKeyChars.AtLeastOnce().Text();
        public static readonly Parser<string> QuotedKey = BasicString;

        public static readonly Parser<char> CommentChar = Parse.Char('#');
        public static readonly Parser<string> Comment =
            from open in CommentChar.Token()
            from rest in Parse.AnyChar.Except(Newline).Many().Text()
            select rest;

        public static readonly Parser<string> TableValue =
            from value in BasicString.Or(MultilineBasicString).Token()
            from rest in Newline.Once().Text().Or(Comment)
            select value;

        public static readonly Parser<KeyValuePair> KeyValuePair =
            from key in BareKey.Or(QuotedKey).Token()
            from eq in Parse.Char('=')
            from value in TableValue
            select new KeyValuePair() { Key = key, Value = value };



        
        //public static readonly Parser<char> EscapedBackspace = Escaped(Parse.Char('b')).Select(x => '\b');
        //public static readonly Parser<char>
    }
}
