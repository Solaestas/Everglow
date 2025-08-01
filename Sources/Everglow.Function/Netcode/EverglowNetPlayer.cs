using Everglow.Commons.Netcode.Packets;

namespace Everglow.Commons.Netcode;

public class EverglowNetPlayer : ModPlayer
{
	private static PacketResolver PacketResolver => ModIns.PacketResolver;

	public Vector2 mouseWorld;
	public Vector2 oldMouseWorld;

	public override void PreUpdate()
	{
		// Syncing mouse controls
		if (Main.myPlayer == Player.whoAmI)
		{
			mouseWorld = Main.MouseWorld;

			if (Vector2.Distance(mouseWorld, oldMouseWorld) > 5f)
			{
				oldMouseWorld = mouseWorld;
			}
			if (Math.Abs((mouseWorld - Player.MountedCenter).ToRotation() - (oldMouseWorld - Player.MountedCenter).ToRotation()) > 0.15f)
			{
				oldMouseWorld = mouseWorld;
			}
		}
	}

	public override void PostUpdateMiscEffects()
	{
		SyncMousePosition(Main.dedServ);
	}

	internal void HandleMousePosition(BinaryReader reader)
	{
		mouseWorld = reader.ReadVector2();
		if (Main.netMode == NetmodeID.Server)
		{
			SyncMousePosition(true);
		}
	}

	public void SyncMousePosition(bool server)
	{
		PacketResolver.Send(new MousePositionSyncPacket(mouseWorld), ignoreClient: server ? Player.whoAmI : -1);
	}
}