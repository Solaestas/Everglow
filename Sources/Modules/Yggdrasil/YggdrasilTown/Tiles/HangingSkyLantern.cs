using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;
using Everglow.Yggdrasil.Common.Utils;
using Everglow.Yggdrasil.Common;
using Everglow.Commons.TileHelper;
using Terraria.GameContent.Drawing;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles;

public class HangingSkyLantern : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;

		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		DustType = DustID.DynastyWood;
		AdjTiles = new int[] { TileID.Chandeliers };

		// Placement

		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
		TileObjectData.newTile.Origin = new Point16(1, 0);
		TileObjectData.addTile(Type);

		AddMapEntry(new Color(135, 103, 90));
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 3, 3);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX < 54)
		{
			r = 0.8f;
			g = 0.75f;
			b = 0.4f;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
		}
	}
	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (closer)
		{
			var tile = Main.tile[i, j];
			if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
			{
				foreach (Player player in Main.player)
				{
					if (player.Hitbox.Intersects(new Rectangle(i * 16 - 14, j * 16 + 18, 12, 30)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j)].X;
							rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j)].Y;
							float mass = 16f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j));
						}
					}
					if (Main.tile[(i * 16 - 14) / 16, (j * 16 + 26) / 16].WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
					}



					if (player.Hitbox.Intersects(new Rectangle(i * 16 + 2, j * 16 + 16, 12, 28)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)].X;
							rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)].Y;
							float mass = 16f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j));
						}
					}
					if (Main.tile[(i * 16 + 2) / 16, (j * 16 + 24) / 16].WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
					}

					if (player.Hitbox.Intersects(new Rectangle(i * 16 - 4, j * 16 + 10, 12, 32)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j + 1)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j + 1), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 1)].X;
							rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 1)].Y;
							float mass = 16f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 1)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j + 1));
						}
					}
					if (Main.tile[(i * 16 - 4) / 16, (j * 16 + 16) / 16].WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j + 1)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j + 1), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
					}

					if (player.Hitbox.Intersects(new Rectangle(i * 16 + 8, j * 16 + 28, 12, 32)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)].X;
							rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)].Y;
							float mass = 16f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j));
						}
					}
					if (Main.tile[(i * 16 + 8) / 16, (j * 16 + 36) / 16].WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
					}


					if (player.Hitbox.Intersects(new Rectangle(i * 16 - 10, j * 16 + 14, 12, 32)))
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j + 2)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j + 2), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
						else
						{
							float rot;
							float Omega;
							Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 2)].X;
							rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 2)].Y;
							float mass = 16f;
							float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
							if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
								TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 2)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
							if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
								TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j + 2));
						}
					}

					if (Main.tile[(i * 16 - 10) / 16, (j * 16 + 22) / 16].WallType == 0)
					{
						if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j + 2)))
							TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j + 2), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
					}
				}
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0) {
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing) {
		var tile = Main.tile[pos];
		var drawCenterPos = pos.ToWorldCoordinates(autoAddY: 0) - screenPosition;
		int Adx = 0;
		if (tile.TileFrameX > 54)
			Adx = 70; // 改了下贴图，所以是70
		// 对了：要给卷筒纸吊灯用油漆的话，卷筒纸贴图估计得分开很多份（对应不同物块位置的油漆）
		// 不过如果只考虑中心物块漆的话就会省事很多。或者分成三份三个位置的油漆也可以
		DrawLanternPiece(42 + Adx, 58, 0.15f, -2, pos, drawCenterPos, spriteBatch, tileDrawing);
		DrawLanternPiece(56 + Adx, 44, 0.11f, -4, pos, drawCenterPos, spriteBatch, tileDrawing);
		DrawLanternPiece(28 + Adx, 40, 0.13f, 2, pos, drawCenterPos, spriteBatch, tileDrawing);
		DrawLanternPiece(14 + Adx, 44, 0.09f, 8, pos + new Point(1, 0), drawCenterPos, spriteBatch, tileDrawing);
		DrawLanternPiece(0 + Adx, 48, 0.09f, -8, pos + new Point(-1, 0), drawCenterPos, spriteBatch, tileDrawing);
	}

	/// <summary>
	/// 绘制灯的一个小Piece
	/// </summary>
	private void DrawLanternPiece(int frameX, int frameHeight, float swayCoefficient, int offsetX, Point tilePos, Vector2 drawCenterPos, SpriteBatch spriteBatch, TileDrawing tileDrawing) {
		Texture2D tex = ModAsset.HangingSkyLantern_Depart.Value;
		var frame = new Rectangle(frameX, 0, 12, frameHeight);

		int sizeX = 1;
		int sizeY = 2;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(tilePos.X, tilePos.Y, sizeX, sizeY))
			windCycle = tileDrawing.GetWindCycle(tilePos.X, tilePos.Y, tileDrawing._sunflowerWindCounter);

		int totalPushTime = 60;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(tilePos.X, tilePos.Y, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;
		
		Color tileLight = Lighting.GetColor(tilePos);
		float rotation = -windCycle * swayCoefficient;
		var origin = new Vector2(6, 0);
		var tileSpriteEffect = SpriteEffects.None;
		spriteBatch.Draw(tex, drawCenterPos + new Vector2(offsetX, -2), frame, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
	}
}
