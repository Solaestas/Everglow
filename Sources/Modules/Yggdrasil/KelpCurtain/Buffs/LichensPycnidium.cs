using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class LichensPycnidium : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.width = 15;
		Projectile.height = 15;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		base.SetDefaults();
	}
	public NPC AttachTarget;
	public float Maturity;
	public override void OnSpawn(IEntitySource source)
	{
		if (AttachTarget == null)
		{
			float closestLength = 50;
			foreach (NPC npc in Main.npc)
			{
				if (npc.active && !npc.friendly)
				{
					if ((npc.Center - Projectile.Center).Length() < closestLength)
					{
						closestLength = (npc.Center - Projectile.Center).Length();
						AttachTarget = npc;
					}
				}
			}
		}
		if (AttachTarget == null)
		{
			Projectile.active = false;
		}
		Maturity = 0;
		base.OnSpawn(source);
	}
	public override void AI()
	{
		if (AttachTarget == null)
		{
			Projectile.active = false;
		}
		if(Maturity < 100)
		{
			Maturity++;
		}
		base.AI();
	}
	public void Explode()
	{

	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = ModAsset.LichensPycnidium.Value;
		lightColor.A = 0;
		Color color = Color.Lerp(lightColor , new Color(0.1f, 0.7f, 0.9f, 0), Maturity / 100f);
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, 0, texture.Size() / 2f, 0.5f, SpriteEffects.None, 0);
		return false;
	}
}