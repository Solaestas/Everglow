using Everglow.Commons.DataStructures;
using Everglow.Commons.Enums;
using Everglow.Commons.MEAC;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Everglow.MEAC.NonTrueMeleeProj;

public class GoldShield : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1200;
		Projectile.penetrate = -1;
	}
	public void DrawWarp(VFXBatch spriteBatch)
	{
		float WaveRange = 0.7f;
		//Texture2D BackG = ModContent.Request<Texture2D>("Everglow/MEAC/NonTrueMeleeProj/Black").Value;

		float k0 = (float)Math.Sqrt(1200 - Projectile.timeLeft) / 6f;//画方波
		if (k0 is < 1 and > 0)
		{
			k0 = Math.Max(k0 - 0.025f, 0);
			float k1 = 1 - k0;
			float k2 = k1 * k1;
			float k3 = (float)Math.Sqrt(k1);
			Vector2 DrawCen = Projectile.Center - Main.screenPosition;

			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(spriteBatch, DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
		}
	}
	public void DrawDoubleLine(VFXBatch spriteBatch, Vector2 StartPos, Vector2 EndPos, Color color1, Color color2)
	{
		Vector2 DrawCen = Projectile.Center - Main.screenPosition;
		float Wid = (Projectile.timeLeft - 1170) / 2f;
		Vector2 WidthS = Vector2.Normalize(StartPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
		Vector2 WidthE = Vector2.Normalize(EndPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
		var vertex2Ds = new List<Vertex2D>();

		for (int x = 0; x < 3; x++)
		{
			vertex2Ds.Add(new Vertex2D(StartPos + WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos + WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));

			vertex2Ds.Add(new Vertex2D(EndPos + WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos - WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
		}

		spriteBatch.Draw(TextureAssets.MagicPixel.Value, vertex2Ds, PrimitiveType.TriangleList);
	}
	public void DrawDoubleLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2)
	{
		Vector2 DrawCen = Projectile.Center - Main.screenPosition;
		float Wid = (Projectile.timeLeft - 1170) / 2f;
		Vector2 WidthS = Vector2.Normalize(StartPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
		Vector2 WidthE = Vector2.Normalize(EndPos - DrawCen).RotatedBy(Math.PI / 2d) * Wid;
		var vertex2Ds = new List<Vertex2D>();

		for (int x = 0; x < 3; x++)
		{
			vertex2Ds.Add(new Vertex2D(StartPos + WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos + WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));

			vertex2Ds.Add(new Vertex2D(EndPos + WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(EndPos - WidthE + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(0, 0, 0)));
			vertex2Ds.Add(new Vertex2D(StartPos - WidthS + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
		}


		Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
	}

	public void DrawPost(Color color, int Width, float Height, float StarPos, Texture2D tex)
	{
		var vertex2Ds = new List<Vertex2D>();
		Vector2 DrawCen = Main.player[Projectile.owner].Center - Main.screenPosition - new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
		if (DrawCen.Length() < 5f)
		{
			DrawCen = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
		}
		else
		{
			DrawCen = Main.player[Projectile.owner].Center - Main.screenPosition;
		}

		float SPos = StarPos;
		float R = Width;
		for (int x = -Width; x < Width; x++)
		{
			float y = (float)Math.Sqrt(R * R - x * x);
			float newy = (float)Math.Sqrt(R * R - (x + 1) * (x + 1));

			float r1 = (float)(Math.Acos(Math.Clamp(x / R, -1, 1)) / Math.PI);
			float r2 = (float)(Math.Acos(Math.Clamp((x + 1) / R, -1, 1)) / Math.PI);

			float deltaY = newy - y;
			float length = (float)Math.Sqrt(deltaY * deltaY + 1);

			//vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x - 0.5f, Height), color, new Vector3(SPos, 1, 0)));
			//vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x + 0.5f, Height), color, new Vector3(SPos + length, 1, 0)));
			//vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x + 0.5f, -Height), color, new Vector3(SPos + length, 0, 0)));

			//vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x + 0.5f, -Height), color, new Vector3(SPos + length, 0, 0)));
			//vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x - 0.5f, Height), color, new Vector3(SPos, 1, 0)));
			//vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x - 0.5f, -Height), color, new Vector3(SPos, 0, 0)));

			const float scale = 0.19f;
			const float frequency = 0.3f;

			vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale - 0.5f, Height), color, new Vector3(r1 * frequency, 1, 0)));
			vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale + 0.5f, Height), color, new Vector3(r2 * frequency, 1, 0)));
			vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale + 0.5f, -Height), color, new Vector3(r2 * frequency, 0, 0)));

			vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale + 0.5f, -Height), color, new Vector3(r2 * frequency, 0, 0)));
			vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale - 0.5f, Height), color, new Vector3(r1 * frequency, 1, 0)));
			vertex2Ds.Add(new Vertex2D(DrawCen + new Vector2(x * scale - 0.5f, -Height), color, new Vector3(r1 * frequency, 0, 0)));


			SPos += length;
		}

		Main.graphics.GraphicsDevice.Textures[0] = tex;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
	}
	public override void AI()
	{
		if (Projectile.ai[0] > 0)
		{
			Projectile.ai[0]--;
		}
		else
		{
			Projectile.ai[0] = 0;
		}
		Projectile.Center = Main.player[Projectile.owner].Center;
		Projectile.hide = true;
		Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0.8f, 0.6f, 0);
	}
	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		overPlayers.Add(index);

	}
	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
		int LeftTime = 1200 - Projectile.timeLeft;
		float glowStrength = 0;
		float glowStrength2 = 0;

		if (Projectile.ai[0] > 0)
		{
			if (Projectile.ai[0] < 10)
				glowStrength2 = (float)(-Math.Cos(Projectile.ai[0] / 5d * Math.PI) + 1) * 120f;
		}


		if (LeftTime < 10)
		{
			glowStrength = (float)(-Math.Cos(LeftTime / 10d * Math.PI) + 1) * 120f;
		}
		else if (LeftTime < 40)
		{
			glowStrength = (float)(-Math.Cos((LeftTime + 75) / 30d * Math.PI) + 1) * 120f;
		}

		if (Projectile.timeLeft < 10)
		{
			glowStrength = (float)(-Math.Cos(Projectile.timeLeft / 10d * Math.PI) + 1) * 120f;
		}
		else if (Projectile.timeLeft < 40)
		{
			glowStrength = (float)(-Math.Cos((Projectile.timeLeft + 75) / 30d * Math.PI) + 1) * 120f;
		}
		if (glowStrength + glowStrength2 > 0)//光效
		{
			for (int x = 0; x < glowStrength + glowStrength2; x++)
			{
				Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, -12) + new Vector2(x / 20f).RotatedBy(x), null, new Color(2, 2, 2, 0), Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), 1, SpriteEffects.None, 0);
			}
		}
		if (Projectile.timeLeft <= 9 && Projectile.timeLeft >= 0 && Projectile.timeLeft % 3 == 0)
		{
			for (int x = 0; x < 12; x++)
			{
				float X = (float)Math.Sqrt(Main.rand.NextFloat(0, 0.5f));
				float Y = (float)Math.Sqrt(Main.rand.NextFloat(0, 0.5f));
				var v0 = new Vector2(X * Math.Sign(Main.rand.NextFloat(-1, 1)) * 38, Y * Math.Sign(Main.rand.NextFloat(-1, 1)) * 38);
				int k = Dust.NewDust(Projectile.Center + v0 - new Vector2(4) + new Vector2(0, -12), 0, 0, DustID.GoldFlame, 0, 0, 0, default, Main.rand.NextFloat(0.8f, 2f));
				Main.dust[k].noGravity = true;

			}
		}
		Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0, -12), null, new Color(255, 255, 255, 0), Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), 1, SpriteEffects.None, 0);
		Texture2D rt = GoldShieldTargetLoad.ShieldTexture;
		if(rt != null)
		{
			Main.spriteBatch.Draw(rt, Projectile.Center - Main.screenPosition + new Vector2(0, -12), null, new Color(255, 255, 255, 0), Projectile.rotation, rt.Size() / 2f, 1, SpriteEffects.None, 0);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect Post = ModContent.Request<Effect>("Everglow/MEAC/Effects/Post", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		Post.Parameters["uTime"].SetValue((float)(Main.timeForVisualEffects * 0.003));
		Post.CurrentTechnique.Passes[0].Apply();

		Texture2D StoneSquire = ModAsset.GoldShieldGlowMap.Value;
		DrawPost(new Color(255, 255, 255, 0), 200, 50, 1, StoneSquire);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Post.CurrentTechnique.Passes[0].Apply();
		Texture2D StoneSquireD = ModAsset.GoldShieldDarkMap.Value;
		DrawPost(new Color(255, 255, 255, 155), 200, 50, 1, StoneSquireD);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


		float WaveRange = 0.7f;
		//Texture2D BackG = ModContent.Request<Texture2D>("Everglow/MEAC/NonTrueMeleeProj/Black").Value;

		float k0 = (float)Math.Sqrt(1200 - Projectile.timeLeft) / 6f;//画方波
		if (k0 is < 1 and > 0)
		{
			k0 = Math.Max(k0 - 0.025f, 0);
			float k1 = 1 - k0;
			float k2 = k1 * k1;
			float k3 = (float)Math.Sqrt(k1);
			Vector2 DrawCen = Projectile.Center - Main.screenPosition;


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			Effect KEx = Everglow.Commons.ModAsset.DrawWarp.Value;
			KEx.CurrentTechnique.Passes[0].Apply();

			DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(-k0 * 75, -k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

			DrawDoubleLine(DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(0, k0 * 150) * WaveRange, DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(-k0 * 75, k0 * 75) * WaveRange, DrawCen + new Vector2(-k0 * 150, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			KEx.CurrentTechnique.Passes[0].Apply();

			DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 120) * WaveRange, DrawCen + new Vector2(k0 * 60, -k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(k0 * 60, -k0 * 60) * WaveRange, DrawCen + new Vector2(k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(0, -k0 * 120) * WaveRange, DrawCen + new Vector2(-k0 * 60, -k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(-k0 * 60, -k0 * 60) * WaveRange, DrawCen + new Vector2(-k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));

			DrawDoubleLine(DrawCen + new Vector2(0, k0 * 120) * WaveRange, DrawCen + new Vector2(k0 * 60, k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(k0 * 60, k0 * 60) * WaveRange, DrawCen + new Vector2(k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(0, k0 * 120) * WaveRange, DrawCen + new Vector2(-k0 * 60, k0 * 60) * WaveRange, new Color(1f * k3, 0.6f * k3, 0f, 0f), new Color(1f * k2, 0.7f * k2, 0f, 0f));
			DrawDoubleLine(DrawCen + new Vector2(-k0 * 60, k0 * 60) * WaveRange, DrawCen + new Vector2(-k0 * 120, 0) * WaveRange, new Color(1f * k2, 0.7f * k2, 0f, 0f), new Color(1f * k3, 0.6f * k3, 0f, 0f));
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
		return false;
	}
	
}
public class GoldShieldTargetLoad : ModSystem
{
	public static RenderTarget2D shieldScreen;
	public override void Load()
	{
		Ins.MainThread.AddTask(() =>
		{
			AllocateRenderTarget(new Vector2(Main.screenWidth, Main.screenHeight));
		});
		Ins.HookManager.AddHook(CodeLayer.ResolutionChanged, (Vector2 size) =>
		{
			shieldScreen?.Dispose();
			AllocateRenderTarget(size);
		}, "Realloc RenderTarget");

		Ins.HookManager.AddHook(CodeLayer.PostDrawProjectiles, TheBackgroundTextureOfShield);
	}

	public void AllocateRenderTarget(Vector2 size)
	{
		var gd = Main.instance.GraphicsDevice;
		shieldScreen = new RenderTarget2D(gd, (int)size.X, (int)size.Y, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
	}

	private static void TheBackgroundTextureOfShield()
	{
		//if(Main.instance.GraphicsDevice == null)
		//{
		//	return;
		//}
		//var gDevice = Main.instance.GraphicsDevice;
		//SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		//Main.spriteBatch.End();
		//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
		//var cur = Main.screenTarget;


		//gDevice.SetRenderTarget(shieldScreen);
		//gDevice.Clear(Color.Transparent);
		////尝试绘制花纹
		//float timeValue = (float)Main.timeForVisualEffects * 0.02f;
		//Texture2D texPiece = ModAsset.GoldShieldScale_dark.Value;
		//Texture2D texPieceBack = ModAsset.GoldShieldScale_light.Value;
		//Vector2 drawPos = new Vector2(0);
		//for (int i = 0; i < 6; i++)
		//{
		//	float time2 = timeValue % 1f;
		//	float phi = (time2 + i) / 6f;
		//	phi = MathF.Sin(phi * MathF.PI);
		//	float distance = time2 * 10 + i * 10;
		//	Main.EntitySpriteDraw(texPieceBack, drawPos + new Vector2(distance - 60, distance + 20), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);
		//	Main.EntitySpriteDraw(texPieceBack, drawPos + new Vector2(distance - 60, -distance - 20), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);
		//	Main.EntitySpriteDraw(texPieceBack, drawPos + new Vector2(distance + 60, distance - 80), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);
		//	Main.EntitySpriteDraw(texPieceBack, drawPos + new Vector2(distance + 60, -distance + 80), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);

		//	Main.EntitySpriteDraw(texPiece, drawPos + new Vector2(distance + 60, distance + 20), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);
		//	Main.EntitySpriteDraw(texPiece, drawPos + new Vector2(distance + 60, -distance - 20), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);
		//	Main.EntitySpriteDraw(texPiece, drawPos + new Vector2(distance - 60, distance - 80), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);
		//	Main.EntitySpriteDraw(texPiece, drawPos + new Vector2(distance - 60, -distance + 80), null, Color.White, 0, texPiece.Size() / 2f, phi, SpriteEffects.None);
		//}
		//Main.spriteBatch.End();
		//gDevice.SetRenderTarget(Main.screenTarget);
		//ShieldTexture = shieldScreen;
		//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
		//Main.spriteBatch.Draw(cur, Vector2.Zero, Color.White);
		//Main.spriteBatch.End();
		//Main.spriteBatch.Begin(sBS);
	}
	public static Texture2D ShieldTexture;
}

public class GlodShieldPlayer : ModPlayer
{

	public int GlodShieldDurability;
	public bool Dodge;

	/*public override bool FreeDodge(Player.HurtInfo info)
	{
		
	}*/
	public override bool FreeDodge(Player.HurtInfo info)
	{
		if (Dodge)
		{
			Dodge = false;
			return true;
		}
		return base.FreeDodge(info);
	}

	public void PreHurt(ref Player.HurtInfo info)
	{
		GlodShieldDurability = 0;
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active & proj.owner == Player.whoAmI & proj.type == ModContent.ProjectileType<GoldShield>())
			{
				GlodShieldDurability = (int)proj.ai[1];
				if (GlodShieldDurability >= info.Damage)
				{
					Dodge = true;
					this.GlodShieldDurability -= (int)info.Damage;
					info.Damage *= 0;
				}
				else
				{
					info.Damage -= GlodShieldDurability;
					this.GlodShieldDurability *= 0;
				}
				Main.player[proj.owner].immune = true;
				Main.player[proj.owner].immuneTime = 30;
				Main.player[proj.owner].noKnockback = true;
				proj.ai[1] = GlodShieldDurability;
				if (proj.ai[1] <= 0)
				{
					proj.ai[1] = 0;
					proj.ai[0] = 10;
					proj.timeLeft = 15;
				}
			}
		}
	}
	public override void ModifyHurt(ref Player.HurtModifiers modifiers)
	{
		modifiers.ModifyHurtInfo += new Player.HurtModifiers.HurtInfoModifier(this.PreHurt);
		if (GlodShieldDurability > 0)
		{
			modifiers.DisableDust();
			modifiers.DisableSound();
		}
	}
}
