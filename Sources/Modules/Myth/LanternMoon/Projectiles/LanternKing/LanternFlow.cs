using Everglow.Commons.DataStructures;
using Everglow.Commons.Weapons;
using Everglow.Myth.LanternMoon.Gores;
using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;
using Terraria;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class LanternFlow : TrailingProjectile
{
	public override void SetDef()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 600;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;

		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 400;
		TrailColor = new Color(1f, 0.2f, 0f, 0f) * 0.3f;
		TrailWidth = 240f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_2_thick.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		WarpStrength = 1f;
	}
	public NPC OwnerNPC;
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(1f, 1f, 1f, 0.5f));
	}
	public override void OnSpawn(IEntitySource source)
	{
		if(OwnerNPC == null)
		{
			foreach(NPC npc in Main.npc)
			{
				if(npc != null && npc.active)
				{
					if(npc.type == ModContent.NPCType<LanternGhostKing>())
					{
						OwnerNPC = npc;
					}
				}
			}
		}
	}
	public override void AI()
	{
		if (OwnerNPC == null)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc != null && npc.active)
				{
					if (npc.type == ModContent.NPCType<LanternGhostKing>())
					{
						OwnerNPC = npc;
					}
				}
			}
		}
		if (OwnerNPC == null)
		{
			Projectile.active = false;
			return;
		}

		base.AI();
		Vector2 toOwner = OwnerNPC.Center - Projectile.Center;
		if(Projectile.timeLeft > 507)
		{
			Projectile.velocity = Projectile.velocity.RotatedBy(MathF.Sin(Projectile.timeLeft * 0.08f) * 0.02f);
		}
		else if(Projectile.timeLeft > 447)
		{
			Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
		}
		else
		{
			Projectile.velocity += Vector2.Normalize(toOwner) * 0.7f;
			if (Projectile.timeLeft < 420)
			{
				Projectile.velocity *= 0.98f;
			}
			if (Projectile.timeLeft < 220)
			{
				TrailColor = TrailColor * 0.99f;
				WarpStrength *= 0.98f;
			}
		}
		if(Projectile.timeLeft < 580)
		{
			for (int i = 0; i < Projectile.oldPos.Length / 60; i++)
			{
				int checkOldPosIndex = Main.rand.Next(5, (int)MathF.Min(598 - Projectile.timeLeft, Projectile.oldPos.Length - 2));
				float mulScale = Main.rand.NextFloat(0.5f, 1.2f);
				if(Projectile.timeLeft < 120f)
				{
					mulScale *= Projectile.timeLeft / 120f;
				}
				Vector2 addPos = new Vector2(Main.rand.NextFloat(0f, 90f), 0).RotateRandom(6.283);
				var gore2 = new LanternFlow_lantern
				{
					Active = true,
					Visible = true,
					velocity = -Vector2.Normalize(Projectile.oldPos[checkOldPosIndex] - Projectile.oldPos[checkOldPosIndex - 1]) * mulScale * 6 - addPos * 0.01f,
					scale = mulScale,
					position = Projectile.oldPos[checkOldPosIndex] + addPos,
					npcOwner = OwnerNPC
				};
				Ins.VFXManager.Add(gore2);
			}
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}
	public override void DrawSelf()
	{

	}
	public override void DrawTrail()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.0005f;
			factor *= 3f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = TrailColor;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
	public override void DrawTrailDark()
	{
		base.DrawTrailDark();
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		for(int i = 50;i < Projectile.oldPos.Count();i++)
		{
			if((targetHitbox.Center() - (Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) * 0.5f)).Length() < TrailWidth * 0.3f)
			{
				return true;
			}
		}
		return false;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
	}
}