namespace Everglow.Sources.Modules.MythModule.TheFirefly.Walls
{
	public class LiveFluorescentWoodWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<Dusts.FluorescentTreeDust>();
			ItemDrop = ModContent.ItemType<Items.DarkCocoonWall>();
			HitSound = SoundID.Dig;
			AddMapEntry(new Color(33, 1, 53));
		}
	}
}