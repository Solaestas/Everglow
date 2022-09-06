namespace Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds
{
    public class FakeMothBackground : ModUndergroundBackgroundStyle
    {
		public override void FillTextureArray(int[] textureSlots)
		{
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/Dark");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/Dark");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/Dark");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot("Everglow/Sources/Modules/MythModule/TheFirefly/Backgrounds/Dark");
		}
	}
}

