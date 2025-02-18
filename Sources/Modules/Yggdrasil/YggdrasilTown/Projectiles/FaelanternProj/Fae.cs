using Everglow.Commons.DataStructures;
using Everglow.Commons.Skeleton2D.Renderer;
using Everglow.Commons.Skeleton2D;
using Terraria.IO;
using Everglow.Commons.Coroutines;
using Everglow.Commons.Skeleton2D.Reader;
using Terraria.DataStructures;
using Terraria.Audio;
using Everglow.Commons.Skeleton2D.Renderer.DrawCommands;
using Microsoft.Xna.Framework.Graphics;
using Terraria.WorldBuilding;
using Spine;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria;
using static Everglow.Yggdrasil.YggdrasilTown.NPCs.RockElemental;
using Everglow.Commons.Weapons;
using Humanizer;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.FaelanternProj;

public class Fae : TrailingProjectile
{
	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 2;
	}

	public enum State
	{
		Idle,
		Homing,
		Charm,
	}

	public int state = (int)State.Idle;


	public override void SetDef()
	{
		Projectile.width = 20;
		Projectile.height = 24;
		Projectile.netImportant = true;
		Projectile.friendly = true;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 45;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.gfxOffY = 7;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
		TrailColor = new Color(0.13f, 0.976f, 0.977f,0f);
		TrailWidth = 5f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_2_black.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_2_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
	}

	public override bool? CanDamage()
	{
		return false;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[1] = 0;
		state = (int)State.Idle;
	}

	private Projectile owner;
	private NPC target;
	private int timer = 0;

	public CoroutineManager _coroutineManager = new CoroutineManager();

	public override void AI()
	{
		Lighting.AddLight(Projectile.Center, 0.13f, 0.976f, 0.977f);
		owner = Main.projectile[(int)Projectile.ai[0]];
		if (owner == null || !owner.active || owner.type != ModContent.ProjectileType<FaelanternProj>())
		{
			Projectile.Kill();
		}


		timer++;
		if (timer % 4 == 0)
		{
			Projectile.frame++;
		}

		Projectile.timeLeft++;
		Projectile.spriteDirection = Projectile.velocity.X >= 0 ? -1 : 1;
		switch (state)
		{
			case (int)State.Idle:
				{
					Idle();
					break;
				}
			case (int)State.Homing:
				{
					Homing();
					break;
				}
			case (int)State.Charm:
				{
					Charm();
					break;
				}
		}
		base.AI();
	}

	public void Idle()
	{

		FaelanternProj FaelanternProj = owner.ModProjectile as FaelanternProj;
		Vector2 pos = new Vector2(FaelanternProj.FaelanternSkeleton.Skeleton.FindBone("bone6").WorldX, FaelanternProj.FaelanternSkeleton.Skeleton.FindBone("bone6").WorldY);
		Vector2 ToPos = (pos - Projectile.Center).NormalizeSafe() * 0.5f;
		if (Projectile.Hitbox.Intersects(owner.Hitbox))
		{
			Projectile.velocity += new Vector2(-(float)Math.Sin(0.05f * timer) / 2f, -(float)Math.Cos(0.04f * timer) / 3f) * 0.5f;
		}
		else
		{
			Projectile.velocity += new Vector2(0, -(float)Math.Cos(0.04f * timer)) * 0.25f;
			ToPos = (pos - Projectile.Center).NormalizeSafe() * 4f;
		}


		Projectile.velocity = Vector2.Lerp(Projectile.velocity, ToPos, 0.2f);

		if (Projectile.ai[1] != -1)
		{
			state = (int)State.Homing;
		}
	}

	public void Homing()
	{
		if (Projectile.ai[1] != -1)
		{
			target = Main.npc[(int)Projectile.ai[1]];
			if (!target.active || target == null)
			{
				Projectile.ai[1] = -1;
				state = (int)State.Idle;
			}
			else
			{
				Vector2 pos = target.Center + new Vector2(0, -(float)Math.Cos(0.04f * timer));
				Vector2 ToPos = (pos - Projectile.Center).NormalizeSafe() * 3f;
				Projectile.velocity += new Vector2(0, -(float)Math.Cos(0.04f * timer)) * 0.5f;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, ToPos, 0.2f);
				if (Projectile.Distance(target.Center) <= 50)
				{
					i = 60;
					state = (int)State.Charm;
				}
			}
		}
	}

	int i = 60;

	public void Charm()
	{
		i--;
		Projectile.velocity = Vector2.Lerp(Projectile.velocity, new Vector2(-(float)Math.Sin(0.05f * timer), -(float)Math.Cos(0.04f * timer) / 3f), 0.1f);
		if (i == 30)
		{
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<FaeCharmRing>(), Projectile.damage / 2, 0f, Projectile.owner, 10);
			foreach (NPC npc in Main.npc)
			{
				if (Projectile.Distance(npc.Center) <= 150)
				{
					npc.AddBuff(BuffID.Confused, 300);
				}
			}
		}
		Projectile.ai[1] = -1;
		if (i == 0)
		{
			state = (int)State.Idle;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		TrailColor = new Color(0.13f, 0.976f, 0.977f,0f);
		TrailWidth = 10f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_2_thick.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_2_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
		DrawTrail();

		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		var origin = new Vector2(tex.Width / 2, tex.Height / 2);
		Rectangle sourceRec = tex.Frame(1, 2, 0, Projectile.frame % 2);

		Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, 12f), sourceRec, Color.White, 0f, origin, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		return false;
	}

	public override void DrawTrailDark() => base.DrawTrailDark();

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{

		Vector2 size = new Vector2(Projectile.width, Projectile.height);
		float point = 0;
		if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position + size, Projectile.position, 50f, ref point))
		{
			return true;
		}

		return false;
	}

}