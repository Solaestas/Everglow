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
namespace Everglow.Ocean.OceanVolcano.Items
{
    public class CrimsonMoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "赤月");
            // Tooltip.SetDefault("灼热导致它真实伤害远远高于面板");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 200;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 52;
            Item.height = 62;
            Item.useTime = 24;
            Item.rare = 11;
            Item.useAnimation = 12;
            Item.useStyle = 1;
            Item.knockBack = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 22;
            Item.value = 10000;
            Item.scale = 1f;
            Item.shoot = ModContent.ProjectileType<Everglow.Ocean.Projectiles.CrisomMoon>();
            Item.shootSpeed = 8f;
        }
        private int a = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (a % 10 == 1)
            {
                for(int y =0;y < 12;y++)
                {
                    int u = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Everglow.Ocean.Projectiles.CrisomMoon2>(), damage, knockBack, player.whoAmI, 0f, 0f);
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
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(24, 900, false);
        }
    }
}
