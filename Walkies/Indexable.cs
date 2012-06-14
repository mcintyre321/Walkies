using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public class Indexable
    {
        public static object Rule(object root, string fragment)
        {
            if (!root.GetWalkable()) return null;
            var indexer = root.GetType().GetProperty("Item");
            var getter = indexer != null ? indexer.GetGetMethod(true) : null;
            if (getter != null)
            {
                return getter.Invoke(root, new object[] {fragment});
            }
            return null;
        }
    }
}