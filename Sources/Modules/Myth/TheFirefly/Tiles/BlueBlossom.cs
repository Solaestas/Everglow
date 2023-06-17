using Everglow.Commons.TileHelper;
using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly;
using Everglow.Myth.TheFirefly.Dusts;
using Everglow.Myth.TheFirefly.Items;
using Terraria.GameContent.Drawing;
using Terraria.ObjectData;
using static Everglow.Myth.TheFirefly.Tiles.BlackStarShrub;

namespace Everglow.Myth.TheFirefly.Tiles;

public class BlueBlossom : ModTile, ITileFluentlyDrawn
{
	public override void PostSetDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileNoAttach[Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Height = 3;
		TileObjectData.newTile.Width = 1;
		TileObjectData.newTile.CoordinateHeights = new int[]
		{
			16,
			16,
			18
		};
		TileObjectData.newTile.CoordinateWidth = 60;
		TileObjectData.addTile(Type);
		DustType = 191;
		var modTranslation = CreateMapEntryName();
		AddMapEntry(new Color(81, 110, 255), modTranslation);
		HitSound = SoundID.Grass;
	}

	public override void NumDust(int i, int j, bool fail, ref int num)
	{
		num = fail ? 1 : 3;
	}
	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield return new Item(ModContent.ItemType<GlowingPedal>());
		yield return new Item(ModContent.ItemType<GlowingPedal>());
		yield return new Item(ModContent.ItemType<GlowingPedal>());
		yield return new Item(ModContent.ItemType<GlowingPedal>());
		yield return new Item(ModContent.ItemType<GlowingPedal>());
	}
	public override void PlaceInWorld(int i, int j, Item item)
	{
		short num = (short)Main.rand.Next(0, 6);
		Main.tile[i, j].TileFrameX = (short)(num * 48);
		Main.tile[i, j + 1].TileFrameX = (short)(num * 48);
	}

	public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameY == 32)
		{
			TileFluentDrawManager.AddFluentPoint(this, i, j);
		}
		return false;
	}

	private static double _oldWindCounter = 0;
	private static double _ooldWindCounter = 0;

	public void FluentDraw(Vector2 screenPosition, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing) {
		var drawCenterPos = pos.ToWorldCoordinates(autoAddY: 16) - screenPosition;
		DrawBlossomPiece(ModAsset.BlueBlossomDrawPath, pos, drawCenterPos, spriteBatch, tileDrawing);
		DrawBlossomPiece(ModAsset.BlueBlossomGlowPath, pos, drawCenterPos, spriteBatch, tileDrawing, new Color(5, 5, 5, 0));
	}

	/// <summary>
	/// 绘制花花的一个小Piece
	/// </summary>
	private void DrawBlossomPiece(string texPath, Point tilePos, Vector2 drawCenterPos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color? specialColor = null) {
		var tile = Main.tile[tilePos];
		ushort type = tile.TileType;
		var frame = new Rectangle(tile.TileFrameX, 0, 120, 108);

		// 回声涂料
		if (!tileDrawing.IsVisible(tile)) return;
		
		int paint = Main.tile[tilePos].TileColor;
		int textureStyle = tile.TileFrameX + (specialColor is null).ToInt() * 4;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(texPath, type, textureStyle, paint, tileDrawing);
		tex ??= ModContent.Request<Texture2D>($"Everglow/{texPath}", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

		var topLeft = tilePos - new Point(2, 5);
		var size = new Point(5, 6);

		float windCycle = 0;
		if (tileDrawing.InAPlaceWithWind(topLeft.X, topLeft.Y, size.X, size.Y))
			windCycle = tileDrawing.GetWindCycle(tilePos.X, tilePos.Y, tileDrawing._sunflowerWindCounter);

		int totalPushTime = 140;
		float pushForcePerFrame = 0.96f;
		float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(topLeft.X, topLeft.Y, size.X, size.Y, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);
		windCycle += highestWindGridPushComplex;
		float rotation = windCycle * 0.07f;

		// 角速度计算，源码乱抄与瞎写代码的结合，用于抖花粉
        float factor = tilePos.X + tilePos.Y / 100;
        float radiusNow = (float) Math.Cos(tileDrawing._sunflowerWindCounter * Math.PI * 2.0 + (double) factor);
        float radiusPrev = (float) Math.Cos(_ooldWindCounter * Math.PI * 2.0 + (double) factor);
		float angularVelocity = highestWindGridPushComplex + radiusNow - radiusPrev;

		// 颜色
		Color tileLight = Lighting.GetColor(tilePos);
		// 万象写的specialColor特效，似乎是摇曳越强越亮
		if (specialColor is not null) {
			float maxC = Math.Max(specialColor.Value.R / 255f, Math.Abs(rotation * 6) + 0.5f);
			maxC = Math.Clamp(maxC, 0, 1);
			tileLight = new Color(maxC, maxC, maxC, 0);
		}
		// 支持发光涂料
		tileDrawing.DrawAnimatedTile_AdjustForVisionChangers(tilePos.X, tilePos.Y, tile, type, 0, 0, ref tileLight, tileDrawing._rand.NextBool(4));
		tileLight = tileDrawing.DrawTiles_GetLightOverride(tilePos.Y, tilePos.X, tile, type, 0, 0, tileLight);
		
		var origin = new Vector2(54, 108);
		var tileSpriteEffect = SpriteEffects.None;
		spriteBatch.Draw(tex, drawCenterPos + new Vector2(0, 4), frame, tileLight, rotation, origin, 1f, tileSpriteEffect, 0f);
		
		// 抖落蓝色荧光花粉
		if (Math.Abs(angularVelocity) > 0.04 && Main.rand.NextBool(Math.Clamp((int)(100 - Math.Abs(angularVelocity) * 40f), 1, 900)))
		{
			var dustSpawnOffset = new Vector2(-32, -100);
			var dustSpawnPos = drawCenterPos + dustSpawnOffset.RotatedBy(rotation) + Main.screenPosition;
			var dustVelocity = new Vector2(-1f, 0f).RotatedBy(angularVelocity) * 0.7f;
			if (angularVelocity > 0)
				dustVelocity = -dustVelocity;
			var d = Dust.NewDustDirect(dustSpawnPos, 64, 70, ModContent.DustType<BlueParticleDark>(), Alpha: 150);
			var d2 = Dust.NewDustDirect(dustSpawnPos, 64, 70, ModContent.DustType<BlueParticle>(), Alpha: 150);
			d.scale = Main.rand.NextFloat() * 0.2f + 0.2f;
			d2.scale = Main.rand.NextFloat() * 0.2f + 0.3f;
			d.velocity = dustVelocity;
			d2.velocity = dustVelocity;
		}

		_ooldWindCounter = _oldWindCounter;
		_oldWindCounter = tileDrawing._sunflowerWindCounter;
	}
}
