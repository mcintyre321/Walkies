using System.Runtime.CompilerServices;

namespace Walkies
{
    public static class WalkableExtension
    {
        private static readonly ConditionalWeakTable<object, object> Walkables = new ConditionalWeakTable<object, object>();

        public static T SetWalkable<T>(this T obj, bool Walkable)
        {
            Walkables.Remove(obj);
            Walkables.Add(obj, Walkable);
            return obj;
        }

        public static bool GetWalkable(this object obj)
        {
            return Walkables.GetValue(obj, c => null as object) as bool? ?? default(bool);
        }

    }
}