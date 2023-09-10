using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
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
    public class LavaCupStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("熔岩灌顶");
			// base.Tooltip.SetDefault("召唤熔岩杯,请你多喝岩浆");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 140;
			base.Item.mana = 30;
			base.Item.width = 34;
			base.Item.height = 34;
			base.Item.useTime = 36;
			base.Item.useAnimation = 36;
			base.Item.useStyle = 1;
			base.Item.noMelee = true;
			base.Item.knockBack = 2.25f;
			base.Item.value = 50000;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item44;
			base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.LavaCup>();
			base.Item.autoReuse = true;
			base.Item.shootSpeed = 10f;
			base.Item.DamageType = DamageClass.Summon;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            player.AddBuff(base.Mod.Find<ModBuff>("LavaCup").Type, 3600, true);
            float shootSpeed = base.Item.shootSpeed;
			Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
			float num = (float)Main.mouseX + Main.screenPosition.X - vector.X;
			float num2 = (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
			if (player.gravDir == -1f)
			{
				num2 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector.Y;
			}
			float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			if ((float.IsNaN(num) && float.IsNaN(num2)) || (num == 0f && num2 == 0f))
			{
				num = (float)player.direction;
			}
			else
			{
				num3 = shootSpeed / num3;
			}
			num = 0f;
			num2 = 0f;
			vector.X = (float)Main.mouseX + Main.screenPosition.X;
			vector.Y = (float)Main.mouseY + Main.screenPosition.Y;
			Projectile.NewProjectile(null, vector.X, vector.Y, num, num2,ModContent.ProjectileType<Everglow.Ocean.Projectiles.LavaCup>(), damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
