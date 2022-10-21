using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.NPCs
{
    public class MothLandGlobalNPC : GlobalNPC
    {
        private static HashSet<int> mothLandNPCTypes = new HashSet<int>();
        public static void RegisterMothLandNPC(int type) => mothLandNPCTypes.Add(type);
        //TODO 这编辑刷怪池子编辑了个寂寞，还在照常生怪
        //public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        //{
        //    if (MothBackground.BiomeActive())
        //    {
        //        pool = pool.Where(pair => mothLandNPCTypes.Contains(pair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value);
        //    }
        //}
    }
}
