using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class FragmentExtension
    {
        private static readonly ConditionalWeakTable<object, string> fragments = new ConditionalWeakTable<object, string>();

        public static T SetFragment<T>(this T obj, string fragment)
        {
            fragments.Remove(obj);
            fragments.Add(obj, fragment);
            return obj;
        }

        public static string GetFragment(this object obj)
        {
            return fragments.GetValue(obj, c => null);
        }

    }
}