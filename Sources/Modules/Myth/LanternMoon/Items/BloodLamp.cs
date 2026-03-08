using Everglow.Commons.Mechanics.Events;
using Everglow.Myth.LanternMoon.LanternCommon;
using Everglow.Myth.LanternMoon.Projectiles.LanternKing;

namespace Everglow.Myth.LanternMoon.Items;

public class BloodLamp : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 38;
		Item.height = 60;
		Item.rare = ItemRarityID.Green;
		Item.scale = 1;
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.useTurn = true;
		Item.useAnimation = 30;
		Item.useTime = 30;
		Item.autoReuse = false;
		Item.consumable = true;
		Item.maxStack = Item.CommonMaxStack;
		Item.value = 10000;
	}

	public override void HoldItem(Player player)
	{
		if (Main.mouseMiddle && Main.mouseMiddleRelease)
		{
			Projectile.NewProjectileDirect(Item.GetSource_FromAI(), Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<KillLanternMoonMobs>(), 75000, 0, Main.myPlayer);
			//LanternMoonMusicManager musicSystem = ModContent.GetInstance<LanternMoonMusicManager>();

			//musicSystem.Wave15StartTimer = 0;
		}
		if(Main.mouseRight && Main.mouseRightRelease)
		{
			//LanternMoonMusicManager musicSystem = ModContent.GetInstance<LanternMoonMusicManager>();
			//for (int i = 0; i < musicSystem.CustomMusicCues.Count; i++)
			//{
			//	var inst = musicSystem.CustomMusicCues[i];
			//	inst.Active = false;
			//	musicSystem.CustomMusicCues[i] = inst;
			//}
			//musicSystem.Wave15StartTimer = 360000;
		}
	}

	public override bool? UseItem(Player player)
	{
		LanternMoonMusicManager musicSystem = ModContent.GetInstance<LanternMoonMusicManager>();
		musicSystem.Wave15StartTimer = 360000;
		EventSystem.Activate<LanternMoonInvasionEvent>();
		return true;
	}

	public override void AddRecipes()
	{
		CreateRecipe()
		   .AddIngredient(ItemID.ChineseLantern, 1)
		   .AddIngredient(ItemID.FlowerofFire, 10)
		   .AddIngredient(ItemID.Ectoplasm, 5)
		   .AddTile(TileID.DemonAltar)
		   .Register();
	}
}