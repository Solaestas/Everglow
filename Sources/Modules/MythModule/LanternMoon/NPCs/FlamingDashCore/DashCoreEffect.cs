namespace Everglow.Sources.Modules.MythModule.LanternMoon.NPCs.FlamingDashCore
{
    public class DashCoreEffect : ModSystem
    {
        private Effect ef;
        int d = 0;
        public override void PostUpdateWorld()
        {
            d++;
            /*if(d % 241 == 0)
            {
                Projectile.NewProjectile(null, Main.LocalPlayer.Center + new Vector2(0, -30), new Vector2(0, Main.rand.NextFloat(0, 2f)).RotatedByRandom(6.28), ModContent.ProjectileType<Projectiles.Typeless.AcytaeaEffect>(), 0, 1, Main.myPlayer);
            }*/

            /*ef = (Effect)ModContent.Request<Effect>("MythMod/Effects/ef3/DarkFlame").Value;
            ef.Parameters["uImageSize"].SetValue(new Vector2(Main.screenWidth,Main.screenHeight));
            if (!Filters.Scene["DarkFlame"].IsActive())
            {
                Filters.Scene.Activate("DarkFlame");
            }
            base.PostUpdateWorld();*/
        }
        public override void OnWorldLoad()
        {
            //MythMod.CreateRender();
            base.OnWorldLoad();
        }
        float RDas = 0;
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            Color colorShine = FlamingDashCore.ColorShine;
            if (NPC.CountNPCS(ModContent.NPCType<FlamingDashCore>()) > 0 && FlamingDashCore.Shine > 0)
            {
                RDas = 1f;
            }
            else
            {
                if (RDas > 0)
                {
                    RDas -= 0.01f;
                }
                else
                {
                    RDas = 0;
                }
            }
            tileColor = new Color(2.2f * RDas * colorShine.R / 200f + tileColor.R / 255f * (1 - RDas), 2.2f * RDas * colorShine.G / 200f + tileColor.G / 255f * (1 - RDas), 2.2f * RDas * colorShine.B / 200f + tileColor.B / 255f * (1 - RDas));
            backgroundColor = new Color(2.2f * RDas * colorShine.R / 200f + tileColor.R / 255f * (1 - RDas), 2.2f * RDas * colorShine.G / 200f + tileColor.G / 255f * (1 - RDas), 2.2f * RDas * colorShine.B / 200f + tileColor.B / 255f * (1 - RDas));
            Main.ColorOfTheSkies = new Color(2.2f * RDas * colorShine.R / 200f + tileColor.R / 255f * (1 - RDas), 2.2f * RDas * colorShine.G / 200f + tileColor.G / 255f * (1 - RDas), 2.2f * RDas * colorShine.B / 200f + tileColor.B / 255f * (1 - RDas));
            base.ModifySunLightColor(ref tileColor, ref backgroundColor);
        }
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            /*if (Main.playerInventory)
            {
                Main.spriteBatch.DrawTenPieces(TextureAssets.MagicPixel.Value, 10, 10, 10, 10, 10);
            }*/
        }
    }
}
