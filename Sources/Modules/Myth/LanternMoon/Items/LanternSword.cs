namespace Everglow.Myth.LanternMoon.Items;

/// <summary>
/// Mark target with a lantern label.
/// Do at least 500 damage to a labeled target will remove the label and trigger an explosion.
/// </summary>
public class LanternSword : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MeleeWeapons;

	public override void SetDefaults()
	{
		Item.damage = 13;
		Item.DamageType = DamageClass.Melee;
		Item.width = 56;
		Item.height = 56;
		Item.useTime = 20;
		Item.useAnimation = 20;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 5f;
		Item.value = Item.sellPrice(0, 0, 0, 70);
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
	}
}