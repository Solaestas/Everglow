using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.Common.Fish;

public class FishGlobalProjectile : GlobalProjectile
{
	public override void PostAI(Projectile projectile)
	{
		if (!projectile.bobber || !projectile.active)
		{
			return;
		}
		Point center = projectile.Center.ToTileCoordinates();
		var tile = TileUtils.SafeGetTile(center);
		if (tile != null)
		{
			if (tile.LiquidAmount > 0)
			{
				// 如果是原版液体，那就不用处理
				return;
			}
			if (!FishSystem.LiquidList.Contains(tile.TileType))
			{
				return;
			}
			projectile.velocity.X *= 0.7f;
			projectile.velocity.Y *= 0.4f;
			float da = projectile.rotation - MathF.PI / 2f;
			projectile.rotation = MathF.PI / 2f + da * 0.4f;

			Point top = center + new Point(0, -1);
			var topTile = TileUtils.SafeGetTile(top);
			if (topTile != null && FishSystem.LiquidList.Contains(topTile.TileType))
			{
				// 上面一格是液体，需要上浮
				projectile.velocity.Y = MathF.Min(projectile.velocity.Y - 1f, -2f);
			}
		}
	}
}