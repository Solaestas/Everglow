using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class UnstablePollen : ModItem
{
	public override void SetStaticDefaults()
	{
		Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
	}
	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 24;
		Item.value = 90;
		Item.maxStack = Item.CommonMaxStack;
	}
	public override void Update(ref float gravity, ref float maxFallSpeed)
	{
		float timeValue = (float)(Main.time * 0.13f + Item.whoAmI) % 7;
		Vector3 color = new Vector3(0.4f, 0.3f, 0) * (MathF.Sin(timeValue) * 0.25f + 1.1f);
		Lighting.AddLight(Item.Center, color);

		base.Update(ref gravity, ref maxFallSpeed);
	}
	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
	{
		Texture2D texture2D = ModAsset.UnstablePollen.Value;
		Texture2D textureGlow = ModAsset.UnstablePollen_glow.Value;
		int frameCount = (int)(Main.time * 0.33f + MathF.Sin(whoAmI) * 5) % 7;
		Rectangle frame = new Rectangle(0, 30 * frameCount, 30, 30);
		spriteBatch.Draw(texture2D, Item.Center - Main.screenPosition, frame, lightColor, rotation, new Vector2(15f), scale, SpriteEffects.None, 0);
		spriteBatch.Draw(textureGlow, Item.Center - Main.screenPosition, frame, new Color(1f, 1f, 1f, 0), rotation, new Vector2(15f), scale, SpriteEffects.None, 0);
		return false;
	}
}
