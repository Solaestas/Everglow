using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.DataStructures;
using Everglow.Commons.NetUtils;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class VitalizedRocksStone : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = 1;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 1023;
	}

	public override void AI()
	{
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[0] < 36)
		{
			Projectile.ai[0] += 3f;
		}
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.rotation = Projectile.velocity.ToRotation();
		var Portal = new RockPortal
		{
			velocity = Vector2.Zero,
			Active = true,
			Visible = true,
			position = Projectile.Center,
			maxTime = Main.rand.Next(12,30 ),
			scale = Main.rand.NextFloat(30, 54),
			rotation = Projectile.rotation,
			ai = new float[] {1},
		};
		Ins.VFXManager.Add(Portal);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.BrokenArmor, 180);
	}

	public override void OnKill(int timeLeft)
	{
		for (int x = 0; x < 8; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<RockElemental_fragments>(), 0f, 0f, 0, default, 0.7f);
			d.velocity = new Vector2(0, Main.rand.NextFloat(7f, 16f)).RotatedByRandom(6.283);
		}
		for (int x = 0; x < 16; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default, Main.rand.NextFloat(0.6f, 1.1f));
			d.velocity = new Vector2(0, Main.rand.NextFloat(2f, 11f)).RotatedByRandom(6.283);
		}
		GenerateSmog(4);
		SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(0.5f), Projectile.Center);
		ShakerManager.AddShaker(Projectile.Center, new Vector2(0, -1), 1, 30, 120);
	}

	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency / 2 + 1; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(40f, 55f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
		Vector2 pos = Projectile.position - Main.screenPosition;
		Main.spriteBatch.Draw(tex, new Rectangle((int)pos.X + 18, (int)pos.Y + 18, (int)Projectile.ai[0], 36),
							   new Rectangle((int)(36 - Projectile.ai[0]), 0, (int)Projectile.ai[0], 36), Color.White, Projectile.rotation, new Vector2(18, 18), 0, 0);
		return false;
	}
}