using Everglow.Yggdrasil.KelpCurtain.Tiles;
using Everglow.Yggdrasil.KelpCurtain.Walls;
using Terraria.Utilities;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;
namespace Everglow.Yggdrasil.WorldGeneration;
public class KelpCurtainGeneration
{
	public static void BuildKelpCurtain()
	{
		Initialize();
		Main.statusText = "Kelp Curtain Bark Cliff...";
		UnforcablePlaceAreaOfTile(20, 9600, 155, 10650, ModContent.TileType<DragonScaleWood>());
		UnforcablePlaceAreaOfTile(Main.maxTilesX - 155, 9600, Main.maxTilesX - 20, 10650, ModContent.TileType<DragonScaleWood>());

		PlaceRectangleAreaOfWall(20, 9600, 155, 10650, ModContent.WallType<DragonScaleWoodWall>());
		PlaceRectangleAreaOfWall(Main.maxTilesX - 155, 9600, Main.maxTilesX - 20, 10650, ModContent.WallType<DragonScaleWoodWall>());
		//BuildDeathJadeLake();
		BuildRainValley();
	}
	public static int[,] PerlinPixelR = new int[512, 512];
	public static int[,] PerlinPixelG = new int[512, 512];
	public static int[,] PerlinPixelB = new int[512, 512];
	public static int[,] PerlinPixel2 = new int[512, 512];
	public static int AzureGrottoCenterX;
	public static UnifiedRandom GenRand = new UnifiedRandom();

	/// <summary>
	/// 初始化
	/// </summary>
	public static void Initialize()
	{
		GenRand = WorldGen.genRand;
		AzureGrottoCenterX = GenRand.Next(-100, 100) + 600;
		FillPerlinPixel();
	}
	/// <summary>
	/// 噪声信息获取
	/// </summary>
	public static void FillPerlinPixel()
	{
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Yggdrasil/WorldGeneration/Noise_rgb.bmp");
		Vector2 perlinCoordCenter = new Vector2(GenRand.NextFloat(0f, 1f), GenRand.NextFloat(0f, 1f));
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				int newY = (int)(accessor.Height * perlinCoordCenter.Y + y) % accessor.Height;
				var pixelRow = accessor.GetRowSpan(newY);
				for (int x = 0; x < pixelRow.Length; x++)
				{
					int newX = (int)(accessor.Width * perlinCoordCenter.X + x) % accessor.Width;
					ref var pixel = ref pixelRow[newX];
					PerlinPixelR[x, y] = pixel.R;
					PerlinPixelG[x, y] = pixel.G;
					PerlinPixelB[x, y] = pixel.B;
				}
			}
		});

		imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>("Everglow/Yggdrasil/WorldGeneration/Noise_perlin.bmp");
		perlinCoordCenter = new Vector2(GenRand.NextFloat(0f, 1f), GenRand.NextFloat(0f, 1f));
		imageData.ProcessPixelRows(accessor =>
		{
			for (int y = 0; y < accessor.Height; y++)
			{
				int newY = (int)(accessor.Height * perlinCoordCenter.Y + y) % accessor.Height;
				var pixelRow = accessor.GetRowSpan(newY);
				for (int x = 0; x < pixelRow.Length; x++)
				{
					int newX = (int)(accessor.Width * perlinCoordCenter.X + x) % accessor.Width;
					ref var pixel = ref pixelRow[newX];
					PerlinPixel2[x, y] = pixel.R;
				}
			}
		});
	}
	/// <summary>
	/// 亡碧湖
	/// </summary>
	public static void BuildDeathJadeLake()
	{
		int startY = 10000;
		int startX = GenRand.Next(60, 90);
		int direction = 1;
		if (GenRand.NextBool(2))
		{
			direction = -1;
		}
		startX *= direction;
		startX += 600;
		while (startY < 12000)
		{
			startY++;
			Tile tile = SafeGetTile(startX, startY);
			if (tile.HasTile)
			{
				startY -= 20;
				break;
			}
		}
		int randY = GenRand.Next(512);
		int randX = GenRand.Next(512);
		int bankWidth = GenRand.Next(220, 240);
		int peakHeight = 0;//记录一个连续的高度
						   //湖堤
		for (int step = 0; step < bankWidth; step++)
		{
			int height = (int)(step * step / 400f + PerlinPixelB[(step + randX) % 512, randY] / 30f) - 24;
			for (int deltaY = 0; deltaY < step; deltaY++)
			{
				int x = startX + step * direction;
				int y = startY - height;
				while (!SafeGetTile(x, y).HasTile)
				{
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
					y++;
				}
			}
			if (height > peakHeight)
			{
				peakHeight = height;
			}
		}
		//湖水
		if (direction == 1)
		{
			for (int x = 50; x <= startX + bankWidth; x++)
			{
				int y = startY - peakHeight + 7;
				while (!SafeGetTile(x, y).HasTile)
				{
					Tile tile = SafeGetTile(x, y);
					tile.LiquidType = LiquidID.Water;
					tile.LiquidAmount = 255;
					y++;
				}
			}
		}
		else
		{
			for (int x = startX - bankWidth; x <= 1150; x++)
			{
				int y = startY - peakHeight + 7;
				while (!SafeGetTile(x, y).HasTile)
				{
					Tile tile = SafeGetTile(x, y);
					tile.LiquidType = LiquidID.Water;
					tile.LiquidAmount = 255;
					y++;
				}
			}
		}
		int lakePeakX = startX + bankWidth * direction;
		randY = GenRand.Next(512);
		randX = GenRand.Next(512);
		for (int step = 0; step < 30; step++)
		{
			int thick = (int)((30 - step) * (30 - step) / 26d + PerlinPixelB[(step + randX) % 512, randY] / 30f);
			for (int deltaY = 0; deltaY < thick; deltaY++)
			{
				int x = lakePeakX + step * direction;
				int y = startY - peakHeight + deltaY;
				Tile tile = SafeGetTile(x, y);
				tile.TileType = (ushort)ModContent.TileType<OldMoss>();
				tile.HasTile = true;
			}
		}
		if (direction == 1)
		{
			randY = GenRand.Next(512);
			randX = GenRand.Next(512);
			for (int step = 0; step < 30; step++)
			{
				int thick = (int)((30 - step) * (30 - step) / 26d + PerlinPixelB[(step + randX) % 512, randY] / 30f);
				for (int deltaY = 0; deltaY < thick; deltaY++)
				{
					int x = Main.maxTilesX - 155 - step * direction;
					int y = startY - peakHeight + deltaY;
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
				}
			}
		}
		else
		{
			randY = GenRand.Next(512);
			randX = GenRand.Next(512);
			for (int step = 0; step < 30; step++)
			{
				int thick = (int)((30 - step) * (30 - step) / 26d + PerlinPixelB[(step + randX) % 512, randY] / 30f);
				for (int deltaY = 0; deltaY < thick; deltaY++)
				{
					int x = 155 + step * direction;
					int y = startY - peakHeight + deltaY;
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
				}
			}
		}
	}
	/// <summary>
	/// 森雨幽谷
	/// </summary>
	public static void BuildRainValley()
	{
		int startY = 10000;
		int randY = GenRand.Next(512);
		int randX = GenRand.Next(512);
		while (startY < 12000)
		{
			startY++;
			Tile tile = SafeGetTile(600, startY);
			if (tile.HasTile)
			{
				break;
			}
		}
		startY -= 200;
		for (int y = startY; y > 9900; y--)
		{
			for (int x = 100; x <= 1100; x++)
			{
				int dense = PerlinPixelB[(x / 4 + randX) % 512, (y + randY) % 512];
				if (dense > 160)
				{
					Tile tile = SafeGetTile(x, y);
					tile.TileType = (ushort)ModContent.TileType<OldMoss>();
					tile.HasTile = true;
				}
			}
		}
	}
	public static void UnforcablePlaceAreaOfTile(int x0, int y0, int x1, int y1, int type)
	{
		for (int x = x0; x <= x1; x += 1)
		{
			for (int y = y0; y <= y1; y += 1)
			{
				Tile tile = SafeGetTile(x, y);
				if (!tile.HasTile)
				{
					tile.TileType = (ushort)type;
					tile.HasTile = true;
				}
			}
		}
	}
}

