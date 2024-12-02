namespace Everglow.Commons.Skeleton2D;

public class SkeletonSight : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 10;
		Item.height = 10;
		Item.useTime = 7;
		Item.useAnimation = 7;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.noUseGraphic = true;
	}
}