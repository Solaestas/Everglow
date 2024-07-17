using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class HyperockSpearProj : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1500;
		Projectile.aiStyle = -1;
		Projectile.ArmorPenetration = 5;
		Projectile.penetrate = -1;

		ProjectileID.Sets.IsAnNPCAttachedExplosive[Projectile.type] = true;
	}
	internal bool Shot = false;
	internal int Power = 0;
	public Vector2 oldpos = Vector2.Zero;
	public bool CollideOnTile = false;
	public bool CollideOnNPC = false;
	public NPC NPCStickTo = null;
	public Vector2 PostoNPC;

	public override void AI()
	{

		Player player = Main.player[Projectile.owner];
		int PlayerDir = -1;
		if (Main.MouseWorld.X > player.Center.X)
		{
			PlayerDir = 1;
		}
		if (Shot)
		{
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.velocity.Y += 0.163f;
			Projectile.velocity *= 0.998f;
			Projectile.rotation = (float)(MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathF.PI * 0.25);
		}
		else if (CollideOnNPC)
		{
			Vector2 vel = -Vector2.UnitX.RotatedByRandom(MathF.PI * 2) * 6 * MathF.Cos(Projectile.timeLeft * MathF.PI / 40) * Power / 45;
			var Vortex = new HyperockSpear_VortexLine
			{
				velocity = vel,
				Active = true,
				Visible = true,
				positiontoProjectile = vel.RotatedBy(MathF.PI * 0.5) * 5 * MathF.Min(MathF.Cos(Projectile.timeLeft * MathF.PI / 40), 0),
				OnTile = false,
				maxTime = Main.rand.Next(120, 180),
				scale = 6,
				VFXOwner = Projectile,
				ai = new float[] { 1, 0 },
			};
			Ins.VFXManager.Add(Vortex);
			SticktoNPC();
			AbsorbNPC();

		}
		else if (CollideOnTile)
		{
			Projectile.velocity = Vector2.Zero;
			Vector2 vel = -Vector2.UnitX.RotatedByRandom(MathF.PI * 2) * 6 * MathF.Cos(Projectile.timeLeft * MathF.PI / 40) * Power / 45;
			var Vortex = new HyperockSpear_VortexLine
			{
				velocity = vel,
				Active = true,
				Visible = true,
				positiontoProjectile = vel.RotatedBy(MathF.PI * 0.5) * 5 * MathF.Min(MathF.Cos(Projectile.timeLeft * MathF.PI / 40), 0),
				OnTile = true,
				maxTime = Main.rand.Next(120, 180),
				scale = 6,
				VFXOwner = Projectile,
				ai = new float[] { 1, 0 },
			};
			Ins.VFXManager.Add(Vortex);
			AbsorbNPC();
		}
		else
		{
			if (Power < 45)
			{
				Vector2 vel = Vector2.UnitX.RotatedByRandom(MathF.PI * 2) * 4;
				var Vortex = new HyperockSpear_VortexLine
				{
					velocity = vel,
					Active = true,
					Visible = true,
					positiontoProjectile = vel.RotatedBy(MathF.PI * 0.5) * 10,
					OnTile = false,
					maxTime = Main.rand.Next(120, 180),
					scale = 6,
					VFXOwner = Projectile,
					ai = new float[] { 1, 0 },
				};
				Ins.VFXManager.Add(Vortex);
				oldpos = Projectile.Center;
				Power += 1;
			}
			Projectile.timeLeft = 1500;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.MountedCenter, new Vector2(0, -1 * player.gravDir));
			Projectile.Center = player.MountedCenter + Projectile.velocity.RotatedBy(Math.PI * -0.5) * 20 * PlayerDir - Projectile.velocity * (Power / 3f - 50) + new Vector2(0, 6 * player.gravDir);
			Projectile.rotation = (float)(MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathF.PI * 0.25);
			if (Power == 44)
			{
				for (int i = 0; i < 30; i++)
				{
					Vector2 vel = Vector2.UnitX.RotatedByRandom(MathF.PI * 2) * 10;
					Vector2 aimposition = Projectile.Center;
					var Vortex = new HyperockSpear_VortexLine
					{
						velocity = vel,
						Active = true,
						Visible = true,
						positiontoProjectile = Vector2.Zero,
						OnTile = false,
						maxTime = Main.rand.Next(90, 120),
						scale = 6,
						VFXOwner = Projectile,
						ai = new float[] { 0f, Main.rand.NextFloat(-0.05f, 0.05f) },
					};
					Ins.VFXManager.Add(Vortex);
				}
			}
			player.heldProj = Projectile.whoAmI;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + 0.337f * PlayerDir + (float)(Math.PI * 0.25 + Math.PI * 0.6 * PlayerDir - (Power / 80d + 0.2) * PlayerDir));
			player.direction = PlayerDir;

		}
		if (player.controlUseItem && !Shot && !CollideOnNPC && !CollideOnTile)
		{
			Shot = true;
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.MountedCenter, new Vector2(0, -1 * player.gravDir)) * (Power + 180) / 18;
			Projectile.damage = (int)(Projectile.damage * (Power / 90 + 1));
			Projectile.CritChance = (int)(Projectile.CritChance * (Power / 90 + 1));
			SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
			player.heldProj = -1;
		}
		if (player.HeldItem.type != ModContent.ItemType<Items.Weapons.RockElemental.HyperockSpear>())
		{
			Projectile.active = false;
		}

		Lighting.AddLight(Projectile.Center + Vector2.One.RotatedBy(Projectile.rotation + MathF.PI * 0.48) * 12.5f, 0.25f, 0.05f, 0.4f);

	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = ModAsset.HyperockSpearProj.Value;
		Texture2D glow = ModAsset.HyperockSpearProj_glow.Value;

		Player player = Main.player[Projectile.owner];
		Vector2 pos = Projectile.Center - Main.screenPosition - Utils.SafeNormalize(Projectile.velocity, Vector2.zeroVector) * 30;
		if (CollideOnNPC)
		{
			pos = Projectile.Center - Main.screenPosition + Vector2.UnitY.RotatedBy(Projectile.rotation + MathF.PI * 0.25) * 30;
		}
		Main.spriteBatch.Draw(tex, pos, null, lightColor, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(glow, pos, null, Color.White, Projectile.rotation, glow.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (!CollideOnNPC && !CollideOnTile)
		{
			Projectile.velocity = Vector2.Zero;
			CollideOnTile = true;
			Projectile.timeLeft = 60;
			Shot = false;
			return false;
		}
		return true;
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

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!CollideOnNPC && !CollideOnTile)
		{
			Projectile.ai[0] = 1;
			CollideOnNPC = true;
			NPCStickTo = target;
			Projectile.timeLeft = 60;
			Shot = false;
			PostoNPC = NPCStickTo.Center - Projectile.Center;
			SticktoNPC();
		}
	}

	public void AbsorbNPC()
	{
		if (Projectile.timeLeft <= 40 && Projectile.timeLeft >= 30)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc.active && npc != NPCStickTo)
				{

					Vector2 distance = Projectile.Center - npc.Center;
					if (distance.Length() < Power * 8 && !npc.dontTakeDamage && !npc.friendly)
					{
						npc.velocity += Utils.SafeNormalize(distance, Vector2.zeroVector)
					* MathF.Min(Power / 45, npc.knockBackResist * 1145.14f / ((distance.Length() + 191) * Power / 98.10f));
					}
				}
			}
		}
	}

	public void SticktoNPC()
	{
		Projectile.ai[0] = 1;
		Projectile.ai[1] = NPCStickTo.whoAmI;

		Projectile.velocity = Vector2.Zero;
		Projectile.netUpdate = true;

		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;

		if (NPCStickTo.active && !NPCStickTo.dontTakeDamage)
		{
			Projectile.Center = NPCStickTo.Center - PostoNPC;
			Projectile.gfxOffY = NPCStickTo.gfxOffY;
		}
		else
		{
			Projectile.Kill();
		}
	}

	public override bool? CanDamage()
	{
		if (CollideOnNPC && Projectile.timeLeft <= 50)
		{
			return false;
		}
		return base.CanDamage();
	}
}