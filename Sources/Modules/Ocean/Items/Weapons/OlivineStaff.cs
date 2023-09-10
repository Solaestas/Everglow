using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.Items.Weapons.OceanWeapons
{
	// Token: 0x020005C2 RID: 1474
    public class OlivineStaff : ModItem
	{
		// Token: 0x060019F2 RID: 6642 RVA: 0x00008ED2 File Offset: 0x000070D2
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("橄榄石法杖");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石法杖");
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000A88D0 File Offset: 0x000A6AD0
		public override void SetDefaults()
		{
			base.Item.damage = 180;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 22;
			base.Item.width = 30;
			base.Item.height = 30;
			base.Item.useTime = 26;
			base.Item.useAnimation = 26;
			base.Item.useStyle = 5;
			Item.staff[base.Item.type] = true;
			base.Item.noMelee = true;
			base.Item.knockBack = 5f;
			base.Item.value = Item.sellPrice(0, 8, 0, 0);
            base.Item.rare = 11;
            base.Item.UseSound = SoundID.Item43;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.OlivineStaffPro>();
			base.Item.shootSpeed = 10.8f;
		}
		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(null, "Olivine", 8);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 15);
            recipe.requiredTile[0] = 412;
            recipe.Register();
        }
	}
}
