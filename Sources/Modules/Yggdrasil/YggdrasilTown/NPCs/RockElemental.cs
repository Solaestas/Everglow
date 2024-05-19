using Everglow.Commons.CustomTiles;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

[NoGameModeScale]
public class RockElemental : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 5;
		NPCSpawnManager.RegisterNPC(Type);
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
	}

	public override void SetDefaults()
	{
		NPC.width = 64;
		NPC.height = 64;
		NPC.lifeMax = 250;
		NPC.damage = 30;
		NPC.defense = 12;
		NPC.friendly = false;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.9f;
		NPC.noGravity = true;
		NPC.noTileCollide = false;
		NPC.value = 10000;
		NPC.HitSound = SoundID.NPCHit4;
		NPC.DeathSound = SoundID.NPCDeath4;
		NPC.hide = false;
		NPC.behindTiles = true;
		NPC.scale = 1.2f;
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}

	public override void FindFrame(int frameHeight)
	{
		NPC.frame.Y = (int)(NPC.frameCounter / 5 % 5) * frameHeight;
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

	private int state;
	public bool Anger = false;

	private enum NPCState
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
		switch (state)
		{
			// 飞行
			case (int)NPCState.Fly:
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
						state = (int)NPCState.SmashDown_1;
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

			// 下落
			case (int)NPCState.SmashDown_1:
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
						state = (int)NPCState.Defense;
						NPC.ai[0] = 8;
						NPC.frameCounter = 0;
						NPC.velocity = Vector2.Zero;
						Anger = false;
					}
					if (NPC.velocity.Length() <= 15)
					{
						NPC.velocity += new Vector2(0, 0.163f);
					}

					break;
				}

			// 防御
			case (int)NPCState.Defense:
				{
					NPC.noTileCollide = true;
					NPC.defense = 100;
					NPC.knockBackResist = 0;
					NPC.velocity = Vector2.Zero;
					NPC.TargetClosest();
					if (NPC.HasValidTarget && Anger)
					{
						state = (int)NPCState.Throw;
						NPC.ai[0] = 539;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -2.5f);

						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HighSpeedStone>(), 6, 10, -1, NPC.target, NPC.whoAmI);
					}
					break;
				}

			// 投掷
			case (int)NPCState.Throw:
				{
					NPC.defense = 12;
					NPC.noTileCollide = false;
					NPC.ai[0]--;
					NPC.frameCounter++;
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center + new Vector2(0, -75);
					NPC.velocity = new Vector2(Vector2.Normalize(toAim).X * 2, Vector2.Normalize(toAim).Y * 4) +
					new Vector2(MathF.Sin((float)Main.time * 0.03f + NPC.whoAmI), MathF.Cos((float)Main.time * 0.04f + NPC.whoAmI));
					float ro = Math.Clamp(MathF.Abs(toAim.X * (toAim.ToRotation() + MathF.PI / 2)) / toAim.X, -1f, 1f);
					NPC.rotation = MathHelper.Lerp(NPC.rotation, ro, 0.1f);

					Vector2 dust = new Vector2(0, 75 + Main.rand.Next(50)).RotatedByRandom(Math.PI / 6);

					// Dust d = Dust.NewDustDirect(NPC.Center + dust, 1,1,ModContent.DustType<MagicStone>(), -dust.X / 25, -dust.Y / 25);
					NPC.rotation = Math.Clamp(NPC.velocity.X * 0.15f, -1f, 1f);
					if (NPC.ai[0] % 180 == 0)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HighSpeedStone>(), 6, 10, -1, NPC.target, NPC.whoAmI);
					}
					if (NPC.ai[0] <= 0)
					{
						state = (int)NPCState.Move;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -1);
					}
					break;
				}

			// 位移
			case (int)NPCState.Move:
				{
					if (NPC.velocity.X == 0)
					{
						NPC.velocity.X = Main.rand.NextBool(2) ? 5 : -5;
					}
					if (NPC.velocity.Y == 0)
					{
						NPC.velocity.Y = Main.rand.NextBool(2) ? 5 : -5;
					}
					NPC.frameCounter++;
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center + new Vector2(0, 200);
					if (NPC.velocity.Y == 0 || NPC.velocity.X == 0)
					{
						toAim = -toAim;
					}

					toAim = target.Center - NPC.Center + new Vector2(0, -200);
					NPC.velocity = Vector2.Lerp(NPC.velocity, MathUtils.NormalizeSafe(toAim) * NPC.velocity.Length(), 0.2f);
					if (NPC.velocity.Length() < 10)
					{
						NPC.velocity *= 1.02f;
					}
					NPC.rotation = MathHelper.Lerp(NPC.rotation, Math.Clamp(NPC.velocity.X * 0.15f, -1f, 1f), 0.1f);
					if (Vector2.Dot(target.Center - NPC.Center, Vector2.UnitY) / (target.Center - NPC.Center).Length() >= 0.96)
					{
						state = (int)NPCState.SmashDown_2;
						NPC.ai[0] = 60;
						NPC.frameCounter = 0;
						NPC.velocity = Vector2.Zero;
					}

					if (WorldUtils.Find(target.Center.ToTileCoordinates(), Searches.Chain(new Searches.Up(5), _cachedConditions_notNull, _cachedConditions_solid), out var _))
					{
						state = (int)NPCState.Dash;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -1);
					}

					break;
				}

			// 下砸2
			case (int)NPCState.SmashDown_2:
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
						state = (int)NPCState.Rest;
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
					}
					if (NPC.ai[0] <= 0 && NPC.velocity.Length() <= 15)
					{
						NPC.velocity += new Vector2(0, 1);
					}
					break;
				}

			// 休息
			case (int)NPCState.Rest:
				{
					NPC.ai[0]--;
					if (NPC.ai[0] <= 0)
					{
						state = (int)NPCState.Dash;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -7.5f);
					}
					break;
				}

			// 冲刺
			case (int)NPCState.Dash:
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
						state = (int)NPCState.Throw;
						NPC.ai[0] = 539;
						NPC.frameCounter = 0;
						NPC.velocity = new Vector2(0, -1);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HighSpeedStone>(), 6, 10, -1, NPC.target, NPC.whoAmI);
					}
					break;
				}
		}
	}

	public override void HitEffect(NPC.HitInfo hit)
	{
		if (!Anger)
		{
			switch (state)
			{
				case (int)NPCState.Fly:
					{
						Anger = true;
						break;
					}
				case (int)NPCState.SmashDown_1:
					{
						break;
					}
				case (int)NPCState.Defense:
					{
						NPC.ai[0]--;
						if (NPC.ai[0] == 0)
						{
							Anger = true;
						}
						break;
					}
				case (int)NPCState.Throw:
					{
						break;
					}
				case (int)NPCState.Move:
					{
						break;
					}
				case (int)NPCState.SmashDown_2:
					{
						break;
					}
				case (int)NPCState.Rest:
					{
						break;
					}
				case (int)NPCState.Dash:
					{
						break;
					}
			}
		}

		base.HitEffect(hit);
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D glow = ModAsset.RockElemental_glow.Value;
		Texture2D texture = ModAsset.RockElemental.Value;
		Vector2 size = glow.Size();
		size.Y *= 0.2f;
		SpriteEffects spriteEffect = SpriteEffects.None;
		if (NPC.spriteDirection == -1)
		{
			spriteEffect = SpriteEffects.FlipHorizontally;
		}
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);
		spriteBatch.Draw(glow, NPC.Center - Main.screenPosition, NPC.frame, new Color(0.4f, 0.2f, 1f, 0f), NPC.rotation, size * 0.5f, NPC.scale, spriteEffect, 0);
		return false;
	}
}