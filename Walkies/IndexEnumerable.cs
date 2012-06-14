using System.Collections.Generic;
using System.Linq;

namespace Walkies
{
    public class IndexEnumerable
    {
        public static object Rule(object root, string fragment)
        {
            var enumerable = root as IEnumerable<object>;
            var i = -1;
            if (enumerable != null && enumerable.GetWalkable() && int.TryParse(fragment, out i))
            {
                return enumerable.Skip(i).FirstOrDefault();
            }
            return null;
        }
    }
}