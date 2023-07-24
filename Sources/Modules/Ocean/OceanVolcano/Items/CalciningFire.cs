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
namespace MythMod.Items.Volcano
{
    public class CalciningFire : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "灼烧之怒");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 300;
			base.item.width = 52;
			base.item.height = 28;
			base.item.useTime = 5;
			base.item.useAnimation = 5;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.ranged = true;
			base.item.knockBack = 1f;
			base.item.value = 50000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item31;
			base.item.autoReuse = true;
            base.item.shoot = 14;
			base.item.shootSpeed = 20f;
			base.item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Projectile.NewProjectile(position.X, position.Y + Main.rand.Next(-1, 2) * 6f, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            if((int)(Main.time / 5f) % 5 == 0)
            {
                int k = Projectile.NewProjectile(position.X + speedX * 3, position.Y + Main.rand.Next(-2, 2) + speedY * 3, speedX, speedY, 34, damage * 2, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[k].friendly = true;
                Main.projectile[k].hostile = false;
                Main.projectile[k].extraUpdates = 3;
            }
            return false;
		}
		public override bool ConsumeAmmo(Player player)
		{
			return Main.rand.Next(0, 100) > 66;
		}
		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6.0f, 0.0f);
        }
    }
}
