using System.Collections.Generic;
using System.Reflection;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.CyanVine;
using Everglow.Commons.Hooks;
using MonoMod.RuntimeDetour.HookGen;
using Everglow.Commons.Interfaces;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class CyanVinePickaxe : ModItem
{
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

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 16)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
	private delegate int orig_GetPickaxeDamage(int x, int y, int pickPower, int hitBufferIndex, Tile tileTarget);

	private delegate int Hook_GetPickaxeDamage(orig_GetPickaxeDamage orig, int x, int y, int pickPower, int hitBufferIndex, Tile tileTarget);

	// TODO :Need a lazy loading. 
	public override void Load()
	{
		Ins.HookManager.AddHook(typeof(Player).GetMethod("GetPickaxeDamage", BindingFlags.NonPublic | BindingFlags.Instance), Hook_Player_GetPickaxeDamage);
	}

	private static int Hook_Player_GetPickaxeDamage(orig_GetPickaxeDamage orig, int x, int y, int pickPower, int hitBufferIndex, Tile tileTarget)
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
		return orig(x, y, pickPower, hitBufferIndex, tileTarget);
	}
}