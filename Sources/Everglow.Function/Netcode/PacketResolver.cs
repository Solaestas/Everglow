using Everglow.Commons.FeatureFlags;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Utilities;
using SubworldLibrary;
using Player_ID = System.Int32;
using Packet_ID = System.Int32;

#pragma warning disable SA1121 // Use built-in type alias

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

	private byte[] SerializePacket(IPacket packet, RouteDestination destination, Player_ID sourcePlayer)
	{
		using MemoryStream ms = new();
		using BinaryWriter bw = new(ms);

		// 1. 写入 route 目标
		bw.Write((byte)destination);

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

		// 4. 写入封包数据
		packet.Send(bw);
		bw.Flush();

		return ms.ToArray();
	}

	private RouteDestination DeserializeRouteDestination(BinaryReader reader)
	{
		return (RouteDestination)reader.ReadByte();
	}

	/// <summary>
	/// 向指定对象发送一个封包数据的实例
	/// <br/>除特殊情况外，请尽可能使用封装版本<see cref="Send(IPacket, bool, Player)"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="packet"></param>
	/// <param name="toClient"></param>
	/// <param name="ignoreClient"></param>
	private void Send(IPacket packet, RouteDestination destination, Player_ID toClient = -1, Player_ID ignoreClient = -1)
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

	public void Send(IPacket packet, Player_ID toClient = -1, Player_ID ignoreClient = -1)
	{
		Send(packet, RouteDestination.WorldOnly, toClient, ignoreClient);
	}

	/// <summary>
	/// 向指定对象发送一个封包数据的实例
	/// <br/> <see cref="Send(IPacket, Player_ID, Player_ID)"/>的封装版本，自动填充发送对象
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
		var destination = DeserializeRouteDestination(reader);

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

		// Forward packets
		bool shouldForward = NetUtils.IsSubServer && destination != RouteDestination.WorldOnly;
		if (shouldForward)
		{
			var headPosition = reader.BaseStream.Position;
			byte[] remainingData = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

			using var stream = new MemoryStream();
			using var writer = new BinaryWriter(stream);

			writer.Write((byte)destination);
			writer.Write(sourcePlayer);
			writer.Write(packetID);
			writer.Write(remainingData);

			writer.Flush();
			var forwardPacket = stream.ToArray();

			// Forward packets here.
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
}

#pragma warning restore SA1121 // Use built-in type alias