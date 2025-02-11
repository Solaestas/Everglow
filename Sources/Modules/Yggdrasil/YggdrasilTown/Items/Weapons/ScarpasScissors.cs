namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class ScarpasScissors : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 56;
		Item.height = 56;

		Item.DamageType = DamageClass.Melee;
		Item.damage = 17;
		Item.knockBack = 3.5f;
		Item.crit = 8;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item1;
		Item.useTime = Item.useAnimation = 16;
		Item.autoReuse = true;
		Item.useTurn = true;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 3);
	}

	public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
	{
		modifiers.CritDamage += 2f;
	}

	public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!target.active)
		{
			// TODO: Replace type with mission item
			player.QuickSpawnItemDirect(target.GetSource_Loot(), ItemID.DirtBlock, Main.rand.Next(10, 20));
		}
	}
}