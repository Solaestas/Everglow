using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule;
using Terraria.Audio;
using Terraria.DataStructures;
using Everglow.Sources.Modules.MythModule.TheFirefly;
using Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories;

namespace Everglow.MEAC.Projectiles
{
	public class ScaleWingBladeProj : MeleeProj
	{
		FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
		public override void SetDef()
		{
			maxAttackType = 2;
			trailLength = 20;
			shadertype = "Trail";
			Projectile.scale *= 1.1f;
		}
		public override string TrailShapeTex()
		{
			return "Everglow/Sources/Modules/MEACModule/Images/Melee";
		}
		public override string TrailColorTex()
		{
			return "Everglow/Sources/Modules/MEACModule/Images/TestColor";
		}
		public override float TrailAlpha(float factor)
		{
			return base.TrailAlpha(factor) * 1.15f;
		}
		public override BlendState TrailBlendState()
		{
			return BlendState.NonPremultiplied;
		}
		public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor, float HorizontalWidth, float HorizontalHeight, float DrawScale, string GlowPath, double DrawRotation)
		{
			base.DrawSelf(spriteBatch, lightColor, 85, 30, 0.9f, "Projectiles/ScaleWingBladeProjGlow");
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();

			if (attackType == 100)
			{
				damage *= 3;
				knockback *= 3;
				if (Projectile.owner == Player.whoAmI)
				{
					int counts = Main.rand.Next(4, 9);
					for (int i = 0; i < counts; i++)
					{
						Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Main.rand.NextVector2Unit() * Main.rand.Next(6, 13), ModContent.ProjectileType<ButterflyDreamFriendly>(), damage / 4, 0, Main.myPlayer, target.whoAmI);
						proj.netUpdate2 = true;
						proj.CritChance = Projectile.CritChance;
					}
				}
				Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 12, 150)).RotatedByRandom(6.283);
			}
			else
			{
				if (Projectile.owner == Player.whoAmI)
				{
					int counts = Main.rand.Next(1, 3);
					for (int i = 0; i < counts; i++)
					{
						Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Main.rand.NextVector2Unit() * Main.rand.Next(6, 13), ModContent.ProjectileType<ButterflyDreamFriendly>(), damage / 4, 0, Main.myPlayer, target.whoAmI);
						proj.netUpdate2 = true;
						proj.CritChance = Projectile.CritChance;

					}
				}
				Gsplayer.FlyCamPosition = new Vector2(0, Math.Min(target.Hitbox.Width * target.Hitbox.Height / 50, 50)).RotatedByRandom(6.283);
			}
		}
		public override void End()
		{
			Player player = Main.player[Projectile.owner];
			TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
			player.legFrame = new Rectangle(0, 0, player.legFrame.Width, player.legFrame.Height);
			player.fullRotation = 0;
			player.legRotation = 0;
			Tplayer.HeadRotation = 0;
			Tplayer.HideLeg = false;
			player.legPosition = Vector2.Zero;
			Projectile.Kill();
			player.GetModPlayer<MEACPlayer>().isUsingMeleeProj = false;
		}
		public override void Attack()
		{
			Player player = Main.player[Projectile.owner];
			TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
			Tplayer.HideLeg = true;
			useTrail = true;
			float timeMul = 1f - GetMeleeSpeed(player) / 100f;

			Vector2 vToMouse = Main.MouseWorld - player.Top;
			float AddHeadRotation = (float)Math.Atan2(vToMouse.Y, vToMouse.X) + (1 - player.direction) * 1.57f;
			if (player.gravDir == -1)
			{
				if (player.direction == -1)
				{
					if (AddHeadRotation >= 0.57f && AddHeadRotation < 2)
						AddHeadRotation = 0.57f;
				}
				else
				{
					if (AddHeadRotation <= -0.57f)
						AddHeadRotation = -0.57f;
				}
			}
			else
			{
				if (player.direction == -1)
				{
					if (AddHeadRotation >= 2 && AddHeadRotation < 5.71f)
						AddHeadRotation = 5.71f;
				}
				else
				{
					if (AddHeadRotation >= 0.57f)
						AddHeadRotation = 0.57f;
				}
			}

			if (attackType == 0)
			{
				if (timer < 30)//前摇
				{
					useTrail = false;
					LockPlayerDir(player);
					float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
					mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, -1.2f), 0.1f);
					mainVec += Projectile.DirectionFrom(player.Center) * 3;
					Projectile.rotation = mainVec.ToRotation();
				}
				if (timer == 20)
					AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
				if (timer > 30 && timer < 50)
				{
					isAttacking = true;
					Projectile.rotation += Projectile.spriteDirection * 0.25f;
					mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
				}

				if (timer > 50 + 20 * timeMul)
				{
					player.fullRotation = 0;
					player.legRotation = 0;
					NextAttackType();
				}
				else
				{
					float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
					player.fullRotation = BodyRotation;
					player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
					player.legRotation = -BodyRotation;
					player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
					Tplayer.HeadRotation = -BodyRotation;
				}
			}
			if (attackType == 1)
			{
				if (timer < 30)//前摇
				{
					useTrail = false;
					LockPlayerDir(player);
					float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
					mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, +1.2f), 0.1f);
					mainVec += Projectile.DirectionFrom(player.Center) * 3;
					Projectile.rotation = mainVec.ToRotation();
				}
				if (timer == 20)
					AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
				if (timer > 30 && timer < 50)
				{
					isAttacking = true;
					Projectile.rotation += Projectile.spriteDirection * 0.25f;
					mainVec = Vector2Elipse(120, Projectile.rotation, 0.6f);
				}
				if (timer > 50)
					NextAttackType();
				float BodyRotation = (float)Math.Sin((timer - 30) / 40d * Math.PI) * 0.2f * player.direction * player.gravDir;
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation;
			}
			if (attackType == 2)
			{
				float BodyRotation = 0;
				if (timer < 10)//前摇
				{
					useTrail = false;
					LockPlayerDir(player);
					float targetRot = +MathHelper.PiOver2 + player.direction * 0.7f;
					mainVec = Vector2.Lerp(mainVec, targetRot.ToRotationVector2() * 100, 0.15f);
					mainVec += Projectile.DirectionFrom(player.Center) * 3;
					Projectile.rotation = mainVec.ToRotation();

					BodyRotation = (float)Math.Sin((timer - 6) / 8d * Math.PI) * 0.2f * player.direction * player.gravDir;

				}
				if (timer > 10 && timer < 30)
				{
					isAttacking = true;
					Projectile.rotation -= Projectile.spriteDirection * 0.26f;
					mainVec = Projectile.rotation.ToRotationVector2() * 90;

					BodyRotation = -(float)Math.Sin((timer - 22) / 16d * Math.PI) * 0.2f * player.direction * player.gravDir;

				}
				if (timer > 30 && timer < 50)
				{
					isAttacking = true;
					Projectile.rotation += Projectile.spriteDirection * 0.25f;
					mainVec = Projectile.rotation.ToRotationVector2() * 130;

					BodyRotation = (float)Math.Sin((timer - 42) / 16d * Math.PI) * 0.2f * player.direction * player.gravDir;

				}
				if (timer < 50)
				{
					player.fullRotation = BodyRotation;
					player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
					player.legRotation = -BodyRotation;
					player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
					Tplayer.HeadRotation = -BodyRotation;
				}

				if (timer == 1 || timer == 20)
				{
					useTrail = false;
					AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleeSwing"));
					if (Projectile.owner == Main.myPlayer)
					{
						//寻敌
						NPC target = null;
						float maxdis = 1000;
						foreach (NPC npc in Main.npc)
						{
							if (npc.CanBeChasedBy())
							{
								float dis = Vector2.Distance(Projectile.Center, npc.Center);
								if (dis < maxdis)
								{
									maxdis = dis;
									target = npc;
								}
							}
						}
						if (target != null)
						{
							for (int i = Main.rand.Next(2); i < 2; i++)
							{
								var vel = new Vector2(Projectile.spriteDirection * 10, 0);
								Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + vel, vel + Main.rand.NextVector2Unit() * 5, ModContent.ProjectileType<ButterflyDreamFriendly>(), Projectile.damage / 2, 0, Main.myPlayer, target.whoAmI);
								proj.netUpdate2 = true;
								proj.CritChance = Projectile.CritChance;
							}
						}
					}
				}

				if (timer > 55 + 25 * timeMul)
					NextAttackType();
			}
			if (attackType == 100)//右键攻击
			{
				float BodyRotation;
				if (timer < 60)
				{
					CanIgnoreTile = true;
					useTrail = false;
					LockPlayerDir(player);
					float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
					mainVec = Vector2.Lerp(mainVec, Vector2Elipse(100, targetRot, -1.2f), 0.1f);
					mainVec += Projectile.DirectionFrom(player.Center) * 3;
					Projectile.rotation = mainVec.ToRotation();

					Vector2 r = Main.rand.NextVector2Unit();
					float dis = MathHelper.Clamp(60 - timer, 0, 60) * 2;
					Dust d = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.7f, 1.7f));
					d.velocity = -r * 4;
					d.position += Main.rand.NextVector2Unit() * 5;
					d.noGravity = true;

					Dust d2 = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueParticleDark2>(), 0, 0, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
					d2.velocity = -r * 4;
					d2.position += Main.rand.NextVector2Unit() * 5;
					d2.alpha = (int)(d2.scale * 50);

					Dust d3 = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<MothBlue2>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 2f));
					d3.velocity = -r * 4;
					d3.noGravity = true;
				}
				else if (timer < 100)
				{
					useTrail = false;
					LockPlayerDir(player);
					Projectile.ai[0] = GetAngToMouse();
					float targetRot = -MathHelper.PiOver2 - player.direction * 0.8f;
					mainVec = Vector2.Lerp(mainVec, Vector2Elipse(110, targetRot, -1.2f, Projectile.ai[0], 1000), 0.1f);
					Projectile.rotation = mainVec.ToRotation();

					Vector2 r = Main.rand.NextVector2Unit();
					float dis = 0;
					Dust d = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.7f, 1.7f));
					d.velocity = -r * 4;
					d.position += Main.rand.NextVector2Unit() * 5;
					d.noGravity = true;

					Dust d2 = Dust.NewDustDirect(Projectile.Center + r * dis, 10, 10, ModContent.DustType<BlueParticleDark2>(), 0, 0, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
					d2.velocity = -r * 4;
					d2.position += Main.rand.NextVector2Unit() * 5;
					d2.alpha = (int)(d2.scale * 50);

					Dust d3 = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<MothBlue2>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 2f));
					d3.velocity = -r * 4;
					d3.noGravity = true;
				}
				if (timer == 105)
					AttSound(new SoundStyle("Everglow/Sources/Modules/MEACModule/Sounds/TrueMeleePowerSwing"));
				if (timer > 100)
				{
					isAttacking = true;
					drawScaleFactor = 0.6f;
					if (timer < 115)
					{
						isAttacking = true;
						mainVec = Vector2Elipse(220, Projectile.rotation, -1.2f, Projectile.ai[0], 1000);
						Projectile.rotation += Projectile.spriteDirection * 0.42f;
					}

					BodyRotation = (float)Math.Sin((timer - 114.514) / 18d * Math.PI) * 0.7f * player.direction * player.gravDir;

				}
				else
				{
					Vector2 ToMouseWorld = Main.MouseWorld - player.Top;
					float HeadRot = (float)Math.Atan2(ToMouseWorld.Y, ToMouseWorld.X) + (float)(Math.PI * 0.5 * (1 - player.direction));
					BodyRotation = -timer * player.direction * 0.003f * player.gravDir;
				}
				player.fullRotation = BodyRotation;
				player.fullRotationOrigin = new Vector2(player.Hitbox.Width / 2f, player.gravDir == -1 ? 0 : player.Hitbox.Height);
				player.legRotation = -BodyRotation;
				player.legPosition = (new Vector2(player.Hitbox.Width / 2f, player.Hitbox.Height) - player.fullRotationOrigin).RotatedBy(-BodyRotation);
				Tplayer.HeadRotation = -BodyRotation + AddHeadRotation;

				if (timer == 115)
					Projectile.friendly = false;
				if (timer > 125)
				{
					CanIgnoreTile = false;
					End();
				}
			}

			if (isAttacking)
			{
				for (int i = 0; i < 2; i++)
				{
					Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<BlueGlowAppear>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 2f));
					d.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 3;
					d.noGravity = true;
					Dust d2 = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<MothBlue2>(), 0, 0, 0, default, Main.rand.NextFloat(0.5f, 2f));
					d2.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 3;
					d2.noGravity = true;
				}
				/*for (int i = 0; i < 8; i++)//加上这个有点奇怪，特效过多，光污染了
                {
                    Dust d2 = Dust.NewDustDirect(Projectile.Center - new Vector2(20, 20) + mainVec * Main.rand.NextFloat(0.3f, 1f), 40, 40, ModContent.DustType<BlueParticleDark2>(), 0, 0, 0, default, Main.rand.NextFloat(3.7f, 5.1f));
                    d2.velocity += player.velocity * 0.4f + Main.rand.NextVector2Unit() * 0.3f;
                    d2.alpha = (int)(d2.scale * 50);
                }*/
			}
		}
	}
}