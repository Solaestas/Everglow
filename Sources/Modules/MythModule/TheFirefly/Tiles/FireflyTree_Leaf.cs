namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
	public class ExampleTreeLeaf : ModGore
	{
		public override string Texture => "Everglow/Sources/Modules/MythModule/TheFirefly/Tiles/FireflyTree_Leaf";

		public override void SetStaticDefaults() {
			
			GoreID.Sets.SpecialAI[Type] = 3;
		}
	}
}
