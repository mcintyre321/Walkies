using System;
using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class FragmentExtension
    {
        static FragmentExtension()
        {
            GetFragmentRule = obj => fragments.GetValue(obj, c => null);
            SetFragmentRule = (obj, fragment) =>
            {
                fragments.Remove(obj);
                fragments.Add(obj, fragment);
            };
        }

        private static readonly ConditionalWeakTable<object, string> fragments = new ConditionalWeakTable<object, string>();

        public static Action<object, string> SetFragmentRule { get; set; }
        public static T SetFragment<T>(this T obj, string fragment)
        {
            SetFragmentRule(obj, fragment);
            return obj;
        }

        public static Func<object, string> GetFragmentRule { get; set; }
        public static string GetFragment(this object obj)
        {
            return GetFragmentRule(obj);
        }

    }
}