using Everglow.Commons.Mechanics.Cooldown;
using Everglow.Commons.Netcode.Abstracts;
using Everglow.Commons.Netcode.PacketHandle;

namespace Everglow.Commons.Netcode.Packets;

public class CooldownAdditionPacket : IPacket
{
	public CooldownInstance instance;

	public CooldownAdditionPacket()
	{
	}

	public CooldownAdditionPacket(CooldownInstance instance)
	{
		this.instance = instance;
	}

	public void Receive(BinaryReader reader, int whoAmI)
	{
		instance = new CooldownInstance(reader);
	}

	public void Send(BinaryWriter writer)
	{
		instance.Write(writer);
	}

	[HandlePacket(typeof(CooldownAdditionPacket))]
	public class CooldownAdditionPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			var instance = ((CooldownAdditionPacket)packet).instance;
			var id = CooldownRegistry.registry[instance.netID].ID;
			Main.player[whoAmI].GetModPlayer<EverglowPlayer>().cooldowns[id] = instance;

			Console.WriteLine($"Cooldown synced: {id} {instance.cooldown.DisplayName} {instance.timeLeft}/{instance.timeMax}");
		}
	}
}