using Everglow.Yggdrasil.KelpCurtain.Projectiles.Legacies;
using Terraria;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Buffs;

public class LichensPycnidium : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.width = 15;
		Projectile.height = 15;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		base.SetDefaults();
	}
	public NPC AttachTarget;
	public float Maturity;
	public override void OnSpawn(IEntitySource source)
	{
		if (AttachTarget == null)
		{
			float closestLength = 50;
			foreach (NPC npc in Main.npc)
			{
				if (npc.active && !npc.friendly)
				{
					if ((npc.Center - Projectile.Center).Length() < closestLength)
					{
						closestLength = (npc.Center - Projectile.Center).Length();
						AttachTarget = npc;
					}
				}
			}
		}
		if (AttachTarget == null)
		{
			Projectile.active = false;
		}
		Maturity = 0;
		base.OnSpawn(source);
	}
	public override void AI()
	{
		if (AttachTarget == null || AttachTarget.active == false)
		{
			Projectile.active = false;
		}
		float lengthValue = 10 * MathF.Sin(Projectile.whoAmI + AttachTarget.whoAmI * 0.25f + (float)Main.time * 0.03f);
		float rotValue = 0.4f * MathF.Sin(Projectile.whoAmI * 0.25f + AttachTarget.whoAmI + (float)Main.time * 0.03f);
		Projectile.Center = Vector2.Lerp(Projectile.Center, AttachTarget.Center + new Vector2(30 + lengthValue + Maturity / 5f, 0).RotatedBy(Projectile.ai[0] + rotValue), 0.15f);
		Maturity++;
		if (Maturity > 300)
		{
			if(Main.rand.NextFloat(Maturity - 500 ,Maturity - 200) > 150)
			{
				Explode();
			}
		}
		float value = Maturity / 300f;
		if (value > 1)
		{
			value = 1;
		}
		Projectile.scale = value * value * 0.5f;
		base.AI();
	}
	public void Explode()
	{
		Projectile.Kill();
		Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<Pycnidium_explosion>(), Projectile.damage, Projectile.knockBack * 4f, Projectile.owner, 30);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		if (AttachTarget == null)
		{
			Projectile.active = false;
		}
		Texture2D texture = ModAsset.Point_dark.Value;
		Color c0 = lightColor;
		c0.A = 0;
		Color color = Color.Lerp(c0 , new Color(0.1f, 0.7f, 0.7f, 0), Maturity / 100f);
		if(Maturity > 100)
		{
			color = Color.Lerp(new Color(0.1f, 0.7f, 0.7f, 0), new Color(0.4f, 0.7f, 0.4f, 0), (Maturity - 100f) / 100f);
		}
		if (Maturity > 200)
		{
			color = Color.Lerp(new Color(0.4f, 0.7f, 0.4f, 0), new Color(0.8f, 1f, 0.3f, 0), (Maturity - 200f) / 100f);
		}
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, 0, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		texture = ModAsset.LichensPycnidium.Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, 0, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		Vector2 joint = AttachTarget.Center;
		Vector2 jointVel = new Vector2(7, 0).RotatedBy(Projectile.ai[0]);
		List<Vector2> joints = new List<Vector2>();
		List<Vector2> jointVels = new List<Vector2>();
		for (int t = 0; t < 120; t++)
		{
			joints.Add(joint);
			jointVels.Add(jointVel);
			joint += jointVel;
			jointVel = Vector2.Lerp(jointVel, Vector2.Normalize(Projectile.Center - joint - jointVel) * 1.5f, 0.35f);
			float toTargetLength = (Projectile.Center - joint).Length();

			if (toTargetLength < 2)
			{
				break;
			}
		}
		float width = 2f * Projectile.scale;
		List<Vertex2D> bars = new List<Vertex2D>();
		for(int t = 0;t < joints.Count; t++)
		{
			Vector2 velLeft = jointVels[t].NormalizeSafe().RotatedBy(MathHelper.PiOver2);
			float mulWid = 1f;
			if(t < 10)
			{
				mulWid = t / 10f;
			}
			bars.Add(new Vertex2D(joints[t] + velLeft * width * mulWid - Main.screenPosition, lightColor * 0.5f * width * mulWid, new Vector3(0, 0.9f, 0)));
			bars.Add(new Vertex2D(joints[t] - velLeft * width * mulWid - Main.screenPosition, lightColor * 0.5f * width * mulWid, new Vector3(0, 0.1f, 0)));
		}
		if(bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_black.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		bars = new List<Vertex2D>();
		for (int t = 0; t < joints.Count; t++)
		{
			Vector2 velLeft = jointVels[t].NormalizeSafe().RotatedBy(MathHelper.PiOver2);
			float mulWid = 1f;
			if (t < 10)
			{
				mulWid = t / 10f;
			}
			bars.Add(new Vertex2D(joints[t] + velLeft * width * mulWid - Main.screenPosition, Color.Lerp(c0, color, 1 - t / (float)joints.Count), new Vector3(0, 0.7f, 0)));
			bars.Add(new Vertex2D(joints[t] - velLeft * width * mulWid - Main.screenPosition, Color.Lerp(c0, color, 1 - t / (float)joints.Count), new Vector3(0, 0.3f, 0)));
		}
		if (bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail.Value;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
		return false;
	}
}