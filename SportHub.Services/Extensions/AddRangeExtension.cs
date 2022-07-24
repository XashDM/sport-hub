using System;
using System.Collections.Generic;


namespace SportHub.Services.Extensions
{
    public static class AddRangeExtension
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            List<T> list2 = list as List<T>;
            if (list2 != null)
            {
                list2.AddRange(items);
                return;
            }

            foreach (T item in items)
            {
                list.Add(item);
            }
        }
    }
}


