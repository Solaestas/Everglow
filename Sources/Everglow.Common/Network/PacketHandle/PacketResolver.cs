namespace Everglow.Common.Network.PacketHandle;

using System.Reflection;
using Everglow.Common.FeatureFlags;
using Terraria.ID;
using Packet_Id = Int32;

/// <summary>
/// 用于管理封包发送、接收的类
/// </summary>
public class PacketResolver
{
	private Mod _mod;
	private Dictionary<Packet_Id, List<IPacketHandler>> packetHandlers;
	private Dictionary<Type, Packet_Id> packetIDMapping;
	private Dictionary<Packet_Id, Type> packetIDToTypeMapping;
	private Packet_Id packetIDCounter;

	/// <summary>
	/// 用于初始化所有需要监听的 Packet 类型和监听器
	/// </summary>
	public PacketResolver(Mod mod)
	{
		packetIDCounter = 0;
		packetIDMapping = new Dictionary<Type, Packet_Id>();
		packetIDToTypeMapping = new Dictionary<Packet_Id, Type>();
		packetHandlers = new Dictionary<Packet_Id, List<IPacketHandler>>();

		_mod = mod;
		RegisterPackets();
	}

	/// <summary>
	/// 发送一个封包数据的实例，并且指定发送方式
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
			BinaryWriter bw = new(ms);
			int id = packetIDMapping[packet.GetType()];
			packet.Send(bw);
			if (CompileTimeFeatureFlags.NetworkPacketIDUseInt32)
			{
				modPacket.Write(id);
			}
			else
			{
				modPacket.Write((byte)id);
			}
			modPacket.Write(ms.GetBuffer(), 0, (int)ms.Position);
			modPacket.Flush();
		}
		modPacket.Send(toClient, ignoreClient);
	}

	/// <summary>
	/// 查询某个封包类型对应的封包ID，如果不存在则返回-1
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public int QueryPacketId<T>()
		where T : IPacket
	{
		var type = typeof(T);
		if (packetIDMapping.ContainsKey(type))
		{
			return packetIDMapping[type];
		}
		throw new ArgumentException("不存在的Packet类型");
	}

	/// <summary>
	/// 处理封包
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="whoAmI"></param>
	public void Resolve(BinaryReader reader, int whoAmI)
	{
		Packet_Id packetID;

		// 首先读取封包ID
		if (CompileTimeFeatureFlags.NetworkPacketIDUseInt32)
		{
			packetID = reader.ReadInt32();
		}
		else
		{
			packetID = reader.ReadByte();
		}
		if (!packetHandlers.ContainsKey(packetID))
		{
			Ins.Logger.Warn("Received a packet without handler, automatically ignored");
			return;
		}

		// 直接从reader里构造packet数据
		IPacket packet = Activator.CreateInstance(packetIDToTypeMapping[packetID]) as IPacket;
		packet.Receive(reader, whoAmI);

		// 让handler处理封包数据
		foreach (var handler in packetHandlers[packetID])
		{
			handler.Handle(packet, whoAmI);
		}
	}

	/// <summary>
	/// 注册所有IPacket类型和IPacketHandler
	/// </summary>
	private void RegisterPackets()// 命名改为首字母大写
	{
		var assembly = Assembly.GetExecutingAssembly();
		foreach (var type in assembly.GetTypes().Where(type =>
			 !type.IsAbstract &&
			 type.GetInterfaces().Contains(typeof(IPacket))))
		{
			if (!packetIDMapping.ContainsKey(type))
			{
				packetIDMapping.Add(type, packetIDCounter);
				packetIDToTypeMapping.Add(packetIDCounter, type);
				packetIDCounter++;
			}
		}

		foreach (var type in assembly.GetTypes().Where(type =>
			 !type.IsAbstract &&
			 type.GetInterfaces().Contains(typeof(IPacketHandler))))
		{
			// 将 packet 和 PacketHandler 绑定
			if (Attribute.GetCustomAttribute(type, typeof(HandlePacketAttribute)) is HandlePacketAttribute handlePacket)
			{
				Type packetType = handlePacket.PacketType;
				if (!packetIDMapping.ContainsKey(packetType))
				{
					throw new InvalidOperationException("Unknown packet type");
				}

				// 获取封包类型的对应ID，并且将handler绑定上去
				Packet_Id packetId = packetIDMapping[packetType];
				IPacketHandler handler = Activator.CreateInstance(type) as IPacketHandler;
				if (packetHandlers.ContainsKey(packetId))
				{
					packetHandlers[packetId].Add(handler);
				}
				else
				{
					packetHandlers.Add(packetId, new List<IPacketHandler> { handler });
				}
			}
			else
			{
				Ins.Logger.Warn($"Packet Handler {type} does not bind to any packet");
			}
		}

		// 如果有封包没有绑定任何handler就发出警告
		foreach (var packetId in packetIDToTypeMapping)
		{
			if (!packetHandlers.ContainsKey(packetId.Key)
				|| packetHandlers[packetId.Key].Count == 0)
			{
				Ins.Logger.Warn($"Packet {packetId.Value} does not have any handler binded");
			}
		}
	}

	private ModPacket GetPacket()
	{
		return _mod.GetPacket();
	}
}
