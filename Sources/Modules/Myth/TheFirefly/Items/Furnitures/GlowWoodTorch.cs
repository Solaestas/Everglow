using Terraria.GameContent.Creative;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class GlowWoodTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			Item.flame = true;
			Item.width = 10;
			Item.height = 12;
			Item.value = 50;
			Item.maxStack = 999;
			Item.holdStyle = 1;
			Item.noWet = false;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = ItemHoldStyleID.HoldFront;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furnitures.GlowWoodTorch>();
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
		{
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.Torches;
		}

		public override void HoldItem(Player player)
		{
			if (Main.rand.NextBool((player.itemAnimation > 0) ? 10 : 20))
			{
				Dust d = Dust.NewDustDirect(new Vector2(player.itemLocation.X + 10f * player.direction - 6, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<Dusts.BlueToPurpleSpark>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.65f));
				d.velocity.Y = -2;
				d.velocity.X *= 0.05f;
			}
			Lighting.AddLight(player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true, true), 0.7f, 0.06f, 1f);
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, 1f, 1f, 1f);
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			wetTorch = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(3);
			recipe.AddIngredient(ModContent.ItemType<GlowWood>(), 1);
			recipe.AddIngredient(ItemID.Gel, 1);
			recipe.Register();
		}
	}
}