using System;
using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public interface IHasChildren
    {
        IEnumerable<object> Children { get; }
    }

    class EmptyHasChildren : IHasChildren, IHasNamedChildren
    {
        public IEnumerable<object> Children
        {
            get { yield break; }
        }

        IEnumerable<Tuple<string, object>> IHasNamedChildren.Children
        {
            get { yield break; }
        }
    }

    public interface IHasNamedChildren
    {
        IEnumerable<Tuple<string, object>> Children { get; }
    }

    public class HasChildren
    {
        public static IEnumerable<Tuple<string, object>> Rule(object root)
        {
            return (root as IHasChildren ?? new EmptyHasChildren()).Children.Select(c => Tuple.Create(c.GetFragment(), c))
                .Concat((root as IHasNamedChildren ?? new EmptyHasChildren()).Children);
        }
    }
}