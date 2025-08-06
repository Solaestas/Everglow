using Everglow.Commons.Weapons.Slingshots;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Ranged;

public class RustSlingshot_Projectile : SlingshotAmmo
{
	public const int BuffDuration = 600;
	public const float FinalDamageBounusPerStack = 0.05f;

	public override string Texture => Commons.ModAsset.NormalAmmo_Mod;

	public override void SetDef()
	{
		Projectile.penetrate = 2;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		var indexOne = target.FindBuffIndex(ModContent.BuffType<Buffs.RustSlingshotBuffOne>());
		var indexTwo = target.FindBuffIndex(ModContent.BuffType<Buffs.RustSlingshotBuffTwo>());
		var indexThree = target.FindBuffIndex(ModContent.BuffType<Buffs.RustSlingshotBuffThree>());

		if (indexThree >= 0)
		{
			target.AddBuff(ModContent.BuffType<Buffs.RustSlingshotBuffThree>(), BuffDuration);
			modifiers.FinalDamage *= 1f + FinalDamageBounusPerStack * 3;
		}
		else if (indexTwo >= 0)
		{
			if (NetUtils.IsServer || NetUtils.IsSingle)
			{
				target.DelBuff(indexTwo);
			}

			target.AddBuff(ModContent.BuffType<Buffs.RustSlingshotBuffThree>(), BuffDuration);
			modifiers.FinalDamage *= 1f + FinalDamageBounusPerStack * 2;
		}
		else if (indexOne >= 0)
		{
			if (NetUtils.IsServer || NetUtils.IsSingle)
			{
				target.DelBuff(indexOne);
			}

			target.AddBuff(ModContent.BuffType<Buffs.RustSlingshotBuffTwo>(), BuffDuration);
			modifiers.FinalDamage *= 1f + FinalDamageBounusPerStack * 1;
		}
		else
		{
			target.AddBuff(ModContent.BuffType<Buffs.RustSlingshotBuffOne>(), BuffDuration);
		}
	}

	public override void OnKill(int timeLeft)
	{
		for (int x = 0; x < 16; x++)
		{
			var d = Dust.NewDustDirect(Projectile.position, 40, 40, DustID.Dirt);
			d.velocity *= Projectile.velocity.Length() / 10f;
		}
		SoundEngine.PlaySound(SoundID.Shatter, Projectile.Center);
	}
}