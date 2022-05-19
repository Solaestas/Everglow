namespace Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration
{
    public class MothLand : ModSystem
    {
        public override void OnWorldLoad()
        {
            CanSoilC = false;
        }
        public bool CanSoilC = false;
        public void BuildMothCave()
        {
            Texture2D Cocoon = Common.MythContent.QuickTexture("TheFirefly/WorldGeneration/Cocoon");
            Texture2D CocoonWall = Common.MythContent.QuickTexture("TheFirefly/WorldGeneration/CocoonWall");
            Texture2D CocoonKill = Common.MythContent.QuickTexture("TheFirefly/WorldGeneration/CocoonKill");
            Color[] colorCo = new Color[Cocoon.Width * Cocoon.Height];
            Cocoon.GetData(colorCo);
            Color[] colorCoW = new Color[CocoonWall.Width * CocoonWall.Height];
            CocoonWall.GetData(colorCoW);
            Color[] colorCoK = new Color[CocoonKill.Width * CocoonKill.Height];
            CocoonKill.GetData(colorCoK);
            int a = 0;
            int b = 0;
            for (int h0 = Main.maxTilesY - 200; h0 > 200; h0 -= 15)
            {
                for (int w0 = 20; w0 < Main.maxTilesX - 240; w0 += 20)
                {
                    if (Main.tile[w0, h0].TileType == (ushort)ModContent.TileType<Tiles.DarkCocoon>())
                    {
                        return;
                    }
                    if (Main.tile[w0, h0].TileType == 25)
                    {
                        a = w0;
                        b = h0 + 44;
                        h0 = 0;
                        CanSoilC = true;
                        break;
                    }
                }
            }
            if (!CanSoilC)
            {
                a = (int)(Main.maxTilesX * 0.7);
                b = (int)(Main.maxTilesY * 0.3);
                if (Main.tile[a, b].TileType == 226 || Main.tile[a, b].WallType == 112)
                {
                    a -= 1000;
                }
                if (Main.tile[a + 256, b].TileType == 226 || Main.tile[a + 256, b].WallType == 112)
                {
                    a -= 1000;
                }
                if (Main.tile[a, b + 128].TileType == 226 || Main.tile[a, b + 128].WallType == 112)
                {
                    a -= 1000;
                }
                if (Main.tile[a + 256, b + 128].TileType == 226 || Main.tile[a + 256, b + 128].WallType == 112)
                {
                    a -= 1000;
                }
            }
            for (int y = 0; y < CocoonKill.Height; y += 1)
            {
                for (int x = 0; x < CocoonKill.Width; x += 1)
                {
                    if (new Color(colorCoK[x + y * CocoonKill.Width].R, colorCoK[x + y * CocoonKill.Width].G, colorCoK[x + y * CocoonKill.Width].B) == new Color(255, 0, 0))
                    {
                        if (Main.tile[x + a, y + b].TileType != 21 && Main.tile[x + a, y + b - 1].TileType != 21)
                        {
                            Main.tile[x + a, y + b].ClearEverything();
                        }
                    }
                }
            }
            for (int y = 0; y < CocoonWall.Height; y += 1)
            {
                for (int x = 0; x < CocoonWall.Width; x += 1)
                {
                    if (new Color(colorCoW[x + y * CocoonWall.Width].R, colorCoW[x + y * CocoonWall.Width].G, colorCoW[x + y * CocoonWall.Width].B) == new Color(0, 0, 5))
                    {
                        Main.tile[x + a, y + b].WallType = (ushort)ModContent.WallType<Walls.DarkCocoonWall>();
                    }
                }
            }
            for (int y = 0; y < Cocoon.Height; y += 1)
            {
                for (int x = 0; x < Cocoon.Width; x += 1)
                {
                    if (new Color(colorCo[x + y * Cocoon.Width].R, colorCo[x + y * Cocoon.Width].G, colorCo[x + y * Cocoon.Width].B) == new Color(56, 48, 61))
                    {
                        Main.tile[x + a, y + b].TileType = (ushort)ModContent.TileType<Tiles.DarkCocoon>();
                        ((Tile)Main.tile[x + a, y + b]).HasTile = true;
                    }
                    /*if (new Color(colorCo[x + y * Cocoon.Width].R, colorCo[x + y * Cocoon.Width].G, colorCo[x + y * Cocoon.Width].B) == new Color(255, 0, 0))
                    {
                        Main.tile[x + a, y + b].TileType = (ushort)ModContent.TileType<Tiles.AbDarkCocoon>();
                        ((Tile)Main.tile[x + a, y + b]).HasTile = true;
                    }*/
                }
            }
            for (int y = 0; y < Cocoon.Height; y += 1)
            {
                for (int x = 0; x < Cocoon.Width; x += 1)
                {
                    /*if (new Color(colorCo[x + y * Cocoon.Width].R, colorCo[x + y * Cocoon.Width].G, colorCo[x + y * Cocoon.Width].B) == new Color(81, 110, 255))
                    {
                        WorldGen.PlaceTile(x + a, y + b, (ushort)ModContent.TileType<Tiles.BlackStarShrubSmall>());
                        short num0 = (short)(Main.rand.Next(0, 6) * 48);
                        Main.tile[x + a, y + b].TileFrameX = num0;
                        Main.tile[x + a, y + b + 1].TileFrameX = num0;
                    }
                    if (new Color(colorCo[x + y * Cocoon.Width].R, colorCo[x + y * Cocoon.Width].G, colorCo[x + y * Cocoon.Width].B) == new Color(100, 84, 168))
                    {
                        WorldGen.PlaceTile(x + a, y + b, (ushort)ModContent.TileType<Tiles.BlackStarFruit>());
                        short num0 = (short)(Main.rand.Next(0, 6) * 48);
                        Main.tile[x + a, y + b].TileFrameX = num0;
                        Main.tile[x + a, y + b + 1].TileFrameX = num0;
                    }
                    if (new Color(colorCo[x + y * Cocoon.Width].R, colorCo[x + y * Cocoon.Width].G, colorCo[x + y * Cocoon.Width].B) == new Color(84, 172, 255))
                    {
                        WorldGen.PlaceTile(x + a, y + b, (ushort)ModContent.TileType<Tiles.BlackStarShrub>());
                        short num0 = (short)(Main.rand.Next(0, 6) * 72);
                        Main.tile[x + a, y + b].TileFrameX = num0;
                        Main.tile[x + a, y + b + 1].TileFrameX = num0;
                        Main.tile[x + a, y + b + 2].TileFrameX = num0;
                    }*/
                }
            }
            for (int y = 0; y < CocoonWall.Height; y += 1)
            {
                for (int x = 0; x < CocoonWall.Width; x += 1)
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
    }
}

