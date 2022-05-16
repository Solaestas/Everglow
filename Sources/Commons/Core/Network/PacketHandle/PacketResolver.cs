using Everglow.Sources.Commons.Function.FeatureFlags;

namespace Everglow.Sources.Commons.Core.Network.PacketHandle
{
    using Packet_Id = Int32;
    public class PacketResolver
    {
        private Dictionary<Packet_Id, List<IPacketHandler>> m_packetHandlers;
        private Dictionary<Type, Packet_Id> m_packetIDMapping;
        private Dictionary<Packet_Id, Type> m_packetIDToTypeMapping;
        private Packet_Id m_packetIDCounter;

        /// <summary>
        /// 用于初始化所有需要监听的 Packet 类型和监听器
        /// </summary>
        public PacketResolver()
        {
            m_packetIDCounter = 0;
            m_packetIDMapping = new Dictionary<Type, Packet_Id>();
            m_packetIDToTypeMapping = new Dictionary<Packet_Id, Type>();
            m_packetHandlers = new Dictionary<Packet_Id, List<IPacketHandler>>();

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
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter bw = new BinaryWriter(ms);
                int id = m_packetIDMapping[packet.GetType()];
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
        public int QueryPacketId<T>() where T : IPacket
        {
            var type = typeof(T);
            if (m_packetIDMapping.ContainsKey(type))
            {
                return m_packetIDMapping[type];
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
            if (!m_packetHandlers.ContainsKey(packetID))
            {
                Everglow.Instance.Logger.Warn("Received a packet without handler, automatically ignored");
                return;
            }
            // 直接从reader里构造packet数据
            IPacket packet = Activator.CreateInstance(m_packetIDToTypeMapping[packetID]) as IPacket;
            packet.Receive(reader, whoAmI);

            // 让handler处理封包数据
            foreach (var handler in m_packetHandlers[packetID])
            {
                handler.Handle(packet, whoAmI);
            }
        }

        /// <summary>
        /// 注册所有IPacket类型和IPacketHandler
        /// </summary>
        private void RegisterPackets()//命名改为首字母大写
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes().Where(type =>
                !type.IsAbstract &&
                type.GetInterfaces().Contains(typeof(IPacket))
                ))
            {
                if (!m_packetIDMapping.ContainsKey(type))
                {
                    m_packetIDMapping.Add(type, m_packetIDCounter);
                    m_packetIDToTypeMapping.Add(m_packetIDCounter, type);
                    m_packetIDCounter++;
                }
            }

            foreach (var type in assembly.GetTypes().Where(type =>
                !type.IsAbstract &&
                type.GetInterfaces().Contains(typeof(IPacketHandler))
                ))
            {
                // 将 packet 和 PacketHandler 绑定
                if (Attribute.GetCustomAttribute(type, typeof(HandlePacketAttribute)) is HandlePacketAttribute handlePacket)
                {
                    Type packetType = handlePacket.PacketType;
                    if (!m_packetIDMapping.ContainsKey(packetType))
                    {
                        throw new InvalidOperationException("Unknown packet type");
                    }
                    // 获取封包类型的对应ID，并且将handler绑定上去
                    Packet_Id packetId = m_packetIDMapping[packetType];
                    IPacketHandler handler = Activator.CreateInstance(type) as IPacketHandler;
                    if (m_packetHandlers.ContainsKey(packetId))
                    {
                        m_packetHandlers[packetId].Add(handler);
                    }
                    else
                    {
                        m_packetHandlers.Add(packetId, new List<IPacketHandler> { handler });
                    }
                }
                else
                {
                    Everglow.Instance.Logger.Warn($"Packet Handler {type} does not bind to any packet");
                }
            }

            // 如果有封包没有绑定任何handler就发出警告
            foreach (var packetId in m_packetIDToTypeMapping)
            {
                if (!m_packetHandlers.ContainsKey(packetId.Key)
                    || m_packetHandlers[packetId.Key].Count == 0)
                {
                    Everglow.Instance.Logger.Warn($"Packet {packetId.Value} does not have any handler binded");
                }
            }
        }

        private static ModPacket GetPacket()
        {
            return Everglow.Instance.GetPacket();
        }
    }
}
