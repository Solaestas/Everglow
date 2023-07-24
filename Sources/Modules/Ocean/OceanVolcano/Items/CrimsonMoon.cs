using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
namespace MythMod.Items.Volcano
{
    public class CrimsonMoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.AddTranslation(GameCulture.Chinese, "赤月");
            Tooltip.SetDefault("灼热导致它真实伤害远远高于面板");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            item.damage = 200;
            item.melee = true;
            item.width = 52;
            item.height = 62;
            item.useTime = 24;
            item.rare = 11;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 22;
            item.value = 10000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("CrisomMoon");
            item.shootSpeed = 8f;
        }
        private int a = 0;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (a % 10 == 1)
            {
                for(int y =0;y < 12;y++)
                {
                    int u = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("CrisomMoon2"), damage, knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[u].rotation = Main.rand.NextFloat((MathHelper.TwoPi));
                }
            }
            else
            {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            }
            a += 1;
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 900, false);
        }
    }
}
