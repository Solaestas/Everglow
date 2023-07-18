namespace Everglow.Example.TileLayers;
public class TileLayerSystem : ModSystem
{
	/// <summary>
	/// (xy坐标，层)的物块
	/// </summary>
	public static Dictionary<(int, int, int), Tile> LayerTile = new Dictionary<(int, int, int), Tile>();

	/// <summary>
	/// 玩家所在层
	/// </summary>
	public static List<int> PlayerZoneLayer = new List<int>();

	/// <summary>
	/// 正在层级切换的物块
	/// </summary>
	public static List<Tile> SwitchingTiles = new List<Tile>();

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
	/// 深入下一层
	/// </summary>
	public static void LayerDeeper(Player player, int x, int y, int step = 1)
	{
		if(SwitchingTimer == 0)
		{
			int nextLayer = TileLayerSystem.PlayerZoneLayer[player.whoAmI] - step;
			if(LayerTile.ContainsKey((x, y, nextLayer)))
			{
				CheckAreaInLayer(x, y, nextLayer);
			}
			SwitchingTimer = 60;
		}
		else
		{
			return;
		}
	}
	private static void CheckAreaInLayer(int x, int y, int layer)
	{
		if (LayerTile.ContainsKey((x, y, layer)))
		{
			if(!SwitchingTiles.Contains(LayerTile[(x, y, layer)]))
			{
				SwitchingTiles.Add(LayerTile[(x, y, layer)]);
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
	/// 返回上一层
	/// </summary>
	public static void LayerShallower(Player player, int x, int y, int step = 1)
	{
		if (SwitchingTimer == 0)
		{

		}
		else
		{
			return;
		}
	}


	public override void OnWorldLoad()
	{
		//0层的初始化
		for(int x = 0;x < Main.maxTilesX;x++)
		{
			for (int y = 0; y < Main.maxTilesY; y++)
			{
				LayerTile.Add((x, y, 0), Main.tile[x, y]);
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
			SwitchingTiles = new List<Tile>();
		}
	}
}