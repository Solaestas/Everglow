using Everglow.Commons.Utilities;
using Terraria.Audio;
using Terraria.ObjectData;

namespace Everglow.Commons.TileHelper;

public abstract class ShapeDataTile : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = TotalHeight;
		TileObjectData.newTile.Width = TotalWidth;
		TileObjectData.newTile.CoordinateHeights = new int[TotalHeight];
		for (int i = 0; i < TotalHeight; i++)
		{
			TileObjectData.newTile.CoordinateHeights[i] = 16;
			if (i == TotalHeight - 1)
			{
				TileObjectData.newTile.CoordinateHeights[i] = 20;
			}
		}
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.addTile(Type);
	}
	public int[,] PixelHasTile;
	public int TotalWidth;
	public int TotalHeight;
	/// <summary>
	/// 是否掉落多个物品
	/// </summary>
	public bool MultiItem = false;
	/// <summary>
	/// 物品种类
	/// </summary>
	public int CustomItemType = -1;
	/// <summary>
	/// 必填项,实际有物块的轮廓
	/// </summary>
	public virtual string ShapePath => Texture + "_shape.bmp";
	public override void Load()
	{
		var imageData = ImageReader.Read<SixLabors.ImageSharp.PixelFormats.Rgb24>(ShapePath);
		imageData.ProcessPixelRows(accessor =>
		{
			PixelHasTile = new int[accessor.Width, accessor.Height];
			TotalWidth = accessor.Width;
			TotalHeight = accessor.Height;
			for (int y = 0; y < accessor.Height; y++)
			{
				var pixelRow = accessor.GetRowSpan(y);
				for (int x = 0; x < pixelRow.Length; x++)
				{
					ref var pixel = ref pixelRow[x];
					PixelHasTile[x, y] = pixel.R;
				}
			}
		});
		base.Load();
	}
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		if (fail)
			return;
		var thisTile = Main.tile[i, j];
		int x0 = i - thisTile.TileFrameX / 18 + 1;
		int y0 = j - thisTile.TileFrameY / 18 + 1;
		int times = 1;
		for (int x = 0; x < TotalWidth; x++)
		{
			for (int y = 0; y < TotalHeight; y++)
			{
				var tile = Main.tile[x0 + x, y0 + y];
				if (tile.TileFrameX == x * 18 && tile.TileFrameY == y * 18)
				{
					if (tile.TileType == Type && PixelHasTile[x, y] >= 200)
					{
						times++;
						tile.HasTile = false;
						for (int f = 0; f < 15; f++)
						{
							Dust.NewDust(new Vector2(x0 + x, y0 + y) * 16, 16, 16, DustType, 0, 0, 0, default, 1);
						}
						if (MultiItem)
						{
							CustomDropItem(x0 + x, y0 + y);
						}
					}
				}
			}
		}
		if (!MultiItem)
		{
			CustomDropItem(i, j);
		}
		SoundEngine.PlaySound(HitSound, new Vector2(i * 16, j * 16));
	}
	public virtual void CustomDropItem(int i, int j)
	{
		if (CustomItemType > 0)
		{
			Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 16, 16, new Item(CustomItemType, 1));
		}
	}
	/// <summary>
	/// 从左上角安置一个造型物块
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public virtual void PlaceOriginAtTopLeft(int x, int y)
	{
		if (x > Main.maxTilesX - TotalWidth || x < 0 || y > Main.maxTilesY - TotalHeight || y < 0)
			return;
		for (int i = 0; i < TotalWidth; i++)
		{
			for (int j = 0; j < TotalHeight; j++)
			{
				if (PixelHasTile[i, TotalHeight - j] >= 200)
				{
					Tile tile = Main.tile[x + i, y + j];
					tile.TileType = Type;
					tile.TileFrameX = (short)(i * 18);
					tile.TileFrameY = (short)(j * 18);
					tile.HasTile = true;
				}
			}
		}
	}
	/// <summary>
	/// 从左下角安置一个造型物块
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public virtual void PlaceOriginAtBottomLeft(int x, int y)
	{
		if (x > Main.maxTilesX - TotalWidth || x < 0 || y > Main.maxTilesY || y - TotalHeight < 0)
			return;
		for (int i = 0; i < TotalWidth; i++)
		{
			for (int j = 0; j < TotalHeight; j++)
			{
				if (PixelHasTile[i, TotalHeight - j - 1] >= 200)
				{
					Tile tile = Main.tile[x + i, y - j];
					tile.TileType = Type;
					tile.TileFrameX = (short)(i * 18);
					tile.TileFrameY = (short)((TotalHeight - j - 1) * 18);
					tile.HasTile = true;
				}
			}
		}
	}
}

