using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.ExampleModule.VFX
{
    /// <summary>
    /// 测试Visual，与所有弹幕绑定，使用世界坐标系
    /// </summary>
    [Pipeline(typeof(WCSPipeline))]
    internal class TestBindVisual : ProjBindVisual
    {
        public TestBindVisual()
        {
        }

        public TestBindVisual(Projectile proj) : base(proj)
        {
        }

        public override CallOpportunity DrawLayer => CallOpportunity.PostDrawProjectiles;

        public override void Draw()
        {
            VFXManager.spriteBatch.BindTexture(TextureAssets.MagicPixel.Value).Draw(entity.Center, new Rectangle(0, 0, 16, 16), Color.White, 0, entity.Size / 2, 1, SpriteEffects.None);
        }
    }
    internal class TestGProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int timer = 0;
        public override void SetDefaults(Projectile projectile)
        {
            //VFXManager.Instance.Add(new TestBindVisual(projectile));

        }
        public override bool PreAI(Projectile projectile)
        {
            if(timer++ % 30 == 0)
            VFXManager.Instance.Add(new CurseFlameDust(
                Main.rand.Next(24, 40),
                projectile.Center,
                projectile.velocity * 2.5f,
                Main.rand.NextFloat(0f, 0.95f),
                Main.rand.NextFloat(-0.05f, 0.15f),
                Main.rand.NextFloat(13f, 25f),
                projectile.whoAmI));
            return true;
        }
    }
}
