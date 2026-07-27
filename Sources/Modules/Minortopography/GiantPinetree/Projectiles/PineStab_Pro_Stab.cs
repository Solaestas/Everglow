using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Commons.Templates.Weapons.StabbingSwords.VFX;
using Everglow.Commons.TileHelper;
using Everglow.Commons.Utilities;
using Everglow.Minortopography.GiantPinetree.Dusts;
using Terraria.Audio;

namespace Everglow.Minortopography.GiantPinetree.Projectiles
{
	public class PineStab_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetCustomDefaults()
		{
			StabColor = new Color(11, 84, 46);
			StabShade = 2f;
			StabDistance = 0.63f;
			StabEffectWidth = 0.25f;
			HitTileSparkColor = new Color(11, 84, 46, 30);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.Defense *= 3f;
			base.ModifyHitNPC(target, ref modifiers);
		}

		public override void HitTile()
		{
			SoundStyle ss = SoundID.Grass;
			SoundEngine.PlaySound(ss.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), Projectile.Center);
			for (int x = 0; x < 6; x++)
			{
				Dust.NewDustDirect(StabEndPoint_WorldPos, 0, 0, ModContent.DustType<PineDust>());
			}
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