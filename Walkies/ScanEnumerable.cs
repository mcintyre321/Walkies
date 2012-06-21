using System;
using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public class ScanEnumerable
    {
        public static object Rule(object root, string fragment)
        {
            var enumerable = root as IEnumerable<object>;
            if (enumerable != null && enumerable.GetNotWalkable() == false)
            {
                return enumerable.FirstOrDefault(i => i.GetFragment() == fragment);
            }
            return null;
        }

        public static IEnumerable<Tuple<string, object>> ChildrenRule(object parent)
        {
            var enumerable = parent as IEnumerable<object>;
            if (enumerable != null && enumerable.GetNotWalkable())
            {
                return enumerable.Select(i => Tuple.Create(i.GetFragment(), i));
            }
            return null;
        }
    }
}