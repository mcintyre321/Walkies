using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Walkies
{
    internal static class ReflectionExtensions
    {
        public static object GetValue(this MemberInfo mi, object target)
        {
            if (mi is FieldInfo)
                return ((FieldInfo)mi).GetValue(target);
            else
                return ((PropertyInfo)mi).GetValue(target, null);        
        }
    }

    class AttributeLookupCache<TAttribute> where TAttribute : Attribute
    {
        static ConcurrentDictionary<Type, IEnumerable<MemberInfo>> lookup = new ConcurrentDictionary<Type, IEnumerable<MemberInfo>>();

        public static IEnumerable<MemberInfo> Members(Type type)
        {
            var members = lookup.GetOrAdd(type, t => t
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => Attribute.GetCustomAttribute(p, typeof(TAttribute), true) != null));
            return members;
        }

        public static MemberInfo Member(Type t)
        {
            return Members(t).SingleOrDefault();
        }

    }
}