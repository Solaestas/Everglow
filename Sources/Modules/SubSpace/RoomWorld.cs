using SubworldLibrary;
using Terraria.WorldBuilding;

namespace Everglow.SubSpace;

public class RoomWorld : Subworld
{
	/// <summary>
	/// 原世界,为主世界时Null
	/// </summary>
	public static Subworld OriginalWorld;

	/// <summary>
	/// 房间深度
	/// </summary>
	public static int LayerDepth;

	/// <summary>
	/// 返回的层级
	/// </summary>
	public static int ExitToTarget;

	/// <summary>
	/// 入口在原世界中的位置(返回锚点)
	/// </summary>
	public static Point AnchorWorldCoordinate;

	public override int Width => 300;

	public override int Height => 300;

	public override int ReturnDestination
	{
		get
		{
			if (OriginalWorld == null)
			{
				return -1; // 或者处理其他情况
			}
			return SubworldSystem.GetIndex(OriginalWorld.FullName);
		}
	}

	public override bool NormalUpdates => true;

	public override void OnLoad()
	{
		Main.worldSurface = 190;
		Main.rockLayer = 192;
		GenVars.waterLine = 194;
	}

	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new WoodenBoxRoomGenPass(),
	};

	public override bool ShouldSave => false;

	public static Point OldSpawnPos = default;

	public static Point SpawnPos = default;

	public override void OnEnter()
	{
	}

	public override void OnExit()
	{
	}
}