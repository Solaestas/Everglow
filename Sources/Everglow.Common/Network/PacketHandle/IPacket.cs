namespace Everglow.Core.Network.PacketHandle
{
	/// <summary>
	/// 用于表示一个封包的读写操作以及其数据，注意一般在这里我们只操作数据，不做接收后的操作
	/// </summary>
	public interface IPacket
	{
		/// <summary>
		/// 封包发送时写入数据的逻辑部分，注意此时写入的数据开头已经包含了封包ID，所以不用重复写入
		/// </summary>
		/// <param name="writer"></param>
		public void Send(BinaryWriter writer);

		/// <summary>
		/// 封包读取时读入数据的逻辑部分，注意此时已经读入了封包ID，所以不要重复读取
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="whoAmI">the ID of whomever sent the packet (equivalent to the Main.myPlayer of the sender)</param>
		public void Receive(BinaryReader reader, int whoAmI);
	}
}
