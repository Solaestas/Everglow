using Everglow.Sources.Commons.Core.ModuleSystem;

//using Everglow.Sources.Modules.MythModule.LanternMoon.Skies;

namespace Everglow.Sources.Modules.MythModule.Common
{
    public class MythClone : IModule
    {
        public virtual string Name => "Myth";

        public virtual void Load()
        {
            //SkyManager.Instance["LanternSky"] = new LanternSky();
            //Filters.Scene["RainbowVague"] = new Filter(new MythScreenShaderData(new Ref<Effect>(MythContent.QuickEffect("LanternMoon/Projectiles/RainbowVague")), "Test"), EffectPriority.Medium);
            //Filters.Scene["RainbowVague"].Load();
        }

        public virtual void Unload()
        {
        }
    }
}