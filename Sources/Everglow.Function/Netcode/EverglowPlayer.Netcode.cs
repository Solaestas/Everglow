using Everglow.Commons.Mechanics.Cooldown;
using Everglow.Commons.Netcode;
using Everglow.Commons.Netcode.Packets;

namespace Everglow.Commons;

public partial class EverglowPlayer : ModPlayer
{
	private static PacketResolver PacketResolver => ModIns.PacketResolver;

	public const float MousePositionSyncDiff = 5f;
	public const float MouseRotationSyncDiff = 0.15f;

	internal bool mouseRight = false;
	private bool oldMouseRight = false;
	internal Vector2 mouseWorld;
	private Vector2 oldMouseWorld;

	internal bool listenMouseRight = false;
	internal bool listenMouseWorld = false;
	internal bool listenMouseRotation = false;
	internal bool syncMouseControls = false;

	public void SyncMousePosition(bool fromServer)
	{
		PacketResolver.Send(new MousePositionSyncPacket(mouseWorld), fromServer, Player);
	}

	public void SyncMouseRight(bool fromServer)
	{
		PacketResolver.Send(new MouseRightSyncPacket(mouseRight), fromServer, Player);
	}

	public void SyncCooldownAddition(bool fromServer, CooldownInstance cdInstance)
	{
		ModIns.PacketResolver.Send(new CooldownAdditionPacket(cdInstance), fromServer, Player);
	}

	public void SyncCooldownRemoval(bool fromServer, IList<string> cooldownIDs)
	{
		ModIns.PacketResolver.Send(new CooldownRemovalPacket(cooldownIDs), fromServer, Player);
	}

	public void SyncCooldownFullUpdate(bool fromServer)
	{
		ModIns.PacketResolver.Send(new CooldownFullUpdatePacket(cooldowns), fromServer, Player);
	}
}