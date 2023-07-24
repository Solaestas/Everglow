namespace MythMod.OceanMod.Backgrounds
{
    public class VolcanoSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
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
            return BackgroundTextureLoader.GetBackgroundSlot("MythMod/OceanMod/Backgrounds/VolcanoSurfaceFar");
        }
        private static int SurfaceFrameCounter;
        private static int SurfaceFrame;
        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("MythMod/OceanMod/Backgrounds/VolcanoSurfaceMiddle");
        }
        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            //b = 2000;
            return BackgroundTextureLoader.GetBackgroundSlot("MythMod/OceanMod/Backgrounds/VolcanoSurfaceClose");
        }
    }
}