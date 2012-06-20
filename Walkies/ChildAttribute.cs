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

        private delegate object Getter(object target);
        static ConcurrentDictionary<Type, ConcurrentDictionary<string, Getter>> lookup = new ConcurrentDictionary<Type,ConcurrentDictionary<string,Getter>>(); 
        public static object Rule(object root, string fragment)
        {
            var innerLookup = GetLookupDictionary(root);
            Getter getter = null;
            return innerLookup.TryGetValue(fragment, out getter) ? getter(root) : null;
        }
        public static IEnumerable<Tuple<string, object>> ChildrenRule(object root)
        {
            return GetLookupDictionary(root).Select(pair => Tuple.Create(pair.Key, pair.Value(root)));
        }

        private static ConcurrentDictionary<string, Getter> GetLookupDictionary(object root)
        {
            var type = root.GetType();
            var innerLookup = lookup.GetOrAdd(type, t =>
            {
                var cd = new ConcurrentDictionary<string, Getter>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var tuple in MakeFunc(type))
                {
                    cd[tuple.Item1] = tuple.Item2;
                }
                return cd;
            });
            return innerLookup;
        }


        static IEnumerable<Tuple<string, Getter>> MakeFunc(Type type)
        {
            return
                from pi in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                let att =
                    pi.GetCustomAttributes(typeof (ChildAttribute), true).Cast<ChildAttribute>().SingleOrDefault()
                where att != null
                let getMethod = pi.GetGetMethod()
                where getMethod != null
                select Tuple.Create(att._name ?? pi.Name, new Getter(obj => Invoke(obj, getMethod, att.Walkable)));
        }

        private static object Invoke(object obj, MethodInfo getMethod, bool walkable)
        {
            var result = getMethod.Invoke(obj, null);
            if (result != null && walkable) result.SetWalkable(true);
            return result;
        }
    }
}