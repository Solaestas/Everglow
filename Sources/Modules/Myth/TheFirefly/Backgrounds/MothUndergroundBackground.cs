namespace Everglow.Myth.TheFirefly.Backgrounds
{
	public class MothUndergroundBackground : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots)
		{
			string Path = "Sources/Modules/MythModule/TheFirefly/Backgrounds/FireflyUnderground";
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, Path + "1");
		}
	}
}