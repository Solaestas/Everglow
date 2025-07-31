namespace Everglow.Commons.FeatureFlags;

public static class CompileTimeFeatureFlags
{
	/// <summary>
	/// 表示封包ID的表示是否要使用Int32，如果关闭则使用byte，可以节省一点带宽
	/// 但是之后封包数量多了byte就表示不下了
	/// </summary>
	public static readonly bool NetworkPacketIDUseInt32 = true;

	/// <summary>
	/// 是否显示ExampleModule的UI
	/// </summary>
	public static readonly bool ShowExampleUI = true;
}