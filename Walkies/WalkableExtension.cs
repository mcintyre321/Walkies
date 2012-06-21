using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class WalkableExtension
    {
        private static readonly ConditionalWeakTable<object, object> NotWalkableLookup = new ConditionalWeakTable<object, object>();

        public static T SetNotWalkable<T>(this T obj, bool notWalkable)
        {
            NotWalkableLookup.Remove(obj);
            NotWalkableLookup.Add(obj, notWalkable);
            return obj;
        }

        public static bool GetNotWalkable(this object obj)
        {
            return NotWalkableLookup.GetValue(obj, c => null as object) as bool? ?? default(bool);
        }

    }
}