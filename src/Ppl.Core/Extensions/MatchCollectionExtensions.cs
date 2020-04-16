using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ppl.Core.Extensions
{
    public static class MatchCollectionExtensions
    {
        public static IEnumerable<T> Select<T>(this MatchCollection collection, Func<Match, T> func)
        {
            foreach (Match m in collection) yield return func(m);
        }

        public static void ForEach(this MatchCollection collection, Action<Match> action)
        {
            foreach (Match m in collection) action(m);
        }

        public static async void ForEachAsync(this MatchCollection collection, Func<Match, Task> action)
        {
            for (var i = 0; i < collection.Count; i++) await action(collection[i]);
        }

        public static IEnumerable<Match> AsIEnumerable(this MatchCollection collection)
        {
            foreach (Match m in collection) yield return m;
        }
    }
}