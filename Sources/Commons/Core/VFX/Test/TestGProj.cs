using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Sources.Commons.Core.VFX.Test
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
        public override void SetDefaults(Projectile projectile)
        {
            VFXManager.Instance.Add(new TestBindVisual(projectile));
        }
    }
}
