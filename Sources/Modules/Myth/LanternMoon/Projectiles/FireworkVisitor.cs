namespace Everglow.Myth.LanternMoon.Projectiles;

public class FireworkVisitor : ModPlayer
{
	public Vector2 BestFireworkView;
	public Vector2 SwapFireworkView;
	public override void ModifyScreenPosition()
	{
		if (BestFireworkView.Length() > 0.1f)
		{
			SwapFireworkView = SwapFireworkView * 0.9f + BestFireworkView * 0.1f;
			bool hasFirework = false;
			foreach(Projectile p in Main.projectile)
			{
				if(p != null && p.active)
				{
					if (p.type == ModContent.ProjectileType<RisingFirework>())
					{
						RisingFirework risingFirework = p.ModProjectile as RisingFirework;
						if (risingFirework != null)
						{
							if (risingFirework.MoveSight)
							{
								hasFirework = true;
								break;
							}
						}
					}
					FireworkProjectile firework = p.ModProjectile as FireworkProjectile;
					if (firework != null)
					{
						if (firework.MoveSight)
						{
							hasFirework = true;
							break;
						}
					}
				}
			}
			if (!hasFirework)
			{
				BestFireworkView *= 0.8f;
			}
		}
		else
		{
			SwapFireworkView = Vector2.zeroVector;
		}
		if (SwapFireworkView != Vector2.zeroVector)
		{
			Main.screenPosition = Main.screenPosition + SwapFireworkView;
		}
		
		base.ModifyScreenPosition();
	}
}