using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
namespace Everglow.Ocean.OceanVolcano.Items
{
    public class MeteorFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("火陨之杖");
            Item.staff[base.Item.type] = true;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "火陨之杖");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.damage = 210;
            base.Item.DamageType = DamageClass.Magic;
            base.Item.mana = 5;
            base.Item.width = 54;
            base.Item.height = 54;
            base.Item.useTime = 30;
            base.Item.useAnimation = 30;
            Item.crit = 22;
            base.Item.useStyle = 5;
            base.Item.noMelee = true;
            base.Item.knockBack = 0.5f;
            base.Item.value = 12000;
            base.Item.rare = 11;
            base.Item.UseSound = SoundID.Item60;
            base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.MeteorFlame>();
            base.Item.shootSpeed = 2f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float X = Main.rand.NextFloat(-250, 250);
            float Y = Main.rand.NextFloat(-250, 250);
            Vector2 v2 = (new Vector2(Main.screenPosition.X + Main.mouseX + Main.rand.NextFloat(-24, 24), Main.screenPosition.Y + Main.mouseY + Main.rand.NextFloat(-24, 24)) - new Vector2((float)position.X + X, (float)position.Y - 1000f + Y));
            v2 = v2 / v2.Length() * 13f;
            Projectile.NewProjectile(null, (float)position.X + X, (float)position.Y - 1000f + Y, v2.X, v2.Y, (int)type, (int)damage * 4, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
