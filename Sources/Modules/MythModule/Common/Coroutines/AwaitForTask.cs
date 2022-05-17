using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.Common.Coroutines
{
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
			return true;
		}

		public void Update() 
		{
			
		}
	}
}
