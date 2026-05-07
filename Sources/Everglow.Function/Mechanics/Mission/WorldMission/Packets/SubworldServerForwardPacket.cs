using Everglow.Commons.Netcode.Abstracts;
using SubworldLibrary;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Packets;

public class SubworldServerForwardPacket : IPacket
{
	public SubworldServerForwardPacket()
	{
	}

	public SubworldServerForwardPacket(int value)
	{
		_value = value;
	}

	private int _value;

	public void Receive(BinaryReader reader, int whoAmI)
	{
		_value = reader.ReadInt32();
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(_value);
		Console.WriteLine($"Sent {_value}...");
	}

	[HandlePacket(typeof(SubworldServerForwardPacket))]
	public class SubworldServerForwardPacketHandler_ToMain : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			if (SubworldSystem.Current is null)
			{
				// Apply actual logic
				Console.WriteLine(value: $"Main server has received {(packet as SubworldServerForwardPacket)._value}");
			}
			else
			{
				// Forward to main server
				ModIns.PacketResolver.SendToMainServer(packet);
				Console.WriteLine(value: $"Sub server has received {(packet as SubworldServerForwardPacket)._value}");
				Console.WriteLine(value: $"Forward to main server {(packet as SubworldServerForwardPacket)._value}");
			}
		}
	}
}