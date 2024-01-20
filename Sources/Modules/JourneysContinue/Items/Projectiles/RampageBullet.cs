using Everglow.JourneysContinue.Items.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.JourneysContinue.Items.Projectiles;

public class RampageBullet : ModProjectile
{

	public override void SetDefaults()
	{
		Projectile.width = 4; 
		Projectile.height = 15;
		Projectile.scale = 0.7f;

		//Projectile.aiStyle = 1;
		Projectile.timeLeft = 10000;

		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.tileCollide = false;

		Projectile.DamageType = DamageClass.Ranged;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override void PostDraw(Color lightColor)
	{
		
	}

	/*
	public override bool PreDraw(ref Color lightColor)
	{
		Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White),
			Projectile.rotation, TextureAssets.Projectile[Projectile.type].Size() / 2, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	*/
}