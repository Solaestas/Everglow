using Terraria.GameContent.Creative;
namespace Everglow.Yggdrasil.YggdrasilTown.Items.Armors.BloodCultist
{
	[AutoloadEquip(EquipType.Head)]
	public class BloodStainedVeil : ModItem
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
			return body.type == ModContent.ItemType<BloodDrenchedRobe>();
		}
		public override void UpdateArmorSet(Player player)
		{
			// TODO player.setBonus 
			BloodCultistPlayer BloodCultistPlayer = player.GetModPlayer<BloodCultistPlayer>();
			BloodCultistPlayer.hasBloodCultistSet = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.06f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();

			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}

public class BloodCultistPlayer : ModPlayer
{

	public bool hasBloodCultistSet;
	public int BloodTimer=0;
	public override void ResetEffects()
	{
		hasBloodCultistSet = false;

	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
	{
		
		if (hit.DamageType == DamageClass.Magic && hasBloodCultistSet)
		{
			BloodTimer = 180;
			Player.lifeRegen += 1;
			if (Main.rand.NextBool(20))
			{
				CombatText.NewText(Player.Hitbox, CombatText.HealMana, 10);
				Player.statMana += 10;
			}
		}
	}
	public override void PostUpdateEquips()
	{
		if (BloodTimer >= 0)
		{
			BloodTimer--;
			Player.lifeRegen += 1;
		}
	}
}