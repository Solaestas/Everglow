using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;
using MathNet.Numerics.LinearAlgebra;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

public class LampWood_newStyleTree_0 : ShapeDataTile
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
		Texture2D branch = ModAsset.LampWood_newStyleTree_0_leave.Value;
		Texture2D branch_glow = ModAsset.LampWood_newStyleTree_0_leave_glow.Value;
		Color glowColor = new Color(1f,1f,1f, 0);
		if (tile.TileFrameX / 18 == 9 && tile.TileFrameY / 18 == 19)
		{
			Rectangle frame = new Rectangle(0, 0, 218, 224);
			spriteBatch.Draw(branch, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-92, -304), frame, Lighting.GetColor(i - 3, j - 13), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(branch_glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-92, -304), frame, glowColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);

			frame = new Rectangle(0, 496, 102, 54);
			spriteBatch.Draw(branch, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-56, -32), frame, Lighting.GetColor(i - 3, j - 2), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(branch_glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-56, -32), frame, glowColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);

			frame = new Rectangle(0, 228, 106, 98);
			spriteBatch.Draw(branch, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-44, -108), frame, Lighting.GetColor(i - 1, j - 6), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(branch_glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-44, -108), frame, glowColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);

			frame = new Rectangle(0, 328, 118, 166);
			spriteBatch.Draw(branch, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-142, -162), frame, Lighting.GetColor(i - 6, j - 7), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
			spriteBatch.Draw(branch_glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-142, -162), frame, glowColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);

			if (!Main.gamePaused)
			{
				if (Main.rand.NextBool(7))
				{
					float wind = Main.windSpeedCurrent / 15f;
					Vector2 v0 = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 40f).RotatedByRandom(6.283);
					Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) + v0 + new Vector2(-12, -274), 16, 16, ModContent.DustType<LampWood_Dust_fluorescent_appear>());
					dust.alpha = 0;
					dust.rotation = Main.rand.NextFloat(0.7f, 1.4f);
					dust.velocity = v0 * 0.03f + new Vector2(wind * 4, 0);
				}

				if (Main.rand.NextBool(7))
				{
					float wind = Main.windSpeedCurrent / 15f;
					Vector2 v0 = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 40f).RotatedByRandom(6.283);
					Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) + v0 + new Vector2(10, -98), 16, 16, ModContent.DustType<LampWood_Dust_fluorescent_appear>());
					dust.alpha = 0;
					dust.rotation = Main.rand.NextFloat(0.7f, 1.4f);
					dust.velocity = v0 * 0.03f + new Vector2(wind * 4, 0);
				}

				if (Main.rand.NextBool(7))
				{
					float wind = Main.windSpeedCurrent / 15f;
					Vector2 v0 = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 40f).RotatedByRandom(6.283);
					Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) + v0 + new Vector2(-102, -132), 16, 16, ModContent.DustType<LampWood_Dust_fluorescent_appear>());
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
		spriteBatch.Draw(ModAsset.LampWood_newStyleTree_0_glow.Value, new Vector2(i, j) * 16 - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(1f, 1f, 1f, 0), 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}
}