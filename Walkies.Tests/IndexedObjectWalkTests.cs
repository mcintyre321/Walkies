using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Walkies.Tests
{
    public class IndexedObjectWalkTests
    {
        private class TestSubject
        {
            internal object A = new object();
            internal object B = new object();

            public IDictionary<string, object> Dict
            {
                get
                {
                    return new Dictionary<string, object>() {{"itemA", A}, {"itemB", B},};
                }
            }

        }
    
        [Test]
        public void ChildAttributeTest()
        {
            var a = new TestSubject();
            Assert.AreEqual(a.A, a.Walk("dict/itemA").Last());
            Assert.AreEqual(a.B, a.Walk("dict/itemB").Last());

        }
    }
}