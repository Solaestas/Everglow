namespace Everglow.Sources.Modules.MythModule.TheTusk.Walls
{
	public class BloodyStoneWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Stone;
			ItemDrop = ModContent.ItemType<Items.BloodyStoneWall>();
			AddMapEntry(new Color(61, 13, 11));
		}
	}
}