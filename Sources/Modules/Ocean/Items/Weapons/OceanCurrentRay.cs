using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Weapons
{
    public class OceanCurrentRay : ModItem
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("洋流射线");
			Item.staff[base.Item.type] = true;
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "洋流射线");
		}
		public override void SetDefaults()
		{
			base.Item.damage = 290;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 6;
			base.Item.width = 54;
			base.Item.height = 54;
			base.Item.useTime = 12;
			base.Item.useAnimation = 12;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 2f;
			base.Item.value = 20000;
			base.Item.rare = 8;
			base.Item.UseSound = SoundID.Item60;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.OceanCurrentRay>();
			base.Item.shootSpeed = 6f;
		}
	}
}
