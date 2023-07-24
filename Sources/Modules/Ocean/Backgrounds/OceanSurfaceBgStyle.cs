namespace MythMod.OceanMod.Backgrounds
{
    public class OceanSurfaceBgStyle : ModSurfaceBackgroundStyle
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
            return BackgroundTextureLoader.GetBackgroundSlot("MythMod/OceanMod/Backgrounds/OceanSurfaceFar");
        }
        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("MythMod/OceanMod/Backgrounds/OceanSurfaceMiddle");
        }
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            //b = -900;
            return BackgroundTextureLoader.GetBackgroundSlot("MythMod/OceanMod/Backgrounds/OceanSurfaceClose");
        }
    }
}