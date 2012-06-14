using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walkies
{
    public static class TraversalExtensions
    {
        public static IEnumerable<object> AncestorsAndSelf(this object o)
        {
            do
            {
                yield return o;
                o = o.GetParent();
            } while (o != null);
        }

        public static IEnumerable<object> Ancestors(this object o)
        {
            return o.AncestorsAndSelf().Skip(1);
        }

        public static IEnumerable<string> Path(this object descendant)
        {
            return descendant.AncestorsAndSelf().Reverse().Skip(1).Select(o => o.GetName());
        }

        public static IEnumerable<T> Each<T>(IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
                yield return item;
            }
        } 
    }
}
