using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public interface IHasChildren
    {
        IEnumerable<object> Children{ get; }
    }

    public class GetFromChildren
    {
        public static object Rule(object root, string fragment)
        {
            var hasChildren = root as IHasChildren;
            if (hasChildren != null)
            {
                return hasChildren.Children.FirstOrDefault(c => c.GetFragment() == fragment);
            }
            return null;
        }
    }
}