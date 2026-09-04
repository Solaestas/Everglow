using Everglow.Commons.Utilities.BackgroundHelper;
using Spine;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.UI;

namespace Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

public class FurnaceScoreShop : BackgroundSlideBase
{
	public Point TileAnchor;

	public List<Point> BgTiles = new List<Point>();

	public List<Rectangle> GlowFrames = new List<Rectangle>();

	/// <summary>
	/// 0:Normal, 1:Talk, 2:Play
	/// </summary>
	public int SaleGirlState = 0;

	public int SaleGirlAnimationTimer = 0;

	public bool MouseOverSaleGirl = false;

	public bool OpenShop = false;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Texture = ModAsset.FurnaceScoreShop.Value;
		Distance = 1;
		UseColorStyle = 1;
		LayerPriority = 1;
		Shader = Effects.XWrap_YWrap_Shader;
	}

	public override void Update()
	{
		base.Update();
		Player player = Main.LocalPlayer;
		if (SaleGirlAnimationTimer == 0)
		{
			int k = Main.rand.Next(10);
			if (k <= 5)
			{
				SaleGirlState = 0;
			}
			else if (k <= 8)
			{
				SaleGirlState = 1;
			}
			else
			{
				SaleGirlState = 2;
			}
			switch (SaleGirlState)
			{
				case 0:
					SaleGirlAnimationTimer = Main.rand.Next(300, 900);
					break;
				case 1:
					SaleGirlAnimationTimer = 120;
					break;
				case 2:
					SaleGirlAnimationTimer = 180;
					break;
			}
		}
		else
		{
			SaleGirlAnimationTimer--;
		}
		Vector2 girlPos = WorldAnchor + new Vector2(1935, 492);
		MouseOverSaleGirl = false;
		Rectangle girlHitBox = new Rectangle((int)girlPos.X, (int)girlPos.Y, 58, 36);
		if(girlHitBox.Contains(Main.MouseWorld.ToPoint()))
		{
			Main.instance.MouseText("Furnace Points Redemption");
			MouseOverSaleGirl = true;
			if (Main.mouseRight && Main.mouseRightRelease && CanInteract())
			{
				Main.playerInventory = true;
				YggdrasilTownFurnaceSystem.FurnaceScoreShopOpen = true;
				OpenShop = true;
			}
		}
		if (!Main.playerInventory)
		{
			YggdrasilTownFurnaceSystem.FurnaceScoreShopOpen = false;
			OpenShop = false;
		}
		if (OpenShop)
		{
		}
	}

	public override void Draw()
	{
		var bars = new List<Vertex2D>();
		BackgroundHigherPerformanceHelper.Add_TileBgVertice(this, BgTiles, bars, 1);
		DrawVertexBackground(this, PrimitiveType.TriangleStrip, bars);
		Player player = Main.LocalPlayer;

		Texture2D girl = ModAsset.FurnaceScoreShop_SaleGirl.Value;
		Vector2 pos = WorldAnchor + new Vector2(1964, 528);
		var girlFrame = new Rectangle(0, 0, 58, 36);
		switch(SaleGirlState)
		{
			case 0:
				girlFrame.Y = 0;
				if (SaleGirlAnimationTimer % 240 is > 120 and < 130)
				{
					girlFrame.Y = 36;
				}
				break;
			case 1:
				girlFrame.Y = 72 + SaleGirlAnimationTimer / 12 % 4 * 36;
				break;
			case 2:
				girlFrame.Y = 216 + (180 - SaleGirlAnimationTimer) / 10 % 18 * 36;
				break;
		}
		Main.spriteBatch.Draw(girl, pos - Main.screenPosition, girlFrame, Lighting.GetColor(pos.ToTileCoordinates()), 0, new Vector2(girlFrame.Width * 0.5f, girlFrame.Height), 1f, SpriteEffects.None, 0);
		if (MouseOverSaleGirl && CanInteract())
		{
			Texture2D chatBubble = TextureAssets.Chat.Value;
			Main.spriteBatch.Draw(chatBubble, pos - Main.screenPosition + new Vector2(0, -16), null, Lighting.GetColor(pos.ToTileCoordinates()), 0, new Vector2(0, chatBubble.Height), 1f, SpriteEffects.None, 0);
		}
	}

	public bool CanInteract()
	{
		Player player = Main.LocalPlayer;
		Vector2 pos = WorldAnchor + new Vector2(1964, 528);
		return (player.Center - pos).Length() < new Vector2(player.lastTileRangeX, player.lastTileRangeY).Length() * 16 + 16;
	}

	public override bool CanActive()
	{
		return TileUtils.SafeGetTile(TileAnchor).TileType == ModContent.TileType<YggdrasilCommandBlock>();
	}
}
