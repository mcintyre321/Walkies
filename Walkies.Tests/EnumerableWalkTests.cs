using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Walkies.Tests
{
    [TestFixture]
    public class EnumerableWalkTests
    {
        public class TestSubject 
        {
            public TestSubject()
            {
                B = new object().SetFragment("0");
                C = new object().SetFragment("1");
                D = new object().SetFragment("2");
            }

            public object B { get; set; }

      
            public object C { get; set; }

            public object D { get; set; }

            [Child(Walkable = true)]
            public IEnumerable<object> Objects
            {
                get
                {
                    yield return B;
                    yield return C;
                    yield return D;
                }
            }

            [Child]
            public IEnumerable<object> Objects2
            {
                get
                {
                    yield return B;
                    yield return C;
                    yield return D;
                }
            }



        }
        [Test]
        public void CanWalkWalkableEnumerable()
        {
            var a = new TestSubject();
            var objects = a.Walk("objects").Last();
            Assert.AreEqual("objects", objects.WalkedPath("/"));

            Assert.AreEqual(a.B, a.Walk("objects/0").Last());
            Assert.AreEqual("objects/0", a.B.WalkedPath("/"));

            Assert.AreEqual(a.C, a.Walk("objects/1").Last());
            Assert.AreEqual("objects/1", a.C.WalkedPath("/"));
            
            Assert.AreEqual(null, a.Walk("objects/5").Last());

        }
        [Test]
        public void CannotWalkUnwalkableEnumerable()
        {
            var a = new TestSubject();
            Assert.AreEqual(a.Objects2, a.Walk("objects2").Last());

            Assert.AreEqual(null, a.Walk("objects2/0").Last());
            Assert.AreEqual(null, a.Walk("objects2/1").Last());
            Assert.AreEqual(null, a.Walk("objects2/5").Last());

        }

    }
}