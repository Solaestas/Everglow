using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.GlobalNPCs
{
    public class SpawnControl : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            foreach (var kv in pool)
            {
                // 生成位置在流萤地形内的原版 NPC 禁止生成
                if (!CanSpawnToFirefly(kv.Key, spawnInfo))
                {
                    pool.Remove(kv.Key);
                }
            }
        }

        // 能否生成与流萤地形，如果给流萤地生物创建好集合了，还需要在这个地方修改下
        public static bool CanSpawnToFirefly(int type, NPCSpawnInfo spawnInfo)
        {
            // 这个 true 是为了提醒只要判断是不是流萤地形生物就好，因为不需要判断是不是 0
            // 两个判断条件，一个是在没在范围内，另一个是生成敌怪脚下是不是流萤地的砖
            if ((type == 0 || true) && (InFirefly(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY) && spawnInfo.SpawnTileType == ModContent.TileType<Tiles.DarkCocoon>()))
            {
                return false;
            }
            return true;
        }

        // 判断坐标是否在流萤地形内
        public static bool InFirefly(int tileX, int tileY)
        {
            MothLand mothLand = ModContent.GetInstance<MothLand>(); // 不确定是否需要考虑联机问题
            Vector2 BiomeCenter = new Vector2(mothLand.fireflyCenterX * 16, (mothLand.fireflyCenterY - 20) * 16);
            Vector2 distance = new Vector2(tileX * 16, tileY * 16) - BiomeCenter; // 距离中心Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f
            distance.Y *= 1.35f;
            distance.X *= 0.9f;
            return distance.Length() < 2000;
        }
    }
}
