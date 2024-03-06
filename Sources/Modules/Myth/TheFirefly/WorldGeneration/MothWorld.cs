using Terraria.WorldBuilding;
using SubworldLibrary;
using ReLogic.Content;
using Everglow.Myth.Common;
using Microsoft.Xna.Framework.Graphics;
using Star = Everglow.Commons.Menu.Entities.Star;
using Everglow.Commons.Menu.Entities;
using Everglow.Commons.Menu;

namespace Everglow.Myth.TheFirefly.WorldGeneration;

internal class MothWorld : Subworld
{
	public override int Width => 800;
	public override int Height => 600;
	public override bool NormalUpdates => true;
	public override bool ShouldSave => true;
	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new MothLand.MothLandGenPass()
	};

	public const string bgPath = "Everglow/Commons/Menu/bg";
	public static Asset<Texture2D> bg => ModContent.Request<Texture2D>(bgPath, AssetRequestMode.ImmediateLoad);
	public const string MoonPath = "Everglow/Commons/Menu/Moon";
	public static Asset<Texture2D> Moon => ModContent.Request<Texture2D>(MoonPath, AssetRequestMode.ImmediateLoad);
	public const string waterPath = "Everglow/Commons/Menu/water";
	public static Asset<Texture2D> water => ModContent.Request<Texture2D>(waterPath, AssetRequestMode.ImmediateLoad);
	public const string front1Path = "Everglow/Commons/Menu/front1";
	public static Asset<Texture2D> front1 => ModContent.Request<Texture2D>(front1Path, AssetRequestMode.ImmediateLoad);
	public const string front2Path = "Everglow/Commons/Menu/front2";
	public static Asset<Texture2D> front2 => ModContent.Request<Texture2D>(front2Path, AssetRequestMode.ImmediateLoad);
	public const string 上层Path = "Everglow/Commons/Menu/上层";
	public static Asset<Texture2D> 上层 => ModContent.Request<Texture2D>(上层Path, AssetRequestMode.ImmediateLoad);
	public EverglowModMenu EVGMenu = new();
	public List<Star> stars = new();
	public override void Load()
	{

	}
	public override void Unload()
	{
		stars=null;
	}
	/// <summary>
	/// RenderTarget2D Background
	/// </summary>
	RenderTarget2D rt2dbg;
	float t = 0;
	public Vector2 scale => new Vector2(Main.UIScale * Main.screenWidth / 1920f, Main.UIScale * Main.screenHeight / 1080f);
	//public void Update()
	//{
	//	for (int i = 0; i < stars.Count; i++)
	//	{
	//		Star star = stars[i];
	//		star.Update();
	//		star.timeLeft--;
	//		if (star.timeLeft <= 0)
	//		{
	//			stars.Remove(star);
	//		}
	//	}

	//	if (Main.rand.NextBool(25))//生成流星
	//	{
	//		TrailingStar star = new TrailingStar();

	//		byte c = (byte)(50 + Main.rand.Next(100));
	//		star.color.R -= c;
	//		star.color.G -= c;
	//		star.maxTime = 100;
	//		star.timeLeft = star.maxTime;
	//		star.scale *= Main.rand.NextFloat(2.5f);
	//		star.position = new Vector2(Main.rand.Next(Main.screenWidth + 350), -100) * Main.UIScale;
	//		star.velocity = new Vector2(-12 - Main.rand.Next(20), 8 + Main.rand.Next(25)) * 0.8f;
	//		stars.Add(star);
	//	}
	//	//生成旋转星星
	//	//if (Main.rand.NextBool())
	//	{
	//		Star star = new Star();

	//		byte c = (byte)(50 + Main.rand.Next(120));
	//		star.color.R -= c;
	//		star.color.G -= c;
	//		star.scale *= Main.rand.NextFloat(2);
	//		star.maxTime = 200 + Main.rand.Next(200);
	//		star.timeLeft = star.maxTime;

	//		star.position = new Vector2(Main.rand.Next(Main.screenWidth), Main.rand.Next(Main.screenHeight)) * Main.UIScale;

	//		stars.Add(star);
	//	}
	//}
	//public void DrawStars()
	//{
	//	Main.spriteBatch.End();
	//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);


	//	foreach (Star star in stars)
	//	{
	//		star.Draw();
	//	}

	//	Main.spriteBatch.End();
	//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
	//}
	//public void DrawBackGround()//天、星月
	//{
	//	float rotSpeed = 0.0007f;
	//	SpriteBatch spriteBatch = Main.spriteBatch;
	//	t++;
	//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

	//	Texture2D tex = Terraria.GameContent.TextureAssets.MagicPixel.Value;
	//	tex = MothWorld.bg.Value;
	//	spriteBatch.Draw(tex, scale.X * new Vector2(960, 820), null, Color.White, t * rotSpeed, tex.Size() / 2, scale.X, 0, 0);

	//	Main.spriteBatch.End();
	//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

	//	Update();

	//	DrawStars();

	//	Main.spriteBatch.End();
	//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

	//	Texture2D moon = MothWorld.Moon.Value;
	//	//Vector2 moonVec = new Vector2(872, 628) - new Vector2(1250, 1250);
	//	Vector2 moonVec = new Vector2(680, 920) - new Vector2(1250, 1250);
	//	spriteBatch.Draw(moon, scale.X * (new Vector2(960, 820) + moonVec.RotatedBy(t * rotSpeed)), null, Color.White, t * rotSpeed, moon.Size() / 2, 1.05f * scale.X, 0, 0);

	//	Main.spriteBatch.End();
	//}
	public override void DrawMenu(GameTime gameTime)
	{
		if (EverglowModMenu.InModMenu)
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			Main.spriteBatch.End();

			var renderTargets = Ins.RenderTargetPool.GetRenderTarget2DArray(1);
			rt2dbg = renderTargets.Resource[0];

			Main.graphics.GraphicsDevice.SetRenderTarget(rt2dbg);//在rt上绘制背景，方便水反
			Main.graphics.GraphicsDevice.Clear(Color.Black);

			EVGMenu.DrawBackGround();

			Main.graphics.GraphicsDevice.SetRenderTarget(null);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			Main.spriteBatch.Draw(rt2dbg, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);


			//绘制水以及反射的天空
			Texture2D water = MothWorld.water.Value;
			spriteBatch.Draw(water, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * 0.95f);
			Rectangle rec = new Rectangle(864, 729, 592, 168);
			rec.X = (int)(rec.X * Main.UIScale * Main.screenWidth / 1920f);
			rec.Y = (int)(rec.Y * Main.UIScale * Main.screenHeight / 1080f);
			Vector2 size = new Vector2(rt2dbg.Width, rt2dbg.Height);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
			Vertex2D[] vertex = new Vertex2D[] {
			new Vertex2D(new Vector2(rec.X,rec.Y),Color.White*0.7f,new Vector3(rec.X/size.X,rec.Y/size.Y,0)),
			new Vertex2D(new Vector2(rec.X,rec.Y+rec.Height),Color.White*0.2f,new Vector3(rec.X/size.X,(rec.Y-rec.Height)/size.Y,0)),
			new Vertex2D(new Vector2(rec.X+rec.Width,rec.Y),Color.White*0.7f,new Vector3((rec.X+rec.Width)/size.X,rec.Y/size.Y,0)),
			new Vertex2D(new Vector2(rec.X+rec.Width,rec.Y+rec.Height),Color.White*0.2f,new Vector3((rec.X+rec.Width)/size.X,(rec.Y-rec.Height)/size.Y,0))
			};

			Main.graphics.GraphicsDevice.Textures[0] = rt2dbg;

			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex, 0, 2);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			//绘制前置两层
			Texture2D front2 = MothWorld.front2.Value;
			spriteBatch.Draw(front2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

			Texture2D front1 = MothWorld.front1.Value;
			spriteBatch.Draw(front1, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

			//TODO 绘制标题字
			Texture2D front0 = MothWorld.上层.Value;
			spriteBatch.Draw(front0, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);


			rt2dbg = null;
			renderTargets.Release();
		}
		else
		{
			Texture2D MenuUG2SBG = ModAsset.FireflyUnderground1Screen4K.Value;
			Vector2 zero = Vector2.Zero;
			#region MenuUGS2BG
			float uG1Width = (float)Main.screenWidth / (float)MenuUG2SBG.Width;
			float uG1Height = (float)Main.screenHeight / (float)MenuUG2SBG.Height;
			if (uG1Width != uG1Height)
			{
				if (uG1Height > uG1Width)
				{
					uG1Width = uG1Height;
					zero.X -= ((float)MenuUG2SBG.Width * uG1Width - (float)Main.screenWidth) * 0.5f;
				}
				else
				{
					zero.Y -= ((float)MenuUG2SBG.Height * uG1Width - (float)Main.screenHeight) * 0.5f;
				}
			}
			#endregion
			#region MenuSkyBG
			//float skyWidth = (float)Main.screenWidth / (float)MenuSkyBG.Width;
			//float shyHeight = (float)Main.screenHeight / (float)MenuSkyBG.Height;
			//if (skyWidth != shyHeight)
			//{
			//	if (shyHeight > skyWidth)
			//	{
			//		skyWidth = shyHeight;
			//		zero.X -= ((float)MenuSkyBG.Width * skyWidth - (float)Main.screenWidth) * 0f;
			//	}
			//	else
			//	{
			//		zero.Y -= ((float)MenuSkyBG.Height * skyWidth - (float)Main.screenHeight) * 0f;
			//	}
			//}
			#endregion
			#region MenuFarBG
			//float farWidth = (float)Main.screenWidth / (float)MenuFarBG.Width;
			//float farHeight = (float)Main.screenHeight / (float)MenuFarBG.Height;
			//if (farWidth != farHeight)
			//{
			//	if (farHeight > farWidth)
			//	{
			//		farWidth = farHeight;
			//		zero.X -= ((float)MenuFarBG.Width * farWidth - (float)Main.screenWidth) * 0f;
			//	}
			//	else
			//	{
			//		zero.Y -= ((float)MenuFarBG.Height * farWidth - (float)Main.screenHeight) * 0f;
			//	}
			//}
			#endregion
			#region MenuMidCloseBG
			//float midCloseWidth = (float)Main.screenWidth / (float)MenuMidCloseBG.Width;
			//float midCloseHeight = (float)Main.screenHeight / (float)MenuMidCloseBG.Height;
			//if (midCloseWidth != midCloseHeight)
			//{
			//	if (midCloseHeight > midCloseWidth)
			//	{
			//		midCloseWidth = midCloseHeight;
			//		zero.X -= ((float)MenuMidCloseBG.Width * midCloseWidth - (float)Main.screenWidth) * 0f;
			//	}
			//	else
			//	{
			//		zero.Y -= ((float)MenuMidCloseBG.Height * midCloseWidth - (float)Main.screenHeight) * 0f;
			//	}
			//}
			#endregion
			#region MenuMiddleBG
			//float middleWidth = (float)Main.screenWidth / (float)MenuMiddleBG.Width;
			//float middleHeight = (float)Main.screenHeight / (float)MenuMiddleBG.Height;
			//if (middleWidth != middleHeight)
			//{
			//	if (middleHeight > middleWidth)
			//	{
			//		middleWidth = middleHeight;
			//		zero.X -= ((float)MenuMiddleBG.Width * middleWidth - (float)Main.screenWidth) * 0f;
			//	}
			//	else
			//	{
			//		zero.Y -= ((float)MenuMiddleBG.Height * middleWidth - (float)Main.screenHeight) * 0f;
			//	}
			//}
			#endregion
			#region MenuMiddleGlowBG
			//float middleGlowWidth = (float)Main.screenWidth / (float)MenuMiddleGlowBG.Width;
			//float middleGlowHeight = (float)Main.screenHeight / (float)MenuMiddleGlowBG.Height;
			//if (middleGlowWidth != middleGlowHeight)
			//{
			//	if (middleGlowHeight > middleGlowWidth)
			//	{
			//		middleGlowWidth = middleGlowHeight;
			//		zero.X -= ((float)MenuMiddleGlowBG.Width * middleGlowWidth - (float)Main.screenWidth) * 0f;
			//	}
			//	else
			//	{
			//		zero.Y -= ((float)MenuMiddleGlowBG.Height * middleGlowWidth - (float)Main.screenHeight) * 0f;
			//	}
			//}
			#endregion

			Main.spriteBatch.Draw(MenuUG2SBG, zero, null, Color.White, 0f, Vector2.Zero, uG1Width, (SpriteEffects)0, 0f);
		}
		base.DrawMenu(gameTime);
	}
	public override bool ChangeAudio()
	 //TODO: MothBiomeOld should play when entering and exiting the firefly subworld but MothBiome should play while inside the subworld.
	{
		Main.newMusic = MythContent.QuickMusic("MothBiomeOld");
		if (SubworldSystem.Enter<MothWorld>())
		{
			Main.newMusic = MythContent.QuickMusic("MothBiome");
			return true;
		}
		else
		{

		}
		return false;
	}
	public override void OnEnter()
	{
		SubworldSystem.hideUnderworld = true;
	}
	public override void OnLoad()
	{
		Main.worldSurface = 20;
		Main.rockLayer = 150;
		GenVars.waterLine = 50;
	}
}
