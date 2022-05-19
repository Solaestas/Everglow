using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Modules.ZY.InvasionSystem
{
    internal class InvasionSystem : IModule
    {
        public static Invasion CurrentInvasion { get; private set; }
        public string Name => "InvasionSystem";
        public static bool InvasionBegin<T>() where T : Invasion
        {
            if(Main.invasionProgressMode != 0)
            {
                return false;
            }
            var invasion = Everglow.ModuleManager.GetModule<T>();
            CurrentInvasion = invasion;
            return true;
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
            CurrentInvasion.Draw();
        }

        public void Unload()
        {
            CurrentInvasion = null;
        }
    }
}
