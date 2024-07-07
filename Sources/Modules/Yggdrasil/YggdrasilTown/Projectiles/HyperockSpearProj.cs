using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.DataStructures;
using Everglow.Commons.NetUtils;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
	public NPC NPCStickTo;

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
			Vector2 vel = -Vector2.UnitX.RotatedByRandom(MathF.PI * 2) * 6 * MathF.Cos(Projectile.timeLeft * MathF.PI / 40);
			var Vortex = new HyperockSpear_VortexLine
			{
				velocity = vel,
				Active = true,
				Visible = true,
				positiontoProjectile = vel.RotatedBy(MathF.PI * 0.5) * 5 * MathF.Min(MathF.Cos(Projectile.timeLeft * MathF.PI / 40), 0),
				InHand = false,
				maxTime = Main.rand.Next(120, 180),
				scale = 6,
				VFXOwner = Projectile,
				ai = new float[] { 1, 0 },
			};
			Ins.VFXManager.Add(Vortex);
			SticktoNPC();
			/*if (NPCStickTo.active)
			{
				Projectile.velocity = (NPCStickTo.Center - Projectile.Center) * 0.75f;
				Projectile.netUpdate = true;
				
				if (Projectile.timeLeft <= 35 && Projectile.timeLeft >= 27)
				{
					foreach (NPC npc in Main.npc)
					{
						if (npc.active)
						{

							Vector2 distance = Projectile.Center - npc.Center;
							if (distance.Length() < Power * 8 && !npc.dontTakeDamage && !npc.friendly)
							{
								npc.velocity += Utils.SafeNormalize(distance, Vector2.zeroVector)
							* MathF.Min(Power / 30, npc.knockBackResist * 1145 / (distance.Length() * Power / 30));

							}
						}
					}
				}
			}
			else
			{
				Projectile.Kill();
			}*/
		}
		else if (CollideOnTile)
		{
			Projectile.velocity = Vector2.Zero;
			Vector2 vel = -Vector2.UnitX.RotatedByRandom(MathF.PI * 2) * 6 * MathF.Cos(Projectile.timeLeft * MathF.PI / 40);
			var Vortex = new HyperockSpear_VortexLine
			{
				velocity = vel,
				Active = true,
				Visible = true,
				positiontoProjectile = vel.RotatedBy(MathF.PI * 0.5) * 5 * MathF.Min(MathF.Cos(Projectile.timeLeft * MathF.PI / 40), 0),
				InHand = false,
				maxTime = Main.rand.Next(120, 180),
				scale = 6,
				VFXOwner = Projectile,
				ai = new float[] { 1, 0 },
			};
			Ins.VFXManager.Add(Vortex);
			if (Projectile.timeLeft <= 35 && Projectile.timeLeft >= 27)
			{
				foreach (NPC npc in Main.npc)
				{
					if (npc.active)
					{

						Vector2 distance = Projectile.Center - npc.Center;
						if (distance.Length() < Power * 8 && !npc.dontTakeDamage && !npc.friendly)
						{
							npc.velocity += Utils.SafeNormalize(distance, Vector2.zeroVector)
						* MathF.Min(Power / 30, npc.knockBackResist * 1145 / (distance.Length() * Power / 30));
						}
					}
				}
			}

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
					InHand = true,
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
						InHand = true,
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
			Projectile.velocity = Utils.SafeNormalize(Main.MouseWorld - player.MountedCenter, new Vector2(0, -1 * player.gravDir)) * (Power + 100) / 9f;
			Projectile.damage = (int)(Projectile.damage * (Power / 30f + 1));
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
		Main.spriteBatch.Draw(tex, pos, null, lightColor, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(glow, pos, null, Color.White, Projectile.rotation, glow.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		Projectile.velocity = Vector2.Zero;
		CollideOnTile = true;
		Projectile.timeLeft = 60;
		Shot = false;
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!CollideOnNPC)
		{
			Projectile.ai[0] = 1;
			CollideOnNPC = true;
			NPCStickTo = target;
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 60;
			Shot = false;
			SticktoNPC();
		}

	}
	public void SticktoNPC()
	{
		Projectile.ai[0] = 1;
		Projectile.ai[1] = NPCStickTo.whoAmI;
		Projectile.velocity = (NPCStickTo.Center - Projectile.Center) * 0.75f;
		Projectile.netUpdate = true;

		Vector2 center17 = Projectile.Center;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		int num907 = 15;


		bool flag52 = false;
		bool flag53 = false;
		Projectile.localAI[0]++;
		if (Projectile.localAI[0] % 30f == 0f)
			flag53 = true;

		int num908 = (int)Projectile.ai[1];
		if (Projectile.localAI[0] >= (float)(60 * num907))
		{
			flag52 = true;
		}
		else if (NPCStickTo.whoAmI < 0 || NPCStickTo.whoAmI >= 200)
		{
			flag52 = true;
		}
		else if (NPCStickTo.active && !NPCStickTo.dontTakeDamage)
		{
			Projectile.Center = NPCStickTo.Center - Projectile.velocity * 2f;
			Projectile.gfxOffY = NPCStickTo.gfxOffY;
			if (flag53)
			{
				NPCStickTo.HitEffect(0, 1.0);
			}
		}
		else
		{
			Projectile.Kill();
		}
	}



}

/*public override void OnKill(int timeLeft)
{
	/*for (int x = 0; x < 16; x++)
	{
		Dust d = Dust.NewDustDirect(Projectile.position, 40, 40, ModContent.DustType<Dusts.CyanVine>());
		d.velocity *= Projectile.velocity.Length() / 10f;
	}*/
/*SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
*/
/*Vector2 vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d) * Projectile.velocity.Length() / 10f;
Gore.NewGore(null, Projectile.Center + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineThrowingSpearGore0").Type, 1f);

vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d) * Projectile.velocity.Length() / 10f;
Gore.NewGore(null, Projectile.Center + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineThrowingSpearGore1").Type, 1f);

vF = new Vector2(0, Main.rand.NextFloat(0, 3f)).RotatedByRandom(6.28d) * Projectile.velocity.Length() / 10f;
Gore.NewGore(null, Projectile.Center + vF, vF, ModContent.Find<ModGore>("Everglow/CyanVineOre" + Main.rand.Next(11, 13).ToString()).Type, 1f);*/

