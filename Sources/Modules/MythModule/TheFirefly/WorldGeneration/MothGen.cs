using Everglow.Sources.Modules.MythModule.Common;
using System.Drawing;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    public class MothLand : ModSystem
    {
        private class MothLandGenPass : GenPass
        {
            public MothLandGenPass() : base("MothLand", 10)
            {
            }

            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                //TODO 翻译
                Main.statusText = "Building MothCave";
                BuildMothCave();
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) => tasks.Add(new MothLandGenPass());
        /// <summary>
        /// 地形中心坐标
        /// </summary>
        public int FireflyCenterX = 2000;
        public int FireflyCenterY = 500;
        //读存
        public override void OnWorldLoad()
        {
            FireflyCenterX = 2000;
            FireflyCenterY = 500;
        }

        public override void OnWorldUnload()
        {
            FireflyCenterX = 2000;
            FireflyCenterY = 500;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["FIREFLYcenterX"] = FireflyCenterX;
            tag["FIREFLYcenterY"] = FireflyCenterY;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            FireflyCenterX = tag.GetAsInt("FIREFLYcenterX");
            FireflyCenterY = tag.GetAsInt("FIREFLYcenterY");
        }

        /// <summary>
        /// type = 0:Kill,type = 1:place Tiles,type = 2:place Walls
        /// </summary>
        /// <param name="Shapepath"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="type"></param>
        public static void ShapeTile(string Shapepath, int a, int b, int type)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new Exception("Windows限定");
            }
            using Stream Img = Everglow.Instance.GetFileStream("Sources/Modules/MythModule/TheFirefly/WorldGeneration/" + Shapepath);
            Bitmap cocoon = new Bitmap(Img);
            for (int y = 0; y < cocoon.Height; y += 1)
            {
                for (int x = 0; x < cocoon.Width; x += 1)
                {
                    switch (type)
                    {
                        case 0:
                            if (CheckColor(cocoon.GetPixel(x, y), new Vector4(255, 0, 0, 255)))
                            {
                                if (Main.tile[x + a, y + b].TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                {
                                    Main.tile[x + a, y + b].ClearEverything();
                                }
                            }
                            break;
                        case 1:
                            if (CheckColor(cocoon.GetPixel(x, y), new Vector4(56, 48, 61, 255)))
                            {
                                if (Main.tile[x + a, y + b].TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                {
                                    Main.tile[x + a, y + b].TileType = (ushort)ModContent.TileType<Tiles.DarkCocoon>();
                                    ((Tile)Main.tile[x + a, y + b]).HasTile = true;
                                }
                            }
                            break;
                        case 2:
                            if (CheckColor(cocoon.GetPixel(x, y), new Vector4(0, 0, 5, 255)))
                            {
                                if (Main.tile[x + a, y + b].TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                                {
                                    Main.tile[x + a, y + b].WallType = (ushort)ModContent.WallType<Walls.DarkCocoonWall>();
                                }
                            }
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 建造流萤之茧
        /// </summary>
        public static void BuildMothCave()
        {
            Point16 AB = CocoonPos();
            int a = AB.X;
            int b = AB.Y;
            MothLand mothLand = ModContent.GetInstance<MothLand>();
            mothLand.FireflyCenterX = a + 140;
            mothLand.FireflyCenterY = b + 140;
            ShapeTile("CocoonKill.bmp", a, b, 0);
            ShapeTile("Cocoon.bmp", a, b, 1);
            ShapeTile("CocoonWall.bmp", a, b, 2);
            SmoothMothTile(a, b);
        }
        private static int GetCrash(int PoX, int PoY)
        {
            int CrashCount = 0;
            ushort[] DangerTileType = new ushort[]
            {
                41,//蓝地牢砖
                43,//绿地牢砖
                44,//粉地牢砖
                48,//尖刺
                49,//水蜡烛
                50,//书
                137,//神庙机关
                226,//神庙石砖
                232,//木刺
                237,//神庙祭坛
                481,//碎蓝地牢砖
                482,//碎绿地牢砖
                483//碎粉地牢砖
            };
            for (int x = -256; x < 257; x += 8)
            {
                for (int y = -128; y < 129; y += 8)
                {
                    if (Array.Exists<ushort>(DangerTileType, Ttype => Ttype == Main.tile[x + PoX, y + PoY].TileType))
                    {
                        CrashCount++;
                    }
                }
            }
            return CrashCount;
        }
        /// <summary>
        /// 获取一个不与原版地形冲突的点
        /// </summary>
        /// <returns></returns>
        private static Point16 CocoonPos()
        {
            int PoX = Main.rand.Next(300, Main.maxTilesX - 600);
            int PoY = Main.rand.Next(400, Main.maxTilesY - 300);

            while (GetCrash(PoX, PoY) > 0)
            {
                PoX = Main.rand.Next(300, Main.maxTilesX - 600);
                PoY = Main.rand.Next(400, Main.maxTilesY - 300);
            }
            return new Point16(PoX, PoY);
        }
        private static void SmoothMothTile(int a, int b)
        {
            for (int y = 0; y < 256; y += 1)
            {
                for (int x = 0; x < 512; x += 1)
                {
                    if (Main.tile[x + a, y + b].TileType == (ushort)ModContent.TileType<Tiles.DarkCocoon>())
                    {
                        Tile.SmoothSlope(x + a, y + b, false);
                        WorldGen.TileFrame(x + a, y + b, true, false);
                    }
                    else
                    {
                        WorldGen.TileFrame(x + a, y + b, true, false);
                    }
                    WorldGen.SquareWallFrame(x + a, y + b, true);
                }
            }
        }
        /// <summary>
        /// 判定颜色是否吻合
        /// </summary>
        /// <param name="c0"></param>
        /// <param name="RGBA"></param>
        /// <returns></returns>
        private static bool CheckColor(System.Drawing.Color c0, Vector4 RGBA)
        {
            Vector4 v0 = new Vector4(c0.R, c0.G, c0.B, c0.A);
            return v0 == RGBA;
        }
        //加了个环境光，但还要调整下不然看上去很怪
        public readonly Vector3 ambient = new Vector3(0.001f, 0.001f, 0.05f);
        public override void OnModLoad()
        {
            Everglow.HookSystem.AddMethod(DrawBackground, Commons.Core.CallOpportunity.PostDrawBG);
            On.Terraria.Graphics.Light.TileLightScanner.GetTileLight += TileLightScanner_GetTileLight;
        }

        private void TileLightScanner_GetTileLight(On.Terraria.Graphics.Light.TileLightScanner.orig_GetTileLight orig, Terraria.Graphics.Light.TileLightScanner self, int x, int y, out Vector3 outputColor)
        {
            orig(self, x, y, out outputColor);
            outputColor += ambient;
        }

        public override void PostUpdateEverything()
        {          
            if (BiomeActive())
            {
                Everglow.HookSystem.DisableDrawBackground = true;
            }
            else
            {
                Everglow.HookSystem.DisableDrawBackground = false;
            }      
        }
        public bool BiomeActive()
        {
            Vector2 BiomeCenter = new Vector2(FireflyCenterX * 16, (FireflyCenterY - 20) * 16);
            Vector2 v0 = Main.LocalPlayer.Center - BiomeCenter;
            v0.Y *= 1.35f;
            v0.X *= 0.9f;
            return (v0.Length() < 2000);
        }
        private void DrawBackground()
        {
            if (!BiomeActive())
            {
                return;
            }
            var tex = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyClose");
            var glowmask = MythContent.QuickTexture("TheFirefly/Backgrounds/FireflyClose_Glow");
            Vector2 min = Main.screenPosition;
            Vector2 max = Main.screenPosition + Main.ScreenSize.ToVector2();
            Point tlTile = min.ToTileCoordinates();
            Point rbTile = max.ToTileCoordinates() + new Point(1, 1);
            int width = rbTile.X - tlTile.X;
            int height = rbTile.Y - tlTile.Y;
            Point sampleTopleft = Point.Zero;
            Point sampleSize = tex.Size().ToPoint();
            Point sampleCenter = sampleTopleft + (tex.Size() / 2).ToPoint();
            Point screenSize = new Point(Main.screenWidth, Main.screenHeight);
            Player localP = Main.LocalPlayer;
            Vector2 deltaPos = localP.Center - new Vector2(FireflyCenterX * 16f, FireflyCenterY * 16f);
            deltaPos *= 0.25f;
            Point Move = new Point((int)deltaPos.X, (int)(deltaPos.Y));
            //Light Background
            //for (int i = 0; i < width; i++)
            //{
            //    for (int j = 0; j < height; j++)
            //    {
            //        Color light = Lighting.GetColor(tlTile.X + i, tlTile.Y + j);
            //        if (light == Color.Black)
            //        {
            //            continue;
            //        }
            //        int x = (tlTile.X + i) * 16;
            //        int y = (tlTile.Y + j) * 16;
            //        int a = x + 16, b = y + 16;
            //        x = Math.Clamp(x, (int)min.X, (int)max.X);
            //        y = Math.Clamp(y, (int)min.Y, (int)max.Y);
            //        a = Math.Clamp(a, (int)min.X, (int)max.X);
            //        b = Math.Clamp(b, (int)min.Y, (int)max.Y);
            //        Rectangle rect = new Rectangle(x - (int)Main.screenPosition.X, y - (int)Main.screenPosition.Y, a - x, b - y);
            //        Rectangle source = new Rectangle(sampleTopleft.X + sampleSize.X * (x - (int)min.X) / Main.screenWidth, sampleTopleft.Y + sampleSize.Y * (y - (int)min.Y) / Main.screenHeight, a - x, b - y);
            //        Main.spriteBatch.Draw(tex, rect, source, light);
            //    }
            //}


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            //Main.spriteBatch.Draw(tex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Rectangle(sampleCenter.X - screenSize.X / 8 + Move.X, sampleCenter.Y - screenSize.Y / 8 + Move.Y, screenSize.X / 4, screenSize.Y / 4), Color.White);
            Main.spriteBatch.Draw(tex, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Rectangle(sampleCenter.X - screenSize.X / 2 + Move.X, sampleCenter.Y - screenSize.Y / 2 + Move.Y, screenSize.X, screenSize.Y), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //加了环境光所有物块都会绘制就不用填黑了
            //for (int i = 0; i < width; i++)
            //{
            //    for (int j = 0; j < height; j++)
            //    {
            //        Color light = Lighting.GetColor(tlTile.X + i, tlTile.Y + j);
            //        Tile tile = Main.tile[tlTile.X + i, tlTile.Y + j];
            //        if (light != Color.Black || !tile.HasTile)
            //        {
            //            continue;
            //        }
            //        int x = (tlTile.X + i) * 16;
            //        int y = (tlTile.Y + j) * 16;
            //        int a = x + 16, b = y + 16;
            //        x = Math.Clamp(x, (int)min.X, (int)max.X);
            //        y = Math.Clamp(y, (int)min.Y, (int)max.Y);
            //        a = Math.Clamp(a, (int)min.X, (int)max.X);
            //        b = Math.Clamp(b, (int)min.Y, (int)max.Y);
            //        Rectangle rect = new Rectangle(x - (int)Main.screenPosition.X, y - (int)Main.screenPosition.Y, a - x, b - y);
            //        Rectangle source = new Rectangle(sampleTopleft.X + sampleSize.X * (x - (int)min.X) / Main.screenWidth, sampleTopleft.Y + sampleSize.Y * (y - (int)min.Y) / Main.screenHeight, a - x, b - y);
            //        Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, rect, source, light);
            //    }
            //}
        }
    }
}

