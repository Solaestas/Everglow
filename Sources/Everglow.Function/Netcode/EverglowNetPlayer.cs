using Everglow.Commons.Netcode.PacketHandle;
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

#if false
	internal const int GlobalSyncPacketTimer = 15;

	public int packetTimer = 0;

	public bool oldMouseRight = false;
	public bool mouseRight = false;
	public Vector2 mouseWorld;
	public Vector2 oldMouseWorld;

	public bool syncMouseControls = false;
	public bool rightClickListener = false;
	public bool mouseWorldListener = false;
	public bool mouseRotationListener = false;

	public override void PreUpdate()
	{
		// Syncing mouse controls
		if (Main.myPlayer == Player.whoAmI)
		{
			mouseRight = PlayerInput.Triggers.Current.MouseRight;
			mouseWorld = Main.MouseWorld;

			if (rightClickListener && mouseRight != oldMouseRight)
			{
				oldMouseRight = mouseRight;
				syncMouseControls = true;
				rightClickListener = false;
			}
			if (mouseWorldListener && Vector2.Distance(mouseWorld, oldMouseWorld) > 5f)
			{
				oldMouseWorld = mouseWorld;
				syncMouseControls = true;
				mouseWorldListener = false;
			}
			if (mouseRotationListener && Math.Abs((mouseWorld - Player.MountedCenter).ToRotation() - (oldMouseWorld - Player.MountedCenter).ToRotation()) > 0.15f)
			{
				oldMouseWorld = mouseWorld;
				syncMouseControls = true;
				mouseRotationListener = false;
			}
		}
	}

	public override void PostUpdateMiscEffects()
	{
		if (Player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient)
		{
			packetTimer++;
			if (packetTimer == GlobalSyncPacketTimer)
			{
				packetTimer = 0;
				StandardSync();
			}

			if (syncMouseControls)
			{
				syncMouseControls = false;
				MouseControlsSync();
			}
		}
	}

	internal void StandardSync()
	{
	}

	internal void MouseControlsSync()
	{
		SyncRightClick(false);
		SyncMousePosition(false);
	}

	public void SyncRightClick(bool server)
	{
		ModPacket packet = Mod.GetPacket(256);
		packet.Write((byte)CalamityModMessageType.RightClickSync);
		packet.Write(Player.whoAmI);
		packet.Write(mouseRight);
		Player.SendPacket(packet, server);
	}

	public void SyncMousePosition(bool server)
	{
		if (Main.netMode == NetmodeID.MultiplayerClient)
		{
			var packet = Mod.GetPacket();
			packet.Write((byte)EverglowPacketType.MouseWorld);
			packet.WriteVector2(mouseWorld);
			packet.Send();
		}
	}

	public void SyncCooldownAddition(bool server, CooldownInstance cd)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
			return;
		ModPacket packet = Mod.GetPacket(256);
		packet.Write((byte)CalamityModMessageType.CooldownAddition);
		packet.Write(Player.whoAmI);
		cd.Write(packet);
		Player.SendPacket(packet, server);
	}

	public void SyncCooldownRemoval(bool server, IList<string> cooldownIDs)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
			return;
		ModPacket packet = Mod.GetPacket(256);
		packet.Write((byte)CalamityModMessageType.CooldownRemoval);
		packet.Write(Player.whoAmI);
		packet.Write(cooldownIDs.Count);
		foreach (string id in cooldownIDs)
			packet.Write(CooldownRegistry.Get(id).netID);
		Player.SendPacket(packet, server);
	}

	public void SyncCooldownDictionary(bool server)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
			return;
		ModPacket packet = Mod.GetPacket(1024);
		packet.Write((byte)CalamityModMessageType.SyncCooldownDictionary);
		packet.Write(Player.whoAmI);
		packet.Write(cooldowns.Count);
		foreach (CooldownInstance cd in cooldowns.Values)
			cd.Write(packet);
		Player.SendPacket(packet, server);
	}

	#region Reading and Handling Packets

	internal void HandleCooldownAddition(BinaryReader reader)
	{
		// The player ID and message ID are already read. The only remaining data is the serialization of the cooldown instance.
		CooldownInstance instance = new CooldownInstance(reader);

		// Actually assign this freshly synced cooldown to the appropriate player.
		string id = CooldownRegistry.registry[instance.netID].ID;
		cooldowns[id] = instance;
	}

	internal void HandleCooldownRemoval(BinaryReader reader)
	{
		int count = reader.ReadInt32();
		for (int i = 0; i < count; ++i)
		{
			ushort netID = reader.ReadUInt16();
			cooldowns.Remove(CooldownRegistry.registry[netID].ID);
		}
	}

	internal void HandleCooldownDictionary(BinaryReader reader)
	{
		int count = reader.ReadInt32();
		if (count <= 0)
			return;

		// Cooldown dictionary packets are just a span of serialized cooldown instances. So each one can be read exactly as with a single cooldown.
		Dictionary<ushort, CooldownInstance> syncedCooldowns = new Dictionary<ushort, CooldownInstance>(count);
		for (int i = 0; i < count; ++i)
		{
			CooldownInstance instance = new CooldownInstance(reader);
			syncedCooldowns[instance.netID] = instance;
		}

		HashSet<ushort> localIDs = new HashSet<ushort>();
		foreach (CooldownInstance localInstance in cooldowns.Values)
			localIDs.Add(localInstance.netID);

		HashSet<ushort> syncedIDs = new HashSet<ushort>();
		foreach (ushort syncedID in syncedCooldowns.Keys)
			syncedIDs.Add(syncedID);

		HashSet<ushort> combinedIDSet = new HashSet<ushort>();
		combinedIDSet.UnionWith(localIDs);
		combinedIDSet.UnionWith(syncedIDs);

		foreach (ushort netID in combinedIDSet)
		{
			bool existsLocally = localIDs.Contains(netID);
			bool existsRemotely = syncedIDs.Contains(netID);
			string id = CooldownRegistry.registry[netID].ID;

			// Exists locally but not remotely = cull -- destroy the local copy.
			if (existsLocally && !existsRemotely)
				cooldowns.Remove(id);
			// Exists remotely but not locally = add -- insert into the dictionary.
			else if (existsRemotely && !existsLocally)
				cooldowns[id] = syncedCooldowns[netID];
			// Exists in both places = update -- update timing fields but don't replace the instance.
			else if (existsLocally && existsRemotely)
			{
				CooldownInstance localInstance = cooldowns[id];
				localInstance.duration = syncedCooldowns[netID].duration;
				localInstance.timeLeft = syncedCooldowns[netID].timeLeft;
			}
		}
	}


	internal void HandleRightClick(BinaryReader reader)
	{
		mouseRight = reader.ReadBoolean();
		if (Main.netMode == NetmodeID.Server)
		{
			SyncRightClick(true);
		}
	}

	internal void HandleMousePosition(BinaryReader reader)
	{
		mouseWorld = reader.ReadVector2();
		if (Main.netMode == NetmodeID.Server)
		{
			SyncMousePosition(true);
		}
	}
	#endregion
#endif
}