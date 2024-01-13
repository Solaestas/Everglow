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
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 60;
		Projectile.timeLeft = 120;
		Projectile.extraUpdates = 3;
	}
	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
		base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
	}
	public override void AI()
	{
		base.AI();
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = ModAsset.TuskPin.Value;
		Main.EntitySpriteDraw(texture,Projectile.Center - Main.screenPosition,null,lightColor,Projectile.rotation, texture.Size() / 2f, Projectile.scale,SpriteEffects.None);
		return false;
	}
}
