
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

            AddMapEntry(new Color(51, 26, 58), Language.GetText("Mods.Everglow.MapEntry.FireflyTree"));
            DustType = ModContent.DustType<TheFirefly.Dusts.MothBlue2>();
            AdjTiles = new int[] { Type };
            //TODO Hjson
            //ModTranslation modTranslation = CreateMapEntryName(null);
            //modTranslation.SetDefault("Mods.Everglow.MapEntry.FireflyTree");
            //modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "树");
            //AddMapEntry(new Color(155, 173, 183), modTranslation);

            Everglow.HookSystem.AddMethod(DrawRopes, Commons.Core.CallOpportunity.PostDrawTiles);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)//被砍爆的时候更新
        {
            int tileX = i;
            int tileY = j - frameY / 16;
            if (!hasRope.ContainsKey((tileX, tileY)))
            {
                Everglow.Instance.Logger.Warn("KillMultiTile: Trying to access an non-existent FireflyTree rope");
                return;
            }
            var ropes = hasRope[(tileX, tileY)].ropes;
            foreach (var r in ropes)
            {
                Vector2 acc = new Vector2(Main.rand.NextFloat(-1, 1), 0);
                foreach (var m in r.mass)
                {
                    m.force += acc;
                    if (Main.rand.NextBool(7))
                    {
                        Dust d = Dust.NewDustDirect(m.position, 0, 0, ModContent.DustType<Dusts.GlowBluePedal>());
                        d.velocity = m.velocity * 0.01f;
                    }
                    if (Main.rand.NextBool(10))
                    {
                        Gore g = Gore.NewGoreDirect(null, m.position, m.velocity * 0.1f, ModContent.GoreType<Gores.Branch>());
                    }
                    if (Main.rand.NextBool(3))
                    {
                        Item.NewItem(null, m.position, 16, 16, ModContent.ItemType<Items.GlowWood>());
                    }
                    //被砍时对mass操纵写这里
                }
            }
            ropeManager.RemoveRope(hasRope[(tileX, tileY)].ropes);
            hasRope.Remove((tileX, tileY));
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            var tile = Main.tile[i, j];
            int tileY = j - tile.TileFrameY / 16;
            if (!hasRope.ContainsKey((i, tileY)))
            {
                Everglow.Instance.Logger.Warn("KillMultiTile: Trying to access an non-existent FireflyTree rope");
                return;
            }
            var ropes = hasRope[(i, tileY)].ropes;
            foreach (var r in ropes)
            {
                Vector2 acc = new Vector2(Main.rand.NextFloat(-1, 1), 0);
                foreach (var m in r.mass)
                {
                    m.force += acc;
                    if (Main.rand.NextBool(17))
                    {
                        Dust d = Dust.NewDustDirect(m.position, 0, 0, ModContent.DustType<Dusts.GlowBluePedal>());
                        d.velocity = m.velocity * 0.01f;
                    }
                    if (Main.rand.NextBool(48))
                    {
                        Gore g = Gore.NewGoreDirect(null, m.position, m.velocity * 0.1f, ModContent.GoreType<Gores.Branch>());
                    }
                    //被砍时对mass操纵写这里
                }
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }

        private RopeManager ropeManager = new RopeManager();
        private List<Rope>[] ropes = new List<Rope>[16];
        private Vector2[] basePositions = new Vector2[16];
        private Dictionary<(int x, int y), (int style, List<Rope> ropes)> hasRope = new Dictionary<(int x, int y), (int style, List<Rope>)>();

        /// <summary>
        /// 记录当前每个有树枝的节点的绳子Style（即TileFrameX / 256）
        /// </summary>
        public List<(int x, int y, int style)> GetRopeStyleList()
        {
            List<(int x, int y, int style)> result = new List<(int x, int y, int style)>();
            foreach (var keyvaluepair in hasRope)
            {
                result.Add((keyvaluepair.Key.x, keyvaluepair.Key.y, keyvaluepair.Value.style));
            }
            return result;
        }

        public void InitTreeRopes(List<(int x, int y, int style)> ropesData)
        {
            hasRope.Clear();
            ropeManager.Clear();
            for (int i = 0; i < ropes.Length; i++)
            {
                ropes[i] = null;
            }

            foreach (var (x, y, style) in ropesData)
            {
                InsertOneTreeRope(x, y, style);
            }
        }

        public void InsertOneTreeRope(int xTS, int yTS, int style)
        {
            const int Count = 16;
            Texture2D treeTexture = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTree");
            Vector2 HalfSize = treeTexture.Size() / 2f;
            HalfSize.X /= Count;

            Point point = new Point(xTS, yTS);
            Vector2 tileCenterWS = point.ToWorldCoordinates(8f, 8f);

            if (ropes[style] is null)
            {
                ropes[style] = ropeManager.LoadRope("Everglow/Sources/Modules/MythModule/TheFirefly/Tiles/FireflyTreeRope",
                    new Rectangle(style * 51, 0, 51, 51), tileCenterWS - HalfSize, () => Vector2.Zero);
                hasRope.Add((xTS, yTS), (style, ropes[style]));
                basePositions[style] = tileCenterWS;
            }
            else if (!hasRope.ContainsKey((xTS, yTS)))
            {
                Vector2 deltaPosition = tileCenterWS - basePositions[style];
                List<Rope> rs = ropes[style].Select(r => r.Clone(deltaPosition)).ToList();
                ropeManager.LoadRope(rs);
                hasRope.Add((xTS, yTS), (style, rs));
            }
        }

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
            if (!hasRope.ContainsKey((i, j)))
            {
                InsertOneTreeRope(i, j, tile.TileFrameX / 256);
            }
            Point point = new Point(i, j);
            Vector2 tileCenterWS = point.ToWorldCoordinates(8f, 8f);
            Color color = Lighting.GetColor(i, j);
            SpriteEffects effects = SpriteEffects.None;
            const int Count = 16;
            Vector2 HalfSize = treeTexture.Size() / 2f;
            HalfSize.X /= Count;
            spriteBatch.Draw(treeTexture, tileCenterWS - Main.screenPosition + zero,
                new Rectangle(tile.TileFrameX, 0, treeTexture.Width / Count, treeTexture.Height),
                color, 0f, HalfSize, 1f, effects, 0f);
            spriteBatch.Draw(glowTexture, tileCenterWS - Main.screenPosition + zero,
                new Rectangle(tile.TileFrameX, 0, treeTexture.Width / Count, treeTexture.Height),
                new Color(1f, 1f, 1f, 0), 0f, HalfSize, 1f, effects, 0f);


            if (tileCenterWS.Distance(Main.LocalPlayer.position) < 200)
            {
                var playerRect = Main.LocalPlayer.Hitbox;
                var (_, ropes) = hasRope[(i, j)];
                foreach (var rope in ropes)
                {
                    foreach (var m in rope.mass)
                    {
                        if (playerRect.Contains(m.position.ToPoint()))
                        {
                            m.force += Main.LocalPlayer.velocity / 1.5f;
                        }
                    }
                }
            }
            return false;
        }
    }
}
