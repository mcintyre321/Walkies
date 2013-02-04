using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Walkies.Tests
{
  

    public class ChildrenAttributeTests
    {
        public class SomeCollection
        {
            [Children] internal List<object> children = new List<object>()
            {
                new object().SetFragment("first"),
                new object()
            };
        }

        [Test]
        public void CanBeWalkedToByName()
        {
            var parent = new SomeCollection();
            var child = parent.Walk("first").Last();
            Assert.AreEqual(parent.children.First(), child);
        }

        [Test]
        public void WillGenerateNameForUnnamed()
        {
            var parent = new SomeCollection();
            var knownChildren = parent.KnownChildren().Skip(1).First();

            var generatedFragment = parent.children[1].GetFragment();
            Assert.False(string.IsNullOrEmpty(generatedFragment));

        }

        [Test]
        public void WalkedChildrenHaveCorrectParent()
        {
            var parent = new SomeCollection();
            var child = parent.WalkTo("first");
            Assert.AreEqual(parent, child.Parent());
        }
        [Test]
        public void KnownChildrenHaveCorrectParent()
        {
            var parent = new SomeCollection();
            var child = parent.KnownChildren().Skip(1).First();
            Assert.AreEqual(parent, child.Parent());
        }

    }
}
