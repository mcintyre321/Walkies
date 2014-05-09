using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Walkies
{
    public class OnWalkRules
    {
        static OnWalkRules()
        {
            Rules = new List<OnWalkRule>();
        }

        public static List<OnWalkRule> Rules = new List<OnWalkRule>();

        private static OnWalkRule IHandlesOnWalkRule = (parent, fragment, child) =>
        {
            if (child is IHandlesOnWalk)
            {
                return ((IHandlesOnWalk) child).OnWalk(parent, fragment);
            }
            return null;
        };
    }

    public interface IHandlesOnWalk
    {
        object OnWalk(object parent, string fragment);
    }

    public delegate object OnWalkRule(object parent, string fragment, object child);
}