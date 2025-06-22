using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.KelpCurtain.Items.Armors.Ruin;
using Everglow.Yggdrasil.KelpCurtain.VFXs;
using Everglow.Yggdrasil.WorldGeneration;
using Terraria.Audio;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WoodlandWraithStaff_FungiBall : ModProjectile
{
	public enum States
	{
		FlyAround,
		Mycelume,
		Attack,
	}

	public enum AttackCases
	{
		Dash,
		RedLiquid,
	}

	public const float DashAttackDamageMultiplier = 2.4f;
	public const float AggroRange = 450f;
	public const float ChaseRange = 600f;
	public const int MyceliumAmountMax = 256;
	private const int PredictMyceliumPosCheckDistance = 30;

	private Player Owner => Main.player[Projectile.owner];

	private bool OwnerBuffed => Owner.GetModPlayer<RuinMask.RuinSetPlayer>().RuinSetBuffActive;

	private int TargetWhoAmI
	{
		get => (int)Projectile.ai[0];
		set => Projectile.ai[0] = value;
	}

	public NPC Target
	{
		get
		{
			if (Owner.HasMinionAttackTargetNPC && ProjectileUtils.MinionCheckTargetActive(Owner.MinionAttackTargetNPC))
			{
				return Main.npc[Owner.MinionAttackTargetNPC];
			}
			else if (ProjectileUtils.MinionCheckTargetActive(TargetWhoAmI))
			{
				return Main.npc[TargetWhoAmI];
			}
			else
			{
				return null;
			}
		}
	}

	public States State
	{
		get => (States)(int)Projectile.ai[1];

		set => Projectile.ai[1] = (float)value;
	}

	public AttackCases AttackCase
	{
		get => (AttackCases)(int)Projectile.ai[2];

		set => Projectile.ai[2] = (float)value;
	}

	public ref float AttackTimer => ref Projectile.localAI[0];

	public ref float PlayerStopTimer => ref Projectile.localAI[1];

	public ref float MyceliumKillTimer => ref Projectile.localAI[2];

	public List<Point> ContinueTiles = [];

	public int MyceliumAmount = 0;

	public Point RootTileCoord = Point.Zero;

	/// <summary>
	/// The position where the mycelium will be predicted to be placed.
	/// <para/> Defaults to <see cref="Vector2.Zero"/>.
	/// </summary>
	public Vector2 PredictMyceliumPos = Vector2.Zero;

	public bool ReachPredictPos = false;

	public Vector2 PlayerStopPos = Vector2.Zero;

	public float PlayerStartMoveDistance = 0;

	public float Omega = 0;

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 1;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;

		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
		ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
	}

	public override void SetDefaults()
	{
		Projectile.width = 24;
		Projectile.height = 24;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;

		Projectile.minion = true;
		Projectile.minionSlots = 1f;
	}

	public override void AI()
	{
		if (Owner.active && Owner.HasBuff<WoodlandWraithStaffBuff>())
		{
			Owner.AddBuff(ModContent.BuffType<WoodlandWraithStaffBuff>(), 2);
			Projectile.timeLeft = 2;
		}

		UpdateTarget();

		CheckState();
		switch (State)
		{
			case States.FlyAround:
				{
					FlyAroundOwner();
					break;
				}
			case States.Mycelume:
				{
					StopAndBecomeMycelium();
					break;
				}
			case States.Attack:
				{
					AttackEnemy();
					break;
				}
		}
	}

	public override bool? CanHitNPC(NPC target) => AttackTimer > 0 && AttackCase == AttackCases.Dash ? base.CanHitNPC(target) : false;

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		// Deals 240% damage when using dash attack.
		if (AttackTimer > 0 && AttackCase == AttackCases.Dash)
		{
			modifiers.FinalDamage *= 2.4f;
		}
	}

	public void UpdateTarget()
	{
		// Search for target if no target or too far away.
		if (Target == null || Vector2.Distance(Target.Center, Owner.Center) > ChaseRange)
		{
			TargetWhoAmI = ProjectileUtils.FindTarget(Projectile.Center, AggroRange);
		}
	}

	public void CheckState()
	{
		if (State == States.FlyAround)
		{
			if (Owner.IsStandingStillForSpecialEffects)
			{
				PlayerStopTimer++;
				if (PlayerStopTimer > 120)
				{
					PlayerStopPos = Owner.Center;
					SetMyceliumPos();
					State = States.Mycelume;
				}
			}
			else
			{
				PlayerStopTimer = 0;
			}

			if (MyceliumKillTimer > 0)
			{
				MyceliumKillTimer--;
			}
			else
			{
				ContinueTiles = [];
				MyceliumKillTimer = 0;
			}

			if (Target != null)
			{
				State = States.Attack;
			}
		}

		if (State == States.Mycelume)
		{
			MyceliumKillTimer = 60;
			PlayerStartMoveDistance = (Owner.Center - PlayerStopPos).Length();
			var playerToProj = (Owner.Center - Projectile.Center).Length();
			if (PlayerStartMoveDistance > 200 && playerToProj > 200)
			{
				State = States.FlyAround;
				MyceliumAmount = 0;
				ReachPredictPos = false;
			}

			if (Target != null)
			{
				State = States.Attack;
				MyceliumAmount = 0;
				ReachPredictPos = false;
			}
		}

		if (State == States.Attack)
		{
			if (MyceliumKillTimer > 0)
			{
				MyceliumKillTimer--;
			}
			else
			{
				ContinueTiles = [];
				MyceliumKillTimer = 0;
			}

			if (Target == null)
			{
				PlayerStopTimer = 0;
				State = States.FlyAround;
			}
		}
	}

	public void FlyAroundOwner()
	{
		var wanderingPos = Owner.Center + new Vector2(MathF.Sin((float)Main.time * 0.03f + Projectile.whoAmI) * 90, MathF.Sin((float)Main.time * 0.06f + Projectile.whoAmI) * 20 - 40);
		var toWander = wanderingPos - Projectile.Center;
		if (toWander.Length() >= 6)
		{
			toWander = Vector2.Normalize(toWander) * 12f;
		}
		Projectile.velocity = Vector2.Lerp(Projectile.velocity, toWander, 0.05f);
		Projectile.rotation += MathF.Sin((float)Main.time * 0.02f + Projectile.whoAmI) * 0.02f;
		Projectile.rotation *= 0.995f;
	}

	public void AttackEnemy()
	{
		float rot = (float)(Projectile.whoAmI + Main.time * 0.03);
		float distanceHit = 100;
		if (AttackCase == AttackCases.RedLiquid)
		{
			distanceHit = 200;
			rot = (float)(Projectile.whoAmI + Main.time * 0.01);
		}
		var targetPos = Target.Center + new Vector2(distanceHit, 0).RotatedBy(rot);
		int count = 0;
		while (Collision.IsWorldPointSolid(targetPos))
		{
			rot += 1f;
			targetPos = Target.Center + new Vector2(distanceHit, 0).RotatedBy(rot);
			count++;
			if (count > 20)
			{
				break;
			}
		}
		var toTarget = targetPos - Projectile.Center;
		if (AttackTimer == 0)
		{
			if (toTarget.Length() > 16)
			{
				Projectile.velocity = Projectile.velocity * 0.96f + toTarget.NormalizeSafe() * 0.04f * 16;
			}
			else
			{
				Projectile.velocity *= 0.6f;
				if (Projectile.velocity.Length() < 1f)
				{
					AttackTimer = 90;
				}
			}
		}
		if (AttackTimer > 0)
		{
			AttackTimer--;
			if (AttackTimer == 89)
			{
				if (AttackCase == AttackCases.Dash)
				{
					Projectile.velocity = (Target.Center - Projectile.Center).NormalizeSafe() * 24f;
					Omega = Main.rand.NextFloat(-0.3f, 0.3f);
				}
				if (AttackCase == AttackCases.RedLiquid)
				{
					Projectile.velocity *= 0;
				}
			}
			if (AttackCase == AttackCases.Dash)
			{
				Projectile.velocity *= 0.95f;
				if (AttackTimer < 78)
				{
					if (Collision.IsWorldPointSolid(Projectile.Center + new Vector2(Projectile.velocity.X, 0)))
					{
						Projectile.velocity.X *= -0.95f;
						Omega *= 0.75f;
					}
					if (Collision.IsWorldPointSolid(Projectile.Center + new Vector2(0, Projectile.velocity.Y)))
					{
						Projectile.velocity.Y *= -0.95f;
						Omega *= 0.75f;
					}
				}
				Projectile.rotation += Omega;
				Omega *= 0.98f;
			}
			if (AttackCase == AttackCases.RedLiquid)
			{
				if (AttackTimer > 30)
				{
					AttackTimer = 30;
				}
				if (AttackTimer % 6 == 3)
				{
					Vector2 shootTargetVel = Target.Center - Projectile.Center;
					float distance = MathF.Abs(shootTargetVel.X);
					shootTargetVel = shootTargetVel.NormalizeSafe() * 15f;
					int step = (int)(distance / 15);
					float deltaY = 0.5f * 0.5f * step * step;
					shootTargetVel.Y -= deltaY / step;
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, shootTargetVel, ModContent.ProjectileType<WoodlandWraithStaff_BloodStream>(), Projectile.damage, Projectile.knockBack * 0.3f, Projectile.owner);
					SoundEngine.PlaySound(SoundID.Item21, Projectile.Center);
				}
			}
			if (AttackTimer == 0)
			{
				AttackCase = AttackCase.NextEnum();
			}
		}
		else
		{
			AttackTimer = 0;
		}
	}

	public void SetMyceliumPos()
	{
		ReachPredictPos = false;

		// The index of this minion in collection of minions with same type and owner, from 0 to n.
		int minionIndex = Main.projectile
			.Where(proj =>
				proj != null
				&& proj.active
				&& proj != Projectile
				&& proj.type == Projectile.type
				&& proj.owner == Projectile.owner
				&& proj.whoAmI < Projectile.whoAmI)
			.Count();

		float posOffsetX = 480 * (minionIndex % 2 - 0.5f) * (minionIndex / 2f + (minionIndex % 2) / 2f);
		PredictMyceliumPos = Owner.Center + Vector2.UnitX * Owner.direction * posOffsetX;
	}

	public void StopAndBecomeMycelium()
	{
		if (PredictMyceliumPos != Vector2.Zero && !ReachPredictPos)
		{
			Vector2 toPredictMycelium = (PredictMyceliumPos - Projectile.Center).NormalizeSafe() * 6f;
			Projectile.velocity = Projectile.velocity * 0.9f + toPredictMycelium * 0.1f;

			// Check if the projectile is close enough to the predicted mycelium position.
			ReachPredictPos = (Projectile.Center - PredictMyceliumPos).Length() < PredictMyceliumPosCheckDistance;
		}

		if (ReachPredictPos)
		{
			if (Collision.IsWorldPointSolid(Projectile.Center))
			{
				RootTileCoord = Projectile.Center.ToTileCoordinates();
				Projectile.velocity *= 0;
				if (MyceliumAmount == 0)
				{
					Projectile.rotation = YggdrasilWorldGeneration.TerrianSurfaceAngle(RootTileCoord.X, RootTileCoord.Y, 8) - MathHelper.PiOver2;
					var dustVFX = new MyceliumTiles
					{
						Active = true,
						Visible = true,
						position = Projectile.Center,
						RootPos = RootTileCoord,
						maxTime = 1200,
						scale = 1,
						rotation = 0,
						LockProjectile = Projectile,
						FungiBall = this,
						ai = new float[] { 0, 0, 0 },
					};
					Ins.VFXManager.Add(dustVFX);
				}
				if (MyceliumAmount < MyceliumAmountMax)
				{
					MyceliumAmount += 10;
					if (MyceliumAmount >= MyceliumAmountMax)
					{
						MyceliumAmount = MyceliumAmountMax;
					}
					BFSContinueTile(RootTileCoord);
				}
				CheckPlayerInMycelium();
			}
			else
			{
				Projectile.velocity.Y += 0.8f;
				Projectile.velocity *= 0.95f;
			}
		}
	}

	public void CheckPlayerInMycelium()
	{
		var playerFootLeft = (Owner.Bottom + new Vector2(-8, 8)).ToTileCoordinates();
		var playerFootRight = (Owner.Bottom + new Vector2(8, 8)).ToTileCoordinates();
		if (ContinueTiles.Contains(playerFootLeft) || ContinueTiles.Contains(playerFootRight))
		{
			if (Main.rand.NextBool(8))
			{
				Dust dust = Dust.NewDustDirect(Owner.BottomLeft, Owner.width, 15, ModContent.DustType<WoodlandWraithStaff_Spore>());
				dust.velocity *= 0.2f;
			}

			Owner.AddBuff(ModContent.BuffType<WoodlandWraithStaffMyceliumBuff>(), 2);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		// Draw self when not mycelume.
		if (MyceliumAmount <= 0 || State != States.Mycelume)
		{
			var texture = ModContent.Request<Texture2D>(Texture).Value;
			texture = ModAsset.WoodlandWraithStaff_FungiBall_Trail.Value;
			var mainStrength = 0.45f + 0.15f * MathF.Sin((float)Main.timeForVisualEffects * 0.02f);
			var mainColor = Color.Lerp(lightColor, Color.White, mainStrength);
			if (!Main.IsItDay()) // Mix purple in night time.
			{
				mainColor = Color.Lerp(mainColor, Color.MediumPurple, mainStrength);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, mainColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

			// Draw blood glow.
			var texGlow = ModAsset.WoodlandWraithStaff_FungiBall_BloodGlow.Value;
			var glowStrength = 0.7f + 0.1f * MathF.Sin((float)Main.timeForVisualEffects * 0.08f);
			if (OwnerBuffed)
			{
				glowStrength = 0.9f + 0.1f * MathF.Sin((float)Main.timeForVisualEffects * 0.04f);
			}
			var glowColor = Color.Lerp(lightColor, Color.White, glowStrength);
			Main.spriteBatch.Draw(texGlow, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.velocity.ToRotation() - MathHelper.PiOver2, texGlow.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		}

		// Draw mycelium cover.
		float fadeColor = 1 - MyceliumKillTimer / 60f;
		float enhance = 0.5f;
		List<Vertex2D> bars = [];
		foreach (var pos in ContinueTiles)
		{
			DrawSide(bars, pos);
			Vector2 drawPos = pos.ToWorldCoordinates();
			var p0 = drawPos + new Vector2(-8, -8);
			var p1 = drawPos + new Vector2(8, -8);

			var p2 = drawPos + new Vector2(-8, 8);
			var p3 = drawPos + new Vector2(8, 8);

			var color0 = Lighting.GetColor(p0.ToTileCoordinates()) * enhance;
			var color1 = Lighting.GetColor(p1.ToTileCoordinates()) * enhance;
			var color2 = Lighting.GetColor(p2.ToTileCoordinates()) * enhance;
			var color3 = Lighting.GetColor(p3.ToTileCoordinates()) * enhance;

			color0.A = 0;
			color1.A = 0;
			color2.A = 0;
			color3.A = 0;

			bars.Add(p0, color0, new Vector3(p0 * 0.0075f, fadeColor));
			bars.Add(p1, color1, new Vector3(p1 * 0.0075f, fadeColor));
			bars.Add(p3, color3, new Vector3(p3 * 0.0075f, fadeColor));

			bars.Add(p3, color3, new Vector3(p3 * 0.0075f, fadeColor));
			bars.Add(p0, color0, new Vector3(p0 * 0.0075f, fadeColor));
			bars.Add(p2, color2, new Vector3(p2 * 0.0075f, fadeColor));
		}
		if (bars.Count >= 6)
		{
			SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_forceField_medium.Value;
			Main.graphics.GraphicsDevice.Textures[1] = Commons.ModAsset.Noise_cell.Value;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;

			Effect fade = ModAsset.MyceliumFadeEffect.Value;
			fade.Parameters["uTransform"].SetValue(model * projection);
			fade.CurrentTechnique.Passes[0].Apply();

			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(sBS);
		}
		return false;
	}

	public void DrawSide(List<Vertex2D> bars, Point pos)
	{
		(int, int)[] directions =
		{
			(0, 1),
			(1, 0),
			(0, -1),
			(-1, 0),
		};
		float fadeColor = 1 - MyceliumKillTimer / 60f;
		foreach (var (dx, dy) in directions)
		{
			var drawPos = pos.ToWorldCoordinates();
			int checkX = pos.X + dx;
			int checkY = pos.Y + dy;
			Point point = new Point(checkX, checkY);
			float enhance = 2f;
			if (!ContinueTiles.Contains(point))
			{
				drawPos += new Vector2(dx, dy) * 8;

				var p0 = drawPos + (new Vector2(dx, dy) * 8 + new Vector2(dx, dy).RotatedBy(MathHelper.PiOver2)).RotatedBy(MathHelper.PiOver2);
				var p1 = drawPos + (new Vector2(dx, dy) * 8 + new Vector2(dx, dy).RotatedBy(-MathHelper.PiOver2)).RotatedBy(MathHelper.PiOver2);

				var p2 = drawPos + (new Vector2(dx, dy) * 8 + new Vector2(dx, dy).RotatedBy(MathHelper.PiOver2)).RotatedBy(-MathHelper.PiOver2);
				var p3 = drawPos + (new Vector2(dx, dy) * 8 + new Vector2(dx, dy).RotatedBy(-MathHelper.PiOver2)).RotatedBy(-MathHelper.PiOver2);

				var color0 = Lighting.GetColor(p0.ToTileCoordinates()) * enhance;
				var color1 = Lighting.GetColor(p1.ToTileCoordinates()) * enhance;
				var color2 = Lighting.GetColor(p2.ToTileCoordinates()) * enhance;
				var color3 = Lighting.GetColor(p3.ToTileCoordinates()) * enhance;

				color0.A = 0;
				color1.A = 0;
				color2.A = 0;
				color3.A = 0;

				bars.Add(p0, color0, new Vector3(p0 * 0.025f, fadeColor));
				bars.Add(p1, color1, new Vector3(p1 * 0.025f, fadeColor));
				bars.Add(p3, color3, new Vector3(p3 * 0.025f, fadeColor));

				bars.Add(p3, color3, new Vector3(p3 * 0.025f, fadeColor));
				bars.Add(p1, color1, new Vector3(p1 * 0.025f, fadeColor));
				bars.Add(p2, color2, new Vector3(p2 * 0.025f, fadeColor));
			}
		}
	}

	private void BFSContinueTile(Point checkPoint)
	{
		ContinueTiles = new List<Point>();
		int maxContinueCount = MyceliumAmount;
		(int, int)[] directions =
		{
			(0, 1),
			(1, 0),
			(0, -1),
			(-1, 0),
		};
		Queue<Point> queueChecked = new Queue<Point>();

		// 将起始点加入队列
		queueChecked.Enqueue(checkPoint);
		List<Point> visited = new List<Point>();

		while (queueChecked.Count > 0)
		{
			var tilePos = queueChecked.Dequeue();

			foreach (var (dx, dy) in directions)
			{
				int checkX = tilePos.X + dx;
				int checkY = tilePos.Y + dy;
				Point point = new Point(checkX, checkY);
				Tile tile = YggdrasilWorldGeneration.SafeGetTile(checkX, checkY);

				// 检查边界和障碍物
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					tile.HasTile && !visited.Contains(point))
				{
					bool canAdd = true;
					foreach (var proj in Main.projectile)
					{
						if (proj != null && proj.active && proj != Projectile && proj.type == Type)
						{
							WoodlandWraithStaff_FungiBall wWSFB = proj.ModProjectile as WoodlandWraithStaff_FungiBall;
							if (wWSFB != null)
							{
								if (wWSFB.ContinueTiles.Contains(point))
								{
									canAdd = false;
									break;
								}
							}
						}
					}
					if (canAdd)
					{
						queueChecked.Enqueue(point);
						visited.Add(point);
					}
				}
			}
			if (queueChecked.Count > maxContinueCount || visited.Count > maxContinueCount)
			{
				break;
			}
		}
		ContinueTiles = visited;
	}
}