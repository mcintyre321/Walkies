using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Walkies.Tests
{


    public class HasChildrenInterfaceTests
    {
        public class HasChildren : IHasChildren
        {
            public HasChildren()
            {
                SomeChild = new object();
            }

            IEnumerable<Tuple<string, object>> IHasChildren.Children
            {
                get { yield return Tuple.Create("SomeChild", SomeChild); }
            }

            public object SomeChild { get; set; }
        }

        [Test]
        public void AttributedPropertyCanBeWalkedToByPropertyName()
        {
            var parent = new HasChildren();
            var child = parent.Walk("SomeChild").Last();
            Assert.AreEqual(parent.SomeChild, child);
        }

        ///// <summary>
        ///// Children referenced via IGetChild are not KnownChildren as they cannot be statically found
        ///// </summary>
        //[Test]
        //public void InterfacedChildrenAreNotKnown()
        //{
        //    var p = new HasChildren();
        //    Assert.IsEmpty(p.KnownChildren());
        //}
    }
}
