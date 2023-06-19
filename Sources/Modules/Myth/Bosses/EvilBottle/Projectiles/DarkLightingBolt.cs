using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Everglow.Myth.Bosses.EvilBottle.Dusts;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class DarkLightingBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("闪电霹雳");
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 80;
            Projectile.timeLeft = 900;
            Projectile.alpha = 255;
            Projectile.penetrate = 3;
            Projectile.scale = 1f;
            this.CooldownSlot = 1;
        }
        public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, base.Projectile.alpha));
		}
        private bool chase = false;
        private bool initialization = true;
        private double X;
        private float Y = 0;
        private float b;
        private float M = 0;
        private float N = 0;
        public override void AI()
        {
			SoundStyle 雷击 = new SoundStyle("Everglow/Myth/Sounds/雷击");
			雷击.MaxInstances = 10;

			if (Projectile.timeLeft == 899)
            {
                SoundEngine.PlaySound(雷击, (int)Projectile.Center.X, (int)Projectile.Center.Y);
            }
            if (Projectile.timeLeft < 872)
            {
                Lighting.AddLight(base.Projectile.Center, (float)(255 - base.Projectile.alpha) * 0.8f / 255f, (float)(255 - base.Projectile.alpha) * 0.8f / 255f, (float)(255 - base.Projectile.alpha) * 1.0f / 255f);
                base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X);
                if (Projectile.timeLeft % 2 == 0)
                {
                    int num25 = Dust.NewDust(base.Projectile.Center, 0, 0, ModContent.DustType<DarkF2>(), 0, 0, 150, default(Color), (float)(1.4f * Math.Log10(Projectile.damage)));
                    Main.dust[num25].noGravity = true;
                    Main.dust[num25].velocity.X = 0;
                    Main.dust[num25].velocity.Y = 0;
                }
            }
            if (Projectile.timeLeft % 6 == 1 && Projectile.timeLeft < 872 && !chase && Main.rand.Next(2) == 1)
            {
                Vector2 v2 = new Vector2(Projectile.ai[0], Projectile.ai[1]) - Projectile.Center;
                if (v2.Length() > 500)
                {
                    M = Projectile.Center.X + v2.X * 2000f;
                    N = Projectile.Center.Y + v2.Y * 2000f;
                }
                if (v2.Length() > 30)
                {
                    v2 = v2 / v2.Length() * 2;
                    if (!chase)
                    {
                        float num1 = (float)(Main.rand.NextFloat(-500, 500) / 800f);
                        Projectile.velocity = v2.RotatedBy(Math.PI * num1);
                        if (Projectile.timeLeft < 600)
                        {
                            Projectile.timeLeft = 600;
                        }
                    }
                }
                else
                {
                    v2 = v2 / v2.Length() * 2;
                    if (!chase)
                    {
                        Projectile.timeLeft = 872;
                        chase = true;
                        float num1 = (float)(Main.rand.NextFloat(-v2.Length() * 5, v2.Length() * 5) / 800f);
                        Projectile.velocity = v2.RotatedBy(Math.PI * num1);
                    }
                }
            }
            if (Projectile.timeLeft % 6 == 1 && Projectile.timeLeft < 872 && Main.rand.Next(2) == 1 && chase)
            {
                Vector2 v2 = new Vector2(M, N) - Projectile.Center;
                v2 = v2 / v2.Length() * 2;
                float num1 = (float)(Main.rand.NextFloat(-500, 500) / 800f);
                Projectile.velocity = v2.RotatedBy(Math.PI * num1);
            }
            if (Main.rand.Next(1, 900) == 3)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), base.Projectile.Center.X, base.Projectile.Center.Y, base.Projectile.velocity.X, base.Projectile.velocity.Y, ModContent.ProjectileType<DarkLighting>(), 0, 2f, Main.myPlayer, 0f, 0);
            }
            float num2 = base.Projectile.Center.X;
            float num3 = base.Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(base.Projectile, false) && Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < 50)
                    {
                        Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f,1.15f)), Projectile.knockBack, Projectile.direction, Main.rand.Next(200) > 150 ? true : false);
                        Projectile.penetrate--;
                        NPC target = Main.npc[j];
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
		{
			/*for (int j = 0; j < 4; j++)
			{
                float num44 = (float)Main.rand.Next(0, 3600) / 1800 * 3.14159265359f;
                double num45 = Math.Cos((float)num44);
                double num46 = Math.Sin((float)num44);
                float num47 = (float)Main.rand.Next(0, 10000) / 50000;
                int num40 = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, (float)num45 * (float)num47, (float)num46 * (float)num47, base.ModContent.ProjectileType<>("Lighting2"), 70, 2f, Main.myPlayer, 0f, 0);
                Main.projectile[num40].timeLeft = 800;
                Main.projectile[num40].tileCollide = false;
			}*/
            if(timeLeft != 0)
            {
                for (int a = 0; a < 150; a++)
                {
                    Vector2 v = new Vector2(0, Main.rand.Next(25, 175) / 150f).RotatedByRandom(Math.PI * 2) * (float)Math.Log10(Projectile.damage);
                    int num25 = Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<DarkF2>(), v.X, v.Y, 150, default(Color), Main.rand.NextFloat(0, (float)(0.5f * Math.Log10(Projectile.damage))));
                    Main.dust[num25].noGravity = false;
                }
            }
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 200);
        }
    }
}