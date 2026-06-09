using Everglow.Commons.Coroutines;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Mechanics.Miscs;
using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Enemies;
using Everglow.Yggdrasil.KelpCurtain.VFXs.VampireMat;
using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.NPCs.VampireMat;

[AutoloadBossHead]
[NoGameModeScale]
public class VampireMat : ModNPC
{
	public CoroutineManager AICoroutine = new CoroutineManager();

	public Rope BodyRope;

	public enum TextureState
	{
		Flat,
		TowardScreen,
		ProjRelease,
	}

	public int NPCTextureState = 0;

	public bool TowardScreenAndAttacking = false;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 11;
		NPCSpawnManager.RegisterNPC(Type);
	}

	public override void SetDefaults()
	{
		NPC.width = 100;
		NPC.height = 100;
		NPC.boss = true;
		NPC.noGravity = true;
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.value = 32000;

		NPC.lifeMax = 26000;
		NPC.damage = 70;
		NPC.defense = 45;
		NPC.knockBackResist = 0f;

		NPC.noTileCollide = true;
		NPC.aiStyle = -1;
	}

	public override void OnSpawn(IEntitySource source)
	{
		NPC.localAI[0] = 0;
		NPC.TargetClosest();
		BodyRope = Rope.Create(NPC.Center + new Vector2(150, 0), NPC.Center - new Vector2(-150, 0), 20, 5, 5, 20, 5);
		AICoroutine.StartCoroutine(new Coroutine(Dash_0()));
	}

	public override bool CheckActive()
	{
		return false;
	}

	public override void AI()
	{
		AICoroutine.Update();
		BodyRope.Masses[0].Position = NPC.Center + new Vector2(150, 0).RotatedBy(NPC.rotation);
		BodyRope.ApplyForce();
	}

	public IEnumerator<ICoroutineInstruction> Dash_0()
	{
		yield return new WaitUntil(() => NPC.target >= 0);
		Player player = Main.player[NPC.target];
		int direction = NPC.Center.X > player.Center.X ? 1 : -1;

		NPC.spriteDirection = direction;
		Vector2 toTarget = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 11f;
		float rot = toTarget.ToRotation();
		NPC.velocity = toTarget;
		for (int k = 0; k < 30; k++)
		{
			NPC.rotation = rot * 0.05f + NPC.rotation * 0.95f;
			yield return new SkipThisFrame();
		}
		for (int k = 0; k < 30; k++)
		{
			NPC.velocity *= 0.96f;
			yield return new SkipThisFrame();
		}
		AICoroutine.StartCoroutine(new Coroutine(NextAttack()));
	}

	public IEnumerator<ICoroutineInstruction> TentacleRelease()
	{
		for (int k = 0; k < 120; k++)
		{
			NPC.velocity *= 0.96f;
			NPC.rotation *= 0.96f;
			if (NPCTextureState == (int)TextureState.Flat)
			{
				if (NPC.frame.Y == 1060)
				{
					NPCTextureState = (int)TextureState.TowardScreen;
					NPC.frame = new Rectangle(0, 0, 400, 400);
					break;
				}
			}
			yield return new SkipThisFrame();
		}
		NPC.velocity *= 0f;
		NPC.rotation *= 0f;
		for (int k = 0; k < 19; k++)
		{
			if (k % 4 == 3)
			{
				NPC.frame.Y += 400;
			}
			yield return new SkipThisFrame();
		}
		TowardScreenAndAttacking = true;
		yield return new WaitForFrames(20);
		List<int> projRots = [0, 1, 2, 3, 4, 5, 6];
		for (int k = 0; k < 48; k++)
		{
			if (k % 7 == 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.zeroVector, ModContent.ProjectileType<VampireMat_Tentacle>(), 88, 5, NPC.target);
				int number = projRots[Main.rand.Next(projRots.Count)];
				switch (number)
				{
					case 0:
						proj.rotation = -15.8f / 360f * MathHelper.TwoPi;
						break;
					case 1:
						proj.rotation = 25.54f / 360f * MathHelper.TwoPi;
						break;
					case 2:
						proj.rotation = 84.36f / 360f * MathHelper.TwoPi;
						break;
					case 3:
						proj.rotation = 143.07f / 360f * MathHelper.TwoPi;
						break;
					case 4:
						proj.rotation = -162.52f / 360f * MathHelper.TwoPi;
						break;
					case 5:
						proj.rotation = -114.61f / 360f * MathHelper.TwoPi;
						break;
					case 6:
						proj.rotation = -59.09f / 360f * MathHelper.TwoPi;
						break;
				}
				for (int j = 0; j < 2; j++)
				{
					Projectile proj_tusk = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, new Vector2(4, 0).RotatedBy((j - 0.5f) * 0.6f + proj.rotation), ModContent.ProjectileType<VampireMat_Attack_Proj_Tusk>(), 55, 2.5f, NPC.target);
				}
				projRots.Remove(number);
			}
			yield return new SkipThisFrame();
		}
		yield return new WaitForFrames(180);
		TowardScreenAndAttacking = false;
		for (int k = 0; k < 8; k++)
		{
			if (k % 4 == 3)
			{
				NPC.frame.Y += 400;
			}
			yield return new SkipThisFrame();
		}
		NPCTextureState = (int)TextureState.Flat;
		NPC.frame = new Rectangle(0, 0, 346, 106);
		AICoroutine.StartCoroutine(new Coroutine(NextAttack()));
	}

	public IEnumerator<ICoroutineInstruction> ProjRelease()
	{
		for (int k = 0; k < 120; k++)
		{
			NPC.velocity *= 0.96f;
			NPC.rotation *= 0.96f;
			if (NPCTextureState == (int)TextureState.Flat)
			{
				if (NPC.frame.Y == 1060)
				{
					NPCTextureState = (int)TextureState.ProjRelease;
					NPC.frame = new Rectangle(0, 0, 400, 400);
					break;
				}
			}
			yield return new SkipThisFrame();
		}
		Player player = Main.player[NPC.target];
		NPC.velocity *= 0f;
		Vector2 toTarget = player.Center - NPC.Center;
		NPC.spriteDirection = 1;
		NPC.rotation = toTarget.ToRotationSafe() + MathHelper.PiOver2;
		for (int k = 0; k < 31; k++)
		{
			if (k % 4 == 3)
			{
				NPC.frame.Y += 400;
			}
			if (k == 25)
			{
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, toTarget.NormalizeSafe() * 6f, ModContent.ProjectileType<VampireMat_Attack_Proj_Ball>(), 48, 2.5f, Main.myPlayer);
				NPC.velocity -= toTarget.NormalizeSafe() * 12f;
			}
			NPC.velocity *= 0.9f;
			yield return new SkipThisFrame();
		}
		NPCTextureState = (int)TextureState.Flat;
		NPC.frame = new Rectangle(0, 0, 346, 106);
		AICoroutine.StartCoroutine(new Coroutine(NextAttack()));
	}

	public IEnumerator<ICoroutineInstruction> NextAttack()
	{
		if (NPC.target >= 0)
		{
			Player player = Main.player[NPC.target];
			if ((player.Center - NPC.Center).Length() > 600)
			{
				AICoroutine.StartCoroutine(new Coroutine(Dash_0()));
				yield break;
			}
		}
		switch (Main.rand.Next(3))
		{
			case 0:
				AICoroutine.StartCoroutine(new Coroutine(Dash_0()));
				break;
			case 1:
				AICoroutine.StartCoroutine(new Coroutine(TentacleRelease()));
				break;
			case 2:
				AICoroutine.StartCoroutine(new Coroutine(ProjRelease()));
				break;
		}

		yield return new SkipThisFrame();
	}

	public override void FindFrame(int frameHeight)
	{
		if (NPCTextureState == (int)TextureState.Flat)
		{
			float animationSpeed = 0.4f;
			NPC.frameCounter += animationSpeed;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			NPC.frame.Y = (int)NPC.frameCounter * 106;
		}
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		VampireMatHitCommonEffect(target);
		base.OnHitPlayer(target, info);
	}

	public static void VampireMatHitCommonEffect(Player target)
	{
		if (target.HasBuff(BuffID.Gills))
		{
			target.ClearBuff(BuffID.Gills);
		}
		var screenEffectVFX = new ScreenScaringEffect()
		{
			Active = true,
			Visible = true,
			Timer = 0,
			MaxTime = 120,
		};
		Ins.VFXManager.Add(screenEffectVFX);
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return base.SpawnChance(spawnInfo);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var drawPos = NPC.Center - Main.screenPosition;
		if (NPCTextureState == (int)TextureState.TowardScreen)
		{
			texture = ModAsset.VampireMat_Attack.Value;
			if (TowardScreenAndAttacking)
			{
				List<Vertex2D> bars = new List<Vertex2D>();
				for (int k = 0; k < 20; k++)
				{
					Vector2 offset0 = new Vector2(200, 0).RotatedBy(k / 20f * MathHelper.TwoPi);
					Vector2 offset1 = new Vector2(200, 0).RotatedBy((k + 1) / 20f * MathHelper.TwoPi);
					Vector2 pos0 = drawPos + offset0 * (1 + MathF.Sin(k / 4f * MathHelper.TwoPi + (float)Main.time * 0.09f) * 0.1f);
					Vector2 pos1 = drawPos + offset1 * (1 + MathF.Sin((k + 1) / 4f * MathHelper.TwoPi + (float)Main.time * 0.09f) * 0.1f);
					SpriteBatchUtils.AddVertexWithEnv_Light(bars, pos0, new Vector3((new Vector2(200, 1800) + offset0) / texture.Size(), 0), false);
					SpriteBatchUtils.AddVertexWithEnv_Light(bars, pos1, new Vector3((new Vector2(200, 1800) + offset1) / texture.Size(), 0), false);
					SpriteBatchUtils.AddVertexWithEnv_Light(bars, drawPos, new Vector3(new Vector2(200, 1800) / texture.Size(), 0), false);
				}
				if (bars.Count > 2)
				{
					SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

					Main.graphics.GraphicsDevice.Textures[0] = texture;
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(sBS);
				}
				return false;
			}
		}
		if (NPCTextureState == (int)TextureState.ProjRelease)
		{
			texture = ModAsset.VampireMat_ReleaseProj.Value;
		}
		var frame = NPC.frame;
		var rotation = NPC.rotation;
		var spriteEffect = NPC.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		spriteBatch.Draw(texture, drawPos, frame, drawColor, rotation, frame.Size() / 2, 0.8f, spriteEffect, 0);

		// if (BodyRope is not null)
		// {
		// Texture2D point = Commons.ModAsset.TileBlock.Value;
		// foreach (var mass in BodyRope.Masses)
		// {
		// spriteBatch.Draw(point, mass.Position - Main.screenPosition, null, Color.White, 0, point.Size() * 0.5f, 0.5f, SpriteEffects.None, 0);
		// }
		// }
		return false;
	}
}