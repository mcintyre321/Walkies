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
                InnerObjects2 = new Hashtable() { { "E", B }, { "F", C } };
            }

            public Hashtable InnerObjects2 { get; set; }

            public object B { get; set; }


            public object C { get; set; }

            public object D { get; set; }

            [Child(Walkable = true)]
            public Hashtable Objects
            {
                get { return InnerObjects; }
            }

            //[Child]
            //public Hashtable Objects2
            //{
            //    get { return InnerObjects2; }
            //}



        }
        [Test]
        public void CanWalkWalkableTwice()
        {
            var a = new TestSubject();
            Assert.AreEqual(a.Objects, a.Walk("objects").Last());
            Assert.AreEqual(a.Objects, a.Walk("objects").Last());
        }

        [Test]
        public void CanWalkWalkable()
        {
            var a = new TestSubject();
            Assert.AreEqual(a.Objects, a.Walk("objects").Last());
            Assert.AreEqual(a.Objects, a.Walk("objects").Last());

            Assert.AreEqual(a.B, a.Walk("objects/B").Last());
            Assert.That("objects/B", Is.EqualTo(a.B.WalkedPath("/")).IgnoreCase);
            
            Assert.AreEqual(a.C, a.Walk("objects/C").Last());

        }
        //[Test]
        //public void CannotWalkUnwalkableEnumerable()
        //{
        //    var a = new TestSubject();
        //    Assert.AreEqual(a.Objects2, a.Walk("objects2").Last());

        //    Assert.AreEqual(null, a.Walk("objects2/B").Last());
        //    Assert.AreEqual(null, a.Walk("objects2/E").Last());
        //    Assert.AreEqual(null, a.Walk("objects2/5").Last());

        //}

    }
}