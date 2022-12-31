﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Coroutines
{
    /// <summary>
    /// 管理所有协程运行器的类，支持异步地发起新的协程
    /// </summary>
    public class CoroutineManager
    {
        private LinkedList<ICoroutine> m_coroutines;
        private List<ICoroutine> m_coroutinesToStop;

        public CoroutineManager()
        {
            m_coroutines = new LinkedList<ICoroutine>();
        }

        public void StartCoroutine(ICoroutine coroutine)
        {
            m_coroutines.AddLast(coroutine);
        }

        public void Update()
        {
            var node = m_coroutines.First;

            while (node != null)
            {
                var nextNode = node.Next;

                var current = node.Value;
                bool finished = false;
                if (current != null)
                {
                    finished = !current.MoveNext();
                }

                if (finished)
                {
                    m_coroutines.Remove(node);
                }

                node = nextNode;
            }
        }
    }
}
