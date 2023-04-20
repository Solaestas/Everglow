using Everglow.Myth.Common;
using Everglow.Myth.TheTusk;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Melee.Hepuyuan;

class XiaoHit : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 68;
		Projectile.height = 68;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 120;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
	}
	public override void AI()
	{
		Projectile.velocity *= 0;
		for (int i = 0; i < 23; i++)
		{
			if (DrawLine[i, 0] == Vector2.Zero)
			{
				for (int j = 0; j < 18; j++)
				{
					DrawLine[i, j] = Projectile.Center;
				}
				DrawLineVelocity[i] = new Vector2(Main.rand.NextFloat(17f, 27f), 0).RotatedByRandom(6.283) * Projectile.ai[0];
			}
			DrawLine[i, 0] += DrawLineVelocity[i];
			DrawLineVelocity[i] *= 0.9f;
			for (int j = 17; j > 0; j--)//记录每一组流星火位置
			{
				DrawLine[i, j] = DrawLine[i, j - 1];
			}
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}


	internal Vector2[,] DrawLine = new Vector2[23, 18];
	internal Vector2[] DrawLineVelocity = new Vector2[23];
	public override void PostDraw(Color lightColor)
	{
		Effect ef = MythContent.QuickEffect("Effects/FadeCurseGreen");

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		float widx = Projectile.timeLeft / 120f;
		float widxM = 1f - widx;


		for (int i = 0; i < 23; i++)
		{

			var barsII = new List<VertexBase.CustomVertexInfo>();


			for (int z = 1; z < 18; ++z)
			{
				float widthII = Math.Clamp((DrawLine[i, z] - DrawLine[i, z - 1]).Length(), 0, 20 * Projectile.ai[0]);//宽度为距离(速度决定,上限20)
				if (z > 13)
					widthII *= (18 - z) / 5f;
				var normalDir = Vector2.Normalize(DrawLine[i, z] - DrawLine[i, z - 1]).RotatedBy(1.57);
				var factor = z / 18f;
				var w = MathHelper.Lerp(1f, 0.05f, 0.5f);
				barsII.Add(new VertexBase.CustomVertexInfo(DrawLine[i, z] + normalDir * widthII - Main.screenPosition, new Color(0, 0.7f, 1f, 0.5f), new Vector3((float)Math.Sqrt(factor), 1, w)));
				barsII.Add(new VertexBase.CustomVertexInfo(DrawLine[i, z] + normalDir * -widthII - Main.screenPosition, new Color(0, 0.7f, 1f, 0.5f), new Vector3((float)Math.Sqrt(factor), 0, w)));
			}

			var Vx = new List<VertexBase.CustomVertexInfo>();

			if (barsII.Count > 2)
			{
				Vx.Add(barsII[0]);
				var vertex = new VertexBase.CustomVertexInfo(DrawLine[i, 0], new Color(0, 0.7f, 1f, 0.5f), new Vector3(0, 0.5f, 1));
				Vx.Add(barsII[1]);
				Vx.Add(vertex);
				for (int z = 0; z < barsII.Count - 2; z += 2)
				{
					Vx.Add(barsII[z]);
					Vx.Add(barsII[z + 2]);
					Vx.Add(barsII[z + 1]);

					Vx.Add(barsII[z + 1]);
					Vx.Add(barsII[z + 2]);
					Vx.Add(barsII[z + 3]);
				}
				RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;


				Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("UIImages/VisualTextures/EShoot");


				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);

				Main.graphics.GraphicsDevice.RasterizerState = originalState;

			}
		}
	}
}
