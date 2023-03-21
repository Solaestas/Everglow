using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Modules.FoodModule
{
    internal class FoodSystem : IModule
    {
        string IModule.Name => "食物系统";
        void IModule.Load()
        {
            On.Terraria.Player.UpdateStarvingState += Player_UpdateStarvingState;
        }
        void IModule.Unload()
        {
            On.Terraria.Player.UpdateStarvingState -= Player_UpdateStarvingState;
        }
        private void Player_UpdateStarvingState(On.Terraria.Player.orig_UpdateStarvingState orig, Player self ,bool withEmote)
        {
            orig(self,false);
        }
    }
}
