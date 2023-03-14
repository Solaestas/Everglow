using Everglow.Myth.Common;
using Terraria;

namespace Everglow.Myth.MiscItems.Accessories;

[AutoloadEquip(EquipType.Neck)]
public class SilverWing : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 60;
		Item.height = 26;
		Item.value = 1093;
		Item.accessory = true;
		Item.rare = ItemRarityID.White;
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.SilverBar, 14)
			.AddIngredient(ItemID.Ruby, 8)
			.AddTile(TileID.Anvils)
			.Register();
		CreateRecipe()
			.AddIngredient(ItemID.TungstenBar, 14)
			.AddIngredient(ItemID.Ruby, 8)
			.AddTile(TileID.Anvils)
			.Register();
	}
	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		MythContentPlayer mplayer = player.GetModPlayer<MythContentPlayer>();
		mplayer.CriticalDamage += 0.08f;
		if (player.controlUseItem)
			noContinueUsingWeaponTime = 0;
		else
		{
			noContinueUsingWeaponTime++;
		}
		if (noContinueUsingWeaponTime >= 180)
		{
			SliverWingEquiper sWE = player.GetModPlayer<SliverWingEquiper>();
			sWE.SliverWingEnable = true;
		}
	}
	internal int noContinueUsingWeaponTime = 0;
}
class SliverWingEquiper : ModPlayer
{
	public bool SliverWingEnable = false;
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Item, consider using ModifyHitNPC instead */
	{
		Main.NewText(SliverWingEnable);
		if (SliverWingEnable)
		{
			damage = (int)(damage * 1.4f);
			SliverWingEnable = false;
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
	{
		if (SliverWingEnable)
		{
			damage = (int)(damage * 1.4f);
			SliverWingEnable = false;
		}
	}
	public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)/* tModPorter Note: Removed. Use ModifyHurt on the receiving player and check modifiers.PvP. Use modifiers.DamageSource.SourcePlayerIndex to get the attacking player */
	{
		if (SliverWingEnable)
		{
			damage = (int)(damage * 1.4f);
			SliverWingEnable = false;
		}
	}
	public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)/* tModPorter Note: Removed. Use ModifyHurt on the receiving player and check modifiers.PvP. Use modifiers.DamageSource.SourcePlayerIndex to get the attacking player */
	{
		if (SliverWingEnable)
		{
			damage = (int)(damage * 1.4f);
			SliverWingEnable = false;
		}
	}
}
