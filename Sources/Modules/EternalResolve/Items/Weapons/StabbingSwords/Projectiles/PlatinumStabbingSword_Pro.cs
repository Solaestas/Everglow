using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class PlatinumStabbingSword_Pro : StabbingProjectile
	{
		public override void SetDefaults()
		{
			Color = new Color(166, 166, 226);
			base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.7f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.90f;
			DrawWidth = 0.4f;
		}
		float bottomPos1 = 0f;
		float bottomPos2 = 0f;
		public override void DrawItem(Color lightColor)
		{
			Texture2D itemTexture = ModAsset.PlatinumStabbingSword_withouFlag.Value;
			Main.spriteBatch.Draw(itemTexture, ItemDraw.Postion - Main.screenPosition, null, lightColor, ItemDraw.Rotation, itemTexture.Size() / 2f, ItemDraw.Size, ItemDraw.SpriteEffect, 0f);

			if (!Main.gamePaused)
			{
				bottomPos1 = bottomPos1 * 0.8f + Main.rand.NextFloat(0.45f, 1.75f) * 0.2f;
				bottomPos2 = bottomPos2 * 0.8f + Main.rand.NextFloat(0.45f, 1.75f) * 0.2f;
			}
			else
			{
				//暂停的时候可以有一个渐停效果，看起来很好
				bottomPos1 = bottomPos1 * 0.9f;
				bottomPos2 = bottomPos2 * 0.9f;
			}
			float scale = MathF.Sin((float)Main.timeForVisualEffects);
			DrawFlags(lightColor, -7, 5, ModAsset.PlatinumStabbingSword_flag.Value, bottomPos1 * scale, bottomPos2 * scale);
		}
	}
}