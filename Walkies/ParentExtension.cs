using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class ParentExtension
    {
        private static readonly ConditionalWeakTable<object, object> parents = new ConditionalWeakTable<object, object>();

        public static T SetParent<T>(this T obj, object parent)
        {
            parents.Remove(obj);
            parents.Add(obj, parent);
            return obj;
        }

        public static T SetChild<T>(this T obj, object child)
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
