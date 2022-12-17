using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Tiles
{
    public class StrangeTuskStone : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 7;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16,
                16,
                20
            };
            TileObjectData.newTile.CoordinateWidth = 64;
            TileObjectData.addTile((int)base.Type);
            DustType = 1;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(100, 90, 90), modTranslation);
            modTranslation.SetDefault("Strange Stone");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "石碑");
            HitSound = SoundID.DD2_SkeletonHurt;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (frameX == 0)
            {
                Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.TuskStatusI>());
            }
            if (frameX == 64)
            {
                Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.TuskStatusII>());
            }
            if (frameX == 128)
            {
                Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.TuskStatusIII>());
            }
            if (frameX == 192)
            {
                Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.TuskStatusIV>());
            }
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Player player = Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 0, 0)];
            int C = 255 - (int)((player.Center - new Vector2(i * 16, j * 16)).Length());
            if (C < 0)
            {
                C = 0;
            }
            spriteBatch.Draw(ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheTusk/Tiles/StrangeTuskStoneGlow").Value, new Vector2(i * 16 - 24 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 64, 18), new Color(C, C, C, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
