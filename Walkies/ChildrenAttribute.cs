using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Walkies
{
    public class ChildrenAttribute : Attribute
    {
        static ConcurrentDictionary<Type, Func<object, IEnumerable<object>>> lookup = new ConcurrentDictionary<Type, Func<object, IEnumerable<object>>>();  
        public static IEnumerable<Tuple<string, object>> ChildrenRule(object root)
        {
            var getValues = lookup.GetOrAdd(root.GetType(), t =>
            {
                var properties = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(p => new { Property = p, Att = p.GetCustomAttributes(typeof(ChildrenAttribute), false).SingleOrDefault() })
                    .Where(p => p.Att != null)
                    .Select(p => new Func<object, IEnumerable<object>>(o => p.Property.GetValue(o, null) as IEnumerable<object> ?? Enumerable.Empty<object>()));

                var fields = t.GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance)
                    .Select(p => new{ Field = p, Att = p.GetCustomAttributes(typeof(ChildrenAttribute), false).SingleOrDefault()})
                    .Where(p => p.Att != null)
                    .Select(p => new Func<object, IEnumerable<object>>(o => p.Field.GetValue(o) as IEnumerable<object> ?? Enumerable.Empty<object>()));


                return (o => properties.SelectMany(p => p(o)).Concat(fields.SelectMany(f => f(o))));
            });
            
            var children = getValues(root);
            children = children.Select(c => GenerateFragmentIfNeeded(root, c));
            return children.Select(c => Tuple.Create(c.GetFragment(), c)).ToArray();
        }

        static object GenerateFragmentIfNeeded(object parent, object o)
        {
            if (o.GetFragment() == null) o.SetFragment(Guid.NewGuid().ToString());
            if (o.GetParent() == null) o.SetParent(parent);
            return o;
        }
    }
}