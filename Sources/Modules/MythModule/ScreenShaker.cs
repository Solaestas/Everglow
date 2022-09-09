namespace Everglow.Sources.Modules.MythModule
{
    public class ScreenShaker : ModPlayer
    {
        public Vector2 FlyCamPosition = Vector2.Zero;
        public override void ModifyScreenPosition()
        {
            FlyCamPosition *= 0.25f;
            Main.screenPosition += FlyCamPosition;
        }
    }
}