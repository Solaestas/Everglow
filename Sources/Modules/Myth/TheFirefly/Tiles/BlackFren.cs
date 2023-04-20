using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Tiles;

public class BlackFren : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.addTile(Type);
		DustType = 191;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(11, 11, 11), modTranslation);
		HitSound = SoundID.Grass;
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		//for (int x = 0; x < 2; x++)
		//{
		//    Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.BlackStarShrub>());
		//}
	}

	public override void PlaceInWorld(int i, int j, Item item)
	{
		short num = (short)Main.rand.Next(0, 6);
		Main.tile[i, j].TileFrameX = (short)(num * 48);
		Main.tile[i, j + 1].TileFrameX = (short)(num * 48);
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{/*
            if (closer)
            {
                var tile = Main.tile[i, j];
                foreach (Player player in Main.player)
                {
                    if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
                    {
                        if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 16 + 1)))
                        {
                            TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 16 + 1), new Vector2(Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)].X;
                            rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)] = new Vector2(Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 16 + 1));
                            }
                        }

                        if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 16)))
                        {
                            TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 16), new Vector2(Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)].X;
                            rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)] = new Vector2(Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 16));
                            }
                        }
                    }
                }
            }*/
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		/* var tile = Main.tile[i, j];
             if (tile.TileFrameY == 16)
             {
                 TileSpin tileSpin = new TileSpin();
                 tileSpin.UpdateBlackShrub(i, j - tile.TileFrameY / 16, 0.25f, 0.13f, new Vector2(0, -20), 0, 24);
                 tileSpin.Update(i, j - tile.TileFrameY / 16 + 1);
                 Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/BlackStarShrubSmallDraw");
                 tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 2, 48, 36),new Vector2(24, 36), 8, 40, 0.25f);
                 tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16 + 1, tex, new Rectangle(tile.TileFrameX, 38, 48, 36), new Vector2(24, 36), 8, 24, 1.0f);
                 tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16 + 1, tex, new Rectangle(tile.TileFrameX, 218, 48, 36), new Vector2(24, 36), 8, 24, 1.0f, true, new Color(0.57f, 0.57f, 0.57f, 0));
                 tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 72, 48, 36), new Vector2(24, 36), 8, 40, 0.3f);
                 tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 110, 48, 36), new Vector2(24, 36), 8, 40, 0.24f);
                 tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 146, 48, 36), new Vector2(24, 36), 8, 40, 0.19f);
                 tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 182, 48, 36), new Vector2(24, 36), 8, 40, 0.27f);
             }*/
		return true;
	}
}