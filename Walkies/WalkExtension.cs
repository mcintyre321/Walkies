using System;
using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public delegate object GetChildRule(object parent, string fragment);

    public static class WalkExtension
    {
        private static List<Func<object, string, object>> Rules = new List<Func<object, string, object>>()
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
            foreach (var fragment in path)
            {
                var child = Rules.Select(r => r(parent, fragment)).FirstOrDefault(v => v != null);
                if (child == null)
                {
                    yield return null;
                    continue;
                }
                child.SetParent(parent);
                child.SetName(fragment);
                yield return child;
            }
        }
    }
}
