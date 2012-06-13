using System.Collections.Generic;

namespace Walkies.Tests
{
    public class A : IGetChild, IHasChildren
    {
        public A()
        {
            B = new object();
            C = new object();
            D = new object();
        }

        [Child]
        public object B { get; set; }

        public object C { get; set; }

        public object D { get; set; }

        public object this[string fragment]
        {
            get { return (fragment.ToLowerInvariant() == "c") ? C : null; }
        }

        public IEnumerable<object> Children
        {
            get { yield return D.SetName("d"); }
        }
    }
}