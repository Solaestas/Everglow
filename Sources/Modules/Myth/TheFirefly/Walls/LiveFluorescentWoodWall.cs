using Everglow.Myth.TheFirefly.Dusts;
using Everglow.Myth.TheFirefly.Items;

namespace Everglow.Myth.TheFirefly.Walls
{
	public class LiveFluorescentWoodWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<FluorescentTreeDust>();
			ItemDrop = ModContent.ItemType<DarkCocoonWall>();
			HitSound = SoundID.Dig;
			AddMapEntry(new Color(33, 1, 53));
		}
	}
}