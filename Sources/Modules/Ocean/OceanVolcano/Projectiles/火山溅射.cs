using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using Everglow.Ocean.Common;

namespace Everglow.Ocean.OceanVolcano.Projectiles
{
    public class 火山溅射 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("火山溅射");
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 34;
			base.Projectile.height = 34;
			base.Projectile.hostile = false;
			base.Projectile.ignoreWater = true;
			base.Projectile.tileCollide = true;
			base.Projectile.penetrate = 5;
			base.Projectile.timeLeft = 240;
            base.Projectile.friendly = true;
			this.CooldownSlot = 0;
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
		}
        public override void AI()
        {
            int num = Dust.NewDust(Projectile.Center - new Vector2(6, 6), 12, 12, Mod.Find<ModDust>("Flame").Type, 0f, 0f, 100, Color.White, 2f);
            if (Projectile.wet)
            {
                Projectile.timeLeft = 0;
            }
            Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.45f * (float)Projectile.scale / 255f, (float)(255 - base.Projectile.alpha) * 0.1f * (float)Projectile.scale / 255f, (float)(255 - base.Projectile.alpha) * 0f / 255f);
            base.Projectile.velocity.Y += 0.15f;
            float num2 = base.Projectile.Center.X;
            float num3 = base.Projectile.Center.Y;
            float num4 = 400f;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC(Projectile.damage, Projectile.knockBack, Projectile.direction, Main.rand.Next(200) > 150 ? true : false);
                        Projectile.penetrate--;
                        if(Projectile.penetrate == 0)
                        {
                            Projectile.Kill();
                        }
                    }
                }
            }
        }
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(1f, 1f, 1f, base.Projectile.alpha / 255f));
		}
		public override void Kill(int timeLeft)
		{
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            for (int i = 0; i < 65; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0.5f, 2f)).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, Mod.Find<ModDust>("Flame").Type, 0f, 0f, 100, Color.White, 4f);
                Main.dust[num5].velocity = v;
            }
        }
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D = TextureAssets.Projectile[base.Projectile.type].Value;
			int num = TextureAssets.Projectile[base.Projectile.type].Value.Height / Main.projFrames[base.Projectile.type];
			int y = num * base.Projectile.frame;
			SpriteEffects effects = SpriteEffects.None;
            if (base.Projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            return true;
		}
	}
}
