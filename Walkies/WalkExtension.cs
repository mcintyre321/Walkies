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
            //ScanEnumerable.Rule,
            Indexable.Rule
        };

        private static object ScanChildrenEnumerables(object parent, string fragment)
        {
            if (true/*!parent.GetNotWalkable()*/)
            {
                return GetChildrenRules.SelectMany(r => r(parent) ?? new Tuple<string, object>[] { })
                    .Select(
                            pair =>
                            new
                            {
                                IsMatch = StringComparer.InvariantCultureIgnoreCase.Compare(pair.Item1, fragment) == 0,
                                Pair = pair
                            })
                    .Where(triple => triple.IsMatch)
                    .Select(triple => triple.Pair.Item2)
                    .FirstOrDefault();
            }
            return null;
        }

        public static List<GetChildren> GetChildrenRules = new List<GetChildren>()
        {
            ChildrenAttribute.ChildrenRule,
            HasChildren.Rule,
            ChildAttribute.ChildrenRule,
           // ScanEnumerable.ChildrenRule,
        };

        public static IEnumerable<object> Walk(this object parent, string path)
        {
            return Walk(parent, path.Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static IEnumerable<object> Walk(this object parent, IEnumerable<string> path)
        {
            yield return parent;
            var current = parent;
            foreach (var fragment in path)
            {
                var child = current.Child(fragment);
               
                current = child;
                
                yield return current;
            }
        }

        public static object WalkTo (this object parent, string path)
        {
            return parent.Walk(path).Last();
        }

        public static T WalkTo<T>(this object parent, string path)
        {
            return (T) parent.Walk(path).Last();
        }

        public static object WalkTo(this object parent, IEnumerable<string> path)
        {
            return parent.Walk(path).Last();
        }

        public static T WalkTo<T>(this object parent, IEnumerable<string> path)
        {
            return (T)parent.Walk(path).Last();
        }



        static object Child(this object parent, string fragment)
        {
            if (parent == null) return null;
            var child = Rules.Select(r => r(parent, fragment)).FirstOrDefault(v => v != null);
            if (child == null)
            {
                return null;
            }
            //foreach (var rule in OnWalkRules.Rules)
            //{
            //    child = (rule(current, fragment, child)) ?? child;
            //}
            if (child.GetParent() == null) child.SetParent(parent);
            if (child.GetFragment() == null) child.SetFragment(fragment);
            return child;
        }
    }
}
