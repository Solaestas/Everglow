using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using Terraria;
using Terraria.Localization;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// Visulalize the data of mouse-covered-tile.
/// </summary>
public class TileDataReader : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.value = 0;
		Item.rare = ItemRarityID.White;
	}

	public override void HoldItem(Player player)
	{
		int i = Main.MouseWorld.ToTileCoordinates().X;
		int j = Main.MouseWorld.ToTileCoordinates().Y;
		if (!TileDataReaderSystem.OwnerPlayerWhoAmI.Contains(player.whoAmI))
		{
			TileDataReaderSystem.OwnerPlayerWhoAmI.Add(player.whoAmI);
			TileDataReaderSystem vfx = new TileDataReaderSystem { FixPoint = new Point(i, j), Active = true, Visible = true };
			Ins.VFXManager.Add(vfx);
		}
	}
}

[Pipeline(typeof(WCSPipeline))]
public class TileDataReaderSystem : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public Texture2D Texture;
	public Point FixPoint;
	public static List<int> OwnerPlayerWhoAmI = new List<int>();

	public override void OnSpawn()
	{
		Texture = ModAsset.TileBlock.Value;
	}

	public override void Update()
	{
		FixPoint = Main.MouseWorld.ToTileCoordinates();
		int i = FixPoint.X;
		int j = FixPoint.Y;
		Player player = Main.LocalPlayer;
		if (i < 20 || i > Main.maxTilesX - 20)
		{
			if (j < 20 || j > Main.maxTilesY - 20)
			{
				Active = false;
				OwnerPlayerWhoAmI.Remove(player.whoAmI);
				return;
			}
		}

		if (player.HeldItem.type != ModContent.ItemType<TileDataReader>())
		{
			Active = false;
			OwnerPlayerWhoAmI.Remove(player.whoAmI);
			return;
		}
		base.Update();
	}

	public override void Draw()
	{
		int i = FixPoint.X;
		int j = FixPoint.Y;
		Player player = Main.LocalPlayer;
		if (i < 20 || i > Main.maxTilesX - 20)
		{
			if (j < 20 || j > Main.maxTilesY - 20)
			{
				Active = false;
				OwnerPlayerWhoAmI.Remove(player.whoAmI);
				return;
			}
		}
		Tile tile = Main.tile[i, j];
		Ins.Batch.BindTexture<Vertex2D>(Texture);
		int colorType = ItemRarityID.White;
		Color drawColor = Color.White;
		if(!tile.HasTile)
		{
			colorType = ItemRarityID.Gray;
			drawColor = Color.Gray;
		}
		DrawBlockBound(i, j, drawColor);
		string datas = GetDatas(i, j);
		Main.instance.MouseText(datas, colorType);
	}

	public string GetDatas(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		string datas = "HasTile: " + tile.HasTile;
		datas += "\nType :" + tile.TileType;
		if(tile.HasTile)
		{
			datas += "\nCoordinate: [" + i + ", " + j + "]";
			datas += "\nFrame : [" + tile.TileFrameX + ", " + tile.TileFrameY + "]";
		}
		return datas;
	}

	public void DrawBlockBound(int i, int j, Color color)
	{
		Vector2 pos = new Vector2(i, j) * 16;
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(pos, color, new Vector3(0, 0, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),

			new Vertex2D(pos + new Vector2(0, 16), color, new Vector3(0, 1, 0)),
			new Vertex2D(pos + new Vector2(16, 0), color, new Vector3(1, 0, 0)),
			new Vertex2D(pos + new Vector2(16), color, new Vector3(1, 1, 0)),
		};

		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}
}