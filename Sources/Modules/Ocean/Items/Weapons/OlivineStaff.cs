using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Weapons.OceanWeapons
{
	// Token: 0x020005C2 RID: 1474
    public class OlivineStaff : ModItem
	{
		// Token: 0x060019F2 RID: 6642 RVA: 0x00008ED2 File Offset: 0x000070D2
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("橄榄石法杖");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "橄榄石法杖");
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000A88D0 File Offset: 0x000A6AD0
		public override void SetDefaults()
		{
			base.item.damage = 180;
			base.item.magic = true;
			base.item.mana = 22;
			base.item.width = 30;
			base.item.height = 30;
			base.item.useTime = 26;
			base.item.useAnimation = 26;
			base.item.useStyle = 5;
			Item.staff[base.item.type] = true;
			base.item.noMelee = true;
			base.item.knockBack = 5f;
			base.item.value = Item.sellPrice(0, 8, 0, 0);
            base.item.rare = 11;
            base.item.UseSound = SoundID.Item43;
			base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("OlivineStaffPro");
			base.item.shootSpeed = 10.8f;
		}
		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Olivine", 8);
            recipe.AddIngredient(null, "GoldGorgonianBranch", 15);
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
