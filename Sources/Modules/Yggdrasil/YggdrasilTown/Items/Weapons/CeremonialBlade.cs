namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class CeremonialBlade : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 48;
		Item.height = 58;

		Item.DamageType = DamageClass.Melee;
		Item.damage = 27;
		Item.knockBack = 3;

		Item.useStyle = ItemUseStyleID.Swing;
		Item.UseSound = SoundID.Item1;
		Item.useTime = Item.useAnimation = 28;
		Item.autoReuse = true;
		Item.useTurn = true;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(gold: 3);
	}
}