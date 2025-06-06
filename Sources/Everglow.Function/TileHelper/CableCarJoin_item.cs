namespace Everglow.Commons.TileHelper;

/// <summary>
/// 缆车架
/// </summary>
public class CableCarJoin_item : CableTileItem
{
	public override int TileType => ModContent.TileType<CableCarJoint>();

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.width = 32;
		Item.height = 26;
		Item.value = 10000;
	}
}