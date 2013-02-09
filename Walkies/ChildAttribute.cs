using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Walkies
{
    public class ChildAttribute : Attribute
    {
        public bool Walkable { get; set; }
        private readonly string _name;
        public ChildAttribute(){}
        public ChildAttribute(string name)
        {
            _name = name;
        }

        private delegate object Getter(object target, bool walkable, MethodInfo getter);
        static ConcurrentDictionary<Type, ConcurrentDictionary<string, Tuple<bool, MethodInfo, Getter>>> lookup = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Tuple<bool, MethodInfo, Getter>>>(); 
        public static object Rule(object root, string fragment)
        {
            var innerLookup = GetLookupDictionary(root);
            Tuple<bool, MethodInfo, Getter> getter = null;
            return innerLookup.TryGetValue(fragment, out getter) ? getter.Item3(root, getter.Item1, getter.Item2) : null;
        }
        public static IEnumerable<Tuple<string, object>> ChildrenRule(object root)
        {
            if (true /*root.GetNotWalkable() == false*/)
            {
                    foreach(var item in GetLookupDictionary(root))
                    {
                        var fragment = item.Key;
                        var getMethodInfo = item.Value.Item2;
                        var walkable = item.Value.Item1;
                        var getter = item.Value.Item3;
                        yield return Tuple.Create(fragment, getter(root, walkable, getMethodInfo).SetParent(root));
                    }
            }
        }

        private static ConcurrentDictionary<string, Tuple<bool, MethodInfo, Getter>> GetLookupDictionary(object parent)
        {
            var type = parent.GetType();
            var innerLookup = lookup.GetOrAdd(type, t =>
            {
                var cd = new ConcurrentDictionary<string, Tuple<bool, MethodInfo, Getter>>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var tuple in MakeFunc(type))
                {
                    cd[tuple.Item1] = tuple.Item2;
                }
                return cd;
            });
            return innerLookup;
        }


        static IEnumerable<Tuple<string, Tuple<bool, MethodInfo, Getter>>> MakeFunc(Type type)
        {
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                ChildAttribute att = pi.GetCustomAttributes(typeof (ChildAttribute), true).Cast<ChildAttribute>().SingleOrDefault();
                if (att != null)
                {
                    MethodInfo getMethod = pi.GetGetMethod();
                    if (getMethod != null)
                    {
                        var walkable = att.Walkable;
                        var item1 = att._name ?? pi.Name;
                        var getter = new Getter((obj, w, mi) => Invoke(obj, mi, w));
                        yield return Tuple.Create(item1, Tuple.Create(walkable, getMethod, getter));
                    }
                }
            }
        }

        private static object Invoke(object obj, MethodInfo getMethod, bool walkable)
        {
            var result = getMethod.Invoke(obj, null);
            /*if (result != null && !walkable) result.SetNotWalkable(true);*/
            return result;
        }
    }
}