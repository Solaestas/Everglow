using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.Common.Coroutines
{
	/// <summary>
	/// 指示协程机等待一个新的任务完成以后再继续执行（相当于同步函数调用）
	/// </summary>
	public class AwaitForTask : ICoroutineInstruction
	{
		public IEnumerator<ICoroutineInstruction> Task
        {
            get
            {
				return m_task;
            }
        }
		private IEnumerator<ICoroutineInstruction> m_task;
		public AwaitForTask(IEnumerator<ICoroutineInstruction> task)
		{
			m_task = task;
		}

		public bool ShouldWait()
		{
			return false;
		}

		public void Update() 
		{
			
		}
	}
}
