using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Everglow.Commons.TileHelper;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ObjectData;
using static Terraria.ModLoader.Default.LegacyUnloadedTilesSystem;
using Terraria.GameContent.Drawing;
using Everglow.Commons.DataStructures;
using Terraria;

namespace Everglow.Myth.TheFirefly.Tiles.Furnitures;

public class GlowWoodLantern : ModTile, ITileFluentlyDrawn
{
	public override void SetStaticDefaults()
	{
		// Properties
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileSolid[Type] = false;
		Main.tileNoFail[Type] = true;
		
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

		AdjTiles = new int[] { TileID.HangingLanterns };

		// Placement
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
		TileObjectData.newTile.AnchorBottom = default;
		TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile & AnchorType.SolidBottom & AnchorType.Platform, TileObjectData.newTile.Width, 0);
		TileObjectData.addTile(Type);

		LocalizedText name = CreateMapEntryName();
		AddMapEntry(new Color(251, 235, 127), name);
	}
	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = 0;
	}
	public override void HitWire(int i, int j)
	{
		FurnitureUtils.LightHitwire(i, j, Type, 1, 2);
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		Tile tile = Main.tile[i, j];
		if (tile.TileFrameX < 18)
		{
			r = 0.1f;
			g = 0.9f;
			b = 1f;
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
			foreach (Player player in Main.player)
			{
				if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
				{
					if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18)))
						TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
					else
					{
						float rot;
						float omega;
						omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)].X;
						rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)].Y;
						float mass = 15f;
						float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
						if (Math.Abs(omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
							TileSpin.TileRotation[(i, j - tile.TileFrameY / 18)] = new Vector2(omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
						if (Math.Abs(omega) < 0.001f && Math.Abs(rot) < 0.001f)
							TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 18));
					}
				}
			}
			if (tile.WallType == 0)
			{
				if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 18)))
					TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
			}
		}
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		return false;
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing) {
		var tile = Main.tile[pos];
		// 这就是油漆的奥秘，大概就是根据你给出的pos上的油漆给你这个物块的贴图上shader
		// 你可以把里面的代码抄来，把方法里面那个贴图改成一个参数，就可以做到给任意贴图上相应世界物块上的漆了
		Texture2D tex = tileDrawing.GetTileDrawTexture(tile, pos.X, pos.Y);
		
		short tileFrameX = tile.frameX;
		short tileFrameY = tile.frameY;

		int offsetX = 8;
		int offsetY = -2;
		// 锤子是这样的
		if (WorldGen.IsBelowANonHammeredPlatform(pos.X, pos.Y - tileFrameY / 18)) {
			offsetY -= 8;
		}

		// 物块的size
		int sizeX = 1;
		int sizeY = 2;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(pos.X, pos.Y, sizeX, sizeY))
			windCycle = tileDrawing.GetWindCycle(pos.X, pos.Y, tileDrawing._sunflowerWindCounter);

		// 普通源码罢了
		int totalPushTime = 60;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(pos.X, pos.Y, sizeX, sizeY, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;

		Rectangle rectangle = new Rectangle(tileFrameX, tileFrameY, 16, 16);
		Color tileLight = Lighting.GetColor(pos);

		// 由于是分节绘制，对于除第一节外的其他节，这里乘上一个长这样的系数来修正rotation
		float num = (tileFrameY / 18 + 1) / (float)sizeY;
		float rotation = -windCycle * 0.15f * num;

		var origin = new Vector2(sizeX * 16 / 2f, 0);
		var tileSpriteEffect = SpriteEffects.None;

		var drawPos = pos.ToWorldCoordinates(0, tileFrameY / 18 * -16) + new Vector2(offsetX, offsetY) - screenPosition;
		// 同样地，根据节修正position。11是magicNumber，并没有在3节及以上的悬挂物块试用过，先不要管
		drawPos += (Vector2.One * tileFrameY / 18 * 11).RotatedBy(rotation + 0.785f);
		
		spriteBatch.Draw(tex, drawPos, rectangle, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
	}
}