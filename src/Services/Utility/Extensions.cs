using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Presentation.Utility
{
    public static class Extensions
    {
        public static Guid ToGuid(this Guid? source)
        {
            return source ?? Guid.Empty;
        }

        // more general implementation 
        public static T ValueOrDefault<T>(this Nullable<T> source) where T : struct
        {
            return source ?? default(T);
        }

        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            var result = source.SelectMany(selector);
            if (!result.Any())
            {
                return result;
            }
            return result.Concat(result.SelectManyRecursive(selector));
        }
    }
}

