namespace Everglow.Myth.Misc.Items.Weapons;

public class CyanFrost : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 30;
		Item.DamageType = DamageClass.Melee;
		Item.width = 104;
		Item.height = 104;
		Item.useTime = 17;
		Item.useAnimation = 17;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 2;
		Item.value = 10000;
		Item.rare = ItemRarityID.Blue;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.crit = 16;
	}
}
