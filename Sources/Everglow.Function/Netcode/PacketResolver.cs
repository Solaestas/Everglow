using Everglow.Commons.FeatureFlags;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Netcode.PacketHandle;
using Everglow.Commons.Utilities;
using Packet_ID = System.Int32;

#pragma warning disable SA1121 // Use built-in type alias

namespace Everglow.Commons.Netcode;

/// <summary>
/// 用于管理封包发送、接收的类
/// </summary>
public class PacketResolver
{
	private Mod _mod;
	private Dictionary<Packet_ID, List<IPacketHandler>> packetHandlerRegistry;
	private Dictionary<Type, Packet_ID> packetIDMapping;
	private Dictionary<Packet_ID, Type> packetIDToTypeMapping;
	private Packet_ID packetIDCounter;

	/// <summary>
	/// 用于初始化所有需要监听的 Packet 类型和监听器
	/// </summary>
	public PacketResolver(Mod mod)
	{
		packetIDCounter = 0;
		packetIDMapping = [];
		packetIDToTypeMapping = [];
		packetHandlerRegistry = [];

		_mod = mod;
		RegisterPackets();
	}

	/// <summary>
	/// 查询某个封包类型对应的封包ID，如果不存在则返回-1
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public int QueryPacketID<T>()
		where T : IPacket
	{
		var type = typeof(T);
		if (packetIDMapping.TryGetValue(type, out int packetID))
		{
			return packetID;
		}
		throw new ArgumentException($"Packet type {type.Name} does not exist.");
	}

	/// <summary>
	/// 向指定对象发送一个封包数据的实例
	/// <br/>除特殊情况外，请尽可能使用封装版本<see cref="Send(IPacket, bool, Player)"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="packet"></param>
	/// <param name="toClient"></param>
	/// <param name="ignoreClient"></param>
	public void Send(IPacket packet, int toClient = -1, int ignoreClient = -1)
	{
		// 单人模式不要有任何动作
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			return;
		}

		var modPacket = GetPacket();
		using (MemoryStream ms = new())
		{
			// 写入来源玩家ID
			if (NetUtils.IsServer)
			{
				modPacket.Write(ignoreClient);
			}
			else
			{
				modPacket.Write(Main.myPlayer);
			}

			// 写入封包ID
			int id = packetIDMapping[packet.GetType()];
			if (CompileTimeFeatureFlags.NetworkPacketIDUseInt32)
			{
				modPacket.Write(id);
			}
			else
			{
				modPacket.Write((byte)id);
			}

			// 写入封包数据
			BinaryWriter bw = new(ms);
			packet.Send(bw);
			modPacket.Write(ms.GetBuffer(), 0, (int)ms.Position);
			modPacket.Flush();
		}

		// 二次检测，如果是单人模式则不发送
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			return;
		}

		modPacket.Send(toClient, ignoreClient);
	}

	/// <summary>
	/// 向指定对象发送一个封包数据的实例
	/// <br/> <see cref="Send(IPacket, Packet_ID, Packet_ID)"/>的封装版本，自动填充发送对象
	/// </summary>
	/// <param name="packet"></param>
	/// <param name="fromServer"></param>
	/// <param name="player"></param>
	public void Send(IPacket packet, bool fromServer, Player player)
	{
		if (fromServer)
		{
			Send(packet, -1, player.whoAmI);
		}
		else
		{
			Send(packet);
		}
	}

	/// <summary>
	/// 处理封包
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="whoAmI"></param>
	public void Resolve(BinaryReader reader, int _)
	{
		// 读取来源玩家ID
		var sourcePlayer = reader.ReadInt32();

		// 读取封包ID
		Packet_ID packetID;
		if (CompileTimeFeatureFlags.NetworkPacketIDUseInt32)
		{
			packetID = reader.ReadInt32();
		}
		else
		{
			packetID = reader.ReadByte();
		}

		if (!packetHandlerRegistry.TryGetValue(packetID, out List<IPacketHandler> registeredHandlers))
		{
			Ins.Logger.Warn($"Received a packet [{packetID}] without handler, automatically ignored");
			return;
		}

		// 读取封包数据
		var packet = Activator.CreateInstance(packetIDToTypeMapping[packetID]) as IPacket;
		packet.Receive(reader, sourcePlayer);

		// 调用Handlers处理封包数据
		foreach (var handler in registeredHandlers)
		{
			handler.Handle(packet, sourcePlayer);
		}
	}

	/// <summary>
	/// 注册所有<see cref="IPacket"/>和<see cref="IPacketHandler"/>的实现类型.
	/// </summary>
	private void RegisterPackets()
	{
		var modTypes = Ins.ModuleManager.Types.Where(type => !type.IsAbstract);
		foreach (var type in modTypes.Where(type => type.IsAssignableTo(typeof(IPacket))))
		{
			if (!packetIDMapping.ContainsKey(type))
			{
				packetIDMapping.Add(type, packetIDCounter);
				packetIDToTypeMapping.Add(packetIDCounter, type);
				packetIDCounter++;
			}
		}

		foreach (var type in modTypes.Where(type => type.IsAssignableTo(typeof(IPacketHandler))))
		{
			// 将 packet 和 PacketHandler 绑定
			if (Attribute.GetCustomAttribute(type, typeof(HandlePacketAttribute)) is HandlePacketAttribute handlePacket)
			{
				if (!packetIDMapping.TryGetValue(handlePacket.PacketType, out int packetID))
				{
					throw new InvalidOperationException("Unknown packet type");
				}

				var handler = Activator.CreateInstance(type) as IPacketHandler;
				if (packetHandlerRegistry.TryGetValue(packetID, out List<IPacketHandler> registeredHandlers))
				{
					registeredHandlers.Add(handler);
				}
				else
				{
					packetHandlerRegistry.Add(packetID, [handler]);
				}
			}
			else
			{
				Ins.Logger.Warn($"Packet Handler {type} does not bind to any packet");
			}
		}

		// 如果有封包没有绑定任何handler就发出警告
		foreach (var packetID in packetIDToTypeMapping)
		{
			if (!packetHandlerRegistry.TryGetValue(packetID.Key, out var registeredHandlers) || registeredHandlers.Count == 0)
			{
				Ins.Logger.Warn($"Packet {packetID.Value} does not have any handler binded");
			}
		}
	}

	private ModPacket GetPacket()
	{
		return _mod.GetPacket();
	}
}

#pragma warning restore SA1121 // Use built-in type alias