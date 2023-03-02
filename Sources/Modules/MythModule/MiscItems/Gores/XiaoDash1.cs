using Everglow.Sources.Modules.MythModule.LanternMoon.Gores;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Gores
{
    public class XiaoDash1 : DissolveGore
    {
        public override string EffectPath()
        {
            return "Effects/WindDust";
        }
        public override bool Update(Gore gore)
        {
            gore.velocity.Y -= LightValue;
            gore.timeLeft -= 20;
            gore.rotation -= 0.4f;
            return base.Update(gore);
        }
    }
}
