using System.Collections;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Walkies.Tests
{
    public class Tests
    {
        [Test]
        public void ChildAttributeTest()
        {
            var a = new A();
            var b = a.Walk("B").Last();
            Assert.AreEqual(a.B, b);
        }

        [Test]
        public void GetChildTest()
        {
            var a = new A();
            var c = a.Walk("C").Last();
            Assert.AreEqual(a.C, c);
        }

        [Test]
        public void HasChildrenTest()
        {
            var a = new A();
            var d = a.Walk("d").Last();
            Assert.AreEqual(a.D, d);
        }
    }
}
