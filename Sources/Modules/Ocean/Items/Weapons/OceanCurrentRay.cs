using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons
{
    public class OceanCurrentRay : ModItem
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("洋流射线");
			Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "洋流射线");
		}
		public override void SetDefaults()
		{
			base.item.damage = 290;
			base.item.magic = true;
			base.item.mana = 6;
			base.item.width = 54;
			base.item.height = 54;
			base.item.useTime = 12;
			base.item.useAnimation = 12;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 2f;
			base.item.value = 20000;
			base.item.rare = 8;
			base.item.UseSound = SoundID.Item60;
			base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("OceanCurrentRay");
			base.item.shootSpeed = 6f;
		}
	}
}
