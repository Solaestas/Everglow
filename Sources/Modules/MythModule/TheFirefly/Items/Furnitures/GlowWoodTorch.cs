using Terraria.ID;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class GlowWoodTorch : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.width = 10;
			Item.height = 22;
			Item.maxStack = 99;
			Item.holdStyle = 1;
			Item.noWet = false;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Tiles.Furnitures.GlowWoodTorch>();
		}
		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
		{ 
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.Torches; 
		}
		public override void HoldItem(Player player)
		{
			if (Main.rand.Next((player.itemAnimation > 0) ? 10 : 20) == 0)
			{
				//Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, ModContent.DustType<>());
			}
			Lighting.AddLight(player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * (float)((Entity)player).direction + ((Entity)player).velocity.X, player.itemLocation.Y - 14f + ((Entity)player).velocity.Y), true, true), 1f, 1f, 1f);
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, 1f, 1f, 1f);
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			wetTorch = true;
		}
	}
}