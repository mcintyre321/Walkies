using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Walkies.Tests
{
    [TestFixture]
    public class IndexableWalkTests
    {
        public class TestSubject
        {
            internal Hashtable InnerObjects { get; set; }

            public TestSubject()
            {
                B = new object();
                C = new object();
                D = new object();
                InnerObjects = new Hashtable() { { "B", B }, { "C", C } };
            }

            public object B { get; set; }


            public object C { get; set; }

            public object D { get; set; }

            [Child(Walkable = true)]
            public Hashtable Objects
            {
                get { return InnerObjects; }
            }

            [Child]
            public Hashtable Objects2
            {
                get { return InnerObjects; }
            }



        }
        [Test]
        public void CanWalkWalkable()
        {
            var a = new TestSubject();
            Assert.AreEqual(a.Objects, a.Walk("objects").Last());

            Assert.AreEqual(a.B, a.Walk("objects/B").Last());
            Assert.AreEqual("objects/B", a.B.WalkPath("/"));
            
            Assert.AreEqual(a.C, a.Walk("objects/C").Last());

        }
        [Test]
        public void CannotWalkUnwalkableEnumerable()
        {
            var a = new TestSubject();
            Assert.AreEqual(a.Objects2, a.Walk("objects2").Last());

            Assert.AreEqual(null, a.Walk("objects2/B").Last());
            Assert.AreEqual(null, a.Walk("objects2/C").Last());
            Assert.AreEqual(null, a.Walk("objects2/5").Last());

        }

    }
}