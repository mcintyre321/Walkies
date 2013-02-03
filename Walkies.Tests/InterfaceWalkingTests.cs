using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Walkies.Tests
{
    public class InterfaceWalkingTests
    {
        public class ClassWithInterfacedChild : IGetChild
        {
            public ClassWithInterfacedChild()
            {
                SomeChild = new object();
            }
            public object SomeChild { get; set; }
            object IGetChild.this[string fragment]
            {
                get { return (fragment.ToLowerInvariant() == "somechild") ? SomeChild : null; }
            }
        }

        [Test]
        public void GetChildTest()
        {
            var p = new ClassWithInterfacedChild();
            var c = p.Walk("SomeChild").Last();
            Assert.AreEqual(p.SomeChild, c);
        }

        /// <summary>
        /// Children referenced via IGetChild are not KnownChildren as they cannot be statically found
        /// </summary>
        [Test]
        public void InterfacedChildrenAreNotKnown()
        {
            var p = new ClassWithInterfacedChild();
            Assert.IsEmpty(p.KnownChildren());
        }

    }
}