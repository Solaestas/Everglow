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
namespace MythMod.Items.Weapons
{
    public class VolcanoBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("击中敌人有概率引发烈火爆炸");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            item.damage = 185;
            item.melee = true;
            item.width = 62;
            item.height = 68;
            item.useTime = 6;
            item.rare = 4;
            item.useAnimation = 14;
            item.useStyle = 1;
            item.knockBack = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 15;
            item.value = 60000;
            item.scale = 1f;//大小
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
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
            float num1 = (float)damage * 0.04f;
            if ((int)num1 == 0)
            {
                return;
            }
            if (Main.rand.Next(5) == 1)
            {
                Projectile.NewProjectile(target.Center.X, target.Center.Y, 2f, 2f, base.mod.ProjectileType("火山爆炸"), damage * 3, knockback, player.whoAmI, 0f, 0f);
            }
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
