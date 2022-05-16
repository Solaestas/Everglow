using Everglow.Sources.Commons.ModuleSystem;

namespace Everglow.Sources.Modules.ZY.InvasionSystem
{
    internal class InvasionSystem : IModule
    {
        private Dictionary<int, Invasion> invasions = new Dictionary<int, Invasion>();
        public string Name => "InvasionSystem";
        public bool InvasionBegin<T>() where T : Invasion
        {
            var invasion = Everglow.ModuleManager.FindModule<T>();
            if(invasion is null || invasion.Count() > 1)
            return false;
            return false;
        }
        public void Load()
        {
            On.Terraria.Main.DrawInterface_15_InvasionProgressBars += Main_DrawInterface_15_InvasionProgressBars;
        }

        private void Main_DrawInterface_15_InvasionProgressBars(On.Terraria.Main.orig_DrawInterface_15_InvasionProgressBars orig)
        {
            if(Main.invasionProgressMode <= Invasion.VanillaCount)
            {
                orig();
                return;
            }
            invasions[Main.invasionProgressMode]
        }

        public void Unload()
        {

        }
    }
}
