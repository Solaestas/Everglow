using Everglow.Commons.TileHelper;
using SubworldLibrary;

namespace Everglow.SubSpace;

public class RoomManager
{
	/// <summary>
	/// 向内深入一层房间
	/// </summary>
	public static void EnterNextLevelRoom(Point point, string mapPath = "")
	{
		// 如果已经在房间里了,向下一层
		if (SubworldSystem.IsActive<RoomWorld>())
		{
			// 保存世界的地图数据
			SaveRoomDatas();
			RoomWorld.LayerDepth++;

			// 没有记录就新建
			if (!WoodenBoxRoomGenPass.ReadWorldSave())
			{
				// 没有给定的地图数据直接搓一个木制空洞
				WoodenBoxRoomGenPass.MapIOPathOfNewRoom = mapPath;
				WoodenBoxRoomGenPass.BuildWoodenRoom();
			}
		}

		// 如果不在房间里就进入1层房间
		else
		{
			// 保存原世界
			if (SubworldSystem.Current != null)
			{
				RoomWorld.OriginalWorld = SubworldSystem.Current;
			}
			else
			{
				RoomWorld.OriginalWorld = null;
			}
			WoodenBoxRoomGenPass.MapIOPathOfNewRoom = mapPath;
			RoomWorld.AnchorWorldCoordinate = point;
			RoomWorld.LayerDepth = 1;
			SubworldSystem.Enter<RoomWorld>();
		}
	}

	/// <summary>
	/// 向外离开一层房间
	/// </summary>
	public static void ExitALevelOfRoom()
	{
		// 在房间里就向外一层
		if (SubworldSystem.IsActive<RoomWorld>())
		{
			if (RoomWorld.LayerDepth > 1)
			{
				// 保存世界的地图数据
				SaveRoomDatas();
				RoomWorld.LayerDepth--;

				// 没有记录就新建,虽然一般情况下不会发生,保险起见.
				if (!WoodenBoxRoomGenPass.ReadWorldSave())
				{
					WoodenBoxRoomGenPass.BuildWoodenRoom();
				}
			}
			else
			{
				ExitToOriginalWorld();
			}
		}

		// 不在房间里无法离开
		else
		{
			return;
		}
	}

	/// <summary>
	/// 去到指定层级的房间
	/// </summary>
	public static void GoToTargetLevelRoom()
	{
	}

	/// <summary>
	/// 退回原世界
	/// </summary>
	public static void ExitToOriginalWorld()
	{
		// 不在房间里无法退回原世界
		if (!SubworldSystem.IsActive<RoomWorld>())
		{
			return;
		}

		// 保存世界的地图数据
		SaveRoomDatas();

		// 层级深度归零
		RoomWorld.LayerDepth = 0;

		// 原世界为主世界直接退出
		if (RoomWorld.OriginalWorld == null)
		{
			SubworldSystem.Exit();
		}

		// 否则进入原世界
		else
		{
			SubworldSystem.Enter(RoomWorld.OriginalWorld.FullName);
		}

		// 把玩家还原回房间门口的位置
		Player player = Main.LocalPlayer;
		RoomPlayer roomPlayer = player.GetModPlayer<RoomPlayer>();
		roomPlayer.RoomPosition = RoomWorld.AnchorWorldCoordinate;
		RoomWorld.OriginalWorld = null;
	}

	public static void SaveRoomDatas()
	{
		var mapIO = new MapIO(5, 5, Main.maxTilesX - 5, Main.maxTilesY - 5);
		string path = Path.Combine(Main.SavePath, "Mods", "ModDatas", "Everglow_RoomWorlds", Main.worldID.ToString());
		if (RoomWorld.OriginalWorld != null)
		{
			path = Path.Combine(Main.SavePath, "Mods", "ModDatas", "Everglow_RoomWorlds", Main.worldID.ToString(), RoomWorld.OriginalWorld.Name.ToString());
		}
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		string writePath = path + "\\RoomMapIO" + RoomWorld.AnchorWorldCoordinate.X.ToString() + "_" + RoomWorld.AnchorWorldCoordinate.Y.ToString() + "_" + RoomWorld.LayerDepth + ".mapio";
		mapIO.Write(writePath);
	}
}

public class RoomPlayer : ModPlayer
{
	/// <summary>
	/// 需要把玩家还原回去的位点,一般取房间门的锚位
	/// </summary>
	public Point RoomPosition;

	public override void OnEnterWorld()
	{
		if (RoomPosition != Point.zeroPoint)
		{
			Player.Teleport(new Vector2(RoomPosition.X * 16 - 8, RoomPosition.Y * 16 - 48));
			RoomPosition = Point.zeroPoint;
		}

		// 房间深度归零
		if (!SubworldSystem.IsActive<RoomWorld>())
		{
			RoomWorld.LayerDepth = 0;
		}
		base.OnEnterWorld();
	}
}

public class RoomWorldTile : GlobalTile
{
	public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
	{
		if (SubworldSystem.IsActive<RoomWorld>())
		{
			if (Math.Abs(i - Main.maxTilesX / 2) > 80 || Math.Abs(j - Main.maxTilesY / 2) > 80)
			{
				return false;
			}
		}
		return base.CanKillTile(i, j, type, ref blockDamaged);
	}
}