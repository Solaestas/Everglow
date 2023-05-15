using Everglow.Commons.Physics;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Terraria.Enums;

namespace Everglow.Myth.TheFirefly.Tiles;

public class BlackVine : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = false;
		TileID.Sets.IsVine[Type] = true;
		DustType = 191;
		Main.tileCut[Type] = true;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(11, 11, 11), modTranslation);
		HitSound = SoundID.Grass;
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.None, 1, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.addTile(Type);

	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield break;
	}
	public override void RandomUpdate(int i, int j)
	{
		int deltaY = 0;
		while (Main.tile[i, j - 1 - deltaY].TileType == Type)
		{
			deltaY++;
			if (deltaY > j - 1)
			{
				break;
			}
		}
		if(deltaY > 15 + Math.Sin(i + j) * 3)
		{
			return;
		}
		var tileBelow = Main.tile[i, j + 1];
		if(!tileBelow.HasTile)
		{
			tileBelow.TileType = Type;
			tileBelow.HasTile = true;
		}
		base.RandomUpdate(i, j);
	}
	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (closer)
		{
			
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		int screenY = (int)((Main.screenPosition.Y + 8) / 16);
		if(j - screenY < 4)
		{
		}
		int deltaY = 0;
		while(Main.tile[i, j - 1 - deltaY].TileType == Type)
		{
			deltaY++;
			if(deltaY > j - 1)
			{
				break;
			}
		}
		if(deltaY > 0)
		{
			return false;
		}
		var tile = Main.tile[i, j - deltaY];
		var tileUp = Main.tile[i, j - 1 - deltaY];
		if (tileUp.TileType != Type && tile.TileType == Type)
		{
			var tileSpin = new TileSpin();
			if (!TileSpin.SwayVine.ContainsKey((i, j)))
			{
				List<Mass> masses = new List<Mass>();
				while (Main.tile[i, j + masses.Count].TileType == Type)
				{
					masses.Add(new Mass(10, new Vector2(0, masses.Count * 16), masses.Count == 0));
				}
				TileSpin.SwayVine.Add(((i, j)), masses);
			}
			else
			{
				tileSpin.UpdateVine(i, j, 10000000f, 20f);
			}
			draw();
		}
		else
		{
			if (!TileSpin.SwayVine.ContainsKey((i, j)))
			{
				TileSpin.SwayVine.Remove((i, j));
			}
		}

		void draw()
		{
			var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;
			Vector2 ToScreen = zero - Main.screenPosition;
			Texture2D tex = ModAsset.Tiles_BlackVine.Value;
			List<Vertex2D> bars = new List<Vertex2D>();
			for (int y = 0; y < TileSpin.SwayVine[(i, j)].Count - 1; y++)
			{
				var tileVine = Main.tile[i, j + y];
				Color drawColor = Lighting.GetColor(i, j + y);
				Vector2 worldCoord = new Vector2(i, j) * 16f + new Vector2(8, 0);
				Vector2 normalize = Utils.SafeNormalize(TileSpin.SwayVine[(i, j)][y + 1].position - TileSpin.SwayVine[(i, j)][y].position, Vector2.zeroVector);
				Vector2 horizontalize = normalize.RotatedBy(Math.PI / 2);

				Vector2 lastNormalize = new Vector2(0, 1);
				if (y > 0)
					lastNormalize = Utils.SafeNormalize(TileSpin.SwayVine[(i, j)][y].position - TileSpin.SwayVine[(i, j)][y - 1].position, Vector2.zeroVector);
				Vector2 lastHorizontalize = lastNormalize.RotatedBy(Math.PI / 2);
				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y].position + lastHorizontalize * 8, Color.Transparent, new Vector3(tileVine.TileFrameX / (float)tex.Width, tileVine.TileFrameY / (float)tex.Height, 0)));
				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y].position - lastHorizontalize * 8, Color.Transparent, new Vector3((tileVine.TileFrameX + 16) / (float)tex.Width, tileVine.TileFrameY / (float)tex.Height, 0)));

				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y].position + lastHorizontalize * 8 - new Vector2(0, 2), drawColor, new Vector3(tileVine.TileFrameX / (float)tex.Width, tileVine.TileFrameY / (float)tex.Height, 0)));
				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y].position - lastHorizontalize * 8 - new Vector2(0, 2), drawColor, new Vector3((tileVine.TileFrameX + 16) / (float)tex.Width, tileVine.TileFrameY / (float)tex.Height, 0)));

				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y + 1].position + horizontalize * 8, drawColor, new Vector3(tileVine.TileFrameX / (float)tex.Width, (tileVine.TileFrameY + 16) / (float)tex.Height, 0)));
				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y + 1].position - horizontalize * 8, drawColor, new Vector3((tileVine.TileFrameX + 16) / (float)tex.Width, (tileVine.TileFrameY + 16) / (float)tex.Height, 0)));

				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y + 1].position + horizontalize * 8, Color.Transparent, new Vector3(tileVine.TileFrameX / (float)tex.Width, (tileVine.TileFrameY + 16) / (float)tex.Height, 0)));
				bars.Add(new Vertex2D(worldCoord + ToScreen + TileSpin.SwayVine[(i, j)][y + 1].position - horizontalize * 8, Color.Transparent, new Vector3((tileVine.TileFrameX + 16) / (float)tex.Width, (tileVine.TileFrameY + 16) / (float)tex.Height, 0)));
			}
			if (bars.Count > 2)
			{
				Main.graphics.GraphicsDevice.Textures[0] = tex;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
		}
		return true;
	}
}