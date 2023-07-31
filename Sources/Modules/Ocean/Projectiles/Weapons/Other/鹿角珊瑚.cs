using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.GameContent;
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
namespace Everglow.Ocean.Projectiles.Weapons.Other
{
    //135596
    public class 鹿角珊瑚 : ModProjectile
    {
        //4444444
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("鹿角珊瑚");
        }
        private bool num100 = true;
        private Vector2 v = new Vector2(0, 0);
        private Vector2 v2 = new Vector2(0, 0);
        //7359668
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = (int)1.5f;
            Projectile.timeLeft = 40;
            Projectile.alpha = 0;
            Projectile.penetrate = 2;
            Projectile.scale = 1f;
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }
        //55555
        public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(100, 100, 100, 0));
		}
        public override void AI()
        {
            if (num100)
            {
                v = Projectile.velocity.RotatedBy(Math.PI * 0.07f * (4 - Projectile.ai[0])) * 0.95f;
                v2 = Projectile.velocity.RotatedBy(Math.PI * -0.07f * (4 - Projectile.ai[0])) * 0.95f;
                if (base.Projectile.ai[0] != 0)
                {
                    Projectile.timeLeft = (int)(Projectile.ai[0] * 17f + 10);
                }
                num100 = false;
            }
            base.Projectile.velocity *= 0.995f;
            base.Projectile.rotation = (float)System.Math.Atan2((double)Projectile.velocity.Y,(double)Projectile.velocity.X) + 1.57f;
            NPC target = null;
            float num2 = base.Projectile.Center.X;
			float num3 = base.Projectile.Center.Y;
			float num4 = 400f;
            bool flag = false;
            if (Projectile.timeLeft == 24)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), base.Projectile.Center.X, base.Projectile.Center.Y, v.X, v.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.鹿角珊瑚>(), base.Projectile.damage, base.Projectile.knockBack, Main.myPlayer, Projectile.ai[0] - 1, 0f);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), base.Projectile.Center.X, base.Projectile.Center.Y, v2.X, v2.Y, ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.鹿角珊瑚>(), base.Projectile.damage, base.Projectile.knockBack, Main.myPlayer, Projectile.ai[0] - 1, 0f);
                Projectile.velocity *= 0.1f;
            }
        }
        //14141414141414
        //14141414141414
        //14141414141414
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
            int frameHeight = 20;
            Vector2 value = new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y);
            Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            Vector2 vector2 = value - Main.screenPosition;
            if(Projectile.timeLeft < 80)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * (((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 80f * (float)Projectile.timeLeft);
                    Main.EntitySpriteDraw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/紫灰色"), drawPos, new Rectangle(0, frameHeight * Projectile.frame, ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/红色").Width(), frameHeight), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            else
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/紫灰色"), drawPos, new Rectangle(0, frameHeight * Projectile.frame, ModContent.Request<Texture2D>("Everglow/Ocean/Projectiles/红色").Width(), frameHeight), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
    }
}