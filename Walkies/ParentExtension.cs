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

        public static T GetParent<T>(this T obj)
        {
            var value = parents.GetValue(obj, k => null);
            return value == null ? default(T) : (T) value;
        }


    }
}
