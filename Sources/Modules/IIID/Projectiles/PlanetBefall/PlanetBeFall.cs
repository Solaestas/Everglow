using Everglow.Commons.IIID;
using Everglow.Commons.Utilities;
using Everglow.IIID.Projectiles.NonIIIDProj.GoldenCrack;
using Everglow.IIID.Projectiles.NonIIIDProj.PlanetBefallArray;
using Everglow.IIID.Projectiles.NonIIIDProj.PlanetBefallExplosion;
using Everglow.IIID.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.IIID.Projectiles.PlanetBefall
{

	public class PlanetBeFall : IIIDProj
	{
		public Vector2 target;
		public Vector2 spawnposition;
		public int Array;

		public override void SetDef()
		{
			model = ObjReader.LoadFile("Everglow/IIID/Projectiles/PlanetBefall/PlanetBefall.obj");
			IIIDTexture = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallTexture").Value;
			NormalTexture = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallTexture").Value;
			MaterialTexture = TextureAssets.MagicPixel.Value;
			EmissionTexture = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallEmission").Value;
			bloom = new BloomParams
			{
				BlurIntensity = 1.0f,
				BlurRadius = 1.0f
			};
			artParameters = new ArtParameters
			{
				EnablePixelArt = true,
				EnableOuterEdge = true,
			};
			viewProjectionParams = new ViewProjectionParams
			{
				ViewTransform = Matrix.Identity,
				FieldOfView = MathF.PI / 3f,
				AspectRatio = 1.0f,
				ZNear = 1f,
				ZFar = 1200f
			};
		}

		public override Matrix ModelMovementMatrix()
		{
			var t = new Vector3(5, -50, 5000 - s);
			return
			     Matrix.CreateScale((float)1000/RenderTargetSize)
				*Matrix.CreateRotationX((float)Main.timeForVisualEffects * 0.01f)
				* Matrix.CreateRotationZ((float)Main.timeForVisualEffects * 0.01f)
				* Matrix.CreateTranslation(t)
				* Matrix.CreateLookAt(new Vector3((Projectile.Center.X - lookat.X) / -1f, (Projectile.Center.Y - lookat.Y) / -1f, 0)*rate,
									 new Vector3((Projectile.Center.X - lookat.X) / -1f, (Projectile.Center.Y - lookat.Y) / -1f, 500) * rate,
									 new Vector3(0, -1, 0) * rate)
				* Main.GameViewMatrix.ZoomMatrix
				* Matrix.CreateTranslation(new Vector3(-Main.GameViewMatrix.TransformationMatrix.M41, -Main.GameViewMatrix.TransformationMatrix.M42, 0));
			;

		}
		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			Array = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<PlanetBefallArray>(), 0, 0, player.whoAmI);
			Main.projectile[Array].Center = Main.MouseWorld;
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active && proj.type == ModContent.ProjectileType<PlanetBefallArray>() && proj == Main.projectile[Array])
				{
					(proj.ModProjectile as PlanetBefallArray).PlanetBeFallProj = Projectile.whoAmI;
				}
			}
			Projectile.ai[0] = Main.projectile[Array].Center.X;
			Projectile.ai[1] = Main.projectile[Array].Center.Y;
			Projectile.velocity = Vector2.Normalize(Main.projectile[Array].Center - Projectile.Center) / 10;

			for (int i = 0; i < 16; i++)
			{
				Vector2 v = new Vector2(0.001f, 0);
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, v.RotatedBy(Math.PI * i / 8).RotatedByRandom(Math.PI * i / 100), ModContent.ProjectileType<GoldenCrack>(), 10, 0);
			}

			PlanetBeFallScreenMovePlayer PlanetBeFallScreenMovePlayer = player.GetModPlayer<PlanetBeFallScreenMovePlayer>();
			PlanetBeFallScreenMovePlayer.PlanetBeFallAnimation = true;
			PlanetBeFallScreenMovePlayer.proj = Projectile;

			target = new Vector2(Projectile.ai[0], Projectile.ai[1]);

			base.OnSpawn(source);
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			target = new Vector2(Projectile.ai[0], Projectile.ai[1]);

			if ((Projectile.Center - target).Length() < 10)
			{
				Projectile.Kill();
			}
			if (Projectile.timeLeft < 1170)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 3f)).RotatedByRandom(MathHelper.TwoPi);
				var somg = new Spark_RockCrackDust
				{
					velocity = newVelocity + Projectile.velocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + newVelocity * 22 + new Vector2(0, Main.rand.NextFloat(0f, 200f)).RotatedByRandom(MathHelper.TwoPi) - Projectile.velocity * 5 + new Vector2(0, -100),
					maxTime = Main.rand.Next(160, 250),
					scale = Main.rand.NextFloat(30f, 146f),
					ai = new float[] { 0, 0 }
				};
				Ins.VFXManager.Add(somg);
				if (Projectile.velocity.Length() < 12.5f)
				{
					Projectile.velocity *= 1.1f;
				}
				if (s < 3500)
					s = MathUtils.Lerp(0.05f, s, 3500);
			}
			player.heldProj = Projectile.whoAmI;
		}

		public override void OnKill(int timeLeft)
		{

			Player player = Main.player[Projectile.owner];
			PlanetBeFallScreenMovePlayer PlanetBeFallScreenMovePlayer = player.GetModPlayer<PlanetBeFallScreenMovePlayer>();
			PlanetBeFallScreenMovePlayer.PlanetBeFallAnimation = false;
			PlanetBeFallScreenMovePlayer.proj = null;
			PlanetBeFallScreenMovePlayer.AnimationTimer = 0;

			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center - new Vector2(0, 100), Vector2.zeroVector, ModContent.ProjectileType<PlanetBefallExplosion>(), (int)(Projectile.damage * (100) / 100f), Projectile.knockBack * 0.4f, Projectile.owner, 60);
			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, player.Center);
			ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
			Gsplayer.FlyCamPosition = new Vector2(0, 150).RotatedByRandom(6.283);
			base.OnKill(timeLeft);
		}

		//public ObjReader.Model model = ObjReader.LoadFile("Everglow/IIID/Projectiles/PlanetBefall/PlanetBefall.obj");

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			//overPlayers.Add(index);
		}
		float s = 0;

		public class TestProjModelSystem : ModSystem
		{
			public override void OnModLoad()
			{
				//PlanetBeFall.model = ObjReader.LoadFile("Everglow/IIID/Projectiles/PlanetBefall/PlanetBefall.obj");
				//PlanetBeFall.NormalMap = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/PlanetBefall/PlanetBeFallTexture");
				base.OnModLoad();
			}

		}
		public class PlanetBeFallScreenMovePlayer : ModPlayer
		{
			public int AnimationTimer = 0;
			public bool PlanetBeFallAnimation = false;
			public Projectile proj;
			const float MaxTime = 135;
			public override void ModifyScreenPosition()
			{
				Vector2 target;
				if (proj != null)
				{
					if (proj.owner == Player.whoAmI)
					{
						target = proj.Center - Main.ScreenSize.ToVector2() / 2;
						if (PlanetBeFallAnimation)
						{
							Player.immune = true;
							Player.immuneTime = 60;
							AnimationTimer += 1;
							float Value = (1 - MathF.Cos((AnimationTimer) * MathF.PI / 45)) / 2f;
							if (AnimationTimer >= 45 && AnimationTimer < 90)
							{
								Value = 1;
							}
							if (AnimationTimer >= 90)
							{
								Value = (1 + MathF.Cos((AnimationTimer - 90) * MathF.PI / 45)) / 2f;
							}

							if (AnimationTimer >= MaxTime)
							{
								AnimationTimer = (int)MaxTime;
								PlanetBeFallAnimation = false;
							}

							Main.screenPosition = (Value).Lerp(Main.screenPosition, target);
						}
					}
				}
			}
		}
	}
}
