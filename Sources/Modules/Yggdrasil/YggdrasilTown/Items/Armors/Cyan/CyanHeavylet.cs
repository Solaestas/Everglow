using Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Valiant;
using Terraria.GameContent.Creative;
namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Cyan
{
	[AutoloadEquip(EquipType.Head)]
	public class ValiantHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			// ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawBackHair[Item.headSlot] = true;
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true; 
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = 3750;
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ValiantBreastplate>() && legs.type == ModContent.ItemType<ValiantLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.statDefense += 3;
			player.lifeRegen += 2;
			player.runAcceleration += 10;
		}
		public override void UpdateEquip(Player player)
		{
			player.endurance += 0.04f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();

			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}

