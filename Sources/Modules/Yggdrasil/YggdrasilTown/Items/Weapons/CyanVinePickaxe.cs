using System.Reflection;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class CyanVinePickaxe : ModItem
{
	private delegate int GetPickaxeDamageDelegate(Player self, int x, int y, int pickPower, int hitBufferIndex, Tile tileTarget);

	private delegate int Hook_GetPickaxeDamage(GetPickaxeDamageDelegate orig, int x, int y, int pickPower, int hitBufferIndex, Tile tileTarget);

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 16)
			.AddTile(TileID.WorkBenches)
			.Register();
	}

	public override void Load()
	{
		Ins.HookManager.AddHook(typeof(Player).GetMethod(nameof(Player.GetPickaxeDamage), BindingFlags.NonPublic | BindingFlags.Instance), Hook_Player_GetPickaxeDamage);
	}

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 48;
		Item.height = 52;
		Item.useAnimation = 14;
		Item.useTime = 14;
		Item.knockBack = 1.8f;
		Item.damage = 11;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.useTurn = true;
		Item.DamageType = DamageClass.Melee;

		Item.value = 4200;

		Item.pick = 65;
	}

	private static int Hook_Player_GetPickaxeDamage(GetPickaxeDamageDelegate orig, Player self, int x, int y, int pickPower, int hitBufferIndex, Tile tileTarget)
	{
		if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<CyanVinePickaxe>())
		{
			if (tileTarget.TileType == ModContent.TileType<StoneScaleWood>())
				return 10;
			if (tileTarget.TileType == ModContent.TileType<CyanVineOreLarge>())
				return 30;
			if (tileTarget.TileType == ModContent.TileType<CyanVineOreLargeUp>())
				return 30;
			if (tileTarget.TileType == ModContent.TileType<CyanVineOreMiddle>())
				return 30;
			if (tileTarget.TileType == ModContent.TileType<CyanVineOreSmall>())
				return 30;
			if (tileTarget.TileType == ModContent.TileType<CyanVineOreSmallUp>())
				return 30;
			if (tileTarget.TileType == ModContent.TileType<CyanVineOreTile>())
				return 30;
			if (tileTarget.TileType == ModContent.TileType<CyanVineStone>())
				return 60;
		}
		return orig(self, x, y, pickPower, hitBufferIndex, tileTarget);
	}
}