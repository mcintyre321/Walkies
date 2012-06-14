using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public class ScanEnumerable
    {
        public static object Rule(object root, string fragment)
        {
            var enumerable = root as IEnumerable<object>;
            if (enumerable != null && enumerable.GetWalkable())
            {
                return enumerable.FirstOrDefault(i => i.GetFragment() == fragment);
            }
            return null;
        }
    }
}