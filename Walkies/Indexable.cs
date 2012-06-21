using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public class Indexable
    {
        public static object Rule(object parent, string fragment)
        {
            if (parent.GetNotWalkable()) return null;
            var indexer = parent.GetType().GetProperty("Item");
            var getter = indexer != null ? indexer.GetGetMethod(true) : null;
            if (getter != null)
            {
                return getter.Invoke(parent, new object[] {fragment});
            }
            return null;
        }
    }
}