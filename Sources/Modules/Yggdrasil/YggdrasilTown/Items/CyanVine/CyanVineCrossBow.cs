using Everglow.Commons.Weapons.CrossBow;
using Everglow.Yggdrasil.YggdrasilTown.Items;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.CyanVine;

public class CyanVineCrossBow : CrossBowItem
{
	public override void SetDef()
	{
		Item.width = 74;
		Item.height = 34;
		Item.rare = ItemRarityID.White;
		Item.value = 3800;
		Item.useTime = 20;
		Item.useAnimation = 20;
		Item.damage = 15;
		Item.knockBack = 4f;
		CrossBowProjType = ModContent.ProjectileType<CyanVineCrossBow_Proj>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 26)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 12)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
