using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class FragmentExtension
    {
        static FragmentExtension()
        {
            GetFragmentFromWeakTableRule = obj => fragments.GetValue(obj, c => null);
            SetFragmentInWeakTableRule = (obj, fragment) =>
            {
                fragments.Remove(obj);
                fragments.Add(obj, fragment);
            };
        }

        private static readonly ConditionalWeakTable<object, string> fragments = new ConditionalWeakTable<object, string>();

        public static Action<object, string> SetFragmentInWeakTableRule { get; set; }
        public static T SetFragment<T>(this T obj, string fragment)
        {
            SetFragmentInWeakTableRule(obj, fragment);
            return obj;
        }

        public static Func<object, string> GetFragmentFromWeakTableRule { get; set; }
        
        
        public static string GetFragment(this object obj)
        {
            return GetFragmentFromWeakTableRule(obj) ?? GetFragmentFromParent(obj);
        }

        private static string GetFragmentFromParent(object obj)
        {
            var parent = obj.GetParent();
            if (parent != null)
            {
                var children = parent.KnownChildrenWithFragments();
                if (children != null)
                {
                    var child = children.FirstOrDefault(c => c.Item2 == obj);
                    if (child != null)
                    {
                        return child.Item1;
                    }
                }
            }
            return null;
        }
    }
}