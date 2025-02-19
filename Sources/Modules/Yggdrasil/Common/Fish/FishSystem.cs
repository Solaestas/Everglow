namespace Everglow.Yggdrasil.Common.Fish;

public class FishSystem : ModSystem
{
	public static Dictionary<int, FishableItem> FishMap = [];

	/// <summary>
	/// 注册一种可以被钩取的渔获，会自然生成在指定生态群系的水面上
	/// </summary>
	/// <param name="biome">会自然生成渔获的mod群系</param>
	/// <param name="item">会自然生成的物品</param>
	public static void RegisterFish(ModBiome biome, FishableItem item)
	{
		FishMap[biome.Type]	= item;
	}

	/// <summary>
	/// 注册一种可以被钩取的渔获，会自然生成在指定生态群系的水面上
	/// </summary>
	/// <param name="biome">会自然生成渔获的原版群系</param>
	/// <param name="item">会自然生成的物品</param>
	public static void RegisterFish(int biome, FishableItem item)
	{
		FishMap[biome] = item;
	}

	public override void PostUpdateTime()
	{
		base.PostUpdateTime();
	}
}