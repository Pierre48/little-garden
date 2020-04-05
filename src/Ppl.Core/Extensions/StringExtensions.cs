using System;
using System.Collections.Generic;
using System.Threading;

namespace Ppl.Core.Extensions
{
    public static class StringExtensions
    {
        public static float ToFloat(this string value)
        {
            if (value == "") return 0;
            return float.Parse(
                value
                .Replace(",",Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                .Replace(".",Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator));
        }

        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }
        public static DateTime ToDate(this string value, string dateformat)
        {
            return DateTime.ParseExact(value,dateformat,null);
        }
    }
}