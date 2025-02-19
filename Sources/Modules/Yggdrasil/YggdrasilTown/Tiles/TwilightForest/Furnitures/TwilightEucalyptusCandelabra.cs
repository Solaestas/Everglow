using Everglow.Yggdrasil.YggdrasilTown.Dusts.TwilightForest;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.TwilightForest.Furnitures;

public class TwilightEucalyptusCandelabra : ModTile
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileTable[Type] = true;
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		TileID.Sets.HasOutlines[Type] = true;
		TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
		TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
		TileID.Sets.IsValidSpawnPoint[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = ModContent.DustType<TwilightEucalyptusWoodDust>(); // You should set a kind of dust manually.
		AdjTiles = new int[] { TileID.Candelabras };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2); // this style already takes care of direction for us
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(69, 36, 78), name);
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.240f;
		g = 0.600f;
		b = 0.720f;
	}

	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 2, 2);
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 54)
		{
			int frequency = 20;
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)) && Main.rand.NextBool(frequency))
			{
				Rectangle dustBox = Utils.CenteredRectangle(new Vector2(i * 16 + 8, j * 16 + 4), new Vector2(16, 16));
				int numForDust = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, ModContent.DustType<TwilightEucalyptusWoodDust>(), 0f, 0f, 254, default, Main.rand.NextFloat(0.95f, 1.75f));
				Dust obj = Main.dust[numForDust];
				obj.velocity *= 0.4f;
				Main.dust[numForDust].velocity.Y -= 0.4f;
			}
		}
	}



	public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
	{
		offsetY = 2;
	}
}