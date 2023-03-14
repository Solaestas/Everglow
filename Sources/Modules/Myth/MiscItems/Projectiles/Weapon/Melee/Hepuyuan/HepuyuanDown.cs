using Everglow.Myth.Common;
using Everglow.Myth.MiscItems.Gores;
using Terraria.Audio;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Melee.Hepuyuan;

public class HepuyuanDown : ModProjectile, IWarpProjectile
{
	protected virtual float HoldoutRangeMin => 24f;
	protected virtual float HoldoutRangeMax => 150f;

	public override void SetDefaults()
	{
		Projectile.CloneDefaults(ProjectileID.Spear);
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 80;
		Projectile.extraUpdates = 6;
		Projectile.width = 80;
		Projectile.height = 80;
	}

	internal bool Crash = false;
	internal int timer = 0;
	private float addK = 1.0f;
	private bool max = false;
	internal Vector2 FirstVel = Vector2.Zero;
	internal float[] wid = new float[60];
	internal Vector2[] OldplCen = new Vector2[60];
	internal int TrueL = 0;
	internal float[] statrP = new float[4];
	public override bool PreAI()
	{
		Player player = Main.player[Projectile.owner];
		float duration = player.itemAnimationMax * 15f;
		OldplCen[0] = Projectile.Center - Vector2.Normalize(Projectile.velocity) * 15f;
		for (int i = 0; i < 1; i++)
		{
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0.8f, 10f), 0).RotatedByRandom(6.28);
			Vector2 v2 = new Vector2(Main.rand.NextFloat(30f), 0).RotatedByRandom(6.28);
			Dust.NewDust(OldplCen[0] - new Vector2(4) + v2, 1, 1, Main.rand.NextBool(2) ? ModContent.DustType<Dusts.XiaoDustCyan>() : ModContent.DustType<Dusts.XiaoDust>(), (Projectile.velocity * 0.35f + v1).X, (Projectile.velocity * 0.35f + v1).Y, 0, default, Main.rand.NextFloat(0.85f, Main.rand.NextFloat(0.85f, 3.75f)));
		}

		timer++;
		if (timer % 10 == 1 && Projectile.timeLeft > 30)
		{
			var v = Vector2.Normalize(Projectile.velocity);
			int h = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<XiaoBlackWave>(), 0, 0, player.whoAmI, Math.Clamp(Projectile.velocity.Length() / 8f, 0f, 4f), 0);
			Main.projectile[h].rotation = (float)(Math.Atan2(v.Y, v.X) + Math.PI / 2d);
		}
		for (int f = OldplCen.Length - 1; f > 0; f--)
		{
			OldplCen[f] = OldplCen[f - 1];
		}
		wid[0] = Math.Clamp(Projectile.velocity.Length() / 6f, 0, 60);
		for (int f = wid.Length - 1; f > 0; f--)
		{
			wid[f] = wid[f - 1];
		}
		if (player.direction == -Math.Sign(Projectile.velocity.X))
			player.direction *= -1;
		player.heldProj = Projectile.whoAmI;
		if (Projectile.timeLeft > duration)
			Projectile.timeLeft = (int)duration;
		if (Projectile.timeLeft <= 6)
		{
			if (Collision.SolidCollision(player.Top, 1, 1))
				player.position.Y -= 64 * player.gravDir;
		}
		float halfDuration = duration * 0.5f;

		if (Projectile.timeLeft < halfDuration + 2 && !max)
			max = true;

		Projectile.velocity *= 0.995f;

		if (Collision.SolidCollision(Projectile.Center + Projectile.velocity, 1, 1))
		{
			Projectile.velocity *= 0.5f;
			Projectile.timeLeft -= 1;
			Projectile.extraUpdates = 0;
			for (int f = wid.Length - 1; f >= 0; f--)
			{
				wid[f] *= 0.9f;
			}
			if (Projectile.timeLeft % 30 == 1)
			{
				for (int h = 0; h < 18; h++)
				{
					Vector2 v = Projectile.Center - Vector2.Normalize(Projectile.velocity) * 16 * h;
					if (Collision.SolidCollision(v, 1, 1))
						Collision.HitTiles(v, Projectile.velocity * 20, 16, 16);
				}
			}
			if (!Crash)
			{
				for (int f = 0; f < 45; f++)
				{
					Vector2 v0 = new Vector2(Main.rand.NextFloat(0.8f, 3f), 0).RotatedByRandom(6.28);
					float Dx = Main.rand.NextFloat(-250f, 250f);
					Vector2 Pos = OldplCen[0] + new Vector2(Dx, (40 + Main.rand.NextFloat(-10f, 10f)) * player.gravDir);
					for (int i = 0; i < 5; i++)
					{
						if (Collision.SolidCollision(Pos, 1, 1))
							Pos.Y -= 16 * player.gravDir;
					}
					Vector2 Dy = v0 + new Vector2(0, Main.rand.NextFloat(Math.Abs(Dx) - 278, 0) / 12f);
					Dy.Y *= player.gravDir;
					if (Main.rand.NextBool(2))
					{
						var g = Gore.NewGoreDirect(null, Pos, Dy * 2f, ModContent.GoreType<XiaoDash0>(), Main.rand.NextFloat(1f, 4.5f));
						g.timeLeft = Main.rand.Next(250, 600);
					}
					else
					{
						var g = Gore.NewGoreDirect(null, Pos, Dy * 2f, ModContent.GoreType<XiaoDash1>(), Main.rand.NextFloat(1f, 4.5f));
						g.timeLeft = Main.rand.Next(250, 600);
					}
				}
				for (int f = 0; f < 205; f++)
				{
					Vector2 v0 = new Vector2(Main.rand.NextFloat(0.8f, 3f), 0).RotatedByRandom(6.28);
					float Dx = Main.rand.NextFloat(-350f, 350f);
					Vector2 Pos = OldplCen[0] + new Vector2(Dx, (40 + Main.rand.NextFloat(-10f, 10f)) * player.gravDir);
					for (int i = 0; i < 5; i++)
					{
						if (Collision.SolidCollision(Pos, 1, 1))
							Pos.Y -= 16 * player.gravDir;
					}
					Vector2 Dy = v0 + new Vector2(0, Main.rand.NextFloat(Math.Abs(Dx) - 278, 0) / 12f);
					Dy.Y *= player.gravDir;
					Vector2 v2 = new Vector2(Main.rand.NextFloat(30f), 0).RotatedByRandom(6.28);
					Dust.NewDust(Pos - new Vector2(4) + v2, 1, 1, Main.rand.NextBool(2) ? ModContent.DustType<Dusts.XiaoDustCyan>() : ModContent.DustType<Dusts.XiaoDust>(), Dy.X * 2f, Dy.Y * 2f, 0, default, Main.rand.NextFloat(0.85f, Main.rand.NextFloat(0.85f, 5.75f)));
				}
				for (int f = 0; f < 8; f++)
				{
					Vector2 Pos = Projectile.Center + new Vector2((f - 3.5f) * 80, -300 * player.gravDir);
					bool empty = false;
					for (int i = 0; i < 75; i++)
					{
						if (!Collision.SolidCollision(Pos, 1, 1))
						{
							Pos.Y += 8 * player.gravDir;
							empty = true;
						}
						else
						{
							empty = false;
						}
					}
					if (empty)
						continue;
					int h = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Pos, Vector2.Zero, ModContent.ProjectileType<HepuyuanSpice>(), Projectile.damage, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(50f, 110f), 0);
					if (player.gravDir == 1)
						Main.projectile[h].rotation = Main.rand.NextFloat((f - 3.5f) / 15f - 0.3f, (f - 3.5f) / 15f + 0.3f);
					else
					{
						Main.projectile[h].rotation = MathF.PI - Main.rand.NextFloat((f - 3.5f) / 15f - 0.3f, (f - 3.5f) / 15f + 0.3f);
					}
				}

				for (int f = 0; f < 12; f++)
				{
					Vector2 Pos = Projectile.Center + new Vector2((f - 5.5f) * 50, -300 * player.gravDir);
					bool empty = false;
					for (int i = 0; i < 75; i++)
					{
						if (!Collision.SolidCollision(Pos, 1, 1))
						{
							Pos.Y += 8 * player.gravDir;
							empty = true;
						}
						else
						{
							empty = false;
						}
					}
					if (empty)
						continue;
					int h = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Pos, Vector2.Zero, ModContent.ProjectileType<HepuyuanShake>(), 0, Projectile.knockBack, player.whoAmI, Main.rand.NextFloat(17f, 32f) * (Math.Abs(f - 5.5f) + 0.5f), 0);


					if (player.gravDir == 1)
						Main.projectile[h].rotation = Main.rand.NextFloat((f - 5.5f) / 15f - 0.1f + Math.Sign(f - 5.5f) * 0.75f, (f - 5.5f) / 15f + 0.1f + Math.Sign(f - 5.5f) * 0.75f);
					else
					{
						Main.projectile[h].rotation = MathF.PI - Main.rand.NextFloat((f - 5.5f) / 15f - 0.1f + Math.Sign(f - 5.5f) * 0.75f, (f - 5.5f) / 15f + 0.1f + Math.Sign(f - 5.5f) * 0.75f);
					}
				}
				ScreenShaker mplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
				mplayer.FlyCamPosition = new Vector2(0, 84).RotatedByRandom(6.283);
				SoundEngine.PlaySound(new SoundStyle("Everglow/Myth/Sounds/Xiao"), Projectile.Center);
				Crash = true;
			}
		}
		else
		{
			if (Projectile.velocity.Length() < 100)
				Projectile.velocity *= addK;
			if (addK < 1.5f)
				addK += 0.001f;
		}
		if (Projectile.timeLeft > 6 && !Collision.SolidCollision(Projectile.Center, 1, 1))
			player.velocity = Projectile.velocity * 6;
		if (Projectile.timeLeft < 6)
		{
			player.velocity *= 0.4f;
			Projectile.velocity *= 0.4f;
		}
		MythContentPlayer myplayer = player.GetModPlayer<MythContentPlayer>();
		myplayer.InvincibleFrameTime = 15;
		return false;
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		if (FirstVel == Vector2.Zero)
			FirstVel = Vector2.Normalize(Projectile.velocity);
		Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);

		var VxII = new List<Vertex2D>();
		var barsII = new List<Vertex2D>();



		for (int i = 1; i < 60; ++i)
		{
			if (OldplCen[i] == Vector2.Zero)
				break;
			float widk = MathF.Sqrt(i * 15);
			Vector2 DeltaV0 = -OldplCen[i] + OldplCen[i - 1];
			float d = DeltaV0.ToRotation() + 3.14f + 1.57f;
			if (d > 6.28f)
				d -= 6.28f;
			float dir = d / MathHelper.TwoPi;
			var factor = i / 60f;
			var factor2 = i / (float)TrueL;

			var c0 = new Color(dir, MathF.Sin(factor * 3.14159f), 0, 0);
			barsII.Add(new Vertex2D(OldplCen[i] + FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 1, 1 - factor2)));
			barsII.Add(new Vertex2D(OldplCen[i] - FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 0, 1 - factor2)));
		}
		if (barsII.Count > 2)
		{
			VxII.Add(barsII[0]);
			var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 90, new Color(255, 255, 255, 255), new Vector3(0, 0.5f, 1));
			VxII.Add(barsII[1]);
			VxII.Add(vertex);
			for (int i = 0; i < barsII.Count - 2; i += 2)
			{
				VxII.Add(barsII[i]);
				VxII.Add(barsII[i + 2]);
				VxII.Add(barsII[i + 1]);

				VxII.Add(barsII[i + 1]);
				VxII.Add(barsII[i + 2]);
				VxII.Add(barsII[i + 3]);
			}
		}

		spriteBatch.Draw(MythContent.QuickTexture("UIImages/VisualTextures/BladeShadow"), VxII, PrimitiveType.TriangleList);
	}
	public override void PostDraw(Color lightColor)
	{
		if (FirstVel == Vector2.Zero)
			FirstVel = Vector2.Normalize(Projectile.velocity);
		Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);

		for (int i = 1; i < 60; ++i)
		{
			TrueL++;
			if (OldplCen[i] == Vector2.Zero)
				break;
		}

		for (int d = 0; d < 4; d++)
		{
			var VxII = new List<Vertex2D>();
			var barsII = new List<Vertex2D>();
			Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 2d * Math.PI + Main.time / 32d);
			float widk = Vector2.Dot(Vector2.Normalize(deltaPos), Vector2.Normalize(Projectile.velocity)) + 1f;
			float widV = (float)Math.Clamp(1.6 - Math.Sqrt(Projectile.velocity.Length() / 16f), 0, 1.6f);
			if (d == 0)
			{
				deltaPos *= 0;
				statrP[d] = 1;
			}
			else
			{
				if (statrP[d] == 0)
					statrP[d] = Main.rand.NextFloat(1f, 2f);
			}
			for (int i = 1; i < 60; ++i)
			{
				if (OldplCen[i] == Vector2.Zero)
					break;
				var factor = i / 60f;
				var w = statrP[d] - factor;
				if (w > 1)
					w = 2 - w;
				barsII.Add(new Vertex2D(deltaPos * (float)Math.Clamp(Math.Sqrt(factor * 3), 0, 1) * widV + OldplCen[i] + FlipVel * wid[i] * widk * widk * 0.6f - Main.screenPosition, new Color(0, 0.7f, 0.7f, 0.5f), new Vector3((float)Math.Sqrt(factor), 1, w)));
				barsII.Add(new Vertex2D(deltaPos * (float)Math.Clamp(Math.Sqrt(factor * 3), 0, 1) * widV + OldplCen[i] - FlipVel * wid[i] * widk * widk * 0.6f - Main.screenPosition, new Color(0, 0.7f, 0.7f, 0.5f), new Vector3((float)Math.Sqrt(factor), 0, w)));
			}
			if (barsII.Count > 2)
			{
				VxII.Add(barsII[0]);
				if (statrP[d] > 1)
					statrP[d] = 2 - statrP[d];
				var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(0, 0.7f, 1f, 0.5f), new Vector3(0, 0.5f, statrP[d]));
				VxII.Add(barsII[1]);
				VxII.Add(vertex);
				for (int i = 0; i < barsII.Count - 2; i += 2)
				{
					VxII.Add(barsII[i]);
					VxII.Add(barsII[i + 2]);
					VxII.Add(barsII[i + 1]);

					VxII.Add(barsII[i + 1]);
					VxII.Add(barsII[i + 2]);
					VxII.Add(barsII[i + 3]);
				}
			}
			RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
			Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIImages/VisualTextures/EShoot");
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);
			Main.graphics.GraphicsDevice.RasterizerState = originalState;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		if (FirstVel == Vector2.Zero)
			FirstVel = Vector2.Normalize(Projectile.velocity);
		Vector2 FlipVel = FirstVel.RotatedBy(Math.PI / 2d);
		for (int d = 0; d < 7; d++)
		{
			var VxII = new List<Vertex2D>();
			var barsII = new List<Vertex2D>();
			Vector2 deltaPos = new Vector2(0, 24).RotatedBy(d / 3d * Math.PI + Main.time / 7d);
			float widk = Vector2.Dot(Vector2.Normalize(deltaPos), Vector2.Normalize(Projectile.velocity)) + 1.2f;
			float widV = (float)Math.Clamp(1.6 - Math.Sqrt(Projectile.velocity.Length() / 16f), 0, 1.6f);
			Color c0 = Color.White;
			if (d == 0)
			{
				deltaPos *= 0;
				widk = 4f * Projectile.timeLeft / 60f;
				c0 = new Color(255, 255, 255, 0);
			}
			for (int i = 1; i < 60; ++i)
			{
				if (OldplCen[i] == Vector2.Zero)
					break;
				var factor = i / 60f;
				var factor2 = i / (float)TrueL;
				barsII.Add(new Vertex2D(deltaPos * (float)Math.Clamp(Math.Sqrt(factor * 3), 0, 1) * widV + OldplCen[i] + FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 1, 1 - factor2)));
				barsII.Add(new Vertex2D(deltaPos * (float)Math.Clamp(Math.Sqrt(factor * 3), 0, 1) * widV + OldplCen[i] - FlipVel * wid[i] * widk - Main.screenPosition, c0, new Vector3((float)Math.Sqrt(factor), 0, 1 - factor2)));
			}
			if (barsII.Count > 2)
			{
				VxII.Add(barsII[0]);
				var vertex = new Vertex2D((barsII[0].position + barsII[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 255, 255, 255), new Vector3(0, 0.5f, 1));
				VxII.Add(barsII[1]);
				VxII.Add(vertex);
				for (int i = 0; i < barsII.Count - 2; i += 2)
				{
					VxII.Add(barsII[i]);
					VxII.Add(barsII[i + 2]);
					VxII.Add(barsII[i + 1]);

					VxII.Add(barsII[i + 1]);
					VxII.Add(barsII[i + 2]);
					VxII.Add(barsII[i + 3]);
				}
			}

			Texture2D t0 = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/heatmapShadeXiao").Value;
			if (d == 0)
				t0 = ModContent.Request<Texture2D>("Everglow/Myth/UIImages/VisualTextures/heatmapShadeXiaoGreen").Value;
			Main.graphics.GraphicsDevice.Textures[0] = t0;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, VxII.ToArray(), 0, VxII.Count / 3);
		}
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - new Vector2(0, player.gravDir * 140f), null, lightColor, player.gravDir == 1 ? MathF.PI * 1.25f : MathF.PI * 0.25f, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
	public static int CyanStrike = 0;
	public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
	{
		CyanStrike = 1;
		Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), target.Center, Vector2.Zero, ModContent.ProjectileType<XiaoHit>(), 0, 0, Projectile.owner, 0.45f);
		base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
	}
	public override void Load()
	{
		On_CombatText.NewText_Rectangle_Color_string_bool_bool += CombatText_NewText_Rectangle_Color_string_bool_bool;
	}
	private int CombatText_NewText_Rectangle_Color_string_bool_bool(On_CombatText.orig_NewText_Rectangle_Color_string_bool_bool orig, Rectangle location, Color color, string text, bool dramatic, bool dot)
	{
		if (CyanStrike > 0)
		{
			color = new Color(0, 255, 174);
			CyanStrike--;
		}
		return orig(location, color, text, dramatic, dot);
	}
}
