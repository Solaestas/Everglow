using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Summon;

public class AshBone : ModProjectile
{
	public List<Vector2> WhipPointsForCollision = new List<Vector2>();

	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Ash Bone");
				DefaultToWhip();
	}
	private void DefaultToWhip()
	{
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;//165
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.scale = 1f;
		Projectile.ownerHitCheck = true;
		Projectile.extraUpdates = 1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
		Projectile.DamageType = DamageClass.Summon;
	}
	private int PreC = 0;
	public override bool? CanCutTiles()
	{
		WhipPointsForCollision.Clear();
		FillWhipControlPoints(Projectile, WhipPointsForCollision);
		var value = new Vector2(Projectile.width * Projectile.scale / 2f, 0f);
		for (int i = 0; i < WhipPointsForCollision.Count; i++)
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(WhipPointsForCollision[i] - value, WhipPointsForCollision[i] + value, Projectile.height * Projectile.scale, new Utils.TileActionAttempt(DelegateMethods.CutTiles));
		}
		return base.CanCutTiles();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{

	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		WhipPointsForCollision.Clear();
		FillWhipControlPoints(Projectile, WhipPointsForCollision);

		for (int n = 0; n < WhipPointsForCollision.Count; n++)
		{
			var point = WhipPointsForCollision[n].ToPoint();
			projHitbox.Location = new Point(point.X - projHitbox.Width / 2, point.Y - projHitbox.Height / 2);
			if (projHitbox.Intersects(targetHitbox))
				return true;
		}
		return false;
	}
	public override void AI()
	{
		AI_165_Whip();

		return;
	}
	private void AI_165_Whip()
	{
		Player player = Main.player[Projectile.owner];
		//CombatText.NewText(new Rectangle((int)player.Center.X - 10, (int)player.Center.Y - 50, 20, 20), Color.Red, Projectile.damage);
		Projectile.rotation = Projectile.velocity.ToRotation() + 1.5707964f;
		Projectile.ai[0] += 1f;
		float num;
		int num2;
		float num3;
		GetWhipSettings(Projectile, out num, out num2, out num3);
		Projectile.tileCollide = false;
		Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * (Projectile.ai[0] - 1f);
		Projectile.spriteDirection = Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f ? -1 : 1;
		if (Projectile.ai[0] >= num || player.itemAnimation == 0)
		{
			Projectile.Kill();
			return;
		}
		player.heldProj = Projectile.whoAmI;
		player.itemAnimation = player.itemAnimationMax - (int)(Projectile.ai[0] / Projectile.MaxUpdates);
		player.itemTime = player.itemAnimation;
		if (Projectile.ai[0] == (int)(num / 2f))
		{
			WhipPointsForCollision.Clear();
			FillWhipControlPoints(Projectile, WhipPointsForCollision);
			Vector2 position = WhipPointsForCollision[WhipPointsForCollision.Count - 1];
			SoundEngine.PlaySound(SoundID.Item153, position);
		}
		float t = Projectile.ai[0] / num;
		if (Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true) > 0.5f && Main.rand.Next(3) != 0)
		{
			WhipPointsForCollision.Clear();
			FillWhipControlPoints(Projectile, WhipPointsForCollision);
			int num5 = Main.rand.Next(WhipPointsForCollision.Count - 10, WhipPointsForCollision.Count);
			Rectangle rectangle = Utils.CenteredRectangle(WhipPointsForCollision[num5], new Vector2(30f, 30f));
			int num6 = 57;
			if (Main.rand.NextBool(3))
				num6 = 43;
			var dust = Dust.NewDustDirect(rectangle.TopLeft(), rectangle.Width, rectangle.Height, DustID.Bone, 0f, 0f, 100, Color.White, 1f);
			dust.position = WhipPointsForCollision[num5];
			dust.fadeIn = 0.3f;
			Vector2 spinningpoint = WhipPointsForCollision[num5] - WhipPointsForCollision[num5 - 1];
			dust.noGravity = true;
			dust.velocity *= 0.5f;
			dust.velocity += spinningpoint.RotatedBy((double)(player.direction * 1.5707964f), default);
			dust.velocity *= 0.5f;
		}
	}
	public static void GetWhipSettings(Projectile proj, out float timeToFlyOut, out int segments, out float rangeMultiplier)
	{
		timeToFlyOut = Main.player[proj.owner].itemAnimationMax * proj.MaxUpdates;
		rangeMultiplier = 1f;
		int num = proj.type;

		segments = 20;
		rangeMultiplier *= 1.2f;//鞭长
	}
	public static void FillWhipControlPoints(Projectile proj, List<Vector2> controlPoints)
	{
		float num;
		int num2;
		float num3;
		GetWhipSettings(proj, out num, out num2, out num3);
		float num4 = proj.ai[0] / num;
		float num5 = 0.5f;
		float num6 = 1f + num5;
		float num7 = 31.415928f * (1f - num4 * num6) * (float)-(float)proj.spriteDirection / num2;
		float num8 = num4 * num6;
		float num9 = 0f;
		if (num8 > 1f)
		{
			num9 = (num8 - 1f) / num5;
			num8 = MathHelper.Lerp(1f, 0f, num9);
		}
		float num10 = proj.ai[0] - 1f;
		Player player = Main.player[proj.owner];
		Item heldItem = Main.player[proj.owner].HeldItem;
		num10 = ContentSamples.ItemsByType[heldItem.type].useAnimation * 2 * num4 * player.whipRangeMultiplier;
		float num11 = proj.velocity.Length() * num10 * num8 * num3 / num2;
		float num12 = 1f;
		Vector2 playerArmPosition = Main.GetPlayerArmPosition(proj);
		Vector2 vector = playerArmPosition;
		float num13 = 0f;
		float num14 = num13 - 1.5707964f;
		Vector2 value = vector;
		float num15 = num13 + 1.5707964f + 1.5707964f * proj.spriteDirection;
		Vector2 value2 = vector;
		float num16 = num13 + 1.5707964f;
		controlPoints.Add(playerArmPosition);
		for (int i = 0; i < num2; i++)
		{
			float num17 = i / (float)num2;
			float num18 = num7 * num17 * num12;
			Vector2 vector2 = vector + num14.ToRotationVector2() * num11;
			Vector2 vector3 = value2 + num16.ToRotationVector2() * (num11 * 2f);
			Vector2 vector4 = value + num15.ToRotationVector2() * (num11 * 2f);
			float num19 = 1f - num8;
			float num20 = 1f - num19 * num19;
			var value3 = Vector2.Lerp(vector3, vector2, num20 * 0.9f + 0.1f);
			var value4 = Vector2.Lerp(vector4, value3, num20 * 0.7f + 0.3f);
			Vector2 spinningpoint = playerArmPosition + (value4 - playerArmPosition) * new Vector2(1f, num6);
			float num21 = num9;
			num21 *= num21;
			Vector2 item = spinningpoint.RotatedBy((double)(proj.rotation + 4.712389f * num21 * proj.spriteDirection), playerArmPosition);
			controlPoints.Add(item);
			num14 += num18;
			num16 += num18;
			num15 += num18;
			vector = vector2;
			value2 = vector3;
			value = vector4;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawWhip(Projectile);
		return false;
	}
	bool[] Hitj = new bool[200];
	private void DrawWhip(Projectile proj)
	{
		Player player = Main.player[Projectile.owner];
		var list = new List<Vector2>();
		FillWhipControlPoints(proj, list);
		Texture2D value = TextureAssets.Projectile[proj.type].Value;
		//Texture2D value = ModContent.Request<Texture2D>("Everglow/Myth/MiscItems/Projectiles/Weapon/Summon/AshBone2").Value;
		Rectangle rectangle = value.Frame(1, 1, 0, 0, 0, 0);
		var origin = new Vector2(rectangle.Width / 2, 2f);
		Color originalColor = Color.White;
		int type = proj.type;
		originalColor = Color.OrangeRed;
		Vector2 value2 = list[0];
		for (int i = 0; i < list.Count - 1; i++)
		{
			Vector2 vector = list[i];
			Vector2 vector2 = list[i + 1] - vector;
			float rotation = vector2.ToRotation() - 1.5707964f;
			Vector2 vector3 = value2;
			Color color = Lighting.GetColor(vector3.ToTileCoordinates());
			var scale = new Vector2(1f, (vector2.Length() + 2f) / rectangle.Height);
			Main.spriteBatch.Draw(value, value2 - Main.screenPosition, new Rectangle?(rectangle), color, rotation, origin, scale, SpriteEffects.None, 0f);

			for (int x = -3; x < 4; x++)
			{
				for (int y = -3; y < 4; y++)
				{
					int TT = Main.tile[(int)(value2.X / 16f) + x, (int)(value2.Y / 16f) + y].TileType;
					if (TT == 3 || TT == 69 || TT == 71 || TT == 73 || TT == 74 || TT == 82 || TT == TileID.Pots || TT == TileID.VineFlowers || TT == TileID.Vines || TT == TileID.CrimsonVines || TT == TileID.CrimsonVines || TT == TileID.HallowedVines || TT == TileID.JungleVines || TT == 84 || TT == 110 || TT == 113 || TT == 184 || TT == 236 || TT == 24 || TT == 32)
						WorldGen.KillTile((int)(value2.X / 16f) + x, (int)(value2.Y / 16f) + y);
				}
			}

			for (int j = 0; j < 200; j++)
			{
				if (!Hitj[j])
				{
					if ((Main.npc[j].Center - value2).Length() < 60 && !Main.npc[j].dontTakeDamage && !Main.npc[j].friendly && Main.npc[j].active && Collision.CanHit(value2, 1, 1, Main.npc[j].Center, 1, 1))
					{
						Hitj[j] = true;
						float CritC = player.GetCritChance(DamageClass.Summon);
						Main.npc[j].StrikeNPC((int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 8, Math.Sign(Projectile.velocity.X), Main.rand.Next(100) < CritC);
						player.addDPS((int)(Projectile.damage * (1 + CritC / 100f) * 1.0f));
						int[] array = Projectile.localNPCImmunity;
						bool flag = !Projectile.usesLocalNPCImmunity && !Projectile.usesIDStaticNPCImmunity || Projectile.usesLocalNPCImmunity && array[j] == 0 || Projectile.usesIDStaticNPCImmunity && Projectile.IsNPCIndexImmuneToProjectileType(Projectile.type, j);
						if (Main.npc[j].active && !Main.npc[j].dontTakeDamage && flag && (Main.npc[j].aiStyle != 112 || Main.npc[j].ai[2] <= 1f))
						{
							if (Main.npc[j].active)
								Main.player[Projectile.owner].MinionAttackTargetNPC = j;
						}
						for (int z = 0; z < 40; z++)
						{
							Vector2 v4 = new Vector2(0, Main.rand.NextFloat(0.15f, 5.05f)).RotatedByRandom(MathHelper.TwoPi);
							int h = Dust.NewDust(Main.npc[j].Center, 0, 0, DustID.Bone, v4.X, v4.Y, 0, default, Main.rand.NextFloat(1.5f, 3f));
							Main.dust[h].noGravity = true;
						}
						for (int z = 0; z < 12; z++)
						{
							float num7 = (float)(Main.rand.Next(0, 2000) / 1000f * Math.PI);
							if (Main.rand.NextBool(2) && PreC < 8)
							{
								//SoundEngine.PlaySound(SoundID.Item33, Projectile.Center);
								Vector2 v = new Vector2(0, Main.rand.NextFloat(4f, 8f)).RotatedBy(Math.PI * z / 6d + num7);
								int num2 = Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Main.npc[j].Center + v * 7, v, ProjectileID.Bone, Projectile.damage, 0.2f, Main.LocalPlayer.whoAmI, player.GetCritChance(DamageClass.Summon), (int)(Projectile.damage * 0.3));
								PreC += 1;
							}
						}
					}
				}
			}
			value2 += vector2;
		}
		value = ModContent.Request<Texture2D>("Everglow/Myth/MiscItems/Projectiles/Weapon/Summon/AshBone3").Value;
		originalColor = Color.OrangeRed;
		rectangle = value.Frame(1, 1, 0, 0, 0, 0);
		value2 = list[0];
		for (int i = 0; i < list.Count - 1; i++)
		{
			if (i == list.Count - 2)
				value = ModContent.Request<Texture2D>("Everglow/Myth/MiscItems/Projectiles/Weapon/Summon/AshBone4").Value;
			Vector2 vector = list[i];
			Vector2 vector2 = list[i + 1] - vector;
			float rotation = vector2.ToRotation() - 1.5707964f;
			Vector2 vector3 = value2;
			Color color = Lighting.GetColor(vector3.ToTileCoordinates());
			var scale = new Vector2(1f, 1f);
			Main.spriteBatch.Draw(value, value2 - Main.screenPosition, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
			value2 += vector2;
		}
	}
	/*public static Vector2 DrawWhip_WhipSword(Projectile proj, List<Vector2> controlPoints)
	{
		Texture2D value = ModContent.Request<Texture2D>("Everglow/Myth/MiscItems/Projectiles/Weapon/Summon/AshBone2").Value;
		Microsoft.Xna.Framework.Rectangle rectangle = value.Frame(1, 5, 0, 0, 0, 0);
		int height = rectangle.Height;
		rectangle.Height -= 2;
		Vector2 vector = rectangle.Size() / 2f;
		Vector2 vector2 = controlPoints[0];
		for (int i = 0; i < controlPoints.Count - 1; i++)
		{
			bool flag = true;
			Vector2 origin = vector;
			if (i != 0)
			{
				switch (i)
				{
					case 3:
					case 5:
					case 7:
						rectangle.Y = height;
						goto IL_F0;
					case 9:
					case 11:
					case 13:
						rectangle.Y = height * 2;
						goto IL_F0;
					case 15:
					case 17:
						rectangle.Y = height * 3;
						goto IL_F0;
					case 19:
						rectangle.Y = height * 4;
						goto IL_F0;
				}
				flag = false;
			}
			else
			{
				origin.Y -= 4f;
			}
			IL_F0:
			Vector2 vector3 = controlPoints[i];
			Vector2 vector4 = controlPoints[i + 1] - vector3;
			if (flag)
			{
				float rotation = vector4.ToRotation() - 1.5707964f;
				Microsoft.Xna.Framework.Color color = Lighting.GetColor(vector3.ToTileCoordinates());
				Main.spriteBatch.Draw(value, vector2 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle), color, rotation, origin, 1f, SpriteEffects.None, 0f);
			}
			vector2 += vector4;
		}
		return vector2;
	}*/
}
