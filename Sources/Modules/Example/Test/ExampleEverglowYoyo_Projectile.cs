using Everglow.Commons.Templates.Weapons.Yoyos;

namespace Everglow.Example.Test;

public class ExampleEverglowYoyo_Projectile : YoyoProjectile
{
	public override void SetCustomDefaults()
	{
		MaxStaySeconds = 35000000;
		MaxRopeLength = 2500;
		Acceleration = 15;
	}

	public override void YoyoAI()
	{
		base.YoyoAI();
	}

	public override void DrawYoyo_String(Vector2 playerHeldPos = default)
	{
		base.DrawYoyo_String();
	}

	public override Color ModifyYoyoStringColor_PostVanillaRender(Color vanillaColor, Vector2 worldPos, float index, float stringCount)
	{
		float value = index / stringCount;
		Color color = Color.Lerp(Color.White, new Color(0.5f, 0f, 0.1f, 0f), value);
		color = Lighting.GetColor(worldPos.ToTileCoordinates(), color);
		return color;
	}
}