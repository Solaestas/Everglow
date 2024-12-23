namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class HeatProofPlatingWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.HeatProofPlatingWall>());
		Item.width = 24;
		Item.height = 24;
	}
}