using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class LampWood_newStyleTree_1 : ShapeDataTile
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		Main.tileAxe[Type] = true;
		AddMapEntry(new Color(49, 41, 96));
	}
	public override void PostSetDefaults()
	{
		base.PostSetDefaults();
		MultiItem = true;
		CustomItemType = ModContent.ItemType<LampWood_Wood>();
	}
	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;
		Texture2D branch = ModAsset.LampWood_newStyleTree_1_leave.Value;
		Texture2D branch_glow = ModAsset.LampWood_newStyleTree_1_leave_glow.Value;
		Color glowColor = new Color(1f, 1f, 1f, 0);
		if (tile.TileFrameX / 18 == 12 && tile.TileFrameY / 18 == 11)
		{
			Color getlight = Lighting.GetColor(i - 3, j - 13);
			getlight.A = 255;
			Rectangle frame = new Rectangle(0, 0, 254, 198);
			spriteBatch.Draw(branch, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-192, -174), frame, getlight, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(branch_glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-192, -174), frame, glowColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);

			frame = new Rectangle(0, 200, 142, 138);
			spriteBatch.Draw(branch, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-190, -14), frame, Lighting.GetColor(i - 8, j + 2), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(branch_glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-190, -14), frame, glowColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);

			frame = new Rectangle(0, 344, 148, 168);
			spriteBatch.Draw(branch, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-56, -54), frame, Lighting.GetColor(i + 1, j), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(branch_glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-56, -54), frame, glowColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			if(!Main.gamePaused)
			{
				if (Main.rand.NextBool(7))
				{
					float wind = Main.windSpeedCurrent / 15f;
					Vector2 v0 = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 40f).RotatedByRandom(6.283);
					Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) + v0 + new Vector2(-122, -154), 16, 16, ModContent.DustType<LampWood_Dust_fluorescent_appear>());
					dust.alpha = 0;
					dust.rotation = Main.rand.NextFloat(0.7f, 1.4f);
					dust.velocity = v0 * 0.03f + new Vector2(wind * 4, 0);
				}

				if (Main.rand.NextBool(7))
				{
					float wind = Main.windSpeedCurrent / 15f;
					Vector2 v0 = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 40f).RotatedByRandom(6.283);
					Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) + v0 + new Vector2(-160, -14), 16, 16, ModContent.DustType<LampWood_Dust_fluorescent_appear>());
					dust.alpha = 0;
					dust.rotation = Main.rand.NextFloat(0.7f, 1.4f);
					dust.velocity = v0 * 0.03f + new Vector2(wind * 4, 0);
				}

				if (Main.rand.NextBool(7))
				{
					float wind = Main.windSpeedCurrent / 15f;
					Vector2 v0 = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 40f).RotatedByRandom(6.283);
					Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) + v0 + new Vector2(26, -44), 16, 16, ModContent.DustType<LampWood_Dust_fluorescent_appear>());
					dust.alpha = 0;
					dust.rotation = Main.rand.NextFloat(0.7f, 1.4f);
					dust.velocity = v0 * 0.03f + new Vector2(wind * 4, 0);
				}
			}
		}
		return base.PreDraw(i, j, spriteBatch);
	}
	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);

		if (Main.drawToScreen)
			zero = Vector2.Zero;
		spriteBatch.Draw(ModAsset.LampWood_newStyleTree_1_glow.Value, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}
}