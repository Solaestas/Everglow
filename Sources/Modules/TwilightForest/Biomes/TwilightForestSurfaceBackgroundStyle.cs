namespace Everglow.TwilightForest.Biomes
{
	public class TwilightForestSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
	{
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					if (fades[i] > 1f)
					{
						fades[i] = 1f;
					}
				}
				else
				{
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f)
					{
						fades[i] = 0f;
					}
				}
			}
		}

		public override int ChooseFarTexture()
		{
			return BackgroundTextureLoader.GetBackgroundSlot(Mod, "TwilightForest/Backgrounds/TwilightForestFar");
		}
		public override int ChooseMiddleTexture()
		{
			Main.ColorOfTheSkies = new Color(0.3f, 0f, 0.8f);
			return BackgroundTextureLoader.GetBackgroundSlot(Mod, "TwilightForest/Backgrounds/TwilightForestMiddle");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			b = 1100;
			scale = 1.6f;
			return BackgroundTextureLoader.GetBackgroundSlot(Mod, "TwilightForest/Backgrounds/TwilightForestClose");
		}
	}
}