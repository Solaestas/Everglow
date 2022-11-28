using Everglow.Sources.Modules.ZYModule.TileModule;
using Terraria.Localization;

namespace Everglow.Sources.Modules.YggdrasilModule.Common.Elevator.Tiles
{
    public class Winch : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(112, 75, 75));

            DustType = DustID.Iron;

        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            blockDamaged = false;
            return false;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            bool HasLift = false;
            foreach (var Dtile in TileSystem.GetTiles<YggdrasilElevator>())
            {
                Vector2 Dc = Dtile.Center;
                float Dy = Math.Abs(Dc.Y / 16f - j);
                //电梯至少要在绞盘下10格。如果大于1000格且还有阻挡，那么不认为还具有所属电梯
                if (Math.Abs(Dc.X / 16f - i) < Dtile.size.X / 32f + 2 && Dy > 10 && (Dy < 1000 || Collision.CanHit(new Vector2(i, j + 2) * 16, 1, 1, Dc, 1, 1)))
                {
                    HasLift = true;
                }
            }
            if (!HasLift)
            {
                TileSystem.AddTile(new YggdrasilElevator() { Position = new Vector2(i, j + 15) * 16 - new Vector2(48, 8) });
            }
            base.NearbyEffects(i, j, closer);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Texture2D t = YggdrasilContent.QuickTexture("Common/Elevator/Tiles/LiftWinch");
            Color c0 = Lighting.GetColor(i, j);

            spriteBatch.Draw(t, new Vector2(i * 16, j * 16) - Main.screenPosition + new Vector2(8, 6)/* + new Vector2((int)Vdrag.X, (int)Vdrag.Y)*/ + zero, null, c0, 0, t.Size() / 2f, 1, SpriteEffects.None, 0);
            base.PostDraw(i, j, spriteBatch);
        }
    }
}
