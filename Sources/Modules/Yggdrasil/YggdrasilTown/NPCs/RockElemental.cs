using Everglow.Commons.CustomTiles;
using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

[NoGameModeScale]
public class RockElemental : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 8;
		NPCSpawnManager.RegisterNPC(Type);
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
	}

	public int State;
	public bool Anger = false;
	public Vector2 SuckPoint;
	public Vector2 NextStandPoint;

	public override void SetDefaults()
	{
		NPC.width = 64;
		NPC.height = 64;
		NPC.lifeMax = 400;
		NPC.damage = 30;
		NPC.defense = 12;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.9f;
		NPC.noGravity = true;
		NPC.noTileCollide = false;
		NPC.value = 10000;
		NPC.HitSound = SoundID.NPCHit2;
		NPC.DeathSound = SoundID.NPCDeath2;
		NPC.hide = false;
		NPC.behindTiles = true;
		NPC.scale = 1.2f;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}

	public override void FindFrame(int frameHeight)
	{
		if (State is (int)BehaviorState.SmashDown_1 or (int)BehaviorState.Defense)
		{
			if (NPC.frame.Y < 630 && NPC.frameCounter % 5 == 4)
			{
				NPC.frame.Y += 90;
				NPC.frameCounter = 0;
			}
		}
		else
		{
			NPC.frame.Y = (int)(NPC.frameCounter / 5 % 5) * frameHeight;
		}
	}

	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		YggdrasilTownBiome YggdrasilTownBiome = ModContent.GetInstance<YggdrasilTownBiome>();
		if (!YggdrasilTownBiome.IsBiomeActive(Main.LocalPlayer))
		{
			return 0f;
		}

		return 3f;
	}

	public override void OnSpawn(IEntitySource source)
	{
		NPC.spriteDirection = -1;
		targetVel = new Vector2(Main.rand.NextBool(2) ? 10 : -10, 0);
	}

	public enum BehaviorState
	{
		Fly,
		SmashDown_1,
		Defense,
		Throw,
		Move,
		SmashDown_2,
		Rest,
		Dash,
	}

	private Vector2 targetVel;

	private static Conditions.NotNull _cachedConditions_notNull = new Terraria.WorldBuilding.Conditions.NotNull();
	private static Conditions.IsSolid _cachedConditions_solid = new Terraria.WorldBuilding.Conditions.IsSolid();

	public override void AI()
	{
		Player player = Main.player[NPC.target];
		switch (State)
		{
			// 中立悬浮
			case (int)BehaviorState.Fly:
				{
					Collide();
					NPC.frameCounter++;
					NPC.TargetClosest();
					targetVel = new Vector2(1 * Math.Abs(targetVel.X) / targetVel.X, MathF.Cos((float)Main.time * 0.04f + NPC.whoAmI));
					if (Main.rand.NextBool(500))
					{
						targetVel.X = -targetVel.X;
					}
					NPC.velocity = Vector2.Lerp(NPC.velocity, targetVel, 0.25f);
					NPC.rotation = Math.Clamp(NPC.velocity.X * 0.15f, -1f, 1f);

					if (NPC.HasValidTarget && Anger)
					{
						NPC.localAI[0] = 0;
						State = (int)BehaviorState.SmashDown_1;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, 1);
					}
					foreach (NPC npc in Main.npc)
					{
						if (npc != null && npc.active && npc != NPC)
						{
							if (npc.type == Type)
							{
								Vector2 v0 = NPC.Center - npc.Center;
								if (v0.Length() < 120)
								{
									NPC.velocity += Vector2.Normalize(v0) * 0.125f;
								}
							}
						}
					}
					break;
				}

			// 受击,下落
			case (int)BehaviorState.SmashDown_1:
				{
					NPC.noTileCollide = true;
					NPC.defense = 100;
					NPC.knockBackResist = 0;
					NPC.frameCounter++;
					NPC.TargetClosest();
					NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.1f);
					Point p = NPC.Center.ToTileCoordinates();
					Tile tile = Main.tile[p.X, p.Y + 1];
					if (tile != null && !tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
					{
						State = (int)BehaviorState.Defense;
						NPC.ai[0] = 8;
						NPC.frameCounter = 0;
						for (int i = 0; i < 2; i++)
						{
							Collision.HitTiles(NPC.position, new Vector2(0, 10), NPC.width, NPC.height);
						}
						SoundEngine.PlaySound(SoundID.Item89, NPC.Center);
						ShakerManager.AddShaker(NPC.Center, new Vector2(0, -1), 60, 60, 120);
						for (int g = 0; g < 15; g++)
						{
							Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
							var somg = new RockSmogDust
							{
								velocity = newVelocity,
								Active = true,
								Visible = true,
								position = NPC.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + new Vector2(Main.rand.NextFloat(-72f, 72f), -10),
								maxTime = Main.rand.Next(37, 45),
								scale = Main.rand.NextFloat(40f, 55f),
								rotation = Main.rand.NextFloat(6.283f),
								ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
							};
							Ins.VFXManager.Add(somg);
						}
						Anger = false;
						NPC.velocity = Vector2.Zero;
					}
					if (NPC.velocity.Length() <= 45)
					{
						NPC.velocity += new Vector2(0, 0.63f);
					}

					break;
				}

			// 伪装成石头防御
			case (int)BehaviorState.Defense:
				{
					NPC.noTileCollide = true;
					NPC.defense = 100;
					NPC.knockBackResist = 0;
					NPC.velocity = Vector2.Zero;
					NPC.TargetClosest();
					if (NPC.HasValidTarget && Anger)
					{
						SuckPoint = GetClosestTilePos(player.Center + new Vector2(0, -200));
						if (SuckPoint != Vector2.Zero)
						{
							NextStandPoint = SuckPoint + GetNormalOfTile(SuckPoint) * 150f;
							State = (int)BehaviorState.Throw;
							NPC.ai[0] = 400;
							NPC.frameCounter = 0;
							NPC.velocity = new Vector2(0, -2.5f);
						}
						else
						{
							State = (int)BehaviorState.Dash;
							NPC.ai[0] = 0;
							NPC.frameCounter = 0;
							NPC.velocity = new Vector2(0, -7.5f);
						}
					}
					break;
				}

			// 吸取石块 & 投掷
			case (int)BehaviorState.Throw:
				{
					NPC.defense = 12;
					NPC.noTileCollide = false;
					NPC.ai[0]--;
					NPC.frameCounter++;
					NPC.TargetClosest();
					Vector2 toAim = NextStandPoint - NPC.Center;
					if(SuckPoint == Vector2.zeroVector)
					{
						SuckPoint = GetClosestTilePos(player.Center + new Vector2(0, -200));
						if (SuckPoint != Vector2.Zero)
						{
							NextStandPoint = SuckPoint + GetNormalOfTile(SuckPoint) * 150f;
							State = (int)BehaviorState.Throw;
							NPC.ai[0] = 400;
							NPC.ai[3] = 0;
							if (NPC.rotation > MathHelper.Pi)
							{
								NPC.rotation -= MathHelper.TwoPi;
							}
							if (NPC.rotation < -MathHelper.Pi)
							{
								NPC.rotation += MathHelper.TwoPi;
							}
						}
						else
						{
							State = (int)BehaviorState.Dash;
							NPC.ai[0] = 0;
						}
						return;
					}
					if (toAim.Length() > 50)
					{
						if (NPC.ai[3] > 120)
						{
							NPC.ai[3] = 0;
							SuckPoint = GetClosestTilePos(player.Center + new Vector2(0, -200));
							NextStandPoint = SuckPoint + GetNormalOfTile(SuckPoint) * 150f;
						}
						NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Normalize(toAim) * 3.25f, 0.1f);
						float ro = Math.Clamp(NPC.velocity.X * 0.03f, -1f, 1f);
						NPC.rotation = MathHelper.Lerp(NPC.rotation, ro, 0.05f);
						NPC.ai[0]++;
						NPC.ai[3]++;
					}
					else
					{
						NPC.velocity *= 0.9f;
						if (NPC.ai[0] > 280)
						{
							NPC.rotation = MathHelper.Lerp(NPC.rotation, (NextStandPoint - SuckPoint).ToRotation() + 0.3f, 0.1f);
						}
					}

					// 吸起一个石块
					if (NPC.ai[0] == 380)
					{
						Projectile projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<RockElemental_ThrowingStone>(), 6, 10, -1, NPC.target, NPC.whoAmI);
						RockElemental_ThrowingStone RockElemental_ThrowingStone = projectile.ModProjectile as RockElemental_ThrowingStone;
						if (RockElemental_ThrowingStone != null)
						{
							RockElemental_ThrowingStone.MyOwner = NPC;
						}
					}
					if (NPC.ai[0] <= 260 && NPC.ai[0] > 30)
					{
						if (NPC.ai[2] < 0.5f)
						{
							NPC.ai[2] += 0.002f;
						}
						NPC.rotation -= NPC.ai[2];
						if (NPC.ai[0] == 240)
						{
							SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithPitchOffset(-1f).WithVolume(0.2f), NPC.Center);
						}
						if (NPC.ai[0] == 210)
						{
							SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithPitchOffset(-0.8f).WithVolume(0.22f), NPC.Center);
						}
						if (NPC.ai[0] == 190)
						{
							SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithPitchOffset(-0.4f).WithVolume(0.24f), NPC.Center);
						}
						if (NPC.ai[0] == 175)
						{
							SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithPitchOffset(0f).WithVolume(0.26f), NPC.Center);
						}
						if (NPC.ai[0] == 162)
						{
							SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithPitchOffset(0f).WithVolume(0.28f), NPC.Center);
						}
						if (NPC.ai[0] == 150)
						{
							SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithPitchOffset(0f).WithVolume(0.3f), NPC.Center);
						}
					}
					if (NPC.ai[0] == 29)
					{
						SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot.WithPitchOffset(0f).WithVolume(0.7f), NPC.Center);
						NPC.rotation %= MathHelper.TwoPi;
					}
					if (NPC.ai[0] <= 30)
					{
						NPC.rotation -= NPC.ai[2];
						NPC.ai[2] *= 0.9f;
					}
					if (NPC.ai[0] <= 0)
					{
						SuckPoint = GetClosestTilePos(player.Center + new Vector2(0, -200));
						if (SuckPoint != Vector2.Zero)
						{
							NextStandPoint = SuckPoint + GetNormalOfTile(SuckPoint) * 150f;
							State = (int)BehaviorState.Throw;
							NPC.ai[0] = 400;
							NPC.ai[3] = 0;
							if (NPC.rotation > MathHelper.Pi)
							{
								NPC.rotation -= MathHelper.TwoPi;
							}
							if (NPC.rotation < -MathHelper.Pi)
							{
								NPC.rotation += MathHelper.TwoPi;
							}
						}
						else
						{
							State = (int)BehaviorState.Dash;
							NPC.ai[0] = 0;
						}
					}
					break;
				}

			// 位移
			case (int)BehaviorState.Move:
				{
					NPC.frameCounter++;
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center + new Vector2(0, -200);
					NPC.velocity = Vector2.Lerp(NPC.velocity, MathUtils.NormalizeSafe(toAim) * NPC.velocity.Length(), 0.2f);
					if (NPC.velocity.Length() < 10)
					{
						NPC.velocity *= 1.02f;
					}
					NPC.rotation = MathHelper.Lerp(NPC.rotation, Math.Clamp(NPC.velocity.X * 0.15f, -1f, 1f), 0.1f);
					if (Vector2.Dot(target.Center - NPC.Center, Vector2.UnitY) / (target.Center - NPC.Center).Length() >= 0.96)
					{
						State = (int)BehaviorState.SmashDown_2;
						NPC.ai[0] = 60;
						NPC.frameCounter = 0;
						NPC.velocity = Vector2.Zero;
					}
					if (WorldUtils.Find(target.Center.ToTileCoordinates(), Searches.Chain(new Searches.Up(5), _cachedConditions_notNull, _cachedConditions_solid), out var _))
					{
						State = (int)BehaviorState.Dash;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -1);
					}
					break;
				}

			// 攻击态下砸,产生碎片
			case (int)BehaviorState.SmashDown_2:
				{
					NPC.ai[0]--;
					NPC.noTileCollide = true;
					NPC.knockBackResist = 0;
					NPC.frameCounter++;
					NPC.TargetClosest();
					NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.1f);
					Point p = NPC.Center.ToTileCoordinates();
					Tile tile = Main.tile[p.X, p.Y + 1];
					if (tile.active() && Main.tileSolid[tile.type] && !tile.halfBrick())
					{
						NPC.ai[0] = 60;
						State = (int)BehaviorState.Rest;
						NPC.frameCounter = 0;
						NPC.velocity = Vector2.Zero;
						int num = Main.rand.Next(11, 15);
						for (int i = 1; i <= num; i++)
						{
							Vector2 v = new Vector2(4.5f, 0);
							v = v.RotatedBy(-Math.PI * i / (num - 1)).RotatedByRandom(Math.PI / 20);
							if (v.Y > 0)
							{
								v.Y = -v.Y;
							}
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<RockElemental_Stonefragment>(), 6, 0);
						}
						for (int i = 0; i < 2; i++)
						{
							Collision.HitTiles(NPC.position, new Vector2(0, 10), NPC.width, NPC.height);
						}
						SoundEngine.PlaySound(SoundID.Item89, NPC.Center);
						ShakerManager.AddShaker(NPC.Center, new Vector2(0, -1), 60, 60, 120);
						for (int g = 0; g < 15; g++)
						{
							Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
							var somg = new RockSmogDust
							{
								velocity = newVelocity,
								Active = true,
								Visible = true,
								position = NPC.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + new Vector2(Main.rand.NextFloat(-72f, 72f), -10),
								maxTime = Main.rand.Next(37, 45),
								scale = Main.rand.NextFloat(40f, 55f),
								rotation = Main.rand.NextFloat(6.283f),
								ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
							};
							Ins.VFXManager.Add(somg);
						}
					}
					if (NPC.ai[0] <= 0 && NPC.velocity.Length() <= 45)
					{
						NPC.velocity += new Vector2(0, 1);
					}
					break;
				}

			// 休息
			case (int)BehaviorState.Rest:
				{
					NPC.ai[0]--;
					if (NPC.ai[0] <= 0)
					{
						State = (int)BehaviorState.Dash;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -7.5f);
					}
					break;
				}

			// 冲刺
			case (int)BehaviorState.Dash:
				{
					Collide();
					NPC.noTileCollide = false;
					NPC.ai[0]++;
					NPC.frameCounter++;
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center;

					if (NPC.ai[0] % 120 == 0)
					{
						NPC.velocity = Vector2.Normalize(toAim) * 20.5f;
						NPC.ai[2] = 0.7f * Math.Sign(NPC.velocity.X);
					}
					else
					{
						NPC.rotation += NPC.ai[2];
						NPC.velocity *= 0.96f;
						if (NPC.ai[0] % 120 > 90)
						{
							NPC.ai[2] -= 0.006f * Math.Sign(NPC.velocity.X);
						}
						else
						{
							NPC.ai[2] *= 0.968f;
						}
					}
					if (NPC.ai[0] >= 360)
					{
						SuckPoint = GetClosestTilePos(player.Center + new Vector2(0, -200));
						if (SuckPoint != Vector2.Zero)
						{
							NextStandPoint = SuckPoint + GetNormalOfTile(SuckPoint) * 150f;
							State = (int)BehaviorState.Throw;
							NPC.ai[0] = 400;
							NPC.ai[3] = 0;
							NPC.ai[2] = 0;
							if (NPC.rotation > MathHelper.Pi)
							{
								NPC.rotation -= MathHelper.TwoPi;
							}
							if (NPC.rotation < -MathHelper.Pi)
							{
								NPC.rotation += MathHelper.TwoPi;
							}
						}
					}
					break;
				}
		}

		// 防御状态下受击特效计时器
		if (NPC.ai[1] > 0)
		{
			NPC.ai[1] -= 0.5f;
		}
		else
		{
			NPC.ai[1] = 0f;
		}
	}

	/// <summary>
	/// 碰撞
	/// </summary>
	public void Collide()
	{
		bool collide = false;
		if (Collision.SolidCollision(NPC.position + new Vector2(NPC.velocity.X, 0), NPC.width, NPC.height))
		{
			NPC.velocity.X *= -0.9f;
			NPC.position += NPC.velocity;
		}
		if (Collision.SolidCollision(NPC.position + new Vector2(0, NPC.velocity.Y), NPC.width, NPC.height))
		{
			NPC.velocity.Y *= -0.9f;
			NPC.position += NPC.velocity;
		}
		foreach (NPC npc in Main.npc)
		{
			if (npc != null && npc.active && npc != NPC)
			{
				if (npc.type == Type)
				{
					if (Rectangle.Intersect(npc.Hitbox, NPC.Hitbox) != Rectangle.emptyRectangle)
					{
						npc.velocity += Utils.SafeNormalize(npc.Center - NPC.Center, Vector2.zeroVector) * 4f;
						npc.velocity += NPC.velocity * 0.5f;
						if (npc.velocity.Length() > 20f)
						{
							npc.velocity = npc.velocity / npc.velocity.Length() * 20f;
						}
						npc.position += npc.velocity;
						NPC.velocity -= Utils.SafeNormalize(npc.Center - NPC.Center, Vector2.zeroVector) * 4f;
						NPC.velocity += npc.velocity * 0.5f;
						if (NPC.velocity.Length() > 20f)
						{
							NPC.velocity = NPC.velocity / NPC.velocity.Length() * 20f;
						}
						NPC.position += NPC.velocity;
						collide = true;
					}
				}
			}
		}
		if (collide)
		{
			SoundEngine.PlaySound(SoundID.NPCHit2.WithPitchOffset(0.5f).WithVolumeScale(0.5f), NPC.Center);
		}
	}

	/// <summary>
	/// 寻找最近的一个物块,精度一般
	/// </summary>
	/// <returns></returns>
	public Vector2 GetClosestTilePos(Vector2 pos = default, float maxRange = 500)
	{
		if (pos == default)
		{
			pos = NPC.Center;
		}
		for (float radius = 0; radius < maxRange; radius += 10)
		{
			for (float rot = 0; rot < radius; rot += 10)
			{
				Vector2 v0 = pos + new Vector2(0, -radius).RotatedBy(rot / radius * MathHelper.TwoPi);
				if (Collision.SolidCollision(v0, 0, 0))
				{
					return v0;
				}
			}
		}
		return Vector2.zeroVector;
	}

	/// <summary>
	/// 获取一点附近地形朝外的向量
	/// </summary>
	/// <returns></returns>
	public Vector2 GetNormalOfTile(Vector2 checkPoint)
	{
		Vector2 totalVector = Vector2.Zero;
		int TCount = 0;
		for (int a = 0; a < 12; a++)
		{
			Vector2 v0 = new Vector2(20, 0).RotatedBy(a / 6d * Math.PI);
			if (Collision.SolidCollision(checkPoint + v0, 1, 1))
			{
				totalVector -= v0;
				TCount++;
			}
			else
			{
				totalVector += v0;
			}
		}
		for (int a = 0; a < 24; a++)
		{
			Vector2 v0 = new Vector2(40, 0).RotatedBy(a / 12d * Math.PI);
			if (Collision.SolidCollision(checkPoint + v0, 1, 1))
			{
				totalVector -= v0 * 0.5f;
				TCount++;
			}
			else
			{
				totalVector += v0 * 0.5f;
			}
		}
		return Utils.SafeNormalize(totalVector, Vector2.zeroVector);
	}

	/// <summary>
	/// 统计总共存在的投石数量
	/// </summary>
	/// <returns></returns>
	public int GetMyRockElemental_ThrowingStoneCount()
	{
		int count = 0;
		foreach (Projectile projectile in Main.projectile)
		{
			RockElemental_ThrowingStone RockElemental_ThrowingStone = projectile.ModProjectile as RockElemental_ThrowingStone;
			if (RockElemental_ThrowingStone != null)
			{
				if (RockElemental_ThrowingStone.MyOwner == NPC)
				{
					count++;
				}
			}
		}
		return count;
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		if (!Anger)
		{
			switch (State)
			{
				case (int)BehaviorState.Fly:
					{
						Anger = true;
						NPC.frame.Y = 360;
						NPC.frameCounter = 0;
						break;
					}
				case (int)BehaviorState.SmashDown_1:
					{
						break;
					}
				case (int)BehaviorState.Defense:
					{
						NPC.ai[0]--;
						if (NPC.ai[0] == 0)
						{
							Anger = true;
						}
						NPC.ai[1] = 15f;
						break;
					}
				case (int)BehaviorState.Throw:
					{
						break;
					}
				case (int)BehaviorState.Move:
					{
						break;
					}
				case (int)BehaviorState.SmashDown_2:
					{
						break;
					}
				case (int)BehaviorState.Rest:
					{
						break;
					}
				case (int)BehaviorState.Dash:
					{
						break;
					}
			}
		}

		base.HitEffect(hit);
	}

	public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
	{
		if (State == (int)BehaviorState.Defense)
		{
			return false;
		}
		return base.DrawHealthBar(hbPosition, ref scale, ref position);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D glow = ModAsset.RockElemental_glow.Value;
		Texture2D texture = ModAsset.RockElemental.Value;
		Texture2D hiteffect = ModAsset.RockElemental_defense.Value;
		Vector2 size = glow.Size();
		size.Y /= 8;
		SpriteEffects spriteEffect = SpriteEffects.None;
		if (NPC.spriteDirection == -1)
		{
			spriteEffect = SpriteEffects.FlipHorizontally;
		}
		if (State == (int)BehaviorState.Dash)
		{
			if (NPC.ai[0] % 120 > 90f)
			{
				float value = (NPC.ai[0] % 120f - 90f) / 30f;
				for (int k = 0; k < 8; k++)
				{
					spriteBatch.Draw(hiteffect, NPC.Center - Main.screenPosition + new Vector2(0, 8).RotatedBy(k / 8d * MathHelper.TwoPi), null, new Color(0.4f, 0.2f, 1f, 0f) * value * 0.4f, NPC.rotation, size * 0.5f, NPC.scale * 1.05f, spriteEffect, 0);
				}
				spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);
				spriteBatch.Draw(glow, NPC.Center - Main.screenPosition, NPC.frame, new Color(0.4f, 0.2f, 1f, 0f) * ((value + 0.2f) * 5f), NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);
				return false;
			}
		}
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);
		spriteBatch.Draw(glow, NPC.Center - Main.screenPosition, NPC.frame, new Color(0.4f, 0.2f, 1f, 0f), NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);

		if (NPC.ai[1] > 0)
		{
			SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect dissolve = Commons.ModAsset.DissolveWithLight.Value;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
			float dissolveDuration = NPC.ai[1] / 15f * 1.2f - 0.2f;

			dissolve.Parameters["uTransform"].SetValue(model * projection);
			dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_smoothIce.Value);
			dissolve.Parameters["duration"].SetValue(dissolveDuration);
			dissolve.Parameters["uLightColor"].SetValue(new Vector4(0.3f, 0.1f, 0.4f, 0f));
			dissolve.Parameters["uDissolveColor"].SetValue(Vector4.Lerp(new Vector4(0.4f, 0.8f, 0.9f, 0.5f), new Vector4(0.7f, 0.2f, 0.9f, 0.5f), dissolveDuration));
			dissolve.Parameters["uNoiseSize"].SetValue(2f);
			dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(NPC.whoAmI * 0.04f, NPC.position.X * 0.002f));
			dissolve.CurrentTechnique.Passes[0].Apply();

			spriteBatch.Draw(hiteffect, NPC.Center, null, new Color(0.4f, 0.2f, 1f, 0f), NPC.rotation, new Vector2(45f), NPC.scale, spriteEffect, 0);
			spriteBatch.End();
			spriteBatch.Begin(sBS);
		}
		return false;
	}

	public override void OnKill()
	{
		for (int i = 0; i < 6; i++)
		{
			Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 6f)).RotatedByRandom(MathHelper.TwoPi);
			int type = ModContent.Find<ModGore>("Everglow/RockElemental_gore" + i).Type;
			Gore.NewGore(NPC.GetSource_Death(), NPC.Center, v0, type, NPC.scale);
		}
	}

	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
	}
}