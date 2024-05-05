using System.Diagnostics;
using Everglow.Commons.Hooks;
using Everglow.Yggdrasil.YggdrasilTown.Items.Armors.Valiant;
using Terraria.GameContent.Creative;
namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.LightSeeker
{
	[AutoloadEquip(EquipType.Head)]
	public class LightSeekerHelmet_Ranged : ModItem
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
			player.GetModPlayer<LightSeekerPlayer>().LightSeekerHelmet = 1;
            player.GetDamage(DamageClass.Ranged) += 0.05f;
        }
		public override void UpdateEquip(Player player)
		{
			Lighting.AddLight(player.Center, Vector3.one * 0.5f);
			
			player.GetCritChance(DamageClass.Ranged) += 4f;
		}
		public override void AddRecipes()
		{
			
		}
	}
}
public class LightSeekerGNPC : GlobalNPC
{
    public override Color? GetAlpha(NPC npc, Color drawColor)
    {
        Debug.Assert(Main.netMode != NetmodeID.Server);

        if (Main.LocalPlayer.GetModPlayer<LightSeekerPlayer>().LightSeekerHelmet == 1)
        {
            if (npc.CanBeChasedBy() && (npc.noTileCollide || Collision.CanHitLine(npc.Center, 1, 1, Main.LocalPlayer.Center, 1, 1)))
            {
                Vector2 vec1 = Main.MouseWorld - Main.LocalPlayer.Center;
                Vector2 vec2 = npc.Center - Main.LocalPlayer.Center;
                float alpha = (float)Math.Acos(Vector2.Dot(vec1, vec2) / (vec1.Length() * vec2.Length()));//����Ƕ�
                alpha = 1 - alpha;
                alpha = MathHelper.Clamp(alpha, 0, 1f);
                Color baseColor = drawColor;
                return Color.Lerp(baseColor, Color.White, alpha);
            }
        }
        return base.GetAlpha(npc, drawColor);
    }
}
public class LightSeekerPlayer : ModPlayer
{
    public int LightSeekerHelmet = 0;
    public override void ResetEffects()
    {
        LightSeekerHelmet = 0;
    }
	public override bool CanConsumeAmmo(Item weapon, Item ammo)
	{
		return base.CanConsumeAmmo(weapon, ammo) && (LightSeekerHelmet == 1 ? (!Main.rand.NextBool(10)) : true);
	}
}
