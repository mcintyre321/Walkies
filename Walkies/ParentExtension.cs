using System;
using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class PublicParentExtensions
    {
        public static T SetParent<T>(this T obj, object parent, string name)
        {
            obj.SetParent(parent).SetName(name);
            return obj;
        }

        public static T SetParent<T>(this T obj, object parent, Func<T, string> getName)
        {
            return PublicParentExtensions.SetParent(obj, parent, getName(obj));
        }

        public static T SetChild<T>(this T parent, object child, string name)
        {
            child.SetParent(parent).SetName(name);
            return parent;
        }

        public static object Parent(this object obj)
        {
            return obj.GetParent();
        }
    }

    internal static class ParentExtension
    {
        private static readonly ConditionalWeakTable<object, object> parents = new ConditionalWeakTable<object, object>();


        internal static T SetParent<T>(this T obj, object parent)
        {
            parents.Remove(obj);
            parents.Add(obj, parent);
            return obj;
        }

        internal static T SetChild<T>(this T obj, object child)
        {
            child.SetParent(obj);
            return obj;
        }

        public static object GetParent(this object obj)
        {
            var value = parents.GetValue(obj, k => null);
            return value;
        }
    }
}
