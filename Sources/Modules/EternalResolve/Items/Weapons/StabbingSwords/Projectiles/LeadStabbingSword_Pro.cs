using Everglow.Commons.Weapons.StabbingSwords;
namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class LeadStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Color = new Color(85, 94, 123);
			base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.4f;
			Shade = 0.2f;
			FadeTradeShade = 0.44f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.72f;
			DrawWidth = 0.4f;
		}
		float bottomPos1 = 0f;
		float bottomPos2 = 0f;
		public override void DrawItem(Color lightColor)
		{

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
			DrawFlags(lightColor, -9, 10, ModAsset.LeadStabbingSword_flag.Value, bottomPos1, bottomPos2);
			Texture2D itemTexture = ModAsset.LeadStabbingSword_withouFlag.Value;
			Main.spriteBatch.Draw(itemTexture, ItemDraw.Postion - Main.screenPosition, null, lightColor, ItemDraw.Rotation, itemTexture.Size() / 2f, ItemDraw.Size, ItemDraw.SpriteEffect, 0f);
		}
	}
}