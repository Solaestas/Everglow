using Everglow.Commons.FeatureFlags;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Utilities;
using SubworldLibrary;

namespace Everglow.Commons.Netcode;

/// <summary>
/// Specifies the network destination for a packet, relative to the caller's role.
/// <para/> Determined by <see cref="NetUtils"/> based on <see cref="Main.netMode"/> and <see cref="SubworldSystem.Current"/>.
/// </summary>
public enum RouteDestination
{
	/// <summary>
	/// Packet is handled only within the current world.
	/// Useful for vanilla netcode.
	/// </summary>
	WorldOnly,

	/// <summary>
	/// Packet travels upward to the main world server.
	/// <list type="bullet">
	/// <item>
	///     <term>Subworld server</term>
	///     <description>Sends directly to main world.</description>
	/// </item>
	/// <item>
	///     <term>Subworld client</term>
	///     <description>Forwards through its subworld server (server relays transparently, no parsing).</description>
	/// </item>
	/// <item>
	///     <term>Main world client</term>
	///     <description>Sends directly to main world server.</description>
	/// </item>
	/// </list>
	/// Typical use: mission progress reporting, validation requests, data aggregation.
	/// </summary>
	MainServer,

	/// <summary>
	/// Packet is broadcast downstream from the main world server to all endpoints.
	/// Only the main world server is allowed to send this.
	/// <list type="number">
	/// <item>
	///     <term>Main world clients</term>
	///     <description>Direct delivery.</description>
	/// </item>
	/// <item>
	///     <term>All subworld servers</term>
	///     <description>Each subworld server will first execute the packet logic locally, then forward it to its own subworld clients.</description>
	/// </item>
	/// <item>
	///     <term>Subworld clients</term>
	///     <description>Indirectly, via subworld server forwarding.</description>
	/// </item>
	/// </list>
	/// Typical use: global state synchronization, world events, system announcements.
	/// </summary>
	AllDownstream,
}

/// <summary>
/// 用于管理封包发送、接收的类
/// </summary>
public class PacketResolver
{
	private Mod _mod;
	private Dictionary<int, List<IPacketHandler>> packetHandlerRegistry;
	private Dictionary<Type, int> packetIDMapping;
	private Dictionary<int, Type> packetIDToTypeMapping;
	private int packetIDCounter;

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
		return packetIDMapping.TryGetValue(typeof(T), out int packetID) ? packetID : -1;
	}

	/// <summary>
	/// 注册所有<see cref="IPacket"/>和<see cref="IPacketHandler"/>的实现类型.
	/// </summary>
	private void RegisterPackets()
	{
		var modTypes = Ins.ModuleManager.Types.Where(type => !type.IsAbstract);
		foreach (var type in modTypes.Where(type => type.IsAssignableTo(typeof(IPacket))))
		{
			if (packetIDMapping.TryAdd(type, packetIDCounter))
			{
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

	private byte[] SerializePacket(IPacket packet, RouteDestination destination, int sourcePlayer)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		// 1. 写入 route 目标
		bw.Write((int)destination);

		// 2. 写入来源玩家ID
		bw.Write(sourcePlayer);

		// 3. 写入封包ID
		int id = packetIDMapping[packet.GetType()];
		if (CompileTimeFeatureFlags.NetworkPacketIDUseInt32)
		{
			bw.Write(id);
		}
		else
		{
			bw.Write((byte)id);
		}

		// 4. 写入封包数据 (长度 + 数据)
		var lengthPos = ms.Position;
		bw.Write(0);

		var startPos = ms.Position;
		packet.Send(bw);
		var endPos = ms.Position;

		ms.Position = lengthPos;
		bw.Write((int)(endPos - startPos));

		ms.Position = endPos;
		bw.Flush();

		return ms.ToArray();
	}

	private static byte[] SerializePacketWithData(RouteDestination destination, int sourcePlayer, int packetID, byte[] data)
	{
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);

		writer.Write((int)destination);
		writer.Write(sourcePlayer);
		writer.Write(packetID);
		writer.Write(data.Length);
		writer.Write(data);

		writer.Flush();
		return stream.ToArray();
	}

	private static RouteDestination DeserializeRouteDestination(BinaryReader reader)
	{
		return (RouteDestination)reader.ReadInt32();
	}

	private void Send(IPacket packet, RouteDestination destination, int toClient = -1, int ignoreClient = -1)
	{
		// 单人模式不要有任何动作
		if (NetUtils.IsSingle)
		{
			return;
		}

		var sourcePlayer = NetUtils.IsServer ? ignoreClient : Main.myPlayer;
		var data = SerializePacket(packet, destination, sourcePlayer);

		var modPacket = GetPacket();
		modPacket.Write(data);
		modPacket.Send(toClient, ignoreClient);
	}

	/// <summary>
	/// 向指定对象发送一个封包数据的实例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="packet"></param>
	/// <param name="toClient"></param>
	/// <param name="ignoreClient"></param>
	public void Send(IPacket packet, int toClient = -1, int ignoreClient = -1)
	{
		Send(packet, RouteDestination.WorldOnly, toClient, ignoreClient);
	}

	/// <summary>
	/// 向指定对象发送一个封包数据的实例
	/// <br/> <see cref="Send(IPacket, int, int)"/>的封装版本，自动填充发送对象
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

	public void Route(IPacket packet, RouteDestination destination)
	{
		if (NetUtils.IsSingle)
		{
			return;
		}

		Debug.Assert(destination is not RouteDestination.WorldOnly, "Use Send() to send world only packets.");

		switch (destination)
		{
			case RouteDestination.MainServer:
				{
					if (NetUtils.IsMainClient)
					{
						Send(packet, false, Main.LocalPlayer);
					}
					else if (NetUtils.IsSubServer)
					{
						var data = SerializePacket(packet, RouteDestination.MainServer, -1);
						SubworldSystem.SendToMainServer(_mod, data);
					}
					else if (NetUtils.IsSubClient)
					{
						Send(packet, RouteDestination.MainServer);
					}
				}
				break;
			case RouteDestination.AllDownstream:
				{
					Debug.Assert(NetUtils.IsMainServer, "All downstream can only be sent from main world server!");

					// Send to main clients
					Send(packet);

					// Send to all sub servers
					var data = SerializePacket(packet, RouteDestination.AllDownstream, -1);
					SubworldSystem.SendToAllSubservers(_mod, data);
				}
				break;
		}
	}

	/// <summary>
	/// 处理封包
	/// </summary>
	/// <param name="reader"></param>
	/// <param name="whoAmI"></param>
	public void Resolve(BinaryReader reader, int _)
	{
		// 读取路由目标
		var destination = DeserializeRouteDestination(reader);

		// 读取来源玩家ID
		var sourcePlayer = reader.ReadInt32();

		// 读取封包ID
		int packetID;
		if (CompileTimeFeatureFlags.NetworkPacketIDUseInt32)
		{
			packetID = reader.ReadInt32();
		}
		else
		{
			packetID = reader.ReadByte();
		}

		// 读取数据长度
		var length = reader.ReadInt32();

		// Forward packets
		bool shouldForward = NetUtils.IsSubServer && destination != RouteDestination.WorldOnly;
		if (shouldForward)
		{
			var headPosition = reader.BaseStream.Position;
			byte[] remainingData = reader.ReadBytes(length);

			var forwardPacket = SerializePacketWithData(destination, sourcePlayer, packetID, remainingData);

			if (destination == RouteDestination.AllDownstream)
			{
				var modPacket = GetPacket();
				modPacket.Write(forwardPacket);
				modPacket.Send();

				reader.BaseStream.Position = headPosition;
			}
			else if (destination == RouteDestination.MainServer)
			{
				// Forward only, no excuting packet logic.
				Ins.Logger.Debug("Forward packet is sent...");
				SubworldSystem.SendToMainServer(_mod, forwardPacket);
				return;
			}
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
}