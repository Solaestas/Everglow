namespace Everglow.Commons.Utilities;

public static partial class TileUtils
{
	public class Sets
	{
		public static bool[] TileFragile = TileID.Sets.Factory.CreateBoolSet(
			false,
			TileID.LivingMahoganyLeaves,
			TileID.LeafBlock,
			TileID.CrispyHoneyBlock);
	}
}