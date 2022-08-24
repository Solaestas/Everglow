using Everglow.Sources.Modules.PlantModule.Buffs;
using Everglow.Sources.Modules.PlantModule.Common;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Function.Curves;
using Terraria.Audio;

namespace Everglow.Sources.Modules.PlantModule.Projectiles.Melee
{
    public class CactusBallProj : ModProjectile, IWarpProjectile
	{
		public override string Texture=>"Terraria/Images/Projectile_727";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus boulder");
			DisplayName.AddTranslation(PlantUtils.LocaizationChinese, "打你伤害400但是打怪伤害只有40的屑仙人掌球");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			DrawHeldProjInFrontOfHeldItemAndArms = true;
			SetDef();
			trailVecsUp = new Queue<Vector2>(trailLength + 1);
			trailVecsDown = new Queue<Vector2>(trailLength + 1);
		}
		internal int trailLength = 16;
		internal Queue<Vector2> trailVecsUp;
		internal Queue<Vector2> trailVecsDown;
		internal float SpinAcc = 0f;
		internal int SwirlTime = 0;
		public void SetDef()
		{
		}
		
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Vector2 Radial = Projectile.Center;
			Vector2 mainVecUp = Radial;
			Vector2 mainVecDown = Radial * Projectile.scale * 0.25f;

			trailVecsUp.Enqueue(mainVecUp);
			if (trailVecsUp.Count > trailLength)
			{
				trailVecsUp.Dequeue();
			}
			trailVecsDown.Enqueue(mainVecDown);
			if (trailVecsDown.Count > trailLength)
			{
				trailVecsDown.Dequeue();
			}
			if (Projectile.owner == 255)	Projectile.ai[1] = 1f;
			if (Projectile.ai[1] != 0f)
			{
				Projectile.ignoreWater = false;
				Projectile.ai[1] += 1f;
				if (Projectile.ai[1] >= 25f)
				{
					if (Projectile.ai[1] == 25f)	SoundEngine.PlaySound(SoundID.NPCHit11, Projectile.Center);
					Projectile.aiStyle = 25;
					return;
				}
			}
			if(Projectile.ai[1] > 10f)
            {
				Projectile.tileCollide = true;
			}

			if (!player.active || player.dead || player.CCed || !player.channel)
			{
				if (Projectile.ai[1] == 0f)
				{
					Vector2 v0 = Utils.ToRotationVector2(MathHelper.ToRadians(Projectile.ai[0]));
					v0.Y /= 3f;
					Vector2 v1 = player.Center + v0 * 72f;
				    v0 = Utils.ToRotationVector2(MathHelper.ToRadians(Projectile.ai[0] - SpinAcc * player.GetTotalAttackSpeed(DamageClass.Melee) * player.gravDir));
					v0.Y /= 3f;
					Vector2 v2 = player.Center + v0 * 72f;
					Vector2 v3 = Utils.SafeNormalize(v1 - v2, new Vector2(1, 0));
					Vector2 v4 = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, new Vector2(1, 0));
					if((v4 - v3).Length() < 0.1f + SpinAcc * 0.01f)
                    {
						Projectile.ai[1] = 1f;
						if (Projectile.owner == Main.myPlayer)
						{
							Projectile.velocity = Projectile.DirectionToSafe(Main.MouseWorld) * SpinAcc * player.GetTotalAttackSpeed(DamageClass.Melee);
							Projectile.netUpdate2 = true;
							Projectile.netImportant = true;
						}
						return;
					}
				}
			}
			player.direction = Math.Sign(player.DirectionToSafe(Projectile.Center).X);
			player.itemTime = player.itemTimeMax - 1;
			player.SetCompositeArmFront(true, 0, player.AngleToSafe(Projectile.Center) * player.gravDir - MathHelper.PiOver2);
			if (SpinAcc < 17f)
			{
				SpinAcc += 0.05f;
			}
			else
            {
				SwirlTime++;
				if(SwirlTime > 300)
                {
					player.AddBuff(BuffID.Confused,Math.Clamp((SwirlTime - 300) / 6,0, 120));
                }
            }
			if (Projectile.ai[1] == 0f)
			{
				if (Projectile.soundDelay == 0)
				{
					SoundEngine.PlaySound(SoundID.Item1, new Vector2?(Projectile.Center));
					Projectile.soundDelay = (int)(240f / (SpinAcc + 3));
				}
				Projectile.ai[0] += SpinAcc * player.GetTotalAttackSpeed(DamageClass.Melee) * player.gravDir;

				Vector2 vector = Utils.ToRotationVector2(MathHelper.ToRadians(Projectile.ai[0]));
				vector.Y /= 3f;
				Projectile.Center = player.Center + vector * 72f;
				player.fullRotationOrigin = new Vector2(0, 60f);
				player.fullRotation = -vector.X * 0.01f * SpinAcc;
				if (vector.Y > 0f)player.heldProj = Projectile.whoAmI;
				Projectile.velocity = Projectile.DirectionFromSafe(player.Center);
				if(Main.mouseRightRelease)
                {
					player.velocity.X += SpinAcc * Math.Clamp((Main.MouseWorld.X - player.Center.X) / 300f, -1f, 1f) * 0.04f / (player.velocity.Length() + 3);
				}
				Projectile.knockBack = SpinAcc * 0.4f + 1;
			}
            else
            {
				player.fullRotation = 0;

			}

            Projectile.penetrate = 60;
			Vector2 vector2 = player.Center - Projectile.Center;
            Projectile.rotation = (float)(Math.Atan2(vector2.Y, vector2.X) + Math.PI / 2d);
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Projectile.ai[1] <= 10f) damage = (int)(damage / 5d * 0.1f * SpinAcc);
			else damage = (int)(Projectile.damage * 0.1f * SpinAcc);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<CactusBallBuff>(), 150, false);
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<CactusBallBuff>(), 150, true, false);
		}
		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			damage *= 8;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[1] = 60f;
            if (Projectile.velocity.X == oldVelocity.X)
            {
                if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
                if (Math.Abs(Projectile.velocity.Y) > 1.4f)
                {
                    SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        for (int i = 0; i < Main.rand.Next(6, 9); i++)
                        {
                            Vector2 vector = Utils.ToRotationVector2(Main.rand.NextFloat(MathHelper.Pi));
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + vector * 5f, vector * 3f, 
								ModContent.ProjectileType<CactusBallSpike>(), Projectile.damage / 3, 0f, Projectile.owner, 0f, 0f);
                        }
                    }
                }
				return false;
            }
			return true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height = 1;
			return true;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			if (Main.myPlayer == Projectile.owner)
			{
				for (int i = 0; i < Main.rand.Next(8, 12); i++)
				{
					Vector2 vector = Utils.ToRotationVector2(Main.rand.NextFloat(MathHelper.Pi));
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + vector * 5f, vector * 3f, 
						ModContent.ProjectileType<CactusBallSpike>(), Projectile.damage / 3, 0f, Projectile.owner, 0f, 0f);
				}
			}
			Main.player[Projectile.owner].fullRotation = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			DrawTrail(lightColor);
			Color alpha = Projectile.GetAlpha(lightColor);
			Player player = Main.player[Projectile.owner];
			if (Projectile.ai[1] < 25f && (Projectile.Center - player.Center).Length() <= 120)
			{
				Texture2D chain = PlantUtils.GetTexture("Everglow/Sources/Modules/PlantModule/Projectiles/Melee/CactusBallChain");
				Main.spriteBatch.Draw(chain, (Projectile.Center + player.Center) / 2f - Main.screenPosition, null, alpha, Projectile.AngleToSafe(player.Center),
					chain.Size() / 2f, new Vector2((Projectile.Center - player.Center).Length() / 62f, Projectile.scale), 0, 0f);
			}
;
			Texture2D flower = PlantUtils.GetTexture("Everglow/Sources/Modules/PlantModule/Projectiles/Melee/CactusBallFlower");
			float dir = (Projectile.ai[1] < 15f) ? Projectile.AngleFromSafe(player.Center) : Projectile.velocity.ToRotation();
			Main.spriteBatch.Draw(flower, Projectile.Center - dir.ToRotationVector2() * 25f * Projectile.scale - Main.screenPosition, null,
				alpha, dir, flower.Size() / 2f, 1f, 0, 0f);
			Texture2D cactus = PlantUtils.GetTexture(Texture);
			Main.spriteBatch.Draw(cactus, Projectile.Center - Main.screenPosition, null, 
				alpha, Projectile.rotation, cactus.Size() / 2f, Projectile.scale, 0, 0f);
			if (Projectile.ai[1] != 0f)
			{
				int amt = Projectile.oldPos.Length;
				for (int i = 0; i < amt; i++)
				{
					Color color = Color.Lerp(alpha, Color.Transparent, (float)i / amt);
					Main.spriteBatch.Draw(cactus, Projectile.oldPos[i] + Utils.Size(cactus) / 2f - Main.screenPosition, null,
						color, Projectile.rotation, cactus.Size() / 2f, Projectile.scale, 0, 0f);
				}
			}
			return false;
		}
		public virtual string TrailShapeTex()
		{
			return "Everglow/Sources/Modules/MEACModule/Images/Melee";
		}
		public virtual string TrailColorTex()
		{
			return "Everglow/Sources/Modules/MEACModule/Images/TestColor";
		}
		public virtual float TrailAlpha(float factor)
		{
			float w;
			if (factor > 0.5f)
			{
				w = MathHelper.Lerp(0.5f, 0.7f, factor);
			}
			else
			{
				w = MathHelper.Lerp(0f, 0.5f, factor * 2f);
			}

			return w;
		}
		public void DrawTrail(Color color)
		{
			Player player = Main.player[Projectile.owner];
			List<Vector2> SmoothTrailXUp = CatmullRom.SmoothPath(trailVecsUp.ToList());//平滑
			List<Vector2> SmoothTrailUp = new List<Vector2>();
			for (int x = 0; x < SmoothTrailXUp.Count; x++)
			{
				SmoothTrailUp.Add(SmoothTrailXUp[x]);
			}

			int length = SmoothTrailUp.Count;
			if (length <= 3)
			{
				return;
			}
			Vector2[] trailUp = SmoothTrailUp.ToArray();

			List<Vertex2D> bars = new List<Vertex2D>();

			for (int i = 1; i < length; i++)
			{
				float factor = i / (length - 1f);
				float w = TrailAlpha(factor);
				Vector2 Radial = Utils.SafeNormalize(trailUp[i] - trailUp[i - 1], new Vector2(1, 0));
				Radial = Radial.RotatedBy(Math.PI / 2d);
				Color c0 = Lighting.GetColor((int)(trailUp[i].X / 16), (int)(trailUp[i].Y / 16));
				c0.R = (byte)(c0.R * 0.07f * SpinAcc / 15f);
				c0.G = (byte)(c0.G * 0.21f * SpinAcc / 15f);
				c0.B = (byte)(c0.B * 0.07f * SpinAcc / 15f);
				c0.A = 10;
				bars.Add(new Vertex2D(trailUp[i] + Radial * 25f - Main.screenPosition, c0, new Vector3(factor, 1, 0f)));
				bars.Add(new Vertex2D(trailUp[i] - Radial * 15f - Main.screenPosition, c0, new Vector3(factor, 0, w)));
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);

			Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/PlantModule/Projectiles/Melee/CactusBallTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;


			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		public void DrawWarp()
		{
			Player player = Main.player[Projectile.owner];
			List<Vector2> SmoothTrailXUp = CatmullRom.SmoothPath(trailVecsUp.ToList());//平滑
			List<Vector2> SmoothTrailUp = new List<Vector2>();
			for (int x = 0; x < SmoothTrailXUp.Count; x++)
			{
				SmoothTrailUp.Add(SmoothTrailXUp[x]);
			}
			List<Vector2> SmoothTrailDown = new List<Vector2>();
			List<Vector2> SmoothTrailXDown = CatmullRom.SmoothPath(trailVecsDown.ToList());//平滑
			for (int x = 0; x < SmoothTrailXDown.Count; x++)
			{
				SmoothTrailDown.Add(SmoothTrailXDown[x]);
			}
			int length = SmoothTrailUp.Count;
			if (length <= 3)
			{
				return;
			}
			Vector2[] trailUp = SmoothTrailUp.ToArray();

			List<Vertex2D> bars = new List<Vertex2D>();
			for (int i = 1; i < length; i++)
			{
				float factor = i / (length - 1f);
				float w = 1f;
				float d = trailUp[i].ToRotation() + 1.57f;
				float dir = d / MathHelper.TwoPi;
				Vector2 Radial = Utils.SafeNormalize(trailUp[i] - trailUp[i - 1], new Vector2(1, 0));
				Radial = Radial.RotatedBy(Math.PI / 2d);

				bars.Add(new Vertex2D(trailUp[i] + Radial * 25f - Main.screenPosition, new Color(dir * SpinAcc / 15f, w * SpinAcc / 15f, 0, 1), new Vector3(factor, 1, w)));
				bars.Add(new Vertex2D(trailUp[i] - Radial * 15f - Main.screenPosition, new Color(dir * SpinAcc / 15f, w * SpinAcc / 15f, 0, 1), new Vector3(factor, 0, w)));
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Everglow/Sources/Modules/PlantModule/Projectiles/Melee/CactusBallTrail").Value;//扭曲用贴图
			KEx.CurrentTechnique.Passes[0].Apply();
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}
}
