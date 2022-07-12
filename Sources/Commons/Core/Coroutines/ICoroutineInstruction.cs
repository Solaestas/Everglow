using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.MythModule.Common.Coroutines
{
	/// <summary>
	/// 协程的控制指令
	/// </summary>
	public interface ICoroutineInstruction
	{
		void Update();
		bool ShouldWait();
	}
}
