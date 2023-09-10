using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Weapons.Other
{
    public class CrackSoundWave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("爆裂音波");
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 200;
            base.Projectile.scale = 0f;
            base.Projectile.height = 200;
			base.Projectile.hostile = false;
			base.Projectile.ignoreWater = true;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.timeLeft = 120;
            base.Projectile.friendly = true;
			this.CooldownSlot = 1;
		}
        public override void AI()
        {
            if(base.Projectile.ai[0] <= 5)
            {
                base.Projectile.ai[0] -= 0.05f;
            }
            else
            {
                base.Projectile.scale += 0.015f;
                base.Projectile.scale *= 1.02f;
                base.Projectile.ai[0] = 10;
            }
            if (base.Projectile.ai[0] <= 0)
            {
                base.Projectile.scale += 0.015f;
                base.Projectile.scale *= 1.02f;
            }
            if (base.Projectile.friendly)
            {
                for (int j = 0; j < 200; j++)
                {
                    if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1) && Math.Abs((base.Projectile.Center - Main.npc[j].Center).Length() - (100 * base.Projectile.scale)) < 3f && base.Projectile.ai[0] <= 5)
                    {
                        int num = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.npc[j].Center.X, Main.npc[j].Center.Y, 0, 0, base.Mod.Find<ModProjectile>("CrackSoundWave").Type, base.Projectile.damage, base.Projectile.knockBack, Main.myPlayer, 10, 0f);
                        Main.projectile[num].timeLeft = 60;
                    }
                }
            }
            else
            {
                int num5 = (int)Player.FindClosest(base.Projectile.Center, 1, 1);
                if (Math.Abs((Main.player[num5].Center - base.Projectile.Center).Length() - (100 * base.Projectile.scale)) < 3f && base.Projectile.ai[0] <= 5)
                {
                    int num3 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.player[num5].Center.X, Main.player[num5].Center.Y, 0, 0, base.Mod.Find<ModProjectile>("CrackSoundWave").Type, base.Projectile.damage, base.Projectile.knockBack, Main.myPlayer, 10, 0f);
                    Main.projectile[num3].hostile = true;
                    Main.projectile[num3].friendly = false;
                    Main.projectile[num3].timeLeft = 60;
                }
            }
        }
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, 0) * ((Projectile.timeLeft) / 120f));
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
			int num = TextureAssets.Projectile[base.Projectile.type].Value.Height / Main.projFrames[base.Projectile.type];
			int y = num * base.Projectile.frame;
			Main.spriteBatch.Draw(texture2D, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, new Vector2((float)texture2D.Width / 2f, (float)num / 2f), base.Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
    }
}
