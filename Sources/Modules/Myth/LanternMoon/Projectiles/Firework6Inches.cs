using Terraria.DataStructures;
using static Everglow.Myth.LanternMoon.Projectiles.Firework6Inches;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace Everglow.Myth.LanternMoon.Projectiles;

public class Firework6Inches : ModProjectile
{
	public override string Texture => "Everglow/Myth/UIImages/VisualTextures/DarkGrey";
	public struct FlameTrail
	{
		public FlameTrail(Vector3 postion, List<Vector3> oldPos, Vector3 velocity, Color color, float scale, bool active)
		{
			Postion = postion;
			OldPos = oldPos;
			Velocity = velocity;
			Color = color;
			Scale = scale;
			Active = active;
		}
		public List<Vector3> OldPos;
		public Vector3 Postion;
		public Vector3 Velocity;
		public Color Color;
		public float Scale;
		public int TrailStyle;
		public int FlameTimer;
		public int[] ai = new int[5];
		public bool Active;
	}
	public List<FlameTrail> Stars;
	public int Timer;
	public int TrailLength = 75;
	public override void SetDefaults()
	{
		Projectile.timeLeft = 600;
		Projectile.aiStyle = -1;
		Projectile.scale = 1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
		Stars = new List<FlameTrail>();
	}
	private static Vector3 RodriguesRotate(Vector3 origVec, Vector3 axis, float theta)
	{
		if (axis != new Vector3(0, 0, 0))
		{
			axis = Vector3.Normalize(axis);
		}
		else
		{
			axis = new Vector3(0, 0, -1);
		}
		float cos = MathF.Cos(theta);
		return cos * origVec + (1 - cos) * Vector3.Dot(origVec, axis) * axis + MathF.Sin(theta) * Vector3.Cross(origVec, axis);
	}
	private static Vector2 Projection2D(Vector3 vector, Vector2 center, float viewZ, out float scale)
	{
		float value = -viewZ / (vector.Z - viewZ);
		scale = value;
		var v = new Vector2(vector.X, vector.Y);
		return v + (value - 1) * (v - center);
	}
	public override void OnSpawn(IEntitySource source)
	{
		Timer = 0;
		SpawnStyle(Main.rand.Next(13));
	}
	private Color NormalColor()
	{
		Color c0 = Color.White;
		switch (Main.rand.Next(5))
		{
			case 0:
				c0 = new Color(1f, 0.1f, 0.1f, 0);
				break;
			case 1:
				c0 = new Color(0.2f, 1f, 0.4f, 0);
				break;
			case 2:
				c0 = new Color(0.2f, 0.01f, 0.8f, 0);
				break;
			case 3:
				c0 = new Color(0.7f, 0.01f, 0.4f, 0);
				break;
			case 4:
				c0 = new Color(0.9f, 0.4f, 0.05f, 0);
				break;
		}
		return c0;
	}
	public void SpawnStyle(int style)
	{
		Vector3 axis = new Vector3(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
		float rot = Main.rand.NextFloat(6.283f);
		int starsCount = 90;
		Color color = NormalColor();
		Color color1 = NormalColor();
		while(color1 == color)
		{
			color1 = NormalColor();
		}
		switch (style)
		{
		        //球状
			case 0:
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(- velocity, new List<Vector3>(), velocity, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.FlameTimer = Main.rand.Next(-20, 0);
						Stars.Add(flame);
					}
				}
				break;
				//环状
			case 1:
				if(style == 1)
				{
					Color c0 = NormalColor();
					starsCount = 160;
					int theta2 = 0;
					float length2 = MathF.Cos(theta2 / (float)starsCount * MathF.PI);
					for (int phi = 0; phi < length2 * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length2 / starsCount * MathHelper.TwoPi) * length2, MathF.Sin(theta2 / (float)starsCount * MathF.PI), MathF.Sin(phi / length2 / starsCount * MathHelper.TwoPi) * length2) * 10f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, c0, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.FlameTimer = Main.rand.Next(-10, 0);
						flame.ai[0] = 20;
						Stars.Add(flame);
					}
				}
				break;
				//杂点
			case 2:
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, Main.rand.NextBool(2) ? color : color1, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true));
					}
				}
				break;
				//三段球
			case 3:
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, Math.Abs(theta) > starsCount * 0.2f ? color : color1, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true));
					}
				}
				break;
				//金色椰树
			case 4:
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, new Color(0, 0, 0, 0), Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.TrailStyle = 1;
						flame.FlameTimer = Main.rand.Next(-10, 11);
						flame.ai[0] = 20;
						flame.ai[1] = 4;
						Stars.Add(flame);
					}
				}
				break;
				//椰树带心
			case 5:
				starsCount = 60;
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, new Color(0, 0, 0, 0), Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.TrailStyle = 1;
						flame.FlameTimer = Main.rand.Next(-10, 11);
						flame.ai[0] = 20;
						Stars.Add(flame);
					}
				}
				starsCount = 40;
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 4f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.9f, true);
						flame.FlameTimer = Main.rand.Next(-10, 11);
						flame.ai[0] = 60;
						Stars.Add(flame);
					}
				}
				break;
				//两半球
			case 6:
				for (int theta = -starsCount + 5; theta < 5; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, color1, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true));
					}
				}
				for (int theta = 5; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true));
					}
				}
				break;
				//多个集束
			case 7:
				starsCount = 45;
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 10f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, color1, Main.rand.NextFloat(0.85f, 1.15f) * 1f, true));
						for(int x =0;x < 6;x++)
						{
							Vector3 newVel = Vector3.Normalize(RodriguesRotate(new Vector3(velocity.Y, -velocity.X, 0), velocity, x / 6f * MathHelper.TwoPi)) * 0.3f * Main.rand.NextFloat(0.85f, 1.15f);
							FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity * 0.4f + newVel, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.4f, true);
							flame.FlameTimer = Main.rand.Next(-10, 0);
							flame.ai[0] = -30;
							Stars.Add(flame);
						}
					}
				}
				break;
				//金环+核心
			case 8:
				if (style == 8)
				{
					starsCount = 70;
					for (int theta = -starsCount; theta <= starsCount; theta += 10)
					{
						float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
						for (int phi = 0; phi <= length * starsCount; phi += 5)
						{
							Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 5f * Main.rand.NextFloat(0.95f, 1.05f);
							velocity = RodriguesRotate(velocity, axis, rot);
							Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true));
						}
					}
					starsCount = 140;
					int theta2 = 0;
					float length2 = MathF.Cos(theta2 / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length2 * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length2 / starsCount * MathHelper.TwoPi) * length2, MathF.Sin(theta2 / (float)starsCount * MathF.PI), MathF.Sin(phi / length2 / starsCount * MathHelper.TwoPi) * length2) * 10f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, new Color(0, 0, 0, 0), Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.TrailStyle = 1;
						flame.FlameTimer = Main.rand.Next(-10, 11);
						flame.ai[0] = 20;
						flame.ai[1] = 20;
						Stars.Add(flame);
					}
				}
				break;
				//金色漏斗
			case 9:
				if (style == 9)
				{
					Color c0 = NormalColor();
					starsCount = 120;
					int theta2 = 0;
					float length2 = MathF.Cos(theta2 / (float)starsCount * MathF.PI);
					for (int phi = 0; phi < length2 * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length2 / starsCount * MathHelper.TwoPi) * length2, MathF.Sin(theta2 / (float)starsCount * MathF.PI), MathF.Sin(phi / length2 / starsCount * MathHelper.TwoPi) * length2) * 10f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, c0, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.FlameTimer = Main.rand.Next(-10, 0);
						flame.ai[0] = 20;
						Stars.Add(flame);
					}
					starsCount = 180;
					for (int theta = -starsCount; theta < starsCount; theta += 10)
					{
						float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
						for (int phi = 0; phi <= length * starsCount; phi += 5)
						{
							Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
							velocity = RodriguesRotate(velocity, axis, rot);
							if(Math.Abs(theta) > starsCount * 0.24f)
							{
								FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, new Color(0, 0, 0, 0), Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
								flame.TrailStyle = 1;
								flame.FlameTimer = Main.rand.Next(-10, 11);
								flame.ai[0] = 20;
								flame.ai[1] = 8;
								Stars.Add(flame);
							}
						}
					}
				}
				break;
				//双层
			case 10:
				starsCount = 70;
				for (int theta = -starsCount; theta < starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 5f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true));
					}
				}
				starsCount = 70;
				for (int theta = -starsCount; theta < starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						Stars.Add(new FlameTrail(-velocity, new List<Vector3>(), velocity, color1, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true));
					}
				}
				break;
			//球状随机拖尾
			case 11:
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.FlameTimer = Main.rand.Next(-20, 0);
						if(Main.rand.NextBool(10))
						{
							flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, new Color(0, 0, 0, 0), Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
							flame.TrailStyle = 1;
							flame.FlameTimer = Main.rand.Next(-10, 11);
							flame.ai[0] = 20;
							flame.ai[1] = 0;
						}
						Stars.Add(flame);
					}
				}
				break;
			//闪烁实验
			case 12:
				for (int theta = -starsCount; theta <= starsCount; theta += 10)
				{
					float length = MathF.Cos(theta / (float)starsCount * MathF.PI);
					for (int phi = 0; phi <= length * starsCount; phi += 5)
					{
						Vector3 velocity = new Vector3(MathF.Cos(phi / length / starsCount * MathHelper.TwoPi) * length, MathF.Sin(theta / (float)starsCount * MathF.PI), MathF.Sin(phi / length / starsCount * MathHelper.TwoPi) * length) * 9f * Main.rand.NextFloat(0.95f, 1.05f);
						velocity = RodriguesRotate(velocity, axis, rot);
						FlameTrail flame = new FlameTrail(-velocity, new List<Vector3>(), velocity, color, Main.rand.NextFloat(0.85f, 1.15f) * 0.7f, true);
						flame.FlameTimer = Main.rand.Next(-60, -40);
						flame.ai[3] = Main.rand.Next(20) - 20;
						Stars.Add(flame);
					}
				}
				break;
		}
	}
	public FlameTrail UpdateFlameTrail(FlameTrail oldFlametrail)
	{		
		oldFlametrail.Postion += oldFlametrail.Velocity;
		if (oldFlametrail.FlameTimer > 50 + oldFlametrail.ai[0])
		{
			oldFlametrail.Scale *= 0.96f;
		}
		oldFlametrail.Velocity.Y += 0.03f;
		oldFlametrail.Velocity *= 0.98f;
		oldFlametrail.Velocity.X += Main.windSpeedCurrent * 0.02f / (oldFlametrail.Scale + 1);
		oldFlametrail.OldPos.Add(oldFlametrail.Postion);
		if (oldFlametrail.OldPos.Count > TrailLength)
			oldFlametrail.OldPos.RemoveAt(0);
		Vector3 drawPos3D = oldFlametrail.Postion;
		drawPos3D.X += (Projectile.Center - Main.screenPosition).X;
		drawPos3D.Y += (Projectile.Center - Main.screenPosition).Y;
		float scale;
		Vector2 v2Pos = Projection2D(drawPos3D, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 1000, out scale);
		float value = scale * oldFlametrail.Scale / 255f;
		if(value > 0.1f / 255f)
		{
			Lighting.AddLight(v2Pos + Main.screenPosition, oldFlametrail.Color.R * value, oldFlametrail.Color.G * value, oldFlametrail.Color.B * value);
		}
		if(oldFlametrail.TrailStyle == 1)
		{
			if (value > 0.1f / 255f)
			{
				Lighting.AddLight(v2Pos + Main.screenPosition, new Vector3(1f, 0.6f, 0.4f) * value * 255f * 3);
			}
		}
		if (oldFlametrail.Scale < 0.05f)
		{
			oldFlametrail.Active = false;
		}
		oldFlametrail.FlameTimer++;
		return oldFlametrail;
	}
	public override void AI()
	{
		Timer++;
		foreach (FlameTrail flameTrail in Stars)
		{
			//if (!flameTrail.Active)
			//{
			//	Stars.Remove(flameTrail);
			//}
		}
		bool allKilled = true;
		for (int i = 0; i < Stars.Count; i++)
		{
			Stars[i] = UpdateFlameTrail(Stars[i]);
			if (Stars[i].Active)
			{
				allKilled = false;
			}
		}
		if(allKilled)
		{
			Projectile.Kill();
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D flame = Commons.ModAsset.LightPoint.Value;
		List<Vertex2D> trailBars0 = new List<Vertex2D>();
		List<Vertex2D> trailBars1 = new List<Vertex2D>();
		List<Vertex2D> flameBars = new List<Vertex2D>();
		foreach (FlameTrail flameTrail in Stars)
		{
			if(flameTrail.Active)
			{
				Vector3 drawPos3D = flameTrail.Postion;
				drawPos3D.X += (Projectile.Center - Main.screenPosition).X;
				drawPos3D.Y += (Projectile.Center - Main.screenPosition).Y;
				float scale;
				Vector2 v2Pos = Projection2D(drawPos3D, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 1000, out scale);

				Vector3[] trailPos = flameTrail.OldPos.Reverse<Vector3>().ToArray();
				int len = trailPos.Length;
				if (len <= 2)
					continue;

				for (int i = 1; i < len; i++)
				{
					float scale0;
					Vector3 oldPos3D0 = trailPos[i];
					oldPos3D0.X += (Projectile.Center - Main.screenPosition).X;
					oldPos3D0.Y += (Projectile.Center - Main.screenPosition).Y;
					Vector2 old0 = Projection2D(oldPos3D0, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 1000, out scale0);

					float scale1;
					Vector3 oldPos3D1 = trailPos[i - 1];
					oldPos3D1.X += (Projectile.Center - Main.screenPosition).X;
					oldPos3D1.Y += (Projectile.Center - Main.screenPosition).Y;
					Vector2 old1 = Projection2D(oldPos3D1, new Vector2(Main.screenWidth, Main.screenHeight) / 2, 1000, out scale1);

					Vector2 normal = old0 - old1;
					normal = Vector2.Normalize(normal).RotatedBy(MathHelper.PiOver2);

					Color light = Lighting.GetColor((int)((old0 + Main.screenPosition).X / 16f), (int)((old0 + Main.screenPosition).Y / 16f));
					light.A = 0;
					if (i == 1)
					{
						Vector2 addVel = normal.RotatedBy(-MathHelper.PiOver2) * scale0 * 5 * flameTrail.Scale;
						trailBars0.Add(old0 - addVel + normal * scale0 * 5 * flameTrail.Scale, Color.Transparent, new Vector3(0.5f, 0, 0));
						trailBars0.Add(old0 - addVel - normal * scale0 * 5 * flameTrail.Scale, Color.Transparent, new Vector3(0.5f, 1, 0));
					}
					trailBars0.Add(old0 + normal * scale0 * 5 * flameTrail.Scale * MathF.Log(i * 0.1f + 1), light * MathF.Pow(0.965f, i) * 0.25f, new Vector3(0.5f, 0, 0));
					trailBars0.Add(old0 - normal * scale0 * 5 * flameTrail.Scale * MathF.Log(i * 0.1f + 1), light * MathF.Pow(0.965f, i) * 0.25f, new Vector3(0.5f, 1, 0));
					if (i == len - 1)
					{
						Vector2 addVel = normal.RotatedBy(-MathHelper.PiOver2) * scale0 * 2 * flameTrail.Scale;
						trailBars0.Add(old0 + addVel + normal * scale0 * 5 * flameTrail.Scale, Color.Transparent, new Vector3(0.5f, 0, 0));
						trailBars0.Add(old0 + addVel - normal * scale0 * 5 * flameTrail.Scale, Color.Transparent, new Vector3(0.5f, 1, 0));
					}
					if (flameTrail.TrailStyle == 1)
					{
						float zValue = (TrailLength - i) / (float)(TrailLength);
						if(flameTrail.FlameTimer > 60)
						{
							zValue -= (flameTrail.FlameTimer - 60) / 60f;
							if(zValue < 0)
								zValue = 0;
						}
						float width = 1f;
						if(len < 75)
						{
							width *= len / 75f;
						}
						if (Timer - i < flameTrail.ai[1])
						{
							zValue = 0;
						}
						if (Timer - i < flameTrail.ai[1] + 10)
						{
							zValue *= (Timer - i - flameTrail.ai[1]) / 10f;
						}
						if (i == 1)
						{
							Vector2 addVel = normal.RotatedBy(-MathHelper.PiOver2) * scale0 * 5 * flameTrail.Scale;
							trailBars1.Add(old0 - addVel + normal * scale0 * 15 * flameTrail.Scale * MathF.Log(i * 0.1f + 1) * width, Color.Transparent, new Vector3(i / 40f, 0, 1));
							trailBars1.Add(old0 - addVel - normal * scale0 * 15 * flameTrail.Scale * MathF.Log(i * 0.1f + 1) * width, Color.Transparent, new Vector3(i / 40f, 1, 1));
						}
						trailBars1.Add(old0 + normal * scale0 * 15 * flameTrail.Scale * MathF.Log(i * 0.1f + 1) * width, new Color(1f, 0.7f, 0.4f, 0), new Vector3(i / 40f, 0, zValue));
						trailBars1.Add(old0 - normal * scale0 * 15 * flameTrail.Scale * MathF.Log(i * 0.1f + 1) * width, new Color(1f, 0.7f, 0.4f, 0), new Vector3(i / 40f, 1, zValue));
						if (i == len - 1)
						{
							Vector2 addVel = normal.RotatedBy(-MathHelper.PiOver2) * scale0 * 2 * flameTrail.Scale;
							trailBars1.Add(old0 + addVel + normal * scale0 * 15 * flameTrail.Scale * MathF.Log(i * 0.1f + 1) * width, Color.Transparent, new Vector3(i / 40f, 0, 0));
							trailBars1.Add(old0 + addVel - normal * scale0 * 15 * flameTrail.Scale * MathF.Log(i * 0.1f + 1) * width, Color.Transparent, new Vector3(i / 40f, 1, 0));
						}
					}
				}
				if(flameTrail.Color != new Color(0, 0, 0, 0))
				{
					float sizeValue = flame.Width * flameTrail.Scale * scale;
					if (flameTrail.ai[3] != 0 && flameTrail.FlameTimer > flameTrail.ai[3])
					{
						sizeValue *= MathF.Sin(flameTrail.FlameTimer * 0.5f + flameTrail.Postion.Z * 0.1f) + 1;
					}
					flameBars.Add(v2Pos + new Vector2(-0.5f, -0.5f) * sizeValue, Color.Transparent, new Vector3(0, 0, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, -0.5f) * sizeValue, Color.Transparent, new Vector3(1, 0, 0));

					flameBars.Add(v2Pos + new Vector2(-0.5f, -0.5f) * sizeValue, flameTrail.Color * 0.4f, new Vector3(0, 0, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, -0.5f) * sizeValue, flameTrail.Color * 0.4f, new Vector3(1, 0, 0));

					flameBars.Add(v2Pos + new Vector2(-0.5f, 0.5f) * sizeValue, flameTrail.Color * 0.4f, new Vector3(0, 1, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, 0.5f) * sizeValue, flameTrail.Color * 0.4f, new Vector3(1, 1, 0));

					flameBars.Add(v2Pos + new Vector2(-0.5f, 0.5f) * sizeValue, Color.Transparent, new Vector3(0, 1, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, 0.5f) * sizeValue, Color.Transparent, new Vector3(1, 1, 0));

					flameBars.Add(v2Pos + new Vector2(-0.5f, -0.5f) * sizeValue * flameTrail.Scale * 0.4f, Color.Transparent, new Vector3(0, 0, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, -0.5f) * sizeValue * flameTrail.Scale * 0.4f, Color.Transparent, new Vector3(1, 0, 0));

					flameBars.Add(v2Pos + new Vector2(-0.5f, -0.5f) * sizeValue * flameTrail.Scale * 0.4f, flameTrail.Color * 6f, new Vector3(0, 0, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, -0.5f) * sizeValue * flameTrail.Scale * 0.4f, flameTrail.Color * 6f, new Vector3(1, 0, 0));

					flameBars.Add(v2Pos + new Vector2(-0.5f, 0.5f) * sizeValue * flameTrail.Scale * 0.4f, flameTrail.Color * 6f, new Vector3(0, 1, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, 0.5f) * sizeValue * flameTrail.Scale * 0.4f, flameTrail.Color * 6f, new Vector3(1, 1, 0));

					flameBars.Add(v2Pos + new Vector2(-0.5f, 0.5f) * sizeValue * flameTrail.Scale * 0.4f, Color.Transparent, new Vector3(0, 1, 0));
					flameBars.Add(v2Pos + new Vector2(0.5f, 0.5f) * sizeValue * flameTrail.Scale * 0.4f, Color.Transparent, new Vector3(1, 1, 0));
				}
			}
		}
		Main.graphics.GraphicsDevice.Textures[0] = flame;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (trailBars0.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, trailBars0.ToArray(), 0, trailBars0.Count - 2);
		
		if (trailBars1.Count > 3)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
			Effect effect = ModAsset.FireworkTrailStyle1.Value;
			effect.Parameters["uPerlin"].SetValue(ModAsset.CorruptDustFadePowderII.Value);
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Main.GameViewMatrix.TransformationMatrix;
			effect.Parameters["uTransform"].SetValue(model * projection);
			effect.CurrentTechnique.Passes["Test"].Apply();
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, trailBars1.ToArray(), 0, trailBars1.Count - 2);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Main.graphics.GraphicsDevice.Textures[0] = flame;
		}

		if (flameBars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, flameBars.ToArray(), 0, flameBars.Count - 2);
		return false;
	}
}