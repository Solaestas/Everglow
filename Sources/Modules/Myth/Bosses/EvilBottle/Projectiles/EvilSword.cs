using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Everglow.Myth.Bosses.EvilBottle.Projectiles
{
    public class EvilSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("邪魔剑气");
        }

        public override void SetDefaults()
        {
            base.Projectile.width = 28;
            base.Projectile.height = 28;
            base.Projectile.aiStyle = 27;
            base.Projectile.friendly = true;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.ignoreWater = true;
            base.Projectile.penetrate = 1;
            Projectile.alpha = 255;
            base.Projectile.extraUpdates = 3;
            base.Projectile.timeLeft = 600;
            base.Projectile.usesLocalNPCImmunity = true;
            Projectile.tileCollide = true;
            base.Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            if (Projectile.alpha > 5)
            {
                Projectile.alpha -= 5;
            }
            int num9 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) - new Vector2(4, 4) - (base.Projectile.velocity * 3f).RotatedBy(((float)Math.Sin(Main.time / 3d) / 3f)), 4, 4, 27, 0f, 0f, 100, default(Color), 2f);
            Main.dust[num9].noGravity = true;
            Main.dust[num9].velocity *= 0.2f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color(255, 255, 255, base.Projectile.alpha));
        }
        public override void Kill(int timeLeft)
        {
            //int k = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, 0, 0, 612, projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
            //Main.projectile[k].timeLeft = 30;
            for (int i = 0; i < 15; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(Math.PI * 2f);
                int num9 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) - new Vector2(4, 4), 0, 0, 27, v.X, v.Y, 100, default(Color), 2.4f);
                Main.dust[num9].noGravity = true;
                Main.dust[num9].velocity *= 0.0f;
            }
            for (int i = 0; i < 9; i++)
            {
                Vector2 v = new Vector2(0, Main.rand.NextFloat(0, 7f)).RotatedByRandom(Math.PI * 2f);
                int num9 = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) - new Vector2(4, 4), 0, 0, 27, v.X, v.Y, 100, default(Color), 1.8f);
                Main.dust[num9].noGravity = true;
                Main.dust[num9].velocity *= 0.0f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 v = Projectile.velocity.RotatedByRandom(Math.PI * 2) * 0.4f;
                int num4 = Projectile.NewProjectile(Projectile.GetItemSource_OnHit(target, ModContent.ItemType<Items.EvilSword>()), base.Projectile.Center.X, base.Projectile.Center.Y, v.X, v.Y, ModContent.ProjectileType<EvilSword2>(), base.Projectile.damage / 2, base.Projectile.knockBack, base.Projectile.owner, 0f, 20);
            }
            target.AddBuff(153, 900);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Mod mod = ModLoader.GetMod("MythMod");
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, base.Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(20, 20), Projectile.scale, SpriteEffects.None, 0f);
            for(int i =0;i < 4;i++)
            {
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity * i * 3f, null, new Color(1 - 1 / 4f * (float)i, 1 - 1 / 4f * (float)i, 1 - 1 / 4f * (float)i, (1 - 1 / 4f * (float)i) * Projectile.alpha / 255f), Projectile.rotation, new Vector2(20, 20), Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
