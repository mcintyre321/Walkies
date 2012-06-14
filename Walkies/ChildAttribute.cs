using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Walkies
{
    public class ChildAttribute : Attribute
    {
        public string Name { get; set; }
        public ChildAttribute(){}
        public ChildAttribute(string name)
        {
            Name = name;
        }

        private delegate object Getter(object target);
        static ConcurrentDictionary<Type, ConcurrentDictionary<string, Getter>> lookup = new ConcurrentDictionary<Type,ConcurrentDictionary<string,Getter>>(); 
        public static object Rule(object root, string fragment)
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
            Getter getter = null;
            return innerLookup.TryGetValue(fragment, out getter) ? getter(root) : null;
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
                select Tuple.Create(att.Name ?? pi.Name, new Getter(obj => getMethod.Invoke(obj, null)));
        }
    }
}