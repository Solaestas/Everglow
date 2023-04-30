namespace Everglow.Myth.TheFirefly.Tiles;

public class DarkCocoon : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileMergeDirt[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkCocoonMoss>()] = true;
		Main.tileMerge[Type][ModContent.TileType<DarkCocoonSpecial>()] = true;
		MinPick = 175;
		DustType = 191;
		ItemDrop = ModContent.ItemType<Items.DarkCocoon>();
		AddMapEntry(new Color(17, 16, 17));
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
	}

	public override void RandomUpdate(int i, int j)
	{
		if (Main.rand.NextBool(6))
		{
			if (Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 2, j].Slope == SlopeType.Solid && Main.tile[i - 2, j].Slope == SlopeType.Solid &&
				Main.tile[i, j + 1].Slope == SlopeType.Solid && Main.tile[i + 1, j + 1].Slope == SlopeType.Solid && Main.tile[i - 1, j + 1].Slope == SlopeType.Solid && Main.tile[i + 2, j + 1].Slope == SlopeType.Solid && Main.tile[i - 2, j + 1].Slope == SlopeType.Solid)//树木
			{
				int MaxHeight = 0;
				for (int x = -2; x < 3; x++)
				{
					for (int y = -1; y > -30; y--)
					{
						if (j + y > 20)
						{
							if (Main.tile[i + x, j + y].HasTile || Main.tile[i + x, j + y].LiquidAmount > 3)
								return;
						}
						MaxHeight = -y;
					}
				}
				if (MaxHeight > 7)
					BuildFluorescentTree(i, j - 1, MaxHeight);
			}
		}

		if (!Main.tile[i, j - 1].HasTile && Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i, j - 1].LiquidAmount > 0)
		{
			Tile tile = Main.tile[i, j - 1];
			tile.TileType = (ushort)ModContent.TileType<LampLotus>();
			tile.HasTile = true;
			tile.TileFrameX = (short)(28 * Main.rand.Next(8));
		}
		if (Main.rand.NextBool(6))//黑萤藤蔓
		{
			Tile t0 = Main.tile[i, j];

			Tile t2 = Main.tile[i, j + 1];
			if (t0.Slope == SlopeType.Solid && !t2.HasTile)
			{
				t2.TileType = (ushort)ModContent.TileType<BlackVine>();
				t2.HasTile = true;
				t2.TileFrameY = (short)(Main.rand.Next(6, 9) * 18);
			}
		}
		if (Main.rand.NextBool(3))//流萤滴
		{
			int count = 0;
			for (int x = -1; x <= 1; x++)
			{
				for (int y = 1; y <= 3; y++)
				{
					Tile t0 = Main.tile[i + x, j + y];
					if (t0.HasTile)
						count++;
					Tile t1 = Main.tile[i + x, j + y - 1];
					if (y == 1 && (!t1.HasTile || t1.Slope != SlopeType.Solid))
						count++;
				}
			}
			if (count == 0)
				Common.MythUtils.PlaceFrameImportantTiles(i - 1, j + 1, 3, 3, ModContent.TileType<Furnitures.GlowingDrop>());

		}
		Tile tx = Main.tile[i, j + 1];
		if (!tx.HasTile)
			NPC.NewNPC(null, i * 16 + Main.rand.Next(-8, 9), j * 16 + 32, ModContent.NPCType<NPCs.LittleFireBulb>());
		if (Main.rand.NextBool(3))//流萤滴
		{
			int count = 0;
			for (int x = -1; x <= 1; x++)
			{
				for (int y = 1; y <= 10; y++)
				{
					Tile t0 = Main.tile[i + x, j + y];
					if (t0.HasTile)
						count++;
					Tile t1 = Main.tile[i + x, j + y - 1];
					if (y == 1 && (!t1.HasTile || t1.Slope != SlopeType.Solid))
						count++;
				}
				foreach (var npc in Main.npc)
				{
					if (npc.active)
					{
						if (Math.Abs(npc.Center.X - i * 16) < 20)
							count++;
					}
				}
			}
			if (count == 0)
				NPC.NewNPC(null, i * 16 + Main.rand.Next(-8, 9), j * 16 + 180, ModContent.NPCType<NPCs.LargeFireBulb>());
		}
		if (!Main.tile[i, j - 1].HasTile && !Main.tile[i + 1, j - 1].HasTile && !Main.tile[i - 1, j - 1].HasTile && Main.tile[i, j].Slope == SlopeType.Solid && Main.tile[i - 1, j].Slope == SlopeType.Solid && Main.tile[i + 1, j].Slope == SlopeType.Solid)//黑萤苣
		{
			Tile t1 = Main.tile[i, j - 1];
			Tile t2 = Main.tile[i, j - 2];
			Tile t3 = Main.tile[i, j - 3];
			for (int x = -1; x < 2; x++)
			{
				for (int y = -3; y < 4; y++)
				{
					if (Main.tile[i + x, j + y].LiquidAmount > 3)
						return;
				}
			}
			if (Main.rand.NextBool(2))
			{
				switch (Main.rand.Next(1, 11))
				{
					case 1:
						t1.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t2.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t1.HasTile = true;
						t2.HasTile = true;
						short numa = (short)(Main.rand.Next(0, 6) * 48);
						t1.TileFrameX = numa;
						t2.TileFrameX = numa;
						t1.TileFrameY = 16;
						t2.TileFrameY = 0;
						break;

					case 2:
						t1.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t2.TileType = (ushort)ModContent.TileType<BlackStarShrubSmall>();
						t1.HasTile = true;
						t2.HasTile = true;
						short num = (short)(Main.rand.Next(0, 6) * 48);
						t2.TileFrameX = num;
						t1.TileFrameX = num;
						t1.TileFrameY = 16;
						t2.TileFrameY = 0;
						break;

					case 3:
						t1.TileType = (ushort)ModContent.TileType<BlackStarShrub>();
						t2.TileType = (ushort)ModContent.TileType<BlackStarShrub>();
						t3.TileType = (ushort)ModContent.TileType<BlackStarShrub>();
						t1.HasTile = true;
						t2.HasTile = true;
						t3.HasTile = true;
						short num1 = (short)(Main.rand.Next(0, 6) * 72);
						t3.TileFrameX = num1;
						t2.TileFrameX = num1;
						t1.TileFrameX = num1;
						t1.TileFrameY = 32;
						t2.TileFrameY = 16;
						t3.TileFrameY = 0;
						break;

					case 4:
						t1.TileType = (ushort)ModContent.TileType<BlueBlossom>();
						t2.TileType = (ushort)ModContent.TileType<BlueBlossom>();
						t3.TileType = (ushort)ModContent.TileType<BlueBlossom>();
						t1.HasTile = true;
						t2.HasTile = true;
						t3.HasTile = true;
						short num2 = (short)(Main.rand.Next(0, 12) * 120);
						t3.TileFrameX = num2;
						t2.TileFrameX = num2;
						t1.TileFrameX = num2;
						t1.TileFrameY = 32;
						t2.TileFrameY = 16;
						t3.TileFrameY = 0;
						break;

					case 5:
						WorldGen.Place3x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFrenLarge>(), Main.rand.Next(3));
						break;

					case 6:
						WorldGen.Place2x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFren>(), Main.rand.Next(3));
						break;

					case 7:
						WorldGen.Place3x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFrenLarge>(), Main.rand.Next(3));
						break;

					case 8:
						WorldGen.Place2x2(i - 1, j - 1, (ushort)ModContent.TileType<BlackFren>(), Main.rand.Next(3));
						break;

					case 9:
						WorldGen.Place2x1(i - 1, j - 1, (ushort)ModContent.TileType<CocoonRock>(), Main.rand.Next(3));
						break;

					case 10:
						WorldGen.Place2x1(i - 1, j - 1, (ushort)ModContent.TileType<CocoonRock>(), Main.rand.Next(3));
						break;
				}
			}
		}
	}
	public static void BuildFluorescentTree(int i, int j, int height = 0)
	{
		if (j < 30)
			return;
		int Height = Main.rand.Next(7, height);

		for (int g = 0; g < Height; g++)
		{
			Tile tile = Main.tile[i, j - g];
			if (g > 3)
			{
				if (Main.rand.NextBool(5))
				{
					Tile tileLeft = Main.tile[i - 1, j - g];
					tileLeft.TileType = (ushort)ModContent.TileType<FluorescentTree>();
					tileLeft.TileFrameY = 4;
					tileLeft.TileFrameX = (short)Main.rand.Next(4);
					tileLeft.HasTile = true;
				}
				if (Main.rand.NextBool(5))
				{
					Tile tileRight = Main.tile[i + 1, j - g];
					tileRight.TileType = (ushort)ModContent.TileType<FluorescentTree>();
					tileRight.TileFrameY = 5;
					tileRight.TileFrameX = (short)Main.rand.Next(4);
					tileRight.HasTile = true;
				}
			}
			if (g == 0)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = 0;
				tile.TileFrameX = 0;
				tile.HasTile = true;
				continue;
			}
			if (g == 1)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = -1;
				tile.TileFrameX = 0;
				tile.HasTile = true;
				continue;
			}
			if (g == 2)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = 3;
				tile.TileFrameX = (short)Main.rand.Next(4);
				tile.HasTile = true;
				continue;
			}
			if (g == Height - 1)
			{
				tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
				tile.TileFrameY = 2;
				tile.TileFrameX = 0;
				tile.HasTile = true;
				continue;
			}
			tile.TileType = (ushort)ModContent.TileType<FluorescentTree>();
			tile.TileFrameY = 1;
			tile.TileFrameX = (short)Main.rand.Next(12);
			tile.HasTile = true;
		}
	}
	public override bool CanExplode(int i, int j)
	{
		return false;
	}
}