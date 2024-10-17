using System.Reflection;

namespace Everglow.Commons.Utilities;

/// <summary>
/// 此特征可以免去由于模式改变而引起的基础数值被tml篡改
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NoGameModeScaleAttribute : Attribute
{
}

public class NoGameModeScale : GlobalNPC
{
	/// <summary>
	/// 拒绝由于模式改变而引起的基础数值被tml篡改
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

public class NPCUtils
{
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
				Main.NewText(bottom);
				npc.spriteDirection = npc.direction;
				npc.velocity *= 0;
				npc.Bottom = bottom + new Vector2(0, 16);
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
		for (int y = 0; y < 4; y++)
		{
			if (!Collision.SolidCollision(npc.Bottom + new Vector2(npc.direction * 15, y * 16), 2, 2))
			{
				empty++;
			}
			else
			{
				break;
			}
		}
		if (empty >= 3)
		{
			return false;
		}

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
		else if(checkTile.IsHalfBlock)
		{
			npc.velocity.Y = -2.5f;
		}
		if (obstructionHeight >= 5)
		{
			return false;
		}
		return true;
	}
}