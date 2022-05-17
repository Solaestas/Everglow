using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.Common.Coroutines
{
    /// <summary>
    /// 表示一个标准的协程运行器
    /// </summary>
    public class Coroutine : ICoroutine
    {
        private List<IEnumerator<ICoroutineInstruction>> m_enumerator;
        public Coroutine(IEnumerator<ICoroutineInstruction> enumerator)
        {
            m_enumerator = new List<IEnumerator<ICoroutineInstruction>> { enumerator };
        }

        public bool MoveNext()
        {
            if (m_enumerator.Count == 0)
            {
                return false;
            }

            var currentIE = m_enumerator[^1];
            var instruction = currentIE.Current;

            if (instruction != null)
            {
                if(instruction is AwaitForTask)
                {
                    m_enumerator.Add((instruction as AwaitForTask).Task);
                }
                instruction.Update();

                if (instruction.ShouldWait())
                {
                    return true;
                }
            }

            if (!currentIE.MoveNext())
            {
                m_enumerator.RemoveAt(m_enumerator.Count - 1);
                if (m_enumerator.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
