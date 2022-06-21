using Terraria.Localization;
using Terraria.ObjectData;
using Everglow.Sources.Modules.MythModule.TheFirefly.Physics;
using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Modules.MythModule.Common;
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
            Main.tileAxe[(int)base.Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 8;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(51, 26, 58));
            DustType = ModContent.DustType<Bosses.CorruptMoth.Dusts.MothBlue2>();
            AdjTiles = new int[] { Type };
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("Tree");
            modTranslation.AddTranslation((int)GameCulture.CultureName.Chinese, "树");
            base.AddMapEntry(new Color(155, 173, 183), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
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
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        private List<List<Mass>> masses = new List<List<Mass>>();
        private List<List<Spring>> springs = new List<List<Spring>>();
        private List<Vector2> RopPosFir = new List<Vector2>();
        public List<float> RopPosFirS = new List<float>();
        public List<int> RopPosFirC = new List<int>();
        public void GetRopePosFir(string Shapepath)
        {
            var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Sources/Modules/MythModule/TheFirefly/Tiles/" + Shapepath);
            imageData.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    var pixelRow = accessor.GetRowSpan(y);
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        ref var pixel = ref pixelRow[x];
                        if (pixel.R == 255)
                        {
                            RopPosFir.Add(new Vector2(x * 5, y * 5));
                            RopPosFirC.Add(pixel.G + 2);
                            RopPosFirS.Add((pixel.B + 240) / 300f);
                        }
                    }
                }
            });
        }
        private void InitMass_Spring()
        {
            masses.Clear();
            springs.Clear();
            for (int j = 0; j < RopPosFir.Count; j++)
            {
                masses.Add(new List<Mass>());
                springs.Add(new List<Spring>());
                for (int i = 0; i < RopPosFirC[j]; i++)
                {
                    float x = i == RopPosFirC[j] - 1 ? 1.3f : 1f;
                    masses[j].Add(new Mass(RopPosFirS[j] * Main.rand.NextFloat(0.45f, 0.55f) * x,
                        Main.MouseScreen + new Vector2(0, 6 * i), i == 0));
                }
                for (int i = 1; i < RopPosFirC[j]; i++)
                {
                    springs[j].Add(new Spring(0.3f, 20, 0.05f, masses[j][i - 1], masses[j][i]));
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {     
            if(RopPosFir.Count < 1)
            {
                GetRopePosFir("FireflyTreeRope");
                InitMass_Spring();
            }
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameY == 0)
            {
                Point point = new Point(i, j);
                if (tile != null && tile.HasTile)
                {
                    Texture2D value = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTree");
                    Texture2D valueG = MythContent.QuickTexture("TheFirefly/Tiles/FireflyTreeGlow");
                    Vector2 value2 = point.ToWorldCoordinates(8f, 8f);
                    Color color = Lighting.GetColor(i ,j);
                    SpriteEffects effects = SpriteEffects.None;
                    int Count = 16;
                    Vector2 HalfSize = value.Size() / 2f;
                    HalfSize.X /= Count;
                    Main.spriteBatch.Draw(value, value2 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, 0, value.Width / Count, value.Height), color, 0f, HalfSize, 1f, effects, 0f);
                    Main.spriteBatch.Draw(valueG, value2 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, 0, value.Width / Count, value.Height), new Color(1f, 1f, 1f,0), 0f, HalfSize, 1f, effects, 0f);

                    Vector2 RopOffset = new Vector2(i * 16 - tile.TileFrameX, j * 16) - Main.screenPosition;

                   

                    for (int k = 0; k < RopPosFir.Count; k++)
                    {
                        foreach (var massJ in masses[k])
                        {
                            if(RopPosFir[k].X > tile.TileFrameX && RopPosFir[k].X <= tile.TileFrameX + 256)
                            {
                                Vector2 DrawP = massJ.position + RopPosFir[k] + RopOffset;
                                massJ.force += new Vector2(0.02f + 0.02f * (float)(Math.Sin(Main.timeForVisualEffects / 72f + DrawP.X / 13d + DrawP.Y / 4d)), 0) * (Main.windSpeedCurrent + 1f) * 2f;
                                //mass.force -= mass.velocity * 0.1f;
                                //重力加速度（可调
                                massJ.force += new Vector2(0, 1.0f) * massJ.mass;
                                Texture2D t0 = MythContent.QuickTexture("TheFirefly/Backgrounds/Drop");
                                int FiIdx = masses[k].FindIndex(mass => mass.position == massJ.position);
                                float Scale = massJ.mass * 2f;
                                if (FiIdx > 0)
                                {
                                    Vector2 v0 = massJ.position - masses[k][FiIdx - 1].position;
                                    float Rot = (float)(Math.Atan2(v0.Y, v0.X)) - (float)(Math.PI / 2d);
                                    for (int z = 0; z < FiIdx; z++)
                                    {
                                        Main.spriteBatch.Draw(t0, DrawP, null, color, Rot, t0.Size() / 2f, Scale, SpriteEffects.None, 0);
                                    }
                                }
                            }             
                        }
                    }

                    for (int k = 0; k < RopPosFir.Count; k++)
                    {
                        masses[k][0].position = Vector2.Zero;
                        float deltaTime = 1;
                        foreach (var spring in springs[k])
                        {
                            spring.ApplyForce(deltaTime);
                        }
                        List<Vector2> massPositions = new List<Vector2>();
                        foreach (var massJ in masses[k])
                        {
                            massJ.Update(deltaTime);
                            massPositions.Add(massJ.position);
                        }
                        List<Vector2> massPositionsSmooth = new List<Vector2>();
                        massPositionsSmooth = Commons.Function.BezierCurve.Bezier.SmoothPath(massPositions);
                        if (massPositionsSmooth.Count > 0)
                        {
                            //DrawRope(massPositionsSmooth, RopPosFir[k] + RopOffset, Vertices);
                        }
                    }
                }
            }

            
            return false;
        }
    }
}
