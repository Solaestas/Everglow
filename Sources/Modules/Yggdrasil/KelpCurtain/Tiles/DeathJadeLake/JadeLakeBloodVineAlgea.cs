using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.WorldGeneration;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

public class JadeLakeBloodVineAlgea : ModTile, ITileFluentlyDrawn
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = false;
		Main.tileNoAttach[Type] = true;
		Main.tileCut[Type] = true;

		//TileID.Sets.TouchDamageDestroyTile[Type] = true;
		TileID.Sets.TouchDamageImmediate[Type] = 30;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 1;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateWidth = 16;
		TileObjectData.addTile(Type);

		DustType = ModContent.DustType<JadeLakeRedAlgaeDust>();

		AddMapEntry(new Color(127, 36, 89));
		HitSound = SoundID.Grass;
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		Tile bottomTile = YggdrasilWorldGeneration.SafeGetTile(i, j + 1);
		Tile tile = Main.tile[i, j];
		if (tile.HasTile && tile.TileType == Type)
		{
			if (!bottomTile.HasTile || (bottomTile.TileType != Type && !Main.tileSolid[bottomTile.type] && !Main.tileSolidTop[bottomTile.type]))
			{
				int deltaY = 0;
				while (true)
				{
					Tile topTile = YggdrasilWorldGeneration.SafeGetTile(i, j - deltaY);
					if (topTile.HasTile && topTile.TileType == Type && j - deltaY > 0)
					{
						WorldGen.KillTile(i, j - deltaY, false, false, true);
					}
					else
					{
						break;
					}
					deltaY++;
				}
			}
		}
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public override bool CreateDust(int i, int j, ref int type)
	{
		for (int k = 0; k < 2; k++)
		{
			Vector2 pos = new Point(i, j).ToWorldCoordinates();
			Dust d = Dust.NewDustDirect(pos - new Vector2(20, 40) + new Vector2(4), 40, 50, type);
			d.noGravity = true;
		}
		return false;
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		if (Main.tile[i, j + 1].TileType != Type)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		else if (j == (Main.screenPosition + new Vector2(0, Main.screenHeight)).ToTileCoordinates().Y)
		{
			for (int y = 1; y < 133; y++)
			{
				Tile stepDown = YggdrasilWorldGeneration.SafeGetTile(i, j + y);
				if (stepDown.TileType != Type)
				{
					TileFluentDrawManager.AddFluentPoint(this, i, j + y - 1);
					break;
				}
			}
		}
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		DrawAlgae(pos, pos.ToWorldCoordinates() - screenPosition, spriteBatch, tileDrawing);
	}

	/// <summary>
	/// Draw a piece of lotus
	/// </summary>
	private void DrawAlgae(Point tilePos, Vector2 drawCenterPos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		var lastOffset = new Vector2(0, 12);
		int height = 0;
		for (int j = 0; j < 133; j++)
		{
			height++;
			if (tilePos.Y - j < 21)
			{
				break;
			}
			var tile = Main.tile[tilePos + new Point(0, -j)];
			if (!(tile.TileType == Type && tile.HasTile))
			{
				break;
			}
		}
		for (int j = 0; j < 133; j++)
		{
			if (tilePos.Y - j < 21)
			{
				return;
			}
			var tile = Main.tile[tilePos + new Point(0, -j)];
			if (!(tile.TileType == Type && tile.HasTile))
			{
				return;
			}
			var tileUp = Main.tile[tilePos + new Point(0, -j - 1)];
			bool lastTile = false;
			if (!(tileUp.TileType == Type && tileUp.HasTile))
			{
				lastTile = true;
			}
			ushort type = tile.TileType;
			bool joint = (j + tilePos.X) % 2 == 1;
			var frame = new Rectangle(80, 0, 8, 18);
			if (j == 0)
			{
				frame = new Rectangle(80, 0, 8, 18);
			}
			if (joint)
			{
				frame = new Rectangle(80, 0, 8, 18);
			}
			if (lastTile)
			{
				frame = new Rectangle(52, 0, 24, 46);
			}

			// 回声涂料
			if (!TileDrawing.IsVisible(tile))
			{
				continue;
			}

			int paint = Main.tile[tilePos].TileColor;
			Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.JadeLakeBloodVineAlgea_Path, type, 1, paint, tileDrawing);
			tex ??= ModAsset.JadeLakeBloodVineAlgea.Value;

			float windCycle = 0;
			if (tileDrawing.InAPlaceWithWind(tilePos.X, tilePos.Y, 1, 1))
			{
				windCycle = tileDrawing.GetWindCycle(tilePos.X, tilePos.Y, tileDrawing._sunflowerWindCounter);
			}

			int totalPushTime = 140;
			float pushForcePerFrame = 0.96f;
			float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(tilePos.X, tilePos.Y - j, 1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
			windCycle += highestWindGridPushComplex;
			float rotation = windCycle * 0.12f;
			rotation -= lastOffset.X / (22f + j * 3f);
			rotation += MathF.Sin(j / (float)height * MathHelper.Pi) * MathF.Sin(tilePos.X * 2 + tilePos.Y - j * 0.45f + (float)Main.time / 20f) * 0.15f;
			rotation += MathF.Sin(j / (float)height * MathHelper.Pi) * MathF.Sin(tilePos.X * 2 + tilePos.Y + (float)Main.time / 20f) * 6f / (height + 25);
			var tileLight = Lighting.GetColor(tilePos + new Point(0, -j));

			// 支持发光涂料
			tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(tilePos.X, tilePos.Y - j, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
			tileLight = tileDrawing.DrawTiles_GetLightOverride(tilePos.X, tilePos.Y - j, tile, type, 0, 0, tileLight);

			var origin = new Vector2(4, 18);
			if (lastTile)
			{
				origin = new Vector2(12, 46);
			}

			var drawPos = drawCenterPos + lastOffset;
			var tileSpriteEffect = SpriteEffects.None;
			spriteBatch.Draw(tex, drawPos, frame, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
			float size = MathF.Sin((j / (float)height) * MathHelper.Pi * 0.85f) + 0.2f;
			if(joint)
			{
				frame = new Rectangle(36, 8, 14, 38);
				origin = new Vector2(7, 38);
				float subRot0 = MathF.Sin(j / (float)height * MathHelper.Pi) * MathF.Sin((tilePos.X + 1) * 2 + tilePos.Y - j * 0.45f + (float)Main.time / 20f) * 0.15f;
				float subRot1 = MathF.Sin(j / (float)height * MathHelper.Pi) * MathF.Sin((tilePos.X - 1) * 2 + tilePos.Y - j * 0.45f + (float)Main.time / 20f) * 0.15f;
				spriteBatch.Draw(tex, drawPos, frame, tileLight * 0.5f, rotation + 0.8f + subRot0, origin, size, tileSpriteEffect, 0f);
				spriteBatch.Draw(tex, drawPos, frame, tileLight * 0.5f, rotation - 0.8f + subRot1, origin, size, tileSpriteEffect, 0f);
				frame = new Rectangle(24, 18, 10, 28);
				origin = new Vector2(5, 28);
				subRot0 = MathF.Cos(j / (float)height * MathHelper.Pi) * MathF.Sin((tilePos.X + 1) * 2 + tilePos.Y - j * 0.45f + (float)Main.time / 20f) * 0.1f;
				subRot1 = MathF.Cos(j / (float)height * MathHelper.Pi) * MathF.Sin((tilePos.X - 1) * 2 + tilePos.Y - j * 0.45f + (float)Main.time / 20f) * 0.1f;
				spriteBatch.Draw(tex, drawPos, frame, tileLight * 0.75f, rotation + 0.4f + subRot0, origin, size, tileSpriteEffect, 0f);
				spriteBatch.Draw(tex, drawPos, frame, tileLight * 0.75f, rotation - 0.4f + subRot1, origin, size, tileSpriteEffect, 0f);
			}
			lastOffset += new Vector2(0, -16).RotatedBy(rotation);
		}
	}
}