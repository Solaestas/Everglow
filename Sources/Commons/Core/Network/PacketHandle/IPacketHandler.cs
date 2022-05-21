namespace Everglow.Sources.Commons.Core.Network.PacketHandle
{
    /// <summary>
    /// 接收和处理某个类型的封包的逻辑
    /// </summary>
    public interface IPacketHandler
    {
        /// <summary>
        /// 接收到封包以后的实际逻辑部分
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="whoAmI"></param>
        public void Handle(IPacket packet, int whoAmI);
    }

    /// <summary>
    /// 用于指定一个IPacketHandler需要处理的IPacket类型，初步认定一个Handler只处理一种封包
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HandlePacketAttribute : Attribute
    {
        private Type m_packetType;
        public HandlePacketAttribute(Type type)
        {
            m_packetType = type;
            Debug.Assert(typeof(IPacket).IsAssignableFrom(type));
        }
        public Type PacketType
        {
            get
            {
                return m_packetType;
            }
        }
    }
}
