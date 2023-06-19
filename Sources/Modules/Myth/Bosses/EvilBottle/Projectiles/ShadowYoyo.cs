using System;
using Everglow.Myth.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
	public class ShadowYoyo : ModProjectile
	{
		private bool M = false;
        private float X = 0;
		public override void SetDefaults()
		{
			base.Projectile.CloneDefaults(547);
            base.Projectile.width = 16;
			base.Projectile.height = 16;
			base.Projectile.scale = 1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;
        }
		public override void AI()
		{
            ProjectileExtras.YoyoAI(base.Projectile.whoAmI, 60f, 300f, 14f);
            float num20 = base.Projectile.Center.X;
            float num30 = base.Projectile.Center.Y;
            float num4 = 400f;
            bool flag = false;
            int[] Ix = new int[201];
            for (int j = 0; j < 200; j++)
            {
                Ix[j] = -1;
                if (Collision.CanHit(base.Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1) && Main.npc[j].active && !Main.npc[j].friendly && !Main.npc[j].dontTakeDamage)
                {
                    float num5 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
                    float num6 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
                    float num7 = Math.Abs(base.Projectile.position.X + (float)(base.Projectile.width / 2) - num5) + Math.Abs(base.Projectile.position.Y + (float)(base.Projectile.height / 2) - num6);
                    if (num7 < num4)
                    {
                        num4 = num7;
                        num20 = num5;
                        num30 = num6;
                        Ix[j] = j;
                        flag = true;
                    }
                }
            }
            for (int j = 0; j < 200; j++)
            {
                if (Ix[j] != -1)
                {
                    if (Main.rand.Next(20) == 1)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), base.Projectile.Center.X, base.Projectile.Center.Y, 0, 0, ModContent.ProjectileType<EvilLightingbolt3>(), base.Projectile.damage * 4, base.Projectile.knockBack, base.Projectile.owner, Main.npc[Ix[j]].Center.X, Main.npc[Ix[j]].Center.Y);
                        Main.npc[Ix[j]].AddBuff(153, 300);
                        Main.npc[Ix[j]].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.65f,1.05f)), 0, Projectile.direction, Main.rand.NextFloat(0f, 1f) > 0.75f ? false : true);
                        for (int z = 0; z < 15; z++)
                        {
                            Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0f, 3f)).RotatedByRandom(Math.PI * 2);
                            int num = Dust.NewDust(Main.npc[Ix[j]].Center - new Vector2(4, 4), 2, 2, 21, v0.X, v0.Y, 0, default(Color), Main.rand.NextFloat(0.5f, 1.2f));
                        }
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(153, 900);
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.Draw(MythContent.QuickTexture("Bosses/EvilBottle/Projectiles/ShadowYoyoGlow"), base.Projectile.Center - Main.screenPosition, null, Color.White * 0.7f, base.Projectile.rotation, new Vector2(8f, 8f), 1f, SpriteEffects.None, 0f);
        }
    }
}
