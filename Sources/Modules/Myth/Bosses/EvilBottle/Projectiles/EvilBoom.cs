using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Everglow.Myth.Bosses.EvilBottle.Dusts;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class EvilBoom : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("影炸弹");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.alpha = 0;
			Projectile.penetrate = 11;
			Projectile.timeLeft = 3595;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = true;
        }
        private float Y = 0;
        private float X = 0;
        private float ω = 0;
        private bool Orbit = false;
        public override void AI()
		{
            base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) - (float)Math.PI * 0.5f;
            if (Projectile.timeLeft < 3595)
            {
                Vector2 vector = base.Projectile.Center;
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, 86, 50f, 50f, 0, default(Color), (float)Projectile.scale * 1.2f);
                Main.dust[num].velocity *= 0.0f;
                Main.dust[num].noGravity = true;
                Main.dust[num].alpha = 150;
            }
            if (Projectile.velocity.Y < 2.5f)
            {
                Projectile.velocity.Y += 0.04f;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(new SoundStyle("Sounds/烟花爆炸"), (int)Projectile.Center.X, (int)Projectile.Center.Y);
            for (int i = 0; i < 120; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2.9f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, ModContent.DustType<DarkF2>(), 0f, 0f, 100, Color.White, (float)(4f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v;
            }
        }
    }
}
