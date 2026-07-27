using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Commons.Templates.Weapons.StabbingSwords.VFX;
using Everglow.Commons.TileHelper;
using Everglow.Commons.Utilities;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Projectiles
{
	public class SwordfishBeak_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(47, 72, 80);
			StabShade = 0.2f;
			StabDistance = 0.75f;
			StabEffectWidth = 0.4f;
			HitTileSparkColor = new Color(117, 134, 243, 100);
		}

		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			if (player.wet)
			{
				StabDistance = 1.125f;
			}
			else
			{
				StabDistance = 0.75f;
			}
			base.OnSpawn(source);
		}

		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}

		public override void AI()
		{
			if (Main.rand.NextBool(7))
			{
				Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
				Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.24f);
				if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
				{
					var dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<SeaWater>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
					dust.velocity = vel;
				}
			}
			base.AI();
		}

		public override void HitTile()
		{
			SoundEngine.PlaySound(SoundID.Dig.WithPitchOffset(Main.rand.NextFloat(0.6f, 1f)), Projectile.Center);
			var hitSparkFixed = new StabbingProjectile_HitEffect()
			{
				Active = true,
				Visible = true,
				Position = StabEndPoint_WorldPos,
				MaxTime = 16,
				Scale = 0.24f,
				Rotation = Projectile.velocity.ToRotation(),
				Color = HitTileSparkColor,
			};
			Ins.VFXManager.Add(hitSparkFixed);

			Vector2 tilePos = StabEndPoint_WorldPos + new Vector2(1, 0).RotatedBy(Projectile.velocity.ToRotation());
			Point tileCoord = tilePos.ToTileCoordinates();
			Tile tile = WorldGenMisc.SafeGetTile(tileCoord);
			if (TileUtils.Sets.TileFragile[tile.TileType])
			{
				WorldGenMisc.DamageTile(tileCoord, 100);
			}
		}
	}
}