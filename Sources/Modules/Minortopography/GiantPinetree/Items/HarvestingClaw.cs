using Terraria.DataStructures;
using static Everglow.Commons.TileHelper.ShakeTreeTweak;

namespace Everglow.Minortopography.GiantPinetree.Items;
//TODO:翻译
//摇树掉落水果
public class HarvestingClaw : FruitPickerTool
{
	public override void SetDefaults()
	{
		Item.damage = 6;
		Item.DamageType = DamageClass.Melee;
		Item.width = 46;
		Item.height = 46;
		Item.useTime = 15;
		Item.useAnimation = 15;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 3;
		Item.value = 1400;
		Item.rare = ItemRarityID.Green;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true; // Automatically re-swing/re-use this item after its swinging animation is over.

		Item.axe = 1; // How much axe power the weapon has, note that the axe power displayed in-game is this value multiplied by 5
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.altFunctionUse == 2)
		{
			return false;
		}
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
	public override bool AltFunctionUse(Player player)
	{
		return true;
	}
}


