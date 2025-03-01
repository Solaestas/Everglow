namespace Everglow.Yggdrasil.Common.Fish;

public class FishSystem : ModSystem
{
	public static Dictionary<ModBiome, List<FishableItem>> FishMap = [];

	/// <summary>
	/// 注册一种可以被钩取的渔获，会自然生成在指定生态群系的水面上
	/// </summary>
	/// <param name="biome">会自然生成渔获的mod群系</param>
	/// <param name="item">会自然生成的物品</param>
	public static void RegisterFish(ModBiome biome, FishableItem item)
	{
		if (!FishMap.TryGetValue(biome, out List<FishableItem> value))
		{
			value = [];
			FishMap[biome] = value;
		}

		value.Add(item);
	}

	public List<FishableItem> ShouldSpawnFish(Player player)
	{
		List<FishableItem> toSpawn = [];
		foreach (KeyValuePair<ModBiome, List<FishableItem>> kvp in FishMap)
		{
			if (player.InModBiome(kvp.Key))
			{
				foreach (var item in kvp.Value)
				{
					float chance = Main.rand.NextFloat(1);
					if (chance < item.chance)
					{
						toSpawn.Add(item);
					}
				}
			}
		}
		return toSpawn;
	}

	public void CheckSpawn(Player player, Point point, List<FishableItem> toSpawn)
	{
		Tile tile = Main.tile[point];
		List<FishableItem> spawned = [];
		foreach (var item in toSpawn)
		{
			if (tile.LiquidType != item.Liquid || tile.LiquidAmount < 26)
			{
				continue;
			}
			spawned.Add(item);
			var itemIndex = Item.NewItem(player.GetSource_FromThis(), new Vector2(point.X * 16f, point.Y * 16f), item.Item);
			var itemGen = Main.item[itemIndex];
			if (itemGen != null)
			{
				if (itemGen.TryGetGlobalItem(out FishGlobalItem globalItem))
				{
					globalItem.Fishable = true;
					globalItem.FloatSpeed = Main.rand.NextFloat(.5f);
				}
			}
		}
		foreach (var item in spawned)
		{
			toSpawn.Remove(item);
		}
	}

	public void SpawnAroundPlayer(Player player)
	{
		var toSpawn = ShouldSpawnFish(player);

		// 在刷怪区域的左右两侧生成，上下不生成
		int startX = -84;
		int endX = -74;
		int startY = -47;
		int endY = 47;

		for (int x = startX; x <= endX; x++)
		{
			for (int y = startY; y <= endY; y++)
			{
				Point point = player.Center.ToTileCoordinates() + new Point(x, y);
				CheckSpawn(player, point, toSpawn);
			}
		}

		for (int x = -endX; x <= -startX; x++)
		{
			for (int y = startY; y <= endY; y++)
			{
				Point point = player.Center.ToTileCoordinates() + new Point(x, y);
				CheckSpawn(player, point, toSpawn);
			}
		}
	}

	public override void PostUpdateTime()
	{
		foreach (var player in Main.player)
		{
			if (player.active)
			{
				SpawnAroundPlayer(player);
			}
		}

		base.PostUpdateTime();
	}
}