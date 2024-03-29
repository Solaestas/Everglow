using Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Valiant;
using Terraria.GameContent.Creative;
namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Valiant
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
			Item.value = 2500;
			Item.rare = ItemRarityID.Green;
			Item.defense = 3;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ValiantBreastplate>() && legs.type == ModContent.ItemType<ValiantLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.aggro -= 400;
			player.statDefense += 2;
			player.endurance += 0.04f;

		}
		public override void UpdateEquip(Player player)
		{
			ValiantHelmetPlayer ValiantHelmetPlayer = player.GetModPlayer<ValiantHelmetPlayer>();
			ValiantHelmetPlayer.hasValiantHelmet = true;

		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();

			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
public class ValiantHelmetPlayer : ModPlayer
{
	public bool isUsingMeleeProj = false;
	public bool hasValiantHelmet;
	public override void ResetEffects()
	{
		hasValiantHelmet = false;
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
	{
		if (hasValiantHelmet &&
			(Player.GetModPlayer<MEACPlayer>().isUsingMeleeProj ||
			(Player.heldProj > 0 && Main.projectile[Player.heldProj].DamageType == DamageClass.Melee)))
		{
			modifiers.SourceDamage *= 1.05f;
		}

	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
	{
		if (hasValiantHelmet)
		{
			modifiers.SourceDamage *= 1.05f;
		}
	}
}