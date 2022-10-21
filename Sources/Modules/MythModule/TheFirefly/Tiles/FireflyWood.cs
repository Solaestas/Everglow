using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class FireflyWood : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            MinPick = 175;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.GlowWood>();
            AddMapEntry(new Color(37, 46, 47));
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {

            return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/FireflyWoodGlow");
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)];
            float dis = Math.Clamp((player.Center - new Vector2(i * 16, j * 16)).Length() / 480f, 0f, 10f);
            dis = Math.Clamp(dis + (float)Math.Sin(dis * 14d - Main.timeForVisualEffects / 25f) / 2f, 0f, 1f);
            dis = Math.Clamp(1 - dis, 0f, 1f);
            spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(dis, dis, dis, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

            base.PostDraw(i, j, spriteBatch);
        }
    }
}