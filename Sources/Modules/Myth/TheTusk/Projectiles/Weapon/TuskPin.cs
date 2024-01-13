using Everglow.Myth.Misc.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.Projectiles.Weapon;

public class TuskPin : ModProjectile
{
	public override void OnSpawn(IEntitySource source)
	{
	}
	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.penetrate = -1;

		Projectile.timeLeft = 120;
		Projectile.extraUpdates = 3;
		Projectile.tileCollide = true;
	}
	bool HasHitTile = false;
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		HasHitTile = true;
		return false;
	}
	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
		base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
	}
	public override void AI()
	{
		if(!HasHitTile)
		{
			if (Projectile.timeLeft < 60)
			{
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 45;
			}
		}
		base.AI();
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = ModAsset.TuskPin.Value;
		Main.EntitySpriteDraw(texture,Projectile.Center - Main.screenPosition,null,lightColor,Projectile.rotation, texture.Size() / 2f, Projectile.scale,SpriteEffects.None);
		return false;
	}
}
