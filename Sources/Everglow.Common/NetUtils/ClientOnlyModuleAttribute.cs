namespace Everglow.Common.NetUtils
{
	/// <summary>
	/// 只有单人和多人客户端才会加载拥有该特性的Module
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
	internal class ClientOnlyModuleAttribute : Attribute
	{
	}


}
