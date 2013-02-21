using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Walkies
{
    public class FragmentAttribute : Attribute
    {
    }

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
            return GetFragmentFromWeakTableRule(obj)
                ?? GetFragmentFromAttribute(obj)
                   ?? GetFragmentFromParent(obj);
        }

        static ConcurrentDictionary<Type, Func<Object, String>> lookup = new ConcurrentDictionary<Type, Func<object, string>>();
        private static string GetFragmentFromAttribute(object obj)
        {
            var func = lookup.GetOrAdd(obj.GetType(), type =>
                type.GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(FragmentAttribute), true).Any())
                .Select(p => new Func<Object, String>(target => p.GetValue(target).ToString()))
                .SingleOrDefault()
            );
            return func == null ? null : func(obj);
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