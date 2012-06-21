using System;
using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class PublicParentExtensions
    {
        public static T SetParent<T>(this T obj, object parent, Func<T, string> getFragment)
        {
            return obj.SetParent(() => parent, getFragment(obj));
        }
        
        public static T SetParent<T>(this T obj, object parent, string fragment)
        {
            return obj.SetParent(() => parent).SetFragment(fragment);
        }

        public static T SetParent<T>(this T obj, Func<object> getParent, string fragment)
        {
            obj.SetParent(getParent).SetFragment(fragment);
            return obj;
        }

        public static T SetParent<T>(this T obj, Func<object> getParent, Func<T, string> getFragment)
        {
            return obj.SetParent(getParent, getFragment(obj));
        }

        public static T SetChild<T>(this T parent, object child, string fragment)
        {
            child.SetParent(() => parent).SetFragment(fragment);
            return parent;
        }

        public static object Parent(this object obj)
        {
            return obj.GetParent();
        }
    }

    internal static class ParentExtension
    {
        static ParentExtension()
        {
            GetParentFunc = obj =>
            {
                var value = parents.GetValue(obj, k => () => null);
                return value();
            };
        }
        private static readonly ConditionalWeakTable<object, Func<object>> parents = new ConditionalWeakTable<object, Func<object>>();

        internal static T SetParent<T>(this T obj, object parent) { return SetParent(obj, () => parent); }
        internal static T SetParent<T>(this T obj, Func<object> parent)
        {
            parents.Remove(obj);
            parents.Add(obj, parent ?? (() => null));
            return obj;
        }

        internal static T SetChild<T>(this T obj, object child)
        {
            child.SetParent(() => obj);
            return obj;
        }

        public static Func<object, object> GetParentFunc { get; set; }
        public static object GetParent(this object obj)
        {
             
            return GetParentFunc(obj);
        }
    }
}
