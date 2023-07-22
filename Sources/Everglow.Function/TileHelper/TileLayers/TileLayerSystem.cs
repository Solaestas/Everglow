using Everglow.Commons.TileHelper;

namespace Everglow.Commons.TileHelper.TileLayers;
public struct TileClone
{
	public int I { get; set; }
	public int J { get; set; }
	public int WallFrameX { get; set; }
	public int WallFrameY { get; set; }
	public ushort TileType { get; set; }
	public ushort Wall { get; set; }
	public short TileFrameX { get; set; }
	public short TileFrameY { get; set; }
	public byte Liquid { get; set; }
	public byte LiquidAmount { get; set; }
	public byte TileColor { get; set; }
	public byte WallColor { get; set; }
	public bool HasTile { get; set; }
	public bool RedWire { get; set; }
	public bool GreenWire { get; set; }
	public bool BlueWire { get; set; }
	public bool YellowWire { get; set; }
	public SlopeType SlopeType { get; set; }
	public BlockType BlockType { get; set; }

}
public class TileLayerSystem : ModSystem
{
	/// <summary>
	/// (x坐标，y坐标，层)的物块
	/// </summary>
	public static Dictionary<(int, int, int), TileClone> LayerTile = new Dictionary<(int, int, int), TileClone>();

	/// <summary>
	/// 玩家所在层
	/// </summary>
	public static int[] PlayerZoneLayer = new int[256];
	/// <summary>
	/// 正在层级向内切换的物块的坐标
	/// </summary>
	public static List<(int, int)> InSwitchingTileCoords = new List<(int, int)>();
	/// <summary>
	/// 正在层级向外切换的物块的坐标
	/// </summary>
	public static List<(int, int)> OutSwitchingTileCoords = new List<(int, int)>();
	/// <summary>
	/// (层)的房间区域集合
	/// </summary>
	public static Dictionary<int, List<(int, int)>> RoomsInsideLayers = new Dictionary<int, List<(int, int)>>();
	/// <summary>
	/// 层级切换计时器
	/// </summary>
	public static int SwitchingTimer;

	/// <summary>
	/// 切换层级的时候
	/// </summary>
	public virtual void OnLayerSwitching()
	{

	}
	/// <summary>
	/// 返回上一层
	/// </summary>
	public static void LayerShallower(Player player, int x, int y, int step = 1)
	{
		int layer = PlayerZoneLayer[player.whoAmI];
		LayerChange(player, x, y, layer + step);
	}
	/// <summary>
	/// 深入下一层
	/// </summary>
	public static void LayerDeeper(Player player, int x, int y, int step = 1)
	{
		int layer = PlayerZoneLayer[player.whoAmI];
		LayerChange(player, x, y, layer - step);
	}
	/// <summary>
	/// 切换层
	/// </summary>
	public static void LayerChange(Player player, int x, int y, int destinationLayer)
	{
		if (SwitchingTimer == 0)
		{
			int layer = PlayerZoneLayer[player.whoAmI];

			if (destinationLayer < 0)
			{
				ChangeArea(InSwitchingTileCoords, layer, 0);
				InSwitchingTileCoords = new List<(int, int)>();
				if (LayerTile.ContainsKey((x, y, destinationLayer)))
				{
					CheckAreaInLayer(x, y, destinationLayer);
				}
				RoomsInsideLayers[destinationLayer] = InSwitchingTileCoords;
				SwitchingTimer = 60;
				ChangeArea(InSwitchingTileCoords, 0, destinationLayer);
				PlayerZoneLayer[player.whoAmI] = destinationLayer;
			}
			else
			{
				SwitchingTimer = 60;
				ChangeArea(InSwitchingTileCoords, layer, destinationLayer);
				PlayerZoneLayer[player.whoAmI] = destinationLayer;
			}
		}
		else
		{
			return;
		}
	}
	/// <summary>
	/// 把一个区域从一层切换到另一层
	/// </summary>
	/// <param name="coords"></param>
	/// <param name="oldLayer"></param>
	/// <param name="newLayer"></param>
	private static void ChangeArea(List<(int, int)> coords, int oldLayer, int newLayer)
	{
		foreach(var point in coords)
		{
			int x = point.Item1;
			int y = point.Item2;
			WriteToLayerTile(x, y, oldLayer);
			ReadFromLayerTile(x, y, newLayer);
		}
	}
	/// <summary>
	/// 根据一个起始点判连续
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="layer"></param>
	private static void CheckAreaInLayer(int x, int y, int layer)
	{
		if (LayerTile.ContainsKey((x, y, layer)))
		{
			var target = LayerTile[(x, y, layer)];
			if (!InSwitchingTileCoords.Contains((x, y)) && target.TileType != ModContent.TileType<AirTile>())
			{
				InSwitchingTileCoords.Add((x, y));
				CheckAreaInLayer(x + 1, y, layer);
				CheckAreaInLayer(x, y + 1, layer);
				CheckAreaInLayer(x - 1, y, layer);
				CheckAreaInLayer(x, y - 1, layer);
			}
			else
			{
				return;
			}
		}
		else
		{
			return;
		}
		
	}
	/// <summary>
	/// 简单的初始化
	/// </summary>
	public override void OnWorldLoad()
	{
		PlayerZoneLayer[Main.LocalPlayer.whoAmI] = 0;
	}
	/// <summary>
	/// 写入房间信息
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="layer"></param>
	public static void WriteToLayerTile(int x, int y, int layer)
	{
		var myTile = new TileClone();
		var tile = Main.tile[x, y];
		myTile.I = x;
		myTile.J = y;
		myTile.TileType = tile.TileType;
		myTile.Wall = tile.wall;
		myTile.TileFrameX = tile.TileFrameX;
		myTile.TileFrameY = tile.TileFrameY;
		myTile.WallFrameX = tile.WallFrameX;
		myTile.WallFrameY = tile.WallFrameY;
		myTile.BlockType = tile.BlockType;
		myTile.SlopeType = tile.Slope;
		myTile.Liquid = tile.liquid;
		myTile.LiquidAmount = tile.LiquidAmount;
		myTile.HasTile = tile.HasTile;
		myTile.RedWire = tile.RedWire;
		myTile.GreenWire = tile.GreenWire;
		myTile.BlueWire = tile.BlueWire;
		myTile.YellowWire = tile.YellowWire;
		myTile.WallColor = tile.WallColor;
		myTile.TileColor = tile.TileColor;
		LayerTile[(x, y, layer)] = myTile;
	}
	/// <summary>
	/// 读取房间信息
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="layer"></param>
	public static void ReadFromLayerTile(int x, int y, int layer)
	{
		if (LayerTile.ContainsKey((x, y, layer)))
		{
			var myTile = LayerTile[(x, y, layer)];
			var tile = Main.tile[x, y];

			tile.TileType = myTile.TileType;
			tile.wall = myTile.Wall;
			tile.TileFrameX = myTile.TileFrameX;
			tile.TileFrameY = myTile.TileFrameY;
			tile.WallFrameX = myTile.WallFrameX;
			tile.WallFrameY = myTile.WallFrameY;
			tile.BlockType = myTile.BlockType;
			tile.Slope = myTile.SlopeType;
			tile.liquid = myTile.Liquid;
			tile.LiquidAmount = myTile.LiquidAmount;
			tile.RedWire = myTile.RedWire;
			tile.GreenWire = myTile.GreenWire;
			tile.BlueWire = myTile.BlueWire;
			tile.YellowWire = myTile.YellowWire;
			tile.HasTile = myTile.HasTile;
			tile.WallColor = myTile.WallColor;
			tile.TileColor = myTile.TileColor;
		}
	}
	private static void CheckOutOfTheRoom()
	{
		Player player = Main.LocalPlayer;
		int layer = PlayerZoneLayer[player.whoAmI];
		if (PlayerZoneLayer[player.whoAmI] != 0 && SwitchingTimer == 0)
		{
			bool saft = false;
			foreach ((int, int) coord in RoomsInsideLayers[layer])
			{
				Rectangle roomArea = new Rectangle(coord.Item1 * 16, coord.Item2 * 16, 16, 16);
				if (roomArea.Intersects(player.Hitbox))
				{
					saft = true;
					break;
				}
			}
			if (!saft && SwitchingTimer == 0)
			{
				int x = (int)((player.oldPosition.X - player.velocity.X) / 16);
				int y = (int)((player.oldPosition.Y - player.velocity.Y) / 16);
				LayerShallower(player, x, y);
				Main.NewText("Attempt to leave the room illegally.", Color.Red);
			}
		}
	}
	public override void PostUpdateEverything()
	{
		base.PostUpdateEverything();
		if(SwitchingTimer > 0)
		{
			SwitchingTimer--;
		}
		else
		{
			SwitchingTimer = 0;
		}
		CheckOutOfTheRoom();
		//if(Main.time % 60 == 0)
		//{
		//	Main.NewText(PlayerZoneLayer[player.whoAmI]);
		//}
	}
}