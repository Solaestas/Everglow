using Everglow.SubSpace;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.Items.PermanentBoosters;
using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.NPCs.TownNPCs;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles.PlayerArena;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Microsoft.Xna.Framework.Graphics;
using SubworldLibrary;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Everglow.Yggdrasil.YggdrasilTown;

public class YggdrasilTownFurnaceSystem : ModSystem
{
	public static int CurrentScore = 0;

	public static Player CurrentPlayer;

	public override void PostUpdateEverything()
	{
		if(Main.netMode != NetmodeID.Server)
		{
			CurrentPlayer = Main.LocalPlayer;
		}
		if (CurrentPlayer != null)
		{
			FurnacePlayer fPlayer = CurrentPlayer.GetModPlayer<FurnacePlayer>();
			CurrentScore = fPlayer.FurnaceScore;
		}
		base.PostUpdateEverything();
	}
}

public class FurnacePlayer : ModPlayer
{
	public int FurnaceScore;

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		ModPacket packet = Mod.GetPacket();
		packet.Write(MessageID.PlayerLifeMana);
		packet.Write((byte)Player.whoAmI);
		packet.Write((byte)FurnaceScore);
		packet.Send(toWho, fromWho);
	}

	// Called in ExampleMod.Networking.cs
	public void ReceivePlayerSync(BinaryReader reader)
	{
		FurnaceScore = reader.ReadByte();
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		var clone = (FurnacePlayer)targetCopy;
		clone.FurnaceScore = FurnaceScore;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		var clone = (FurnacePlayer)clientPlayer;

		if (FurnaceScore != clone.FurnaceScore)
		{
			SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag["FurnaceScore"] = FurnaceScore;
	}

	public override void LoadData(TagCompound tag)
	{
		FurnaceScore = tag.GetInt("FurnaceScore");
	}
}