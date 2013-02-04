using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Walkies.Tests
{
    

    public class AttributedWalkingTests
    {
        public class ObjectWithAttributedChild
        {
            public ObjectWithAttributedChild()
            {
                SomeChild = new object();
            }
            [Child]
            public object SomeChild { get; set; }
        }

        [Test]
        public void AttributedPropertyCanBeWalkedToByPropertyName()
        {
            var parent = new ObjectWithAttributedChild();
            var child = parent.Walk("SomeChild").Last();
            Assert.AreEqual(parent.SomeChild, child);
        }

        [Test]
        public void AttributedPropertiesAreKnownChildren()
        {
            var parent = new ObjectWithAttributedChild();
            var knownChild = parent.KnownChildrenWithFragments().Single();
            Assert.AreEqual("SomeChild", knownChild.Item1);
            Assert.AreEqual(parent.SomeChild, knownChild.Item2);
        }

        [Test]
        public void AttributedPropertiesHaveCorrectParent()
        {
            var parent = new ObjectWithAttributedChild();
            var knownChild = parent.KnownChildrenWithFragments().Single();
            Assert.AreEqual(parent, knownChild.Item2.Parent());
        }
        [Test]
        public void WalkedAttributedPropertiesHaveCorrectParent()
        {
            var parent = new ObjectWithAttributedChild();
            var child = parent.WalkTo("SomeChild");
            Assert.AreEqual(parent, child.Parent());
        }


    }
}
