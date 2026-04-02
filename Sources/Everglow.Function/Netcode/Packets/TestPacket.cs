using Everglow.Commons.Netcode.Abstracts;
using SubworldLibrary;

namespace Everglow.Commons.Netcode.Packets;

public class TestPacket : IPacket
{
	private int value;

	public TestPacket()
	{
	}

	public TestPacket(int value)
	{
		this.value = value;
	}

	public void Receive(BinaryReader reader, int whoAmI)
	{
		var data = reader.ReadInt32();
		if (SubworldSystem.Current is null)
		{
			Console.WriteLine($"Test received from main: {data}");
		}
		else
		{
			Console.WriteLine($"Test received from sub: {data}");
		}
	}

	public void Send(BinaryWriter writer)
	{
		writer.Write(value);
		if (SubworldSystem.Current is null)
		{
			Console.WriteLine($"Test send from main: {value}");
		}
		else
		{
			Console.WriteLine($"Test send from sub: {value}");
		}
	}

	[HandlePacket(typeof(TestPacket))]
	public class TestPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
		}
	}
}