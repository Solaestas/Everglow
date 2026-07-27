using Everglow.Yggdrasil.WorldGeneration;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

public class MeltingButton : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileNoAttach[Type] = false;
		Main.tileWaterDeath[Type] = false;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
		TileObjectData.newTile.Origin = new(0, 0);
		TileObjectData.newTile.Height = 1;
		TileObjectData.newTile.Width = 1;

		TileObjectData.newTile.CoordinateHeights = new int[1];
		Array.Fill(TileObjectData.newTile.CoordinateHeights, 16);
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new Point16(0, 0);

		TileObjectData.addTile(Type);
		DustType = DustID.Lava;
		AddMapEntry(new Color(255, 10, 10));
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0f;
		g = 0f;
		b = 0f;
		base.ModifyLight(i, j, ref r, ref g, ref b);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		// Tile tile = Main.tile[i, j];
		// var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		// if (Main.drawToScreen)
		// {
		// zero = Vector2.Zero;
		// }
		// var texture = ModContent.Request<Texture2D>(Texture).Value;
		// spriteBatch.Draw(texture, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY + 36, 16, 16), new Color(1f, 0.3f, 0.1f, 0), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
		return base.PreDraw(i, j, spriteBatch);
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		Tile tile = TileUtils.SafeGetTile(i, j);
		tile.TileFrameX = 0;
		base.NearbyEffects(i, j, closer);
	}

	public override void MouseOver(int i, int j)
	{
		Tile tile = TileUtils.SafeGetTile(i, j);
		tile.TileFrameX = 18;
		string text = "Meltdown";
		Main.instance.MouseText(text, ItemRarityID.Red);
		base.MouseOver(i, j);
	}

	public override bool RightClick(int i, int j)
	{
		for (int x = -15; x <= 15; x++)
		{
			for (int y = -15; y <= 15; y++)
			{
				if (new Vector2(x, y).Length() <= 15)
				{
					int chestIndex = Chest.FindChest(i + x, j + y);
					if (chestIndex >= 0)
					{
						Chest chest = Main.chest[chestIndex];
						int totalValue = 0;
						for (int k = 0; k < chest.item.Length; k++)
						{
							Item item = chest.item[k];
							if (item != null)
							{
								float itemValue = 1 + item.value / (100 + MathF.Sqrt(item.value * 10));
								int rare = Math.Min(10, item.rare);
								float rareValue = 6f - (rare - 10) * (rare - 10) / 20f;
								int value = (int)(rareValue * itemValue * item.stack);
								item.stack = 0;
								item.active = false;
								item.type = ItemID.None;
								item.ModItem = null;

								// if(item.stack > 0)
								// {
								// Main.NewText(rareValue, Color.Yellow);
								// Main.NewText(itemValue, Color.Red);
								// Main.NewText(item.stack, Color.Green);
								// }
								totalValue += value;
							}
						}
						string name = chest.name;
						chest = new Chest();
						chest.name = name;
						if (YggdrasilTownFurnaceSystem.CurrentPlayer != null)
						{
							Player player = YggdrasilTownFurnaceSystem.CurrentPlayer;
							FurnacePlayer fPlayer = player.GetModPlayer<FurnacePlayer>();
							fPlayer.FurnaceScore += totalValue;
						}
						YggdrasilTownFurnaceSystem.CurrentEnergy += totalValue;
					}
				}
			}
		}
		return base.RightClick(i, j);
	}
}