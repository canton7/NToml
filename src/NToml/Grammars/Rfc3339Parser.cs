using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml.Grammars
{
    public static class Rfc3339Grammar
    {
        // http://tools.ietf.org/html/rfc3339#section-5.6

        public static readonly Parser<string> DateFullyear = Parse.Digit.Repeat(4).Text();
        public static readonly Parser<string> DateMonth = Parse.Digit.Repeat(2).Text();
        public static readonly Parser<string> DateMDay = Parse.Digit.Repeat(2).Text();
        public static readonly Parser<string> TimeHour = Parse.Digit.Repeat(2).Text();
        public static readonly Parser<string> TimeMinute = Parse.Digit.Repeat(2).Text();
        public static readonly Parser<string> TimeSecond = Parse.Digit.Repeat(2).Text();
        public static readonly Parser<string> TimeSecFrac =
            from point in Parse.Char('.')
            from digits in Parse.Digit.AtLeastOnce().Text()
            select '.' + digits;
        public static readonly Parser<string> TimeNumOffset =
            from sign in Parse.Chars("+-")
            from hour in TimeHour
            from separator in Parse.Char(':')
            from minute in TimeMinute
            select String.Format("{0}{1}:{2}", sign, hour, minute);
        public static Parser<string> TimeOffset = Parse.Chars("Zz").Once().Text().Or(TimeNumOffset);
        public static Parser<string> PartialTime =
            from hour in TimeHour
            from sep1 in Parse.Char(':')
            from minute in TimeMinute
            from sep2 in Parse.Char(':')
            from sec in TimeSecond
            from secFrac in TimeSecFrac.Optional()
            select String.Format("{0}:{1}:{2}{3}", hour, minute, sec, secFrac.GetOrElse(""));
        public static Parser<string> FullDate =
            from year in DateFullyear
            from sep1 in Parse.Char('-')
            from month in DateMonth
            from sep2 in Parse.Char('-')
            from day in DateMDay
            select String.Format("{0}-{1}-{2}", year, month, day);
        public static Parser<string> FullTime =
            from time in PartialTime
            from offset in TimeOffset
            select time + offset;
        public static Parser<string> DateTime =
            from date in FullDate
            from sep in Parse.Chars("Tt")
            from time in FullTime
            select String.Format("{0}T{1}", date, time);

        public static Parser<DateTime> Rfc3339
            = DateTime.Select(x => System.DateTime.Parse(x, null, System.Globalization.DateTimeStyles.RoundtripKind));
    }
}
