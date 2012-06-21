using System;
using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public delegate IEnumerable<Tuple<string, object>> GetChildren(object parent);

    public delegate object GetChildRule(object parent, string fragment);

    public static class WalkExtension
    {
        public static List<GetChildRule> Rules = new List<GetChildRule>()
        {
            GetChild.Rule,
            ScanChildrenEnumerables,
            ChildAttribute.Rule, 
            ScanEnumerable.Rule,
            Indexable.Rule
        };

        private static object ScanChildrenEnumerables(object parent, string fragment)
        {
            return GetChildrenRules.SelectMany(r => r(parent) ?? new Tuple<string, object>[]{})
                .Select(pair => new {IsMatch = pair.Item1 == fragment, Pair = pair})
                .Where(triple => triple.IsMatch)
                .Select(triple => triple.Pair.Item2)
                .FirstOrDefault();
        }

        public static List<GetChildren> GetChildrenRules = new List<GetChildren>()
        {
            HasChildren.Rule,
            ChildAttribute.ChildrenRule,
            ScanEnumerable.ChildrenRule,
        };
 
        public static IEnumerable<object> Walk(this object parent, string path)
        {
            return Walk(parent, path.Trim('/').Split(new[]{'/'}, StringSplitOptions.RemoveEmptyEntries));
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
            if (child.GetParent() == null) child.SetParent(parent);
            if (child.GetFragment() == null) child.SetFragment(fragment);
            return child;
        }
    }
}
