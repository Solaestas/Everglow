using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Dusts;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
    public class DarkFlameball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("恶魔火球");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 3600;
            Projectile.alpha = 0;
            Projectile.penetrate = 3;
            Projectile.scale = 1f;
            this.CooldownSlot = 1;
        }
        private bool initialization = true;
        private double X;
        private float Y;
        private float b;
        private float rg = 0;
        public override void AI()
        {
            base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) - (float)Math.PI * 0.5f;
            if (Projectile.timeLeft < 3595)
            {
                Vector2 vector = base.Projectile.Center;
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, ModContent.DustType<DarkF>(), 50f, 50f, 0, default(Color), (float)Projectile.scale * 2.4f);
                Main.dust[num].velocity *= 0.0f;
                Main.dust[num].noGravity = true;
                Main.dust[num].alpha = 150;
            }
            if(Projectile.velocity.Y < 15)
            {
                Projectile.velocity.Y += 0.2f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, 150));
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            base.Projectile.penetrate--;
            if (base.Projectile.penetrate <= 0)
            {
                base.Projectile.Kill();
            }
            else
            {
                base.Projectile.ai[0] += 0.1f;
                if (base.Projectile.velocity.X != oldVelocity.X)
                {
                    base.Projectile.velocity.X = -oldVelocity.X;
                }
                if (base.Projectile.velocity.Y != oldVelocity.Y)
                {
                    base.Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(153, 900);
        }
    }
}