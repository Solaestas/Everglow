namespace Everglow.Commons.Mechanics.MissionSystem.Enums;

/// <summary>
/// 任务池类型
/// <list type="table">
///     <item>Acceptd: 已接取</item>
///     <item>Available: 可接取</item>
///     <item>Completed: 已完成</item>
///     <item>Overdue: 已过期</item>
///     <item>Failed: 已失败</item>
/// </list>
/// </summary>
public enum PoolType
{
	/// <summary>
	/// 已经被接取的任务池
	/// </summary>
	Accepted,

	/// <summary>
	/// 可以被接取的任务池
	/// </summary>
	Available,

	/// <summary>
	/// 任务完成且已领取奖励的任务池
	/// </summary>
	Completed,

	/// <summary>
	/// 逾期未完成的任务池
	/// </summary>
	Overdue,

	/// <summary>
	/// 任务失败的任务池
	/// </summary>
	Failed,
}
