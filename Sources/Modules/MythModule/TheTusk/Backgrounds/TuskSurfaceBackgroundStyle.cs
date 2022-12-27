namespace Everglow.Sources.Modules.MythModule.TheTusk.Backgrounds
{
    public class TuskSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
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
            return BackgroundTextureLoader.GetBackgroundSlot("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/TuskFar");
        }

        public override int ChooseMiddleTexture()
        {
            return BackgroundTextureLoader.GetBackgroundSlot("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/Empty");
        }

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            b = 1000;
            return BackgroundTextureLoader.GetBackgroundSlot("Everglow/Sources/Modules/MythModule/TheTusk/Backgrounds/TuskMiddle");
        }
    }
}