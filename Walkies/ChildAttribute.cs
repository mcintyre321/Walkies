using System;
using System.Linq;
using System.Reflection;

namespace Walkies
{
    public class ChildAttribute : Attribute
    {
        public static object Rule(object root, string fragment)
        {
            var property = root.GetType().GetProperty(fragment, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (property != null && property.GetGetMethod() != null && property.GetCustomAttributes(typeof(ChildAttribute), true).Any())
            {
                return property.GetGetMethod(true).Invoke(root, null);
            }
            return null;
        }
    }
}