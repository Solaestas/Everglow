using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DemonScythe
{
    public class DemoSparkVFX : Visual
    {
        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            //DrawSpark(Color.White, Math.Min(Projectile.timeLeft / 8f, 20f), Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkDark"));
            //DrawSpark(new Color(131, 0, 255, 0), Math.Min(Projectile.timeLeft / 8f, 20f), Common.MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/SparkLight"));
        }

        public override CallOpportunity DrawLayer => throw new NotImplementedException();

        private void DrawSpark(Color c0, float width, Texture2D tex)
        {
            List<Vertex2D> bars = new List<Vertex2D>();
        }
    }
}