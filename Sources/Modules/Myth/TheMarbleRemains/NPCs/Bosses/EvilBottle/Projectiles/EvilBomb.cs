using System;
using Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Dusts;
using Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
    public class EvilBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("魔焰法瓶");
		}
        private float num = 0;
        public override void SetDefaults()
		{
			base.Projectile.width = 28;
			base.Projectile.height = 28;
			base.Projectile.friendly = true;
			base.Projectile.alpha = 0;
			base.Projectile.penetrate = 1;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Throwing;
            base.Projectile.aiStyle = -1;
		}
        float timer = 0;
        static float j = 0;
        static float m = 0.15f;
        static float n = 0;
        private bool x = false;
        Vector2 pc2 = Vector2.Zero;
        public override void AI()
        {
            Projectile.rotation = (float)System.Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f + num;
            if(Projectile.timeLeft <= 250 && !x)
            {
                num += 0.15f;
            }
            if(x)
            {
                num += m;
                if(m > 0)
                {
                    m -= 0.005f;
                }
                else
                {
                    m = 0;
                }
            }
            if (Projectile.velocity.Y < 15f && !x)
            {
                Projectile.velocity.Y += 0.2f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (base.Projectile.penetrate <= 0)
            {
                base.Projectile.Kill();
            }
            else
            {
                if(Projectile.velocity.Length() > 0.5f)
                {
                    if (base.Projectile.velocity.X != oldVelocity.X)
                    {
                        base.Projectile.velocity.X = -oldVelocity.X * 0.6f;
                    }
                    if (base.Projectile.velocity.Y != oldVelocity.Y)
                    {
                        base.Projectile.velocity.Y = -oldVelocity.Y * 0.6f;
                    }
                }
                else
                {
                    base.Projectile.velocity.Y *= 0;
                    base.Projectile.velocity.X *= 0;
                    x = true;
                }
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
			SoundStyle 烟花爆炸 = new SoundStyle("Everglow/Myth/Sounds/烟花爆炸");
			烟花爆炸.MaxInstances = 10;

			SoundEngine.PlaySound(烟花爆炸, (int)Projectile.Center.X, (int)Projectile.Center.Y);
            for (int i = 0; i < 120; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(2.9f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, ModContent.DustType<DarkF2>(), 0f, 0f, 100, Color.White, (float)(4f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v;
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0f, (float)(2.4 * Math.Log10(Projectile.damage)))).RotatedByRandom(Math.PI * 2);
                int num5 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, ModContent.DustType<DarkF2>(), 0f, 0f, 100, Color.White, (float)(16f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v * 0.1f;
            }
            /*for (int i = 0; i < 80; i++)
            {
                Vector2 v = new Vector2(0, 4f).RotatedByRandom(Math.PI * 2);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, v.X, v.Y, mod.ProjectileType("littleEvilfire0"), projectile.damage, 0f, Main.myPlayer, 0, 0f);
            }*/
            for (int i = 0; i < 200; i++)
            {
                if((Main.npc[i].Center - Projectile.Center).Length() < 50 && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
                {
                    if(Main.rand.Next(100) > 25)
                    { 
                        Main.npc[i].StrikeNPC(Projectile.damage, 0, 1, false);
                    }
                    else
                    {

                        Main.npc[i].StrikeNPC(Projectile.damage, 0, 1, true);
                    }
                    Main.npc[i].AddBuff(153, 900);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                Vector2 v = (new Vector2(4, 4f).RotatedBy(Projectile.rotation - Math.PI / 2d)).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f));
                int u = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, v.X, v.Y, ModContent.ProjectileType<EvilSword3>(), Projectile.damage / 5, 0f, Main.myPlayer, 0, 0f);
                Main.projectile[u].timeLeft = 180;
            }
            for (int i = 0; i < 100; i++)
            {
                float S = Main.rand.NextFloat(0f, 40f);
                Vector2 v = (new Vector2(S, S).RotatedBy(Projectile.rotation - Math.PI / 2d)).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f));
                int num5 = Dust.NewDust(new Vector2(base.Projectile.position.X, base.Projectile.position.Y), base.Projectile.width, base.Projectile.height, ModContent.DustType<DarkF2>(), 0f, 0f, 100, Color.White, (float)(4f * Math.Log10(Projectile.damage)));
                Main.dust[num5].velocity = v;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
            target.AddBuff(153, 900);
        }
	}
}
