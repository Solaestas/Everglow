using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
namespace MythMod.Items.Volcano
{
    public class MeteorFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("火陨之杖");
            Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "火陨之杖");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 210;
            base.item.magic = true;
            base.item.mana = 5;
            base.item.width = 54;
            base.item.height = 54;
            base.item.useTime = 30;
            base.item.useAnimation = 30;
            item.crit = 22;
            base.item.useStyle = 5;
            base.item.noMelee = true;
            base.item.knockBack = 0.5f;
            base.item.value = 12000;
            base.item.rare = 11;
            base.item.UseSound = SoundID.Item60;
            base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("MeteorFlame");
            base.item.shootSpeed = 2f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float X = Main.rand.NextFloat(-250, 250);
            float Y = Main.rand.NextFloat(-250, 250);
            Vector2 v2 = (new Vector2(Main.screenPosition.X + Main.mouseX + Main.rand.NextFloat(-24, 24), Main.screenPosition.Y + Main.mouseY + Main.rand.NextFloat(-24, 24)) - new Vector2((float)position.X + X, (float)position.Y - 1000f + Y));
            v2 = v2 / v2.Length() * 13f;
            Projectile.NewProjectile((float)position.X + X, (float)position.Y - 1000f + Y, v2.X, v2.Y, (int)type, (int)damage * 4, (float)knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
