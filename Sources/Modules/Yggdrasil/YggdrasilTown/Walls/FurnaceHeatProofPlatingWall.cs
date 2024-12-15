using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Everglow.Yggdrasil.YggdrasilTown.Walls;

public class FurnaceHeatProofPlatingWall : ModWall
{
	public override void SetStaticDefaults()
	{
		Main.wallHouse[Type] = true;
		DustType = ModContent.DustType<FurnaceHeatProofPlatingWall_Dust>();
		AddMapEntry(new Color(56, 16, 28));
	}

	public override bool WallFrame(int i, int j, bool randomizeFrame, ref int style, ref int frameNumber)
	{
		return base.WallFrame(i, j, randomizeFrame, ref style, ref frameNumber);
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Tile tile = Main.tile[i, j];
		// 回声涂料
		if (!TileDrawing.IsVisible(tile))
		{
			return;
		}
		var tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.LampWood_newStyleTree_1_leave_Path, Type, textureStyle, paint, tileDrawing);
		tex ??= ModAsset.LampWood_newStyleTree_1_leave.Value;
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var offsetScreen = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
		{
			offsetScreen = Vector2.Zero;
		}
		Vector2 drawPos = new Point(i, j).ToWorldCoordinates() - Main.screenPosition + offsetScreen;
		Color lightColor = Lighting.GetColor(i, j);
		Rectangle frame = new Rectangle(2 + (i % 15) * 16 - 2, 240 + (j % 15) * 16 - 2, 20, 20);
		spriteBatch.Draw(texture, drawPos, frame, lightColor, 0, frame.Size() * 0.5f, 1, SpriteEffects.None, 0);
		Rectangle frameReflect = new Rectangle(246 + (i % 15) * 16 - 4, 240 + (j % 15) * 16 - 4, 24, 24);
		spriteBatch.Draw(texture, drawPos, frameReflect, lightColor * 3f, 0, frameReflect.Size() * 0.5f, 1, SpriteEffects.None, 0);
	}
}