namespace Walkies
{
    public interface IGetChild
    {
        object this[string fragment] { get; }
    }

    public class GetChild
    {
        public static object Rule(object root, string fragment)
        {
            var hasChildren = root as IGetChild;
            if (hasChildren != null)
            {
                return hasChildren[fragment];
            }
            return null;
        }
    }
}