using System.Reflection;
using Everglow.Commons.Mechanics.ElementalDebuff;
using Everglow.Commons.Netcode.Packets;

namespace Everglow.Commons.Utilities;

/// <summary>
/// This Attribute can ignore game-mode-based NPC scale.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NoGameModeScaleAttribute : Attribute
{
}

public class NoGameModeScale : GlobalNPC
{
	/// <summary>
	/// Rejection of base value tampering by vanilla due to mode change.
	/// </summary>
	/// <param name="numPlayers"></param>
	/// <param name="balance"></param>
	/// <param name="bossAdjustment"></param>
	public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
	{
		Type type = npc.ModNPC?.GetType();
		if (type != null && type.GetCustomAttribute<NoGameModeScaleAttribute>() != null)
		{
			NPCID.Sets.DontDoHardmodeScaling[npc.type] = true;
			npc.lifeMax = (int)(npc.lifeMax / Main.GameModeInfo.EnemyMaxLifeMultiplier);
			npc.damage = (int)(npc.damage / Main.GameModeInfo.EnemyDamageMultiplier);
			npc.defense = (int)(npc.defense / Main.GameModeInfo.EnemyDefenseMultiplier);
			npc.value = (int)(npc.value / Main.GameModeInfo.EnemyMoneyDropMultiplier);
			npc.knockBackResist = npc.knockBackResist / Main.GameModeInfo.KnockbackToEnemiesMultiplier;
			return;
		}
		base.ApplyDifficultyAndPlayerScaling(npc, numPlayers, balance, bossAdjustment);
	}
}

public static class NPCUtils
{
	#region Town NPC Behavior

	public static void TryCloseDoor(NPC npc)
	{
		if (npc.closeDoor && ((npc.position.X + npc.width / 2) / 16f > npc.doorX + 2 || (npc.position.X + npc.width / 2) / 16f < npc.doorX - 2))
		{
			Tile tileSafely = Framing.GetTileSafely(npc.doorX, npc.doorY);

			if (TileLoader.CloseDoorID(tileSafely) >= 0)
			{
				if (WorldGen.CloseDoor(npc.doorX, npc.doorY))
				{
					npc.closeDoor = false;
					NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 1, npc.doorX, npc.doorY, npc.direction);
				}

				if ((npc.position.X + npc.width / 2) / 16f > npc.doorX + 4 || (npc.position.X + npc.width / 2) / 16f < npc.doorX - 4 || (npc.position.Y + npc.height / 2) / 16f > npc.doorY + 4 || (npc.position.Y + npc.height / 2) / 16f < npc.doorY - 4)
				{
					npc.closeDoor = false;
				}
			}
			else if (tileSafely.type == 389)
			{
				if (WorldGen.ShiftTallGate(npc.doorX, npc.doorY, closing: true))
				{
					npc.closeDoor = false;
					NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 5, npc.doorX, npc.doorY);
				}

				if ((npc.position.X + npc.width / 2) / 16f > npc.doorX + 4 || (npc.position.X + npc.width / 2) / 16f < npc.doorX - 4 || (npc.position.Y + npc.height / 2) / 16f > npc.doorY + 4 || (npc.position.Y + npc.height / 2) / 16f < npc.doorY - 4)
				{
					npc.closeDoor = false;
				}
			}
			else
			{
				npc.closeDoor = false;
			}
		}
	}

	public static void TryOpenDoor(NPC npc)
	{
		int touchDoorX = (int)((npc.position.X + npc.width / 2 + 15 * npc.direction) / 16f);
		int touchDoorY = (int)((npc.position.Y + npc.height - 16f) / 16f);
		Tile tileSafely5 = Framing.GetTileSafely(touchDoorX, touchDoorY - 2);
		if ((npc.townNPC || NPCID.Sets.AllowDoorInteraction[npc.type]) && tileSafely5.nactive() && (TileLoader.IsClosedDoor(tileSafely5) || tileSafely5.type == 388))
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (WorldGen.OpenDoor(touchDoorX, touchDoorY - 2, npc.direction))
				{
					npc.closeDoor = true;
					npc.doorX = touchDoorX;
					npc.doorY = touchDoorY - 2;
					NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, touchDoorX, touchDoorY - 2, npc.direction);
					npc.netUpdate = true;
					npc.ai[1] += 80f;
				}
				else if (WorldGen.OpenDoor(touchDoorX, touchDoorY - 2, -npc.direction))
				{
					npc.closeDoor = true;
					npc.doorX = touchDoorX;
					npc.doorY = touchDoorY - 2;
					NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 0, touchDoorX, touchDoorY - 2, -npc.direction);
					npc.netUpdate = true;
					npc.ai[1] += 80f;
				}
				else if (WorldGen.ShiftTallGate(touchDoorX, touchDoorY - 2, closing: false))
				{
					npc.closeDoor = true;
					npc.doorX = touchDoorX;
					npc.doorY = touchDoorY - 2;
					NetMessage.SendData(MessageID.ToggleDoorState, -1, -1, null, 4, touchDoorX, touchDoorY - 2);
					npc.netUpdate = true;
					npc.ai[1] += 80f;
				}
				else
				{
					npc.direction *= -1;
					npc.netUpdate = true;
				}
			}
		}
	}

	public static int ChooseDirection(NPC npc)
	{
		if (Collision.SolidCollision(npc.Right + new Vector2(8, 0), 2, 2))
		{
			return -1;
		}
		if (Collision.SolidCollision(npc.Left + new Vector2(-10, 0), 2, 2))
		{
			return 1;
		}
		return Main.rand.NextBool() ? -1 : 1;
	}

	public static bool CheckSit(NPC npc)
	{
		if (npc.Center.X > 320 && npc.Center.X < Main.maxTilesX * 16 - 320 && npc.Center.Y > 320 && npc.Center.Y < Main.maxTilesY * 16 - 320)
		{
			var tile = Main.tile[npc.Center.ToTileCoordinates()];
			int tileType = tile.TileType;
			bool flag = !NPCID.Sets.CannotSitOnFurniture[npc.type] && !NPCID.Sets.IsTownSlime[npc.type];
			if (flag)
			{
				flag &= tile != null && tile.active() && TileID.Sets.CanBeSatOnForNPCs[tile.type];
			}
			if (flag)
			{
				Point point = (npc.Bottom + Vector2.UnitY * -2f).ToTileCoordinates();
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i].active && Main.npc[i].aiStyle == 7 && Main.npc[i].townNPC && Main.npc[i].ai[0] == 5f && (Main.npc[i].Bottom + Vector2.UnitY * -2f).ToTileCoordinates() == point)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				Vector2 bottom = default;
				npc.SitDown(npc.Center.ToTileCoordinates(), out npc.direction, out bottom);
				npc.spriteDirection = npc.direction;
				npc.velocity *= 0;
				return true;
			}
		}
		return false;
	}

	public static bool CanContinueWalk(NPC npc)
	{
		if (npc.Center.X < 320 || npc.Center.X > Main.maxTilesX * 16 - 320 || npc.Center.Y < 320 || npc.Center.Y > Main.maxTilesY * 16 - 320)
		{
			return false;
		}
		Point checkPoint = (npc.Bottom + new Vector2(8 * npc.direction, 8)).ToTileCoordinates() + new Point(npc.direction, -1);
		Tile checkTile = Main.tile[checkPoint];
		if (TileLoader.IsClosedDoor(checkTile.TileType) || checkTile.TileType == 388)
		{
			return true;
		}
		int empty = 0;

		// This check was from 2 tile over NPC's bottom to 3 tiles below.
		for (int y = -2; y < 4; y++)
		{
			if (!TileCollisionUtils.PlatformCollision(npc.Bottom + new Vector2(npc.direction * 15, y * 16)) && !Collision.SolidCollision(npc.BottomLeft + new Vector2(npc.direction * 15, y * 16), npc.width, npc.height))
			{
				empty++;
			}
			else
			{
				break;
			}
		}
		if (empty >= 5)
		{
			empty = 0;

			// To stop a walking NPC, a groove at lease 2 tiles is necessary.
			for (int y = -2; y < 4; y++)
			{
				if (!TileCollisionUtils.PlatformCollision(npc.Bottom + new Vector2(npc.direction * 30, y * 16)) && !Collision.SolidCollision(npc.BottomLeft + new Vector2(npc.direction * 30, y * 16), npc.width, npc.height))
				{
					empty++;
				}
				else
				{
					break;
				}
			}
			if (empty >= 5)
			{
				return false;
			}
		}

		// Jumping limit was 6 tiles.(Only for normal NPC)
		int obstructionHeight = 0;
		for (int y = 1; y < 6; y++)
		{
			if (Collision.SolidCollision(npc.Bottom + new Vector2(16 * npc.direction, 16 * -y + 8), 2, 2))
			{
				obstructionHeight++;
			}
		}
		if (obstructionHeight >= 1 && npc.collideX)
		{
			npc.velocity.Y = -2.5f * obstructionHeight;
		}
		else if (checkTile.IsHalfBlock)
		{
			npc.velocity.Y = -0.5f;
		}
		if (obstructionHeight >= 5)
		{
			return false;
		}
		return true;
	}


	#endregion

	#region Vanilla Stats

	public static int GetVanillaDotDamage(this NPC npc, IEnumerable<int> buffTypes)
	{
		return buffTypes
			.Where(npc.HasBuff)
			.Where(type => BuffUtils.VanillaDotDebuffDamageOnNPC.TryGetValue(type, out int _))
			.Select(type => npc.buffTime[npc.FindBuffIndex(type)] * BuffUtils.VanillaDotDebuffDamageOnNPC[type])
			.Sum();
	}

	#endregion

	#region Elemental Debuff

	/// <summary>
	/// Add build-up to the specific elemental debuff instance of this NPC.
	/// <br/> This accounts for if NPC has resistance to this type of elemental debuff.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="type"></param>
	/// <param name="buildUp"></param>
	/// <param name="penentration"></param>
	/// <returns></returns>
	internal static bool AddElementalDebuffBuildUp(this NPC npc, string type, int buildUp, float penentration = 0)
	{
		// Add to real target of the npc
		if (npc.realLife == -1 || npc.realLife == npc.whoAmI)
		{
			return npc.GetGlobalNPC<ElementalDebuffGlobalNPC>().ElementalDebuffs[type].AddBuildUp(buildUp, penentration);
		}
		else
		{
			var realLife = Main.npc[npc.realLife];
			return realLife.active
				? realLife.GetGlobalNPC<ElementalDebuffGlobalNPC>().ElementalDebuffs[type].AddBuildUp(buildUp, penentration)
				: false;
		}
	}

	/// <summary>
	/// Add build-up to the specific elemental debuff instance of this NPC with the source of player.
	/// <br/> This accounts for if NPC has resistance to this type of elemental debuff, also player's elemental penetration.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="source"></param>
	/// <param name="type"></param>
	/// <param name="buildUp"></param>
	/// <param name="additionalPenentration"></param>
	public static bool AddElementalDebuffBuildUp(this NPC npc, Player source, string type, int buildUp, float additionalPenentration = 0)
	{
		if (NetUtils.IsClient && source.whoAmI == Main.myPlayer)
		{
			ModIns.PacketResolver.Send(new ElementalBuildUpPacket(npc.whoAmI, ElementalDebuffRegistry.NameToNetID[type], buildUp), false, source);
		}

		npc.PlayerInteraction(source.whoAmI);

		// Calculate player's elemental penetration
		if (source != null)
		{
			var typePene = source.GetElementalPenetration(type).ApplyTo(1f) - 1f;
			if (typePene > 0)
			{
				additionalPenentration += typePene;
			}

			var genericPene = source.GetElementalPenetration().Generic.ApplyTo(1f) - 1f;
			if (genericPene > 0)
			{
				additionalPenentration += genericPene;
			}
		}

		return npc.AddElementalDebuffBuildUp(type, buildUp, additionalPenentration);
	}

	/// <summary>
	/// Add build-up to the specific elemental debuff instance of this NPC with source of world/server.
	/// <br/> This accounts for if NPC has resistance to this type of elemental debuff.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="type"></param>
	/// <param name="buildUp"></param>
	/// <param name="additionalPenentration"></param>
	/// <returns></returns>
	public static bool AddElementalDebuffBuildUp_World(this NPC npc, string type, int buildUp, float additionalPenentration = 0)
	{
		if (NetUtils.IsServer)
		{
			ModIns.PacketResolver.Send(new ElementalBuildUpPacket(npc.whoAmI, ElementalDebuffRegistry.NameToNetID[type], buildUp));
		}

		return AddElementalDebuffBuildUp(npc, type, buildUp, additionalPenentration);
	}

	/// <summary>
	/// Get the specific elemental debuff instance of this NPC.
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static ElementalDebuffInstance GetElementalDebuff(this NPC npc, string type) =>
		npc.GetGlobalNPC<ElementalDebuffGlobalNPC>().ElementalDebuffs[type];

	public static ref StatModifier GetElementalResistance(this NPC npc, string type) =>
		ref npc.GetElementalDebuff(type).ElementalResistanceModifier;

	#endregion
}