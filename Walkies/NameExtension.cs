using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class NameExtension
    {
        private static readonly ConditionalWeakTable<object, string> names = new ConditionalWeakTable<object, string>();

        public static T SetName<T>(this T obj, string name)
        {
            names.Remove(obj);
            names.Add(obj, name);
            return obj;
        }

        public static string GetName(this object obj)
        {
            return names.GetValue(obj, c => null);
        }

    }
}