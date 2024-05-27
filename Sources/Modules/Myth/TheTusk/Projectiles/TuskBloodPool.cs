using Everglow.Myth.TheTusk.Tiles;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.Projectiles;

public class TuskBloodPool : ModProjectile
{
	public override string Texture => ModAsset.Empty_Mod;

	public List<Point> DissolvingTile;

	public override void SetDefaults()
	{
		base.SetDefaults();
		DissolvingTile = new List<Point>();
	}

	public override void OnSpawn(IEntitySource source)
	{
		Point point = Projectile.Center.ToTileCoordinates();
		if (point.X < 20 || point.X > Main.maxTilesX - 20)
		{
			Projectile.active = false;
			return;
		}
		if (point.Y < 20 || point.Y > Main.maxTilesY - 20)
		{
			Projectile.active = false;
			return;
		}

		for (int i = -5; i < 6; i++)
		{
			for (int j = -5; j < 6; j++)
			{
				Point checkPoint = new Point(i, j) + point;
				Tile tile = Main.tile[checkPoint];
				if (tile.HasTile && tile.TileType == ModContent.TileType<TuskFlesh>())
				{
					if (new Vector2(i, j).Length() < Main.rand.NextFloat(2.5f, 5f))
					{
						tile.ClearTile();
						DissolvingTile.Add(checkPoint);
					}
				}
			}
		}
		Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<TuskBloodPool_open>(), 0, 0, Projectile.owner);
		TuskBloodPool_open tbpo = projectile.ModProjectile as TuskBloodPool_open;
		tbpo.DissolvingTile = DissolvingTile;
		base.OnSpawn(source);
	}

	public override void AI()
	{
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		Projectile.hide = true;
		behindNPCsAndTiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void OnKill(int timeLeft)
	{
		Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<TuskBloodPool_close>(), 0, 0, Projectile.owner);
		TuskBloodPool_close tbpc = projectile.ModProjectile as TuskBloodPool_close;
	}
}