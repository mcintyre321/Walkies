using System;
using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public interface IHasChildren
    {
        IEnumerable<object> Children{ get; }
    }

    public class HasChildren
    {
        public static IEnumerable<Tuple<string, object>> Rule(object root)
        {
            var hasChildren = root as IHasChildren;
            if (hasChildren != null)
            {
                return hasChildren.Children.Select(c => Tuple.Create(c.GetFragment(), c));
            }
            return null;
        }
    }
}