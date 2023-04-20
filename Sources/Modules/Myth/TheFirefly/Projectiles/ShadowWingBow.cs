using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Items.Accessories;
using Terraria.Audio;
using Terraria.GameContent;

namespace Everglow.Myth.TheFirefly.Projectiles;

internal class ShadowWingBow : ModProjectile
{
	FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
	public override string Texture => "Everglow/Myth/TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowMain";

	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.penetrate = 2;
		Projectile.timeLeft = 90;
		Projectile.alpha = 255;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Ranged;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
	}

	private int Ran = -1;

	//private int Tokill = -1;
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
	}

	private bool Release = true;

	//private int PdamF = 0;
	private Vector2 oldPo = Vector2.Zero;

	private float[] ArRot = new float[5];
	private float[] ArVel = new float[5];
	private float[] SArVel = new float[5];
	private float[] ArVelφ = new float[5];
	private float[] Arcol = new float[5];
	private int addi = 0;

	public override void AI()
	{
		addi++;
		Player player = Main.player[Projectile.owner];
		player.itemAnimation = 1;
		player.heldProj = Projectile.whoAmI;
		TestPlayerDrawer Tplayer = player.GetModPlayer<TestPlayerDrawer>();
		//玩家动作
		Vector2 vToMouse = Main.MouseWorld - player.Top;
		float AddHeadRotation = (float)Math.Atan2(vToMouse.Y, vToMouse.X) + (1 - player.direction) * 1.57f;
		if (player.gravDir == -1)
		{
			if (player.direction == -1)
			{
				if (AddHeadRotation is >= 0.57f and < 2)
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
				if (AddHeadRotation is >= 2 and < 5.71f)
					AddHeadRotation = 5.71f;
			}
			else
			{
				if (AddHeadRotation >= 0.57f)
					AddHeadRotation = 0.57f;
			}
		}
		Tplayer.HeadRotation = AddHeadRotation;
		if (ArRot[0] == 0)
		{
			ArRot[0] = Main.rand.NextFloat(-0.7f, -0.5f);
			for (int s = 1; s < 4; s++)
			{
				ArRot[s] = Main.rand.NextFloat(-0.5f, 0.5f);
			}
			ArRot[4] = Main.rand.NextFloat(0.5f, 0.7f);
		}
		if (SArVel[0] == 0)
		{
			for (int s = 0; s < 5; s++)
			{
				SArVel[s] = Main.rand.NextFloat(24f, 28f);
			}
		}
		if (ArVelφ[0] == 0)
		{
			for (int s = 0; s < 5; s++)
			{
				ArVelφ[s] = Main.rand.NextFloat(0, 6.283f);
			}
		}
		if (Energy is > 60 and < 120)
		{
			for (int s = 0; s < 5; s++)
			{
				ArRot[s] *= 0.96f;
			}
		}
		for (int s = 0; s < 5; s++)
		{
			ArVel[s] = SArVel[s] + (float)(Math.Sin(ArVelφ[s] + Main.timeForVisualEffects / 20d) * 3);
		}
		for (int s = 0; s < 5; s++)
		{
			Arcol[s] = Math.Clamp((float)(Math.Abs(s - 2.5) * 100 + (Energy - 90) * 7) / 255f, 0, 1f) * 0.6f;
		}
		Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].Center;

		Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 0.25);
		Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * 22f;
		Projectile.velocity *= 0;

		if (player.controlUseItem && Release)
		{
			Projectile.timeLeft = 5 + Energy;
			if (Energy <= 120)
			{
				if (addi % 2 == 1)
				{
					if (MothEye.LocalOwner != null && MothEye.LocalOwner.TryGetModPlayer(out MothEyePlayer mothEyePlayer))
					{
						if (mothEyePlayer.MothEyeEquipped && fireflyBiome.IsBiomeActive(Main.LocalPlayer) && Main.hardMode)
						{
							Energy++;
							Energy++;
						}
						else
							Energy++;
					}

				}
				Energy++;
			}
			else
			{
				Energy = 120;
			}
		}
		if (!Main.mouseLeft && Release)//发射
		{
			SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
			Projectile.NewProjectileDirect(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Normalize(v0) * (Energy + 6) / 9f, (int)Projectile.ai[0], Projectile.damage + Energy / 5, Projectile.knockBack, player.whoAmI).extraUpdates++;
			for (int s = 0; s < 5; s++)
			{
				if (Arcol[s] > 0)
					Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Normalize(v0).RotatedBy(ArRot[s]) * ArVel[s] * 1f, ModContent.ProjectileType<MothArrow>(), (int)((Projectile.damage + Energy / 5) * 0.47), Projectile.knockBack, player.whoAmI, 0, player.HeldItem.crit + player.GetCritChance(DamageClass.Ranged) + player.GetCritChance(DamageClass.Generic));
			}
			Energy = 0;
			Release = false;
		}
		if (Projectile.ai[1] > 0)
			Projectile.ai[1] -= 1f;
		if (!Main.mouseLeft && !Release)
		{
			if (Projectile.ai[1] > 0)
			{
				Projectile.ai[1] -= 1f;
				Projectile.Center = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * 22f;
			}
			else
			{
				Tplayer.HeadRotation = 0;
				if (Projectile.timeLeft > 10)
					Projectile.timeLeft = 10;
			}
		}
		if (Ran == -1)
			Ran = Main.rand.Next(9);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	private int Energy = 0;

	private void DrawString()
	{
		Player player = Main.player[Projectile.owner];
		Vector2 vec = player.DirectionTo(Main.MouseWorld);
		Vector2 v = vec.RotatedBy(1.57f);

		Vector2 basePos = player.MountedCenter + vec * 7 + new Vector2(0, 8);
		float b0 = Math.Clamp(Energy / 2f, 0, 60);
		float b3 = b0 / 60f * (b0 / 60f);

		Vector2 arrowPosition = basePos + vec * (-12f * b3);

		var pos = new Vector2[] { //通过这三点连成弦
                basePos + v * 20 ,
			arrowPosition,
			basePos - v * 20
		};

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var c = new Color(1f, 1f, 1f, 1f);
		var vertices = new List<Vertex2D>();
		for (int i = 0; i < pos.Length; i++)
		{
			vertices.Add(new Vertex2D(pos[i] - Main.screenPosition, c, new Vector3(0, (float)i / pos.Length, 1)));
			vertices.Add(new Vertex2D(pos[i] - Main.screenPosition + vec * 6, c, new Vector3(1, (float)i / pos.Length, 1)));
		}
		Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowString");
		if (vertices.Count > 2)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
			Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
	}

	public override void PostDraw(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		//Texture2D TexString = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowString0");
		Texture2D TexMainU0 = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowU0");
		Texture2D TexMainU1 = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowU1");
		Texture2D TexMainU0G = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowU0Glow");
		Texture2D TexMainU1G = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowU1Glow");
		Texture2D TexMainU2 = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowU2");
		Texture2D TexMainD0 = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowD0");
		Texture2D TexMainD1 = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowD1");
		Texture2D TexMain = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowMain");
		Texture2D TexMainG = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowMainGlow");
		Texture2D TexArrow = TextureAssets.Projectile[(int)Projectile.ai[0]].Value;
		Texture2D TexMothArrow = MythContent.QuickTexture("TheFirefly/Projectiles/MothArrow");
		//float a0 = Energy % 60f;
		//float a1 = (60 - a0) / 60f;
		//float a2 = a1 * 1.5f;
		//float a3 = a2 * a2;
		float b0 = Math.Clamp(Energy / 2f, 0, 60);
		float b1 = b0 / 60f;
		float b2 = b1;
		float b3 = b2 * b2;
		Color drawColor = lightColor;
		SpriteEffects se = SpriteEffects.None;
		if (Projectile.Center.X < player.MountedCenter.X)
			player.direction = -1;
		else
		{
			player.direction = 1;
		}
		if (player.direction == -1)
			se = SpriteEffects.FlipVertically;
		if (player.gravDir == -1)
			se = SpriteEffects.FlipVertically;
		if (player.gravDir == -1 && player.direction == -1)
			se = SpriteEffects.None;
		//int StringFrame = 0;
		//StringFrame = Math.Clamp((int)(Energy / 20f), 0, 5);
		//TexString = MythContent.QuickTexture("TheFirefly/Projectiles/ShadowWingBowTex/ShadowWingBowString" + StringFrame.ToString());
		Vector2 v0 = Main.MouseWorld - player.MountedCenter;
		if (player.controlUseItem)
		{
			Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;
			if (Energy > 30)
				PCAS = Player.CompositeArmStretchAmount.ThreeQuarters;
			if (Energy > 60)
				PCAS = Player.CompositeArmStretchAmount.Quarter;
			if (Energy > 90)
				PCAS = Player.CompositeArmStretchAmount.None;
			player.SetCompositeArmFront(true, PCAS, (float)(Math.Atan2(v0.Y, v0.X) * player.gravDir - Math.PI / 2d));
		}
		Vector2 vProA = Main.player[Projectile.owner].Center + Vector2.Normalize(v0) * (28f - 12f * b3);
		for (int s = 0; s < 5; s++)
		{
			Vector2 vProB = Main.player[Projectile.owner].Center + Vector2.Normalize(v0).RotatedBy(ArRot[s]) * (ArVel[s] * 1.4f - 16f * b3);
			Main.spriteBatch.Draw(TexMothArrow, vProB - Main.screenPosition, null, new Color(Arcol[s], Arcol[s], Arcol[s], 0), Projectile.rotation + ArRot[s], new Vector2(TexMothArrow.Width / 2f, TexMothArrow.Height / 2f), 1f, SpriteEffects.None, 0);
		}
		if (Release)
			Main.spriteBatch.Draw(TexArrow, vProA - Main.screenPosition, new Rectangle(0, 0, TexArrow.Width, TexArrow.Height), drawColor, Projectile.rotation + (float)(Math.PI * 0.25), new Vector2(TexArrow.Width / 2f, TexArrow.Height / 2f), 1f, SpriteEffects.None, 0);

		float rotu0 = Energy / 1200f;
		float rotu1 = Energy / 750f;
		float rotu2 = Energy / 600f;
		float rotd0 = Energy / 1050f;
		float rotd1 = Energy / 720f;
		int ColS = (int)(Energy * 3 / 2f + 50f);
		DrawString();
		Main.spriteBatch.Draw(TexMainU0, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) - rotu0 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainU0G, Projectile.Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Projectile.rotation - (float)(Math.PI * 0.25) - rotu0 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainU1, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) - rotu1 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainU1G, Projectile.Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Projectile.rotation - (float)(Math.PI * 0.25) - rotu1 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainU2, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) - rotu2 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainD0, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + rotd0 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainD1, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + rotd1 * player.direction, new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition, null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25), new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Projectile.rotation - (float)(Math.PI * 0.25), new Vector2(TexMain.Width / 2f, TexMain.Height / 2f), 1f, se, 0);
	}
}