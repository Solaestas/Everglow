using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.KelpCurtain.Items.Placeables;
using Everglow.Yggdrasil.WorldGeneration;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.GeyserAirBuds;

public class GeyserAirBuds : ModTile, ITileFluentlyDrawn // 继承ITileFluentlyDrawn实现可旋转动画绘制
{
	public override void SetStaticDefaults()
	{
		MinPick = 350;
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		RegisterItemDrop(ModContent.ItemType<GeyserAirBudsItem>(), 1);

		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.UsesCustomCanPlace = true;
		TileObjectData.newTile.CoordinateHeights = [16, 18];
		TileObjectData.newTile.LavaDeath = false;
		TileObjectData.newTile.Origin = new Point16(0, 1);
		TileObjectData.newSubTile.AnchorBottom = new(Terraria.Enums.AnchorType.SolidTile, 3, 0);
		TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<GeyserAirBudsEntity>().Generic_HookPostPlaceMyPlayer;
		TileObjectData.addTile(Type);
		AnimationFrameHeight = 96;

		AddMapEntry(new Color(89, 177, 255));
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];

		int relX = tile.TileFrameX / 18;
		int relY = tile.TileFrameY / 18;

		int originX = i - relX + 1;
		int originY = j - relY + 1;

		Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
		int height = tile.TileFrameY % AnimationFrameHeight == 18 ? 18 : 16;

		// spriteBatch.Draw(
		// TextureAssets.Tile[Type].Value,
		// new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
		// new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
		// Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
		if (tile.TileFrameY == 18 && tile.TileFrameX == 0)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return false;
	}

	public void ResetAsCoolingGeyserAirBudsPlatform(int i, int j)
	{
		for (int x = 0; x < 2; x++)
		{
			for (int y = 0; y < 2; y++)
			{
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(i + x, j + y);
				if (tile.TileType == Type)
				{
					tile.TileType = (ushort)ModContent.TileType<GeyserAirBudsPlatform>();
					tile.TileFrameX = (short)(x * 18);
					tile.TileFrameY = (short)(y * 18);
					tile.HasTile = true;
				}
			}
		}
	}

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing)
	{
		var tile = Main.tile[pos];
		var drawCenterPos = pos.ToWorldCoordinates(autoAddY: 16) - screenPosition;

		ushort type = tile.TileType;

		// 回声涂料
		if (!TileDrawing.IsVisible(tile))
		{
			return;
		}

		int paint = Main.tile[pos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.GeyserAirBuds_Untrimmed_Path, type, 0, paint, tileDrawing);
		tex ??= ModAsset.GeyserAirBuds_Untrimmed.Value;

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(pos.X, pos.Y - 1, 2, 2))
		{
			windCycle = tileDrawing.GetWindCycle(pos.X, pos.Y - 1, tileDrawing._sunflowerWindCounter);
		}
		float swayStrength = 0.05f;
		int totalPushTime = 80;
		float pushForcePerFrame = 1.26f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(pos.X, pos.Y - 1, 2, 2, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;
		float rotation = windCycle * swayStrength;

		// 颜色
		Color tileLight = Lighting.GetColor(pos);
		tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(pos.X, pos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
		tileLight = tileDrawing.DrawTiles_GetLightOverride(pos.Y, pos.X, tile, type, 0, 0, tileLight);

		var origin = new Vector2(48, 96);
		var tileSpriteEffect = SpriteEffects.None;

		int frameYOffset = 0;

		if (TileEntity.ByPosition.TryGetValue(new Point16(pos.X + 1, pos.Y), out TileEntity entity) && entity is GeyserAirBudsEntity geyser)
		{
			frameYOffset = geyser.GetFrame() * AnimationFrameHeight; // 获取状态帧
		}
		Rectangle frame = new Rectangle(0, frameYOffset, 96, AnimationFrameHeight);
		spriteBatch.Draw(tex, drawCenterPos + new Vector2(8, 6), frame, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		ModContent.GetInstance<GeyserAirBudsEntity>().Kill(i, j);
		Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<GeyserAirBudsItem>());
	}
}