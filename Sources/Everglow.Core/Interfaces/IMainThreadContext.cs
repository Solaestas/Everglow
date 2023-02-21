using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Core.Interfaces;

public interface IMainThreadContext
{
	/// <summary>
	/// 添加一个action在主线程执行
	/// </summary>
	/// <param name="action">准备执行的委托</param>
	public void AddTask(Action action);
}
