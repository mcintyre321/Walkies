using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Walkies.Tests
{
    public class TraversalTests
    {
        public class Node3
        {
        }


        public class Node
        {
            private Node1 _node1 = new Node1();

            [Child]
            public Node1 One
            {
                get
                {
                    return _node1;
                }
            }
        }

        public class Node1
        {
            private Node2 _node2 = new Node2();

            [Child("Two")]
            public Node2 Child
            {
                get
                {
                    return _node2;
                }
            }


        }

        public class Node2
        {
            private Node3 _node3 = new Node3();

            [Child]
            public Node3 Three
            {
                get
                {
                    return _node3;
                }
            }

            
        }
        [Test]
        public void TestWalk()
        {
            var node = new Node();
            Assert.AreEqual(node.One.Child.Three, node.Walk("one/two/three").Last());
        }

        [Test]
        public void TestPath()
        {
            var node = new Node();
            Assert.AreEqual("one,two,three", string.Join(",", node.Walk("one/two/three").Last().Path()));
        }
    }
}