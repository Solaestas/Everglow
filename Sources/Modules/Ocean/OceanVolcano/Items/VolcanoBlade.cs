using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
namespace Everglow.Ocean.Items.Weapons
{
    public class VolcanoBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("0†3¡Â0‰00ˆ40…80ˆ40‡60‡90ˆ70ˆ40†00‡30‡00‡80ˆ60‹5¡¤0„40†90ˆ60†30Š8¡À0…10ˆ9¡§");
            //GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        //public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            //Item.glowMask = GetGlowMask;
            Item.damage = 185;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 62;
            Item.height = 68;
            Item.useTime = 6;
            Item.rare = 4;
            Item.useAnimation = 14;
            Item.useStyle = 1;
            Item.knockBack = 9;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 15;
            Item.value = 60000;
            Item.scale = 1f;//0…7¨®0ˆ40„3
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 48; i++)
            {
                int num = Dust.NewDust(target.position, target.width, target.height, 259, target.velocity.X * 0f, target.velocity.Y * 0f, 259, default(Color), 2.4f);
                Main.dust[num].noGravity = true;
            }
            if (target.type == 488)
            {
                return;
            }
            float num1 = (float)damageDone * 0.04f;
            if ((int)num1 == 0)
            {
                return;
            }
            //if (Main.rand.Next(5) == 1)
            //{
            //    Projectile.NewProjectile(target.Center.X, target.Center.Y, 2f, 2f,ModContent.ProjectileType<Everglow.Ocean.Projectiles.>(), damageDone * 3, hit.Knockback, player.whoAmI, 0f, 0f);
            //}
            target.AddBuff(24, 600);
        }
        //15343648
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int num = Main.rand.Next(3);
            if (num == 0)
            {
                num = 6;
            }
            else if (num == 1)
            {
                num = 6;
            }
            else
            {
                num = 6;
            }
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, num, 0, 0, 259, default(Color), 1f);
        }
    }
}
