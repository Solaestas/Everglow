using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Dusts;
using Everglow.Yggdrasil.KelpCurtain.VFXs;
using Everglow.Yggdrasil.WorldGeneration;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WoodlandWraithStaff_FungiBall : ModProjectile
{
	private Player Owner => Main.player[Projectile.owner];

	public NPC Target;

	public List<Point> ContinueTiles = new List<Point>();

	public int MyceliumAmount = 0;

	public int PlayerStopTimer = 0;

	public int MyceliumKillTimer = 0;

	public Point RootTileCoord = new Point(0, 0);

	public Vector2 PredictMyceliumPos = new Vector2(0, 0);

	public bool ReachPredictPos = false;

	public int DashTimer = 0;

	/// <summary>
	/// 0: Fly Around;1: Mycelume;2: Attack0;3: Attack1
	/// </summary>
	public int State = 0;

	/// <summary>
	/// 0: Dash;1: Red Liquid
	/// </summary>
	public int AttackCase = 0;

	public bool OwnerInMyceliumNet;

	public Vector2 PlayerStopPos = new Vector2(0, 0);

	public float PlayerStartMoveDistance = 0;

	public float Omega = 0;

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 1;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
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
		if (Owner.active)
		{
			Owner.AddBuff(ModContent.BuffType<WoodlandWraithStaffBuff>(), 2);
			Projectile.timeLeft = 2;
		}
		FindEnemy();
		CheckState();
		switch (State)
		{
			case 0:
				{
					FlyAroundOwner();
					break;
				}
			case 1:
				{
					StopAndBecomeMycelium();
					break;
				}
			case 2:
				{
					StrikeEnemy();
					break;
				}
			case 3:
				{
					StrikeEnemy();
					break;
				}
		}
	}

	public override bool? CanHitNPC(NPC target)
	{
		if (DashTimer > 0 && AttackCase == 0)
		{
			return base.CanHitNPC(target);
		}
		else
		{
			return false;
		}
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		if(DashTimer > 0 && AttackCase == 0)
		{
			modifiers.FinalDamage *= 2.4f;
		}
		base.ModifyHitNPC(target, ref modifiers);
	}

	public void CheckState()
	{
		if (State == 0)
		{
			if (Owner.velocity.Length() < 0.05f)
			{
				PlayerStopTimer++;
				if (PlayerStopTimer > 120)
				{
					PlayerStopPos = Owner.Center;
					SetMyceliumPos();
					State = 1;
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
				ContinueTiles = new List<Point>();
				MyceliumKillTimer = 0;
			}
			if (Target != null)
			{
				State = 2;
			}
		}
		if (State == 1)
		{
			MyceliumKillTimer = 60;
			PlayerStartMoveDistance = (Owner.Center - PlayerStopPos).Length();
			var PlayerToProj = (Owner.Center - Projectile.Center).Length();
			if (PlayerStartMoveDistance > 200 && PlayerToProj > 200)
			{
				State = 0;
				MyceliumAmount = 0;
				ReachPredictPos = false;
			}
			if (Target != null)
			{
				State = 2;
				MyceliumAmount = 0;
				ReachPredictPos = false;
			}
		}
		if(State == 2)
		{
			StrikeEnemy();
			if (MyceliumKillTimer > 0)
			{
				MyceliumKillTimer--;
			}
			else
			{
				ContinueTiles = new List<Point>();
				MyceliumKillTimer = 0;
			}
			if (Target == null || !Target.active)
			{
				PlayerStopTimer = 0;
				State = 0;
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

	public void FindEnemy()
	{
		Target = Projectile.FindTargetWithinRange(800);
	}

	public void StrikeEnemy()
	{
		if(Target == null)
		{
			FindEnemy();
			return;
		}
		float rot = (float)(Projectile.whoAmI + Main.time * 0.03);
		float distanceHit = 100;
		if (AttackCase == 1)
		{
			distanceHit = 200;
			rot = (float)(Projectile.whoAmI + Main.time * 0.01);
		}
		var targetPos = Target.Center + new Vector2(distanceHit, 0).RotatedBy(rot);
		int count = 0;
		while(Collision.IsWorldPointSolid(targetPos))
		{
			rot += 1f;
			targetPos = Target.Center + new Vector2(distanceHit, 0).RotatedBy(rot);
			count++;
			if(count > 20)
			{
				break;
			}
		}
		var toTarget = targetPos - Projectile.Center;
		if(DashTimer == 0)
		{
			if (toTarget.Length() > 16)
			{
				Projectile.velocity = Projectile.velocity * 0.96f + toTarget.NormalizeSafe() * 0.04f * 16;
			}
			else
			{
				Projectile.velocity *= 0.6f;
				if(Projectile.velocity.Length() < 1f)
				{
					DashTimer = 90;
				}
			}
		}
		if (DashTimer > 0)
		{
			DashTimer--;
			if(DashTimer == 89)
			{
				if (AttackCase == 0)
				{
					Projectile.velocity = (Target.Center - Projectile.Center).NormalizeSafe() * 24f;
					Omega = Main.rand.NextFloat(-0.3f, 0.3f);
				}
				if (AttackCase == 1)
				{
					Projectile.velocity *= 0;
				}
			}
			if(AttackCase == 0)
			{
				Projectile.velocity *= 0.95f;
				if (DashTimer < 78)
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
			if (AttackCase == 1)
			{
				// Not actually dash, but shoot liquid.
				if (DashTimer > 30)
				{
					DashTimer = 30;
				}
				if (DashTimer % 6 == 3)
				{
					Vector2 shootTargetVel = Target.Center - Projectile.Center;
					float distance = MathF.Abs(shootTargetVel.X);
					shootTargetVel = shootTargetVel.NormalizeSafe() * 15f;
					int step = (int)(distance / 15);
					float deltaY = 0.5f * 0.5f * step * step;
					shootTargetVel.Y -= deltaY / step;
					Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),Projectile.Center,shootTargetVel,ModContent.ProjectileType<WoodlandWraithStaff_BloodStream>(),Projectile.damage,Projectile.knockBack * 0.3f, Projectile.owner);
				}
			}
			if (DashTimer == 0)
			{
				AttackCase = Main.rand.Next(2);
			}
		}
		else
		{
			DashTimer = 0;
		}
	}

	public void SetMyceliumPos()
	{
		int index = 0;
		ReachPredictPos = false;
		foreach (var proj in Main.projectile)
		{
			if (proj != null && proj.active && proj != Projectile && proj.type == Projectile.type && proj.owner == Projectile.owner)
			{
				if (proj.whoAmI < Projectile.whoAmI)
				{
					index++;
				}
			}
		}
		float posAddX = (index % 2 - 0.5f) * 2 * (index / 2f + (index % 2) / 2f);
		PredictMyceliumPos = Owner.Center + new Vector2(240 * posAddX * Owner.direction, 0);
	}

	public void StopAndBecomeMycelium()
	{
		if (!ReachPredictPos && PredictMyceliumPos != new Vector2(0))
		{
			Vector2 toPredictMycelium = (PredictMyceliumPos - Projectile.Center).NormalizeSafe() * 6f;
			Projectile.velocity = Projectile.velocity * 0.9f + toPredictMycelium * 0.1f;
			if ((Projectile.Center - PredictMyceliumPos).Length() < 30)
			{
				ReachPredictPos = true;
			}
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
				if (MyceliumAmount < 256)
				{
					MyceliumAmount += 10;
					if (MyceliumAmount >= 256)
					{
						MyceliumAmount = 256;
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
			OwnerInMyceliumNet = true;
		}
		else
		{
			OwnerInMyceliumNet = false;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		float fadeColor = 1 - MyceliumKillTimer / 60f;
		if (MyceliumAmount <= 0 || State != 1)
		{
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
		}
		float enhance = 0.5f;
		List<Vertex2D> bars = new List<Vertex2D>();
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

	public void BFSContinueTile(Point checkPoint)
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