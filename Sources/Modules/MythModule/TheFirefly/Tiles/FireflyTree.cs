
using Everglow.Sources.Modules.MythModule.Common;

using Terraria.Localization;
using Terraria.ObjectData;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class FireflyTree : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileAxe[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 8;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(51, 26, 58));
            DustType = ModContent.DustType<Bosses.CorruptMoth.Dusts.MothBlue2>();
            AdjTiles = new int[] { Type };
            //TODO Hjson
            ModTranslation modTranslation = CreateMapEntryName(null);
            modTranslation.SetDefault("Tree");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "树");
            AddMapEntry(new Color(155, 173, 183), modTranslation);

            Everglow.HookSystem.AddMethod(DrawRopes, Commons.Core.CallOpportunity.PostDrawTiles);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)//被砍爆的时候更新
        {
            int Times = Main.rand.Next(5, 9);
            for (int d = 0; d < Times; d++)
            {
                Item.NewItem(null, i * 16 + Main.rand.Next(72), j * 16 + Main.rand.Next(64), 16, 16, ModContent.ItemType<Items.GlowWood>());
            }
            /*for (int f = 0; f < 13; f++)
            {
                Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d);
                Gore.NewGore(null, new Vector2(i * 16, j * 16) + vF, vF, ModContent.Find<ModGore>("MythMod/CyanVineOre" + f.ToString()).Type, 1f);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.Silver, vF.X, vF.Y);
                vF = new Vector2(0, Main.rand.NextFloat(0, 4f)).RotatedByRandom(6.28d);
                Dust.NewDust(new Vector2(i * 16, j * 16) + vF, 0, 0, DustID.WoodFurniture, vF.X, vF.Y);
            }*/
            ropeManager.RemoveRope(hasRope[(i, j)]);
            hasRope.Remove((i, j));
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            var tile = Main.tile[i, j];
            var ropes = hasRope[(i, j - tile.TileFrameY / 16)];
            foreach(var r in ropes)
            {
                Vector2 acc = new Vector2(Main.rand.NextFloat(-1, 1), 0);
                foreach(var m in r.mass)
                {
                    m.force += acc;
                    //被砍时对mass操纵写这里
                }
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        private RopeManager ropeManager = new RopeManager();
        private List<Rope>[] ropes = new List<Rope>[16];
        private Vector2[] basePositions = new Vector2[16];
        private Dictionary<(int x, int y), List<Rope>> hasRope = new Dictionary<(int x, int y), List<Rope>>();
        public void DrawRopes()
        {
            if (!Main.gamePaused)
            {
                ropeManager.drawColor = new Color(0, 0, 0, 0);
                ropeManager.Update(1f);
            }
            ropeManager.Draw();
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Texture2D treeTexture = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTree");
            Texture2D glowTexture = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTreeGlow");
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            if (tile.TileFrameY != 0 || !tile.HasTile)
            {
                return false;
            }
            Point point = new Point(i, j);
            Vector2 worldCoord = point.ToWorldCoordinates(8f, 8f);
            Color color = Lighting.GetColor(i, j);
            SpriteEffects effects = SpriteEffects.None;
            const int Count = 16;
            Vector2 HalfSize = treeTexture.Size() / 2f;
            HalfSize.X /= Count;
            spriteBatch.Draw(treeTexture, worldCoord - Main.screenPosition + zero,
                new Rectangle(tile.TileFrameX, 0, treeTexture.Width / Count, treeTexture.Height),
                color, 0f, HalfSize, 1f, effects, 0f);
            spriteBatch.Draw(glowTexture, worldCoord - Main.screenPosition + zero,
                new Rectangle(tile.TileFrameX, 0, treeTexture.Width / Count, treeTexture.Height),
                new Color(1f, 1f, 1f, 0), 0f, HalfSize, 1f, effects, 0f);

            Vector2 RopOffset = new Vector2(i * 16 - tile.TileFrameX - 128, j * 16 - 128) - Main.screenPosition;

            int frameX = tile.TileFrameX / 256;
            if (ropes[frameX] is null)
            {
                ropes[frameX] = ropeManager.LoadRope("Everglow/Sources/Modules/MythModule/TheFirefly/Tiles/FireflyTreeRope",
                    new Rectangle(frameX * 51, 0, 51, 51), worldCoord - HalfSize, () => Vector2.Zero);
                hasRope.Add((i, j), ropes[frameX]);
                basePositions[frameX] = worldCoord;
            }
            else if (!hasRope.ContainsKey((i, j)))
            {
                Vector2 deltaPosition = worldCoord - basePositions[frameX];
                List<Rope> rs = ropes[frameX].Select(r => r.Clone(deltaPosition)).ToList();
                ropeManager.LoadRope(rs);
                hasRope.Add((i, j), rs);
            }
            return false;
        }
    }
}
