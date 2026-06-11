using Everglow.Commons.Coroutines;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Mechanics.Miscs;
using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Enemies;
using Everglow.Yggdrasil.KelpCurtain.VFXs.VampireMat;
using Everglow.Yggdrasil.WorldGeneration;
using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.NPCs.VampireMat;

[AutoloadBossHead]
[NoGameModeScale]
public class VampireMat : ModNPC
{
	public CoroutineManager AICoroutine = new CoroutineManager();

	public Rope BodyRope;

	public MassSpringContainer EularSys = new MassSpringContainer();

	public Vector2 RealCenter;

	public bool DiveAtBackground = false;

	public enum TextureState
	{
		Flat,
		TowardScreen,
		ProjRelease,
	}

	public int NPCTextureState = 0;

	public int Phase = 1;

	public int HitTimer = 0;

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
		BodyRope = Rope.Create_Vine(NPC.Center, 20, 1, 1, 17.3f);
		EularSys.AddMassSpringMesh(BodyRope);
		GlobalRopeSystem.EulerContainers.Add(EularSys);
		AICoroutine.StartCoroutine(new Coroutine(Dash_0()));
		RealCenter = NPC.Center;
	}

	public override bool CheckActive()
	{
		return false;
	}

	public override void AI()
	{
		AICoroutine.Update();
		RealCenter += NPC.velocity;
		BodyRope.Masses[0].Position = RealCenter;
		BodyRope.ApplyForce_VelocityDecay(0.2f);
		Rectangle hitBox = GetAABBBound(BodyRope);
		NPC.position = hitBox.TopLeft();
		NPC.width = hitBox.Width;
		NPC.height = hitBox.Height;
		if (Phase == 1 && NPC.life < NPC.lifeMax * 0.3f)
		{
			Phase = 2;
		}
		if (HitTimer > 0)
		{
			HitTimer--;
		}
	}

	public Rectangle GetAABBBound(Rope rope)
	{
		int min_x = int.MaxValue;
		int max_x = 0;
		int min_y = int.MaxValue;
		int max_y = 0;
		foreach (var mass in rope.Masses)
		{
			int mass_x = (int)mass.Position.X;
			int mass_y = (int)mass.Position.Y;
			if (mass_x < min_x)
			{
				min_x = mass_x;
			}
			if (mass_x > max_x)
			{
				max_x = mass_x;
			}
			if (mass_y < min_y)
			{
				min_y = mass_y;
			}
			if (mass_y > max_y)
			{
				max_y = mass_y;
			}
		}
		int width = max_x - min_x;
		int height = max_y - min_y;
		return new Rectangle(min_x, min_y, width, height);
	}

	#region skills

	public IEnumerator<ICoroutineInstruction> Dash_0()
	{
		yield return new WaitUntil(() => NPC.target >= 0);
		bool reachTarget = false;
		float rot = NPC.rotation;
		int reachTimer = 0;
		for (int k = 0; k < 90; k++)
		{
			Vector2 headPos = RealCenter;
			Player player = Main.player[NPC.target];
			int direction = RealCenter.X > player.Center.X ? 1 : -1;

			NPC.spriteDirection = direction;
			Vector2 toTarget = player.Center - headPos;
			if (!reachTarget && toTarget.Length() < 30)
			{
				reachTarget = true;
				reachTimer = 0;
			}
			if (!reachTarget)
			{
				toTarget = toTarget.SafeNormalize(Vector2.Zero) * 11f;
				rot = toTarget.ToRotation();
				NPC.velocity = toTarget;
			}
			else
			{
				reachTimer++;
			}
			if (reachTimer >= 45)
			{
				reachTarget = false;
			}
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

	public IEnumerator<ICoroutineInstruction> Dash_1()
	{
		yield return new WaitUntil(() => NPC.target >= 0);
		for (int k = 0; k <= 180; k++)
		{
			if (k % 30 == 0)
			{
				Vector2 headPos = RealCenter;
				Player player = Main.player[NPC.target];
				int direction = RealCenter.X > player.Center.X ? 1 : -1;
				NPC.spriteDirection = direction;
				Vector2 toTarget = player.Center - headPos;
				toTarget = toTarget.SafeNormalize(Vector2.Zero) * 31f;
				NPC.velocity = toTarget;
			}
			else
			{
				NPC.velocity *= 0.96f;
			}

			yield return new SkipThisFrame();
		}
		for (int k = 0; k < 10; k++)
		{
			NPC.velocity *= 0.96f;
			yield return new SkipThisFrame();
		}
		AICoroutine.StartCoroutine(new Coroutine(NextAttack()));
	}

	public IEnumerator<ICoroutineInstruction> Escape()
	{
		for (int k = 0; k < 9999; k++)
		{
			if (NPC.target >= 0)
			{
				Player player = Main.player[NPC.target];
				if ((player.Center - KelpCurtainGeneration.VampireMatCaveCenter).Length() < 60 * 16)
				{
					AICoroutine.StartCoroutine(new Coroutine(NextAttack()));
					yield break;
				}
			}
			NPC.velocity *= 0.95f;
			NPC.velocity.Y += 1;
			yield return new SkipThisFrame();
		}
	}

	public IEnumerator<ICoroutineInstruction> GoBehiveBackground()
	{
		float speed = 0f;
		for (int k = 0; k < 99999; k++)
		{
			speed += 0.2f;
			if (speed >= 30)
			{
				speed = 30;
			}
			Vector2 toCenter = KelpCurtainGeneration.VampireMatCaveCenter - RealCenter;
			NPC.velocity = toCenter.SafeNormalize(Vector2.Zero) * speed * 0.25f + NPC.velocity * 0.75f;
			if (toCenter.Length() < speed * 1.5f)
			{
				DiveAtBackground = true;
				AICoroutine.StartCoroutine(new Coroutine(NextAttack()));
				yield break;
			}
			yield return new SkipThisFrame();
		}
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
		if (!PlayerInCave())
		{
			AICoroutine.StartCoroutine(new Coroutine(Escape()));
			yield break;
		}
		if (NPC.target >= 0)
		{
			Player player = Main.player[NPC.target];
			if ((player.Center - RealCenter).Length() > 600)
			{
				AICoroutine.StartCoroutine(new Coroutine(Dash_0()));
				yield break;
			}
		}
		if (Phase == 2 && !DiveAtBackground)
		{
			AICoroutine.StartCoroutine(new Coroutine(GoBehiveBackground()));
			yield break;
		}

		// Main.rand.Next(3)
		switch (Main.rand.Next(3))
		{
			case 0:
				AICoroutine.StartCoroutine(new Coroutine(Dash_1()));
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

	#endregion

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
		HitTimer = 20;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		VampireMatHitCommonEffect(target, info.Damage);
		base.OnHitPlayer(target, info);
	}

	public static void VampireMatHitCommonEffect(Player target, int damage)
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
		NPC owner = NPCUtils.FindNearest(target.Center, ModContent.NPCType<VampireMat>());
		if(owner is not null)
		{
			owner.netUpdate = true;
			int amount = damage * 2;
			owner.HealEffect(amount, true);
			owner.life += amount;
			if (owner.life > owner.lifeMax)
			{
				owner.life = owner.lifeMax;
			}
		}
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return base.SpawnChance(spawnInfo);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (!DiveAtBackground)
		{
			DrawSelf(NPC, this, spriteBatch, drawColor);
		}
		return false;
	}

	public static void DrawSelf(NPC npc, VampireMat vampireMat, SpriteBatch spriteBatch, Color drawColor)
	{
		Effect effect = ModAsset.VampireMat_HitEffect.Value;
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		var texture = ModContent.Request<Texture2D>(npc.ModNPC.Texture).Value;
		var drawPos = vampireMat.RealCenter - Main.screenPosition;
		if (vampireMat.NPCTextureState == (int)TextureState.TowardScreen)
		{
			drawPos = npc.Center - Main.screenPosition;
			texture = ModAsset.VampireMat_Attack.Value;
			if (vampireMat.TowardScreenAndAttacking)
			{
				List<Vertex2D> boss_bottom_bars = new List<Vertex2D>();
				for (int k = 0; k < 20; k++)
				{
					Vector2 offset0 = new Vector2(200, 0).RotatedBy(k / 20f * MathHelper.TwoPi);
					Vector2 offset1 = new Vector2(200, 0).RotatedBy((k + 1) / 20f * MathHelper.TwoPi);
					Vector2 pos0 = drawPos + offset0 * (1 + MathF.Sin(k / 4f * MathHelper.TwoPi + (float)Main.time * 0.09f) * 0.1f);
					Vector2 pos1 = drawPos + offset1 * (1 + MathF.Sin((k + 1) / 4f * MathHelper.TwoPi + (float)Main.time * 0.09f) * 0.1f);
					SpriteBatchUtils.AddVertexWithEnv_Light(boss_bottom_bars, pos0, new Vector3((new Vector2(200, 1800) + offset0) / texture.Size(), 0), false);
					SpriteBatchUtils.AddVertexWithEnv_Light(boss_bottom_bars, pos1, new Vector3((new Vector2(200, 1800) + offset1) / texture.Size(), 0), false);
					SpriteBatchUtils.AddVertexWithEnv_Light(boss_bottom_bars, drawPos, new Vector3(new Vector2(200, 1800) / texture.Size(), 0), false);
				}
				if (boss_bottom_bars.Count > 2)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

					Main.graphics.GraphicsDevice.Textures[0] = texture;
					Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, boss_bottom_bars.ToArray(), 0, boss_bottom_bars.Count / 3);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(sBS);

					if (vampireMat.HitTimer > 0)
					{
						Main.spriteBatch.End();
						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

						effect.Parameters["hitTimer"].SetValue(vampireMat.HitTimer);
						effect.CurrentTechnique.Passes[0].Apply();

						Main.graphics.GraphicsDevice.Textures[0] = texture;
						Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, boss_bottom_bars.ToArray(), 0, boss_bottom_bars.Count / 3);

						Main.spriteBatch.End();
						Main.spriteBatch.Begin(sBS);
					}
				}
				return;
			}
		}
		if (vampireMat.NPCTextureState == (int)TextureState.ProjRelease)
		{
			drawPos = npc.Center - Main.screenPosition;
			texture = ModAsset.VampireMat_ReleaseProj.Value;
		}

		var frame = npc.frame;
		if (vampireMat.NPCTextureState == (int)TextureState.Flat)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			if (vampireMat.BodyRope is null)
			{
				spriteBatch.End();
				spriteBatch.Begin(sBS);
				return;
			}
			List<Vertex2D> bars = new List<Vertex2D>();
			for (int k = 1; k < vampireMat.BodyRope.Masses.Length; k++)
			{
				Vector2 dir = vampireMat.BodyRope.Masses[k].Position - vampireMat.BodyRope.Masses[k - 1].Position;
				dir = dir.SafeNormalize(Vector2.Zero);
				Vector2 normal = new Vector2(-dir.Y, dir.X) * 16;
				Vector2 ropePos = vampireMat.BodyRope.Masses[k - 1].Position - Main.screenPosition;
				float value = k / (float)vampireMat.BodyRope.Masses.Length;
				if (npc.velocity.X > 0)
				{
					vampireMat.AddVertex(bars, ropePos + normal, new Vector3(value, (frame.Y + frame.Height * 0.25f) / texture.Height, 0));
					vampireMat.AddVertex(bars, ropePos - normal, new Vector3(value, (frame.Y + frame.Height * 0.75f) / texture.Height, 0));
				}
				else
				{
					vampireMat.AddVertex(bars, ropePos + normal, new Vector3(value, (frame.Y + frame.Height * 0.75f) / texture.Height, 0));
					vampireMat.AddVertex(bars, ropePos - normal, new Vector3(value, (frame.Y + frame.Height * 0.25f) / texture.Height, 0));
				}
			}
			if (bars.Count > 0)
			{
				Main.graphics.graphicsDevice.Textures[0] = texture;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

				if (vampireMat.HitTimer > 0)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

					effect.Parameters["hitTimer"].SetValue(vampireMat.HitTimer);
					effect.CurrentTechnique.Passes[0].Apply();

					Main.graphics.graphicsDevice.Textures[0] = texture;
					Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(sBS);
				}
			}
		}
		else
		{
			var rotation = npc.rotation;
			var spriteEffect = npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			spriteBatch.Draw(texture, drawPos, frame, drawColor, rotation, frame.Size() / 2, 0.8f, spriteEffect, 0);

			if (vampireMat.HitTimer > 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				effect.Parameters["hitTimer"].SetValue(vampireMat.HitTimer);
				effect.CurrentTechnique.Passes[0].Apply();

				spriteBatch.Draw(texture, drawPos, frame, drawColor, rotation, frame.Size() / 2, 0.8f, spriteEffect, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(sBS);
			}
		}
		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}

	public void AddVertex(List<Vertex2D> bars, Vector2 screenPos, Vector3 coord)
	{
		bars.Add(screenPos, Lighting.GetColor((screenPos + Main.screenPosition).ToTileCoordinates()), coord);
	}

	public bool PlayerInCave()
	{
		if (NPC.target < 0)
		{
			return false;
		}
		Player player = Main.player[NPC.target];
		if ((player.Center - RealCenter).Length() <= 60 * 16)
		{
			return true;
		}
		return false;
	}
}