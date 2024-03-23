using Everglow.Common.VFX.CommonVFXDusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.GreenCore.Items.Weapons
{
	public class CharonDoubleSickle : ModProjectile
	{
		private Projectile projectile
		{
			get => Projectile;
		}

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.DrawScreenCheckFluff[projectile.type] += 500;
		}

		public override void SetDefaults()
		{
			projectile.width = 66;
			projectile.height = 68;
			projectile.aiStyle = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 30;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;//
			projectile.penetrate = -1;
			//projectile.scale = 1.2f;
			projectile.extraUpdates = 1;
			oldPos = new Vector2[15];
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.DamageType = DamageClass.MeleeNoSpeed;
			//projectile.hide = true;

		}
		private Vector2[] oldPos;
		private Player player => Main.player[projectile.owner];

		public override void OnSpawn(IEntitySource source)
		{
			projectile.ai[2] = 100;
		}
		public override bool? CanDamage()
		{
			return !(projectile.ai[1] == 0 && Vector2.Distance(projectile.Center, player.Center) < 30);
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (projectile.hide)
				overPlayers.Add(index);
		}
        bool canSpawnHitEff = false;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //命中特效

            if (canSpawnHitEff)
            {
                FogVFX fog = MEACVFX.Create<FogVFX>(projectile.Center, Main.rand.NextVector2Circular(2, 2) + projectile.velocity * 0.1f, 0);
                // fog.substract = true;
                fog.drawColor = new Color(0.15f, 0.3f, 0.2f, 0f);
                fog.SetTimeleft(90);
                fog.scale = 1.5f * projectile.scale;

                fog = MEACVFX.Create<FogVFX>(projectile.Center, Main.rand.NextVector2Circular(2, 2) + projectile.velocity * 0.1f, 0);
                fog.substract = true;
                fog.drawColor = new Color(0.6f, 0.2f, 0.1f, 1f);
                fog.SetTimeleft(70);
                fog.scale = 1.2f * projectile.scale;
                canSpawnHitEff = false;
            }
            SoundStyle sound = SoundID.NPCDeath44;
            sound.Volume = 0.2f;
            sound.Pitch = -0.2f;
            SoundEngine.PlaySound(sound, projectile.Center);//命中音效

        }
        public override void AI()
		{
            canSpawnHitEff = true;

            int speed = 45;//初速度

			float rangeFactor = 0.20f;//偏移角度

			float acc = 0.01f;//回拉加速度

			int prepareTime = 25;//冷却时间

			projectile.rotation = SafeDirectionTo(player, projectile.Center).ToRotation();
			projectile.spriteDirection = 1;
			projectile.ownerHitCheck = true;//不穿墙
			Lighting.AddLight(projectile.Center, new Vector3(0.1f, 0.4f, 0.1f));
			if (player.channel)
			{
				player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, projectile.rotation - 1.5f);
				projectile.timeLeft++;
			}
			else
			{
				projectile.ai[0] = 1;
			}
			for (int i = oldPos.Length - 1; i > 0; i--)
			{
				oldPos[i] = oldPos[i - 1];
			}
			oldPos[0] = projectile.Center;
			if (projectile.ai[0] == 1)
			{
				projectile.velocity *= 0.8f;
				projectile.Center = Vector2.Lerp(projectile.Center, player.Center, 0.5f);
				return;
			}

			if (Main.myPlayer == projectile.owner)
			{
				ref float timer = ref projectile.ai[2];
				if (projectile.ai[1] == 0)//收回
				{
					projectile.velocity *= 0.9f;
					projectile.Center = Vector2.Lerp(projectile.Center, player.Center, 0.28f);
					//if (Vector2.Distance(projectile.Center, player.Center) < 50)
					{
						if (timer++ > prepareTime)//切换到攻击
						{
							if (Main.rand.NextBool(15))//随机的阴间音效
							{
								SoundStyle sound = SoundID.NPCDeath52;
								sound.Volume = 0.15f;
								sound.Pitch = -Main.rand.NextFloat(0, 0.5f);
								SoundEngine.PlaySound(sound, projectile.Center);
							}
							SoundEngine.PlaySound(SoundID.Item71.WithPitchOffset(Main.rand.NextFloat(0, 0.3f)), projectile.Center);//使用音效

							projectile.ai[1] = Main.rand.Next(1, 3);
							timer = 0;
						}
					}
				}
				if (projectile.ai[1] == 1)
				{
					projectile.spriteDirection = 1;
					player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;

					if (timer++ == 0)
					{
						projectile.velocity = player.DirectionTo(Main.MouseWorld).RotatedBy(-rangeFactor) * speed + player.velocity;
					}
					if (timer > 0 && timer < 30)
					{
						projectile.velocity += acc * SafeDirectionTo(projectile, player.Center).RotatedBy(-rangeFactor * 0.7f) * Vector2.Distance(projectile.Center, player.Center);
					}
					if (timer > 30)
					{
						projectile.ai[1] = 0;
						timer = 0;
					}
				}
				if (projectile.ai[1] == 2)
				{
					projectile.spriteDirection = -1;
					player.direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
					if (timer++ == 0)
					{
						projectile.velocity = player.DirectionTo(Main.MouseWorld).RotatedBy(rangeFactor) * speed + player.velocity;
					}
					if (timer > 0 && timer < 30)
					{
						projectile.velocity += acc * SafeDirectionTo(projectile, player.Center).RotatedBy(rangeFactor * 0.7f) * Vector2.Distance(projectile.Center, player.Center);
					}
					if (timer > 30)
					{
						projectile.ai[1] = 0;
						timer = 0;
					}
				}
			}
			if (projectile.ai[1] > 0 && !Main.rand.NextBool(3))//粒子
			{
				Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.TerraBlade);
				d.velocity = projectile.velocity * 0.5f;
				d.noGravity = true;
			}

		}

		public Vector2 SafeDirectionTo(Entity entity, Vector2 t)
		{
			return t - entity.Center != Vector2.Zero ? Vector2.Normalize(t - entity.Center) : Vector2.Zero;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
			float rot = projectile.rotation + 0.78f + (projectile.spriteDirection == 1 ? 0 : 1.57f);

			float chainScale = 1.4f;
			Vector2 chainCenter = projectile.Center + new Vector2(-33, 34).RotatedBy(projectile.rotation + 0.78f) * projectile.scale;
			Vector2 chainEnd = player.Center + projectile.rotation.ToRotationVector2() * 20;
			Vector2 chainVec = Terraria.Utils.SafeNormalize(chainEnd - chainCenter, Vector2.Zero);

			for (float i = 0; i < Vector2.Distance(chainCenter, chainEnd); i += 12 * chainScale)
			{
				Texture2D chain = ModContent.Request<Texture2D>(Texture + "_Chain").Value;
				Vector2 drawPos = chainCenter + chainVec * i;
				Color l = Color.White * 0.8f;
				Main.spriteBatch.Draw(chain, drawPos - Main.screenPosition, null, l, chainVec.ToRotation(), new Vector2(0, chain.Height / 2), chainScale, 0, 0);

			}
			for (int i = 0; i < oldPos.Length; i++)
			{
				Color c = Color.White;
				float alpha = (1 - (float)i / oldPos.Length) * Main.rand.NextFloat(0.0f, 0.35f);
				;
				c.A = 150;
				c *= alpha;
				Main.spriteBatch.Draw(tex, oldPos[i] - Main.screenPosition, null, c, rot, tex.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			}
			Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor, rot, tex.Size() / 2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}

	}
}