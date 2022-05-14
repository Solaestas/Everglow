using Microsoft.VisualStudio.TestTools.UnitTesting;
using Everglow.Sources.Commons.Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.DataStructures.Tests
{
    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        public void PopTest()
        {
            // Pop需要一直能够吐出最小值

            var queue = new PriorityQueue<int>();
            queue.Push(1);
            queue.Push(1);
            queue.Push(4);
            queue.Push(5);
            queue.Push(1);
            queue.Push(4);

            Assert.AreEqual(1, queue.Pop());
            Assert.AreEqual(1, queue.Pop());
            Assert.AreEqual(1, queue.Pop());
            Assert.AreEqual(4, queue.Pop());
            Assert.AreEqual(4, queue.Pop());
            Assert.AreEqual(5, queue.Pop());
        }

        [TestMethod]
        public void EmptyGetTest()
        {
            // 优先队列要能正确处理内容为空的情况

            var queue = new PriorityQueue<int>();
            queue.Push(114514);
            queue.Pop();

            Assert.AreEqual(true, queue.Empty);
            Assert.ThrowsException<IndexOutOfRangeException>(() => { int t = queue.Top; });
            Assert.ThrowsException<IndexOutOfRangeException>(() => { queue.Pop(); });
        }
    }
}