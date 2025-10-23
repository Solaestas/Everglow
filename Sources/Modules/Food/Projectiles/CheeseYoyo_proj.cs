using Everglow.Commons.Templates.Weapons.Yoyos;
using Everglow.Food.Dusts;
using Terraria.GameContent;

namespace Everglow.Food.Projectiles;

public class CheeseYoyo_proj : YoyoProjectile
{
	public override void SetCustomDefaults()
	{
		RotationalSpeed = 0.3f;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		for (int i = 0; i < 6; i++)
		{
			Dust.NewDust(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<CheeseDust>());
		}
		base.OnHitNPC(target, hit, damageDone);
	}

	public override Color ModifyYoyoStringColor_PostVanillaRender(Color vanillaColor, Vector2 worldPos, float index, float stringCount)
	{
		Color c0 = new Color(255, 198, 83);
		Color c1 = Lighting.GetColor(worldPos.ToTileCoordinates(), c0);
		return c1;
	}
}