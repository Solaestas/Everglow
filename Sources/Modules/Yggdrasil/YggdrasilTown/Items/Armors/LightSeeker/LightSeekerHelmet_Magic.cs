using System.Diagnostics;
using Everglow.Commons.Hooks;
using Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Valiant;
using Terraria.GameContent.Creative;
namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LightSeeker
{
	[AutoloadEquip(EquipType.Head)]
	public class LightSeekerHelmet_Magic : ModItem
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
			Item.defense = 1;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<LightSeekerBreastplate>() && legs.type == ModContent.ItemType<LightSeekerLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{
			Color lightColor = Lighting.GetColor(player.Center.ToTileCoordinates());

			float lightIntensity = MathHelper.Max(MathHelper.Max(lightColor.R, lightColor.G), lightColor.B);
			if (lightIntensity > 220) 
			{
                player.GetDamage(DamageClass.Magic) += 0.1f;
            }

            player.manaCost *= 0.9f;
            player.GetDamage(DamageClass.Magic) += 0.05f;
        }
		public override void UpdateEquip(Player player)
		{
			Lighting.AddLight(player.Center, Vector3.one * 0.5f);
			
			player.GetCritChance(DamageClass.Magic) += 4f;
		}
		public override void AddRecipes()
		{
			
		}
	}
}
