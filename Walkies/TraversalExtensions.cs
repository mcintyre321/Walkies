using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walkies
{
    public static class TraversalExtensions
    {
        public static IEnumerable<object> KnownChildren(this object parent)
        {
            return WalkExtension.GetChildrenRules.SelectMany(r => r(parent) ?? new Tuple<string, object>[] { })
                .Select(pair => pair.Item2);
        }
        public static IEnumerable<object> KnownDescendants(this object parent)
        {
            return parent.RecurseMany(p => p.KnownChildren());
        }


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

        public static IEnumerable<string> WalkedPath(this object descendant)
        {
            return descendant.Walked().Skip(1).Select(o => o.GetFragment());
        }
        public static string WalkedPath(this object descendant, string separator)
        {
            return string.Join(separator, descendant.WalkedPath());
        }
        public static IEnumerable<object> Walked(this object descendant)
        {
            return descendant.AncestorsAndSelf().Reverse();
        } 


        public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
                yield return item;
            }
        } 
    }
}
