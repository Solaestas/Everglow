namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class FurnaceHeatProofPlatingWall_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableWall(ModContent.WallType<Walls.FurnaceHeatProofPlatingWall>());
		Item.width = 24;
		Item.height = 24;
	}
}