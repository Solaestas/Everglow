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

namespace Everglow.Ocean.OceanDeep.Items.Weapons
{
    public class ScaleShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "火鳞散弹");
			//base.Tooltip.AddTranslation(GameCulture.Chinese, "射出一团子弹\n 24%不消耗弹药");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 100;
			base.Item.width = 52;
			base.Item.height = 26;
			base.Item.useTime = 30;
			base.Item.useAnimation = 30;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.DamageType = DamageClass.Ranged;
			base.Item.knockBack = 1f;
			base.Item.value = 50000;
			base.Item.rare = 14;
			base.Item.UseSound = SoundID.Item36;
			base.Item.autoReuse = true;
            base.Item.shoot = base.Mod.Find<ModProjectile>("火鳞散弹").Type;
			base.Item.shootSpeed = 13f;
			base.Item.useAmmo = 97;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for(int j = 0;j < 5;j++)
			{
			    for(int i = 0;i < 8;i++)
	    		{
					Vector2 v2 = position + new Vector2(velocity.X, velocity.Y) * 2.1f;
			    	Vector2 v = new Vector2(velocity.X, velocity.Y).RotatedBy((float)Math.PI * (4 - i) / 50f) * (0.3f + j / 8f) * Main.rand.Next(10,40) / 15f;
		    		Projectile.NewProjectile(null, v2.X, v2.Y - 4, v.X, v.Y, base.Mod.Find<ModProjectile>("火鳞散弹").Type, damage, knockback, player.whoAmI, 0f, 0f);
	    		}
			}
			return false;
		}
		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return Main.rand.Next(0, 100) > 24;
		}
		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16.0f, 0.0f);
        }
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 origin = new Vector2(26f, 13f);
			spriteBatch.Draw(ModAsset.火鳞散弹Glow.Value, base.Item.Center - Main.screenPosition, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
		}
	}
}
