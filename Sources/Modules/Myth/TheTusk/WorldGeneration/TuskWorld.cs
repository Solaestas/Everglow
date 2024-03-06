using Everglow.Commons.Menu;
using Everglow.Myth.TheFirefly.WorldGeneration;
using SubworldLibrary;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Everglow.Myth.TheTusk.WorldGeneration;

internal class TuskWorld : Subworld
{
	public override int Width => 400;
	public override int Height => 300;
	public override bool NormalUpdates => true;
	public override bool ShouldSave => false;
	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new TuskGen.SubWorldTuskLandGenPass()
	};
	public EverglowModMenu EVGMenu = new();
	public List<Star> stars = new();
	public override void Load()
	{

	}
	public override void Unload()
	{
		stars = null;
	}
	/// <summary>
	/// RenderTarget2D Background
	/// </summary>
	RenderTarget2D rt2dbg;
	float t = 0;
	public Vector2 scale => new Vector2(Main.UIScale * Main.screenWidth / 1920f, Main.UIScale * Main.screenHeight / 1080f);
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
			Texture2D MenuUG2SBG = ModAsset.Lightning.Value;
			Main.spriteBatch.Draw(MenuUG2SBG, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Rectangle(0, 0, MenuUG2SBG.Width, MenuUG2SBG.Height), Color.White);
		}
		base.DrawMenu(gameTime);
	}
	public override void OnEnter()
	{
		SubworldSystem.hideUnderworld = true;
	}
	public override void OnLoad()
	{
		Main.worldSurface = 250;
		Main.rockLayer = 300;
		GenVars.waterLine = 260;
	}
}
