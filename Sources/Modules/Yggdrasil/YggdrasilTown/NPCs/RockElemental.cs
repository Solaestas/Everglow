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
		targetVel = new Vector2(Main.rand.NextBool(2) ? 10 : -10, 0);
	}

	public int State;
	public bool Anger = false;

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
		if (NPC.collideX)
		{
			NPC.velocity.X = NPC.velocity.X * -0.5f;
			if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
			{
				NPC.velocity.X = 2f;
			}

			if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
			{
				NPC.velocity.X = -2f;
			}
		}

		if (NPC.collideY)
		{
			NPC.velocity.Y = NPC.velocity.Y * -0.5f;
			if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
			{
				NPC.velocity.Y = 1f;
			}

			if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
			{
				NPC.velocity.Y = -1f;
			}
		}
		if (NPC.ai[1] > 0)
		{
			NPC.ai[1] -= 0.5f;
		}
		else
		{
			NPC.ai[1] = 0f;
		}
		switch (State)
		{
			// 中立悬浮
			case (int)BehaviorState.Fly:
				{
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
						State = (int)BehaviorState.Throw;
						NPC.ai[0] = 539;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -2.5f);

						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HighSpeedStone>(), 6, 10, -1, NPC.target, NPC.whoAmI);
					}
					break;
				}

			// 投掷
			case (int)BehaviorState.Throw:
				{
					NPC.defense = 12;
					NPC.noTileCollide = false;
					NPC.ai[0]--;
					NPC.frameCounter++;
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center + new Vector2(0, -75);
					if (toAim.Length() > 50)
					{
						NPC.velocity = new Vector2(Vector2.Normalize(toAim).X * 2, Vector2.Normalize(toAim).Y * 4) + new Vector2(MathF.Sin((float)Main.time * 0.03f + NPC.whoAmI), MathF.Cos((float)Main.time * 0.04f + NPC.whoAmI));
						float ro = Math.Clamp(MathF.Abs(toAim.X * (toAim.ToRotation() + MathF.PI / 2)) / toAim.X, -1f, 1f);
						NPC.rotation = MathHelper.Lerp(NPC.rotation, ro, 0.1f);
					}
					else
					{
						NPC.velocity *= 0.9f;
						NPC.rotation = MathHelper.Lerp(NPC.rotation, 0, 0.1f);
					}

					Vector2 dust = new Vector2(0, 75 + Main.rand.Next(50)).RotatedByRandom(Math.PI / 6);

					NPC.rotation = Math.Clamp(NPC.velocity.X * 0.15f, -1f, 1f);
					if (NPC.ai[0] % 180 == 0 && GetMyHighSpeedStoneCount() < 10)
					{
						Projectile projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HighSpeedStone>(), 6, 10, -1, NPC.target, NPC.whoAmI);
						HighSpeedStone highSpeedStone = projectile.ModProjectile as HighSpeedStone;
						if (highSpeedStone != null)
						{
							highSpeedStone.MyOwner = NPC;
						}
					}
					if (NPC.ai[0] <= 0)
					{
						State = (int)BehaviorState.Move;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -1);
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
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<Stonefragment>(), 6, 0);
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
					NPC.noTileCollide = false;
					NPC.ai[0]++;
					NPC.frameCounter++;
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center;

					if (NPC.ai[0] % 60 == 0)
					{
						NPC.velocity = Vector2.Normalize(toAim) * 7.5f;
					}
					else
					{
						NPC.velocity *= 0.98f;
					}
					if (NPC.ai[0] >= 360)
					{
						//State = (int)BehaviorState.Throw;
						//NPC.ai[0] = 539;
						//NPC.frameCounter = 0;
						//NPC.velocity = new Vector2(0, -1);
						//Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HighSpeedStone>(), 6, 10, -1, NPC.target, NPC.whoAmI);
					}
					break;
				}
		}
	}

	public int GetMyHighSpeedStoneCount()
	{
		int count = 0;
		foreach (Projectile projectile in Main.projectile)
		{
			HighSpeedStone highSpeedStone = projectile.ModProjectile as HighSpeedStone;
			if (highSpeedStone != null)
			{
				if (highSpeedStone.MyOwner == NPC)
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
		Vector2 size = glow.Size();
		size.Y /= 8;
		SpriteEffects spriteEffect = SpriteEffects.None;
		if (NPC.spriteDirection == -1)
		{
			spriteEffect = SpriteEffects.FlipHorizontally;
		}
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);
		spriteBatch.Draw(glow, NPC.Center - Main.screenPosition, NPC.frame, new Color(0.4f, 0.2f, 1f, 0f), NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);

		if (NPC.ai[1] > 0)
		{
			Texture2D hiteffect = ModAsset.RockElemental_defense.Value;
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