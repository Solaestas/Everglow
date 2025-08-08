using Everglow.Commons.Netcode.Abstracts;

namespace Everglow.Commons.Netcode.PacketHandle;

/// <summary>
/// 定义特定类型网络封包的处理逻辑
/// </summary>
public interface IPacketHandler
{
	/// <summary>
	/// 处理接收到的网络封包
	/// </summary>
	/// <param name="packet"></param>
	/// <param name="whoAmI">发送来源玩家ID. 由服务器转发时为对应玩家ID，来源为服务器时为-1</param>
	public void Handle(IPacket packet, int whoAmI);
}

/// <summary>
/// 用于指定一个IPacketHandler需要处理的IPacket类型，初步认定一个Handler只处理一种封包
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class HandlePacketAttribute : Attribute
{
	private Type packetType;

	public HandlePacketAttribute(Type type)
	{
		packetType = type;
		Debug.Assert(typeof(IPacket).IsAssignableFrom(type));
	}

	public Type PacketType
	{
		get
		{
			return packetType;
		}
	}
}