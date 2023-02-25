namespace Everglow.Commons.Interfaces;

public interface IMainThreadContext
{
	/// <summary>
	/// 添加一个action在主线程执行
	/// </summary>
	/// <param name="action">准备执行的委托</param>
	public void AddTask(Action action);
}
