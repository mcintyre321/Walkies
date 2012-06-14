using System;
using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public delegate object GetChildRule(object parent, string fragment);

    public static class WalkExtension
    {
        public static List<Func<object, string, object>> Rules = new List<Func<object, string, object>>()
        {
            GetChild.Rule,
            GetFromChildren.Rule,
            ChildAttribute.Rule
        };
 
        public static IEnumerable<object> Walk(this object parent, string path)
        {
            return Walk(parent, path.Split('/'));
        }

        public static IEnumerable<object> Walk(this object parent, IEnumerable<string> path)
        {
            yield return parent;
            var current = parent;
            foreach (var fragment in path)
            {
                current = current.Child(fragment);
                yield return current;
            }
        }

        public static object Child(this object parent, string fragment)
        {
            if (parent == null) return null;
            var child = Rules.Select(r => r(parent, fragment)).FirstOrDefault(v => v != null);
            if (child == null)
            {
                return null;
            }
            child.SetParent(parent);
            child.SetName(fragment);
            return child;
        }
    }
}
