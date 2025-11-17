using System.Reflection;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Commons.Utilities;
using Everglow.Example.Projectiles;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace Everglow.Example.Items;

public class ExampleStabbingSwordConfigDrawer : ModSystem
{
	// private Vector2 pos = Vector2.Zero;
	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(sBS.SortMode, BlendState.AlphaBlend, SamplerState.PointWrap, sBS.DepthStencilState, RasterizerState.CullNone, sBS.Effect, Main.GameViewMatrix.TransformationMatrix);
		Player p = Main.LocalPlayer;

		// pos = Vector2.Lerp(pos, p.Center + new Vector2(-50 * p.direction, 40), 0.2f);
		p.GetModPlayer<ExampleStabbingSword_Config>().Draw();
		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}

public class ExampleStabbingSword_Config : ModPlayer
{
	public Projectile StabbingSword = null;

	public StabbingProjectile Stabbing_ModProj => StabbingSword is null ? null : StabbingSword.ModProjectile as StabbingProjectile;

	/// <summary>
	/// Default ExtraUpdates | 默认ExtraUpdates
	/// </summary>
	public const int NormalExtraUpdates = 20;

	/// <summary>
	/// Main color | 主要颜色
	/// </summary>
	public Color Color = Color.White;

	/// <summary>
	/// Shadow intensity | 阴影强度
	/// </summary>
	public float Shade = 0f;

	/// <summary>
	/// 重影深度
	/// </summary>
	public float TradeShade = 0f;

	/// <summary>
	/// 重影彩色部分亮度
	/// </summary>
	public float TradeLightColorValue = 0f;

	/// <summary>
	/// 重影数量 (小于200)
	/// </summary>
	public int TradeLength = 0;

	/// <summary>
	/// 重影大小缩变,小于1
	/// </summary>
	public float FadeScale = 0f;

	/// <summary>
	/// 刀光宽度1
	/// </summary>
	public float DrawWidth = 1f;

	/// <summary>
	/// 重影深度缩变,小于1
	/// </summary>
	public float FadeShade = 0f;

	/// <summary>
	/// 重影彩色部分亮度缩变,小于1
	/// </summary>
	public float FadeLightColorValue = 0f;

	/// <summary>
	/// 表示刺剑攻击长度,标准长度1
	/// </summary>
	public float MaxLength = 1f;

	/// <summary>
	/// 荧光颜色,默认不会发光
	/// </summary>
	public Color GlowColor = Color.Transparent;

	/// <summary>
	/// 荧光颜色缩变,小于1
	/// </summary>
	public float FadeGlowColorValue = 0f;

	public int DraggingIndex = -1;

	public Vector2 PanelPos = Vector2.zeroVector;

	private Vector2 oldPanelPos = Vector2.zeroVector;

	private Vector2 oldClickPos = Vector2.zeroVector;

	public override void UpdateEquips()
	{
		foreach (var proj in Main.projectile)
		{
			if (proj is not null && proj.active)
			{
				if (proj.type == ModContent.ProjectileType<ExampleStabbingSword_Pro>())
				{
					if (proj.owner == Player.whoAmI)
					{
						StabbingSword = proj;
						break;
					}
				}
			}
		}
		base.UpdateEquips();
	}

	public void Draw()
	{
		if (Player == null)
		{
			return;
		}
		if (Player.HeldItem.type != ModContent.ItemType<ExampleStabbingSword>())
		{
			return;
		}
		DrawAndModify(0, 1, ref Shade, 0.2f, nameof(Shade), 0);
		DrawAndModify(0, 1, ref TradeShade, 0.7f, nameof(TradeShade), 1);
		DrawAndModify(0, 1, ref TradeLightColorValue, 1f, nameof(TradeLightColorValue), 2);
		DrawAndModify(0, 1, ref FadeScale, 1f, nameof(FadeScale), 3);
		DrawAndModify(0, 1, ref DrawWidth, 0.4f, nameof(DrawWidth), 4);
		DrawAndModify(0, 1, ref FadeShade, 0.64f, nameof(FadeShade), 5);
		DrawAndModify(0, 1, ref FadeLightColorValue, 0.4f, nameof(FadeLightColorValue), 6);
		DrawAndModify(0, 1, ref FadeGlowColorValue, 0f, nameof(FadeGlowColorValue), 7);
		DrawAndModify(0.5f, 4, ref MaxLength, 0.7f, nameof(MaxLength), 8);
	}

	public void DrawAndModify(float minVallue, float maxValue, ref float currentValue, float defaultValue, string valueName, int order)
	{
		DrawSlideBar(minVallue, maxValue, ref currentValue, defaultValue, valueName, order);
		if (Stabbing_ModProj is not null)
		{
			FieldInfo fi = typeof(StabbingProjectile).GetField(valueName);
			if (fi != null)
			{
				fi.SetValue(Stabbing_ModProj, currentValue);
			}
		}
	}

	/// <summary>
	/// process from 0~1.
	/// </summary>
	/// <param name="minVallue"></param>
	/// <param name="maxValue"></param>
	/// <param name="process"></param>
	/// <param name="value"></param>
	public void AdjustValue(float minVallue, float maxValue, float process, out float value)
	{
		value = minVallue * (1 - process) + maxValue * process;
		StabbingSword.Update(StabbingSword.whoAmI);
	}

	public void DrawSlideBar(float minVallue, float maxValue, ref float currentValue, float defaultValue, string valueName, int order)
	{
		Texture2D track = Commons.ModAsset.SlideTrack_black.Value;
		Texture2D block = Commons.ModAsset.SlideBlock.Value;
		Texture2D defaultCut = Commons.ModAsset.SlideDefaultCut.Value;
		Vector2 pos = Player.Center + new Vector2(-240, -120 + order * 16) + PanelPos;
		Vector2 mouseTransformed = Vector2.Transform(Main.MouseScreen, Matrix.Invert(Main.GameViewMatrix.TransformationMatrix));
		mouseTransformed += Main.screenPosition;

		// Panel
		Vector2 panelPos = pos - Main.screenPosition;
		Rectangle panelArea = new Rectangle((int)panelPos.X - 240, (int)panelPos.Y - 8, 400, 16);
		Rectangle panelAreaLeft = new Rectangle((int)panelPos.X - 240, (int)panelPos.Y - 8, 150, 16);
		Rectangle panelAreaRight = new Rectangle((int)panelPos.X - 240 + 350, (int)panelPos.Y - 8, 50, 16);
		Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, panelArea, new Color(0.05f, 0.05f, 0.05f, 0.4f));
		if (DraggingIndex == -1 && (panelAreaLeft.Contains((mouseTransformed - Main.screenPosition).ToPoint()) || panelAreaRight.Contains((mouseTransformed - Main.screenPosition).ToPoint())))
		{
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				oldClickPos = mouseTransformed;
				oldPanelPos = PanelPos;
				DraggingIndex = -2;
			}
		}
		if(DraggingIndex == -2)
		{
			if(Main.mouseLeft)
			{
				PanelPos = oldPanelPos + mouseTransformed - oldClickPos;
			}
			else
			{
				DraggingIndex = -1;
			}
		}

		// Slide track
		Main.spriteBatch.Draw(track, pos - Main.screenPosition, null, Color.White, 0, track.Size() * 0.5f, 1f, SpriteEffects.None, 0);

		// Calculate silde block position
		float middleValue = (maxValue + minVallue) / 2f;
		float valueRange = maxValue - minVallue;
		float radius = 75;
		float movePos = (currentValue - middleValue) / valueRange * radius * 2;
		Vector2 blockPos = pos + new Vector2(movePos, 0);
		Rectangle blockRec = new Rectangle((int)blockPos.X - 4, (int)blockPos.Y - 8, 8, 16);

		// Drag the slide block and modify the value
		if (DraggingIndex == -1 && Main.mouseLeft && blockRec.Contains(mouseTransformed.ToPoint()))
		{
			DraggingIndex = order;
		}
		if (DraggingIndex == order)
		{
			blockPos.X = Math.Clamp(mouseTransformed.X, pos.X - radius, pos.X + radius);
			float process = (blockPos.X - pos.X) / radius + 1;
			process *= 0.5f;
			AdjustValue(minVallue, maxValue, process, out currentValue);
			if (Main.mouseLeftRelease && !Main.mouseLeft)
			{
				DraggingIndex = -1;
			}
			Main.instance.MouseText(valueName + " : " + currentValue);
		}

		// Draw deafult value position
		float movePosDefault = (defaultValue - middleValue) / valueRange * radius * 2;
		Vector2 defaultCutPos = pos + new Vector2(movePosDefault, 0);
		Rectangle defaultCutkRec = new Rectangle((int)defaultCutPos.X - 4, (int)defaultCutPos.Y - 8, 8, 16);

		// default cut
		if (defaultCutkRec.Contains(mouseTransformed.ToPoint()))
		{
			Main.spriteBatch.Draw(defaultCut, defaultCutPos - Main.screenPosition, null, Color.White, 0, defaultCut.Size() * 0.5f, 1.2f, SpriteEffects.None, 0);
			Main.instance.MouseText(valueName + " default : " + currentValue);
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				currentValue = defaultValue;
			}
		}
		else
		{
			Main.spriteBatch.Draw(defaultCut, defaultCutPos - Main.screenPosition, null, Color.White * 0.75f, 0, defaultCut.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}

		// silde block
		if (blockRec.Contains(mouseTransformed.ToPoint()))
		{
			Main.spriteBatch.Draw(block, blockPos - Main.screenPosition, null, Color.White, 0, block.Size() * 0.5f, 1.2f, SpriteEffects.None, 0);
			Main.instance.MouseText(valueName + " : " + currentValue);
		}
		else
		{
			Main.spriteBatch.Draw(block, blockPos - Main.screenPosition, null, Color.White * 0.75f, 0, block.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}

		// Text beside silde track.
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, valueName, Vector2.One);
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, valueName, pos - Main.screenPosition + new Vector2(-radius - 16, -8), Color.White, 0, new Vector2(stringSize.X, 0), 0.75f, SpriteEffects.None, 0);
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, currentValue.ToString(), pos - Main.screenPosition + new Vector2(radius + 16, -8), Color.White, 0, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0);
	}
}