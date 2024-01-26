using Everglow.Commons.Enums;
using Everglow.Commons.Interfaces;
using Everglow.Commons.Vertex;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Commons.VFX.Pipelines;

/// <summary>
/// 屏幕空间反射Pipeline,以绘制的图像作为法线贴图反射屏幕前的信息
/// </summary>
public class ScreenReflectionPipeline : Pipeline
{
	private RenderTarget2D saveScreenTarget;//保存的原始屏幕
	private RenderTarget2D screenReflectionScreen;//反射法线RenderTarget

	public override void Load()
	{
		Ins.MainThread.AddTask(() =>
		{
			AllocateRenderTarget(new Vector2(Main.screenWidth, Main.screenHeight));
		});
		Ins.HookManager.AddHook(CodeLayer.ResolutionChanged, (Vector2 size) =>
		{
			saveScreenTarget?.Dispose();
			screenReflectionScreen?.Dispose();
			AllocateRenderTarget(size);
		}, "Realloc RenderTarget");
		effect = VFXManager.DefaultEffect;
	}

	private void AllocateRenderTarget(Vector2 size)
	{
		var gd = Main.instance.GraphicsDevice;
		saveScreenTarget = new RenderTarget2D(gd, (int)size.X, (int)size.Y, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
		screenReflectionScreen = new RenderTarget2D(gd, (int)size.X, (int)size.Y, false, gd.PresentationParameters.BackBufferFormat, DepthFormat.None);
	}
	public override void BeginRender()
	{
		var sb = Main.spriteBatch;
		var gd = Main.instance.GraphicsDevice;
		//保存原画

		var cur = Main.screenTarget;
		gd.SetRenderTarget(saveScreenTarget);
		gd.Clear(Color.Transparent);
		sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.Invert(Main.GameViewMatrix.TransformationMatrix));
		sb.Draw(cur, Vector2.Zero, Color.White);
		sb.End();
		//切换到镜面层

		//切换回原屏幕
		gd.SetRenderTarget(Main.screenTarget);
		gd.Clear(Color.Transparent);
		sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.Invert(Main.GameViewMatrix.TransformationMatrix));
		sb.Draw(saveScreenTarget, Vector2.Zero, Color.White);

		sb.End();
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointClamp, RasterizerState.CullNone);
		Effect ef = ModAsset.ScreenReflection_NormalColor.Value;
		var viewport = Main.graphics.GraphicsDevice.Viewport;
		var projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 500);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0));

		ef.Parameters["uFresnelF0"].SetValue(new Vector3(0.17f));
		ef.Parameters["uKs"].SetValue(new Vector3(1.0f));
		ef.Parameters["uScreenDistanceMultipler"].SetValue(Vector2.One);
		ef.Parameters["uViewportSize"].SetValue(new Vector2(saveScreenTarget.Width,
			saveScreenTarget.Height));
		ef.Parameters["uModel"].SetValue(model);
		ef.Parameters["uMNormal"].SetValue(Matrix.Identity);
		ef.Parameters["uViewProj"].SetValue(Main.GameViewMatrix.TransformationMatrix * projection);

		ef.CurrentTechnique.Passes["Test"].Apply();

		Main.graphics.GraphicsDevice.Textures[1] = saveScreenTarget;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
	}
	public override void Render(IEnumerable<IVisual> visuals)
	{
		BeginRender();
		foreach (var visual in visuals)
		{
			visual.Draw();
		}
		Ins.Batch.End();
		EndRender();
	}
	public override void EndRender()
	{

	}

	/// <summary>
	/// 创建一个ABC逆时针组成的三角形面，并且生成其法线信息
	/// </summary>
	/// <param name="A"></param>
	/// <param name="B"></param>
	/// <param name="C"></param>
	/// <returns></returns>
	private List<CrystalVertex> CreateFace(Vector3 A, Vector3 B, Vector3 C, out Vector3 N)
	{
		List<CrystalVertex> data = new List<CrystalVertex>();
		Vector3 Normal = Vector3.Normalize(Vector3.Cross(B - A, C - A));
		data.Add(new CrystalVertex(A, -Normal));
		data.Add(new CrystalVertex(B, -Normal));
		data.Add(new CrystalVertex(C, -Normal));
		N = -Normal;
		return data;
	}
	public struct CrystalVertex : IVertexType
	{
		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[2]
		{
				new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
				new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
		});

		public Vector3 Position;
		public Vector3 Normal;

		public CrystalVertex(Vector3 position, Vector3 normal)
		{
			Position = position;
			Normal = normal;
		}

		public VertexDeclaration VertexDeclaration
		{
			get
			{
				return _vertexDeclaration;
			}
		}
	}
	public struct MirrorFaceVertex : IVertexType
	{
		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
		{
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
		    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
		    new VertexElement(16, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
		});

		public Vector3 Position;
		public Color Color;
		public Vector3 Normal;

		public MirrorFaceVertex(Vector3 position, Color color, Vector3 normal)
		{
			Position = position;
			Color = color;
			Normal = normal;
		}

		public VertexDeclaration VertexDeclaration
		{
			get
			{
				return _vertexDeclaration;
			}
		}
	}
}