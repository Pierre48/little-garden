using System;
using System.Collections.Generic;

namespace Ppl.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration) action(item);
        }
    }
}