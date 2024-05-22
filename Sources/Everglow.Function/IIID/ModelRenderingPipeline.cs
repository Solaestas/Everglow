using ReLogic.Content;

namespace Everglow.Commons.IIID;

// TODO rework
public class ModelRenderingPipeline : ModSystem
{
	private RenderTarget2D m_fakeScreenTarget;

	private RenderTarget2D m_fakeScreenTargetSwap;

	private RenderTarget2D m_emissionTarget;

	private RenderTarget2D m_depthTarget;

	private RenderTarget2D m_bloomTargetSwap;

	private RenderTarget2D m_bloomTargetSwap1;

	private RenderTarget2D[] m_blurRenderTargets;

	private const int MAX_BLUR_LEVELS = 5;

	private int RenderTargetSize = Math.Max(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) / 2;

	//private RenderTarget2D m_albedoTarget;
	//private RenderTarget2D m_normalTarget;
	//private RenderTarget2D m_surfaceTarget;
	//private RenderTarget2D m_worldPosTarget;

	private List<ModelEntity> m_models;

	private ArtParameters m_artParams;

	private BloomParams m_bloomParams;

	private Matrix m_viewProjectionMatrix;

	private Vector4 m_ZbufferParams;

	private Asset<Effect> m_gbufferPassEffect;

	private Asset<Effect> m_filtersEffect;

	private Asset<Effect> m_toneMapping;

	private Asset<Effect> m_ConcaveEdge;

	private Asset<Effect> m_PixelArt;

	private Asset<Effect> m_Edge;

	public RenderTarget2D ModelTarget
	{
		get
		{
			return m_fakeScreenTarget;
		}
	}

	public override void OnModLoad()
	{
		m_gbufferPassEffect = ModAsset.GBufferPass_Async;
		m_filtersEffect = ModAsset.Filters_Async;
		m_toneMapping = ModAsset.ToneMapping_Async;
		m_ConcaveEdge = ModAsset.ConcaveEdge_Async;
		m_PixelArt = ModAsset.PixelArt_Async;
		m_Edge = ModAsset.Edge_Async;

		Main.OnResolutionChanged += Main_OnResolutionChanged;
		Ins.MainThread.AddTask(() =>
		{
			m_fakeScreenTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, RenderTargetSize, RenderTargetSize, false,
				SurfaceFormat.HdrBlendable, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.PreserveContents);
			m_fakeScreenTargetSwap = new RenderTarget2D(Main.graphics.GraphicsDevice, RenderTargetSize, RenderTargetSize, false,
				SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
			m_emissionTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, RenderTargetSize, RenderTargetSize, false,
				SurfaceFormat.HdrBlendable, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);
			m_depthTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, RenderTargetSize, RenderTargetSize, false,
				SurfaceFormat.Vector4, DepthFormat.None, 1, RenderTargetUsage.PreserveContents);

			m_blurRenderTargets = new RenderTarget2D[MAX_BLUR_LEVELS];
			for (int i = 0; i < MAX_BLUR_LEVELS; i++)
			{
				m_blurRenderTargets[i] = new RenderTarget2D(Main.graphics.GraphicsDevice, RenderTargetSize >> i,
					RenderTargetSize >> i, false, SurfaceFormat.HdrBlendable, DepthFormat.None,
					1, RenderTargetUsage.PreserveContents);
			}

			m_bloomTargetSwap1 = new RenderTarget2D(Main.graphics.GraphicsDevice, RenderTargetSize >> 1,
				RenderTargetSize >> 1, false,
			   SurfaceFormat.HdrBlendable, DepthFormat.None,
			   1, RenderTargetUsage.PreserveContents);

			m_bloomTargetSwap = new RenderTarget2D(Main.graphics.GraphicsDevice, RenderTargetSize >> (MAX_BLUR_LEVELS - 1),
				RenderTargetSize >> (MAX_BLUR_LEVELS - 1), false,
			   SurfaceFormat.HdrBlendable, DepthFormat.None,
			   1, RenderTargetUsage.PreserveContents);
		});
		m_models = new List<ModelEntity>();
		base.OnModLoad();
	}

	public override void OnModUnload()
	{
		base.OnModUnload();
	}

	private void Main_OnResolutionChanged(Vector2 obj)
	{
		// CreateRender(obj);
	}

	public void PushModelEntity(ModelEntity entity)
	{
		m_models.Add(entity);
	}

	public void BeginCapture(ViewProjectionParams viewProjectionParams, BloomParams bloomParams, ArtParameters artParams)
	{
		var projectionTransform = Matrix.CreatePerspectiveFieldOfView(viewProjectionParams.FieldOfView,
			viewProjectionParams.AspectRatio, viewProjectionParams.ZNear, viewProjectionParams.ZFar);
		this.m_viewProjectionMatrix = viewProjectionParams.ViewTransform * projectionTransform;
		this.m_bloomParams = bloomParams;

		float zc0 = 1.0f - viewProjectionParams.ZFar / viewProjectionParams.ZNear;
		float zc1 = viewProjectionParams.ZFar / viewProjectionParams.ZNear;
		this.m_ZbufferParams = new Vector4(zc0, zc1, zc0 / viewProjectionParams.ZFar, zc1 / viewProjectionParams.ZFar);
		this.m_artParams = artParams;
		m_models.Clear();
	}

	public void EndCapture()
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;
		spriteBatch.End();

		if (!m_gbufferPassEffect.IsLoaded)
		{
			m_gbufferPassEffect.Wait();
		}
		if (!m_filtersEffect.IsLoaded)
		{
			m_filtersEffect.Wait();
		}
		if (!m_ConcaveEdge.IsLoaded)
		{
			m_ConcaveEdge.Wait();
		}
		if (!m_PixelArt.IsLoaded)
		{
			m_PixelArt.Wait();
		}
		if (!m_Edge.IsLoaded)
		{
			m_Edge.Wait();
		}

		Blit(Main.screenTarget, Main.screenTargetSwap, null, "");

		ShadingPass();

		//BloomPass();
		//ToneMappingPass();
		//ConcaveEdgePass();
		//FinalBlend();
		if (m_artParams.EnablePixelArt)
		{
			PixelArt();
		}
		if (m_artParams.EnableOuterEdge)
		{
			Edge();
		}

		graphicsDevice.SetRenderTarget(Main.screenTarget);
		spriteBatch.Begin();
		spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
	}

	private void ShadingPass()
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		// 绘制在具有深度缓冲的一个RT上，而不是原屏幕
		DepthStencilState dState = DepthStencilState.Default;
		RasterizerState rState = new RasterizerState();
		rState.CullMode = CullMode.CullClockwiseFace;
		rState.FillMode = FillMode.Solid;
		Main.graphics.GraphicsDevice.DepthStencilState = dState;
		Main.graphics.GraphicsDevice.RasterizerState = rState;

		// 切换到GBuffer Targets上
		var renderTargets = new RenderTargetBinding[3]
		{
			m_fakeScreenTarget,
			m_emissionTarget,
			m_depthTarget
		};
		graphicsDevice.SetRenderTargets(renderTargets);
		graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil,
			Color.Transparent, 1.0f, 0);

		for (int i = 0; i < m_models.Count; i++)
		{
			DrawOneModel(i);
		}
	}

	private void DrawOneModel(int index)
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		var model = m_models[index];
		var vertices = model.Vertices;
		var gBufferShader = m_gbufferPassEffect.Value;

		gBufferShader.Parameters["uModel"].SetValue(model.ModelTransform);
		gBufferShader.Parameters["uViewProjection"].SetValue(m_viewProjectionMatrix);

		// 如果Model有非均匀缩放，就要用法线变换矩阵而不是Model矩阵
		gBufferShader.Parameters["uModelNormal"].SetValue(Matrix.Transpose(Matrix.Invert(model.ModelTransform)));
		gBufferShader.Parameters["uCameraPosition"].SetValue(new Vector3(0, 0, 1000));
		gBufferShader.Parameters["uLightDirection"].SetValue(Vector3.Normalize(new Vector3(0f, 1f, 1f)));
		gBufferShader.Parameters["uLightIntensity"].SetValue(new Vector3(1f, 1f, 1f) * 150);
		gBufferShader.Parameters["uNormalIntensity"].SetValue(-1f);

		graphicsDevice.Textures[0] = model.Texture;
		graphicsDevice.Textures[1] = model.NormalTexture;
		graphicsDevice.Textures[2] = model.MaterialTexture;
		graphicsDevice.Textures[3] = model.EmissionTexture;

		gBufferShader.CurrentTechnique.Passes["Forward_Lit"].Apply();
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(),
			0, vertices.Count / 3);
	}

	private void BloomPass()
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		// Blit into m_blurRenderTargets[0]
		graphicsDevice.SetRenderTarget(m_blurRenderTargets[0]);
		graphicsDevice.Clear(Color.Transparent);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp, DepthStencilState.None,
			RasterizerState.CullNone);
		spriteBatch.Draw(m_emissionTarget, m_emissionTarget.Bounds, Color.White);
		spriteBatch.End();

		var filterEffect = m_filtersEffect.Value;
		for (int i = 0; i < MAX_BLUR_LEVELS - 1; i++)
		{
			filterEffect.Parameters["uInvImageSize"].SetValue(new Vector2(1.0f / m_blurRenderTargets[i].Width,
				1.0f / m_blurRenderTargets[i].Height));
			filterEffect.Parameters["uDelta"].SetValue(1.0f);
			Blit(m_blurRenderTargets[i], m_blurRenderTargets[i + 1], filterEffect, "Box", BlendState.Opaque,
				SamplerState.AnisotropicClamp);
		}

		for (int i = 0; i < 1; i++)
		{
			filterEffect.Parameters["uDelta"].SetValue(1.0f);
			filterEffect.Parameters["uHorizontal"].SetValue(true);
			Blit(m_blurRenderTargets[MAX_BLUR_LEVELS - 1], m_bloomTargetSwap, filterEffect, "GBlur", BlendState.Opaque,
				SamplerState.AnisotropicClamp);

			filterEffect.Parameters["uDelta"].SetValue(1.0f);
			filterEffect.Parameters["uHorizontal"].SetValue(false);
			Blit(m_bloomTargetSwap, m_blurRenderTargets[MAX_BLUR_LEVELS - 1], filterEffect, "GBlur", BlendState.Opaque,
				SamplerState.AnisotropicClamp);
		}

		for (int i = MAX_BLUR_LEVELS - 1; i > 0; i--)
		{
			filterEffect.Parameters["uInvImageSize"].SetValue(new Vector2(1.0f / m_blurRenderTargets[i].Width,
				1.0f / m_blurRenderTargets[i].Height));
			filterEffect.Parameters["uDelta"].SetValue(0.5f);
			Blit(m_blurRenderTargets[i], m_blurRenderTargets[i - 1], filterEffect, "Box", BlendState.Opaque,
				SamplerState.AnisotropicClamp);
		}

		// Draw to bloom renderTarget
	}

	private void ToneMappingPass()
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		// Save content of m_fakeScreenTarget
		var toneMappingShader = m_toneMapping.Value;
		graphicsDevice.SetRenderTarget(m_fakeScreenTargetSwap);
		graphicsDevice.Clear(Color.Transparent);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp,
			DepthStencilState.None,
			RasterizerState.CullNone);
		toneMappingShader.CurrentTechnique.Passes[1].Apply();
		spriteBatch.Draw(m_fakeScreenTarget, m_fakeScreenTarget.Bounds, Color.White);
		spriteBatch.Draw(m_emissionTarget, m_emissionTarget.Bounds, Color.White);
		spriteBatch.Draw(m_blurRenderTargets[0], m_blurRenderTargets[0].Bounds, Color.White);
		spriteBatch.End();
	}

	private void ConcaveEdgePass()
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;
		//// Save content of m_fakeScreenTarget
		//graphicsDevice.SetRenderTarget(m_blurRenderTargets[1]);
		//graphicsDevice.Clear(Color.Transparent);
		//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
		//    DepthStencilState.None,
		//    RasterizerState.CullNone);
		//ConcaveEdgeEffect.CurrentTechnique.Passes["DownSample_Naive"].Apply();
		//int width = m_fakeScreenTargetSwap.Width;
		//int height = m_fakeScreenTargetSwap.Height;
		//spriteBatch.Draw(m_fakeScreenTargetSwap, m_blurRenderTargets[1].Bounds, Color.White);
		//// spriteBatch.Draw(m_emissionTarget, m_emissionTarget.Bounds, Color.White);
		//spriteBatch.End();
		var ConcaveEdgeEffect = m_ConcaveEdge.Value;
		graphicsDevice.Textures[1] = m_depthTarget;
		graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
		ConcaveEdgeEffect.Parameters["uBias"].SetValue(0.01f);
		ConcaveEdgeEffect.Parameters["_ZBufferParams"].SetValue(m_ZbufferParams);
		if (m_artParams.EnableOuterEdge)
		{
			ConcaveEdgeEffect.Parameters["_EdgeColor"].SetValue(new Vector4(1f, 0f, 0f, 1f));
			Blit(m_fakeScreenTargetSwap, m_fakeScreenTarget, ConcaveEdgeEffect, "Edge_HighLight_Outer");
		}
		else
		{
			Blit(m_fakeScreenTargetSwap, m_fakeScreenTarget, ConcaveEdgeEffect, "Edge_HighLight_Inner");
		}
		Blit(m_fakeScreenTarget, m_fakeScreenTargetSwap, null, "");
	}

	private void FinalBlend()
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		var filterEffect = m_filtersEffect.Value;

		// Draw to m_fakeScreenTarget
		graphicsDevice.SetRenderTarget(m_fakeScreenTarget);
		graphicsDevice.Clear(Color.Transparent);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
			DepthStencilState.None,
			RasterizerState.CullNone);
		graphicsDevice.Textures[1] = m_blurRenderTargets[0];
		filterEffect.Parameters["uIntensity"].SetValue(0.1f);
		filterEffect.CurrentTechnique.Passes["Blend"].Apply();
		spriteBatch.Draw(m_fakeScreenTargetSwap, m_fakeScreenTarget.Bounds, Color.White);
		spriteBatch.End();
	}

	private void PixelArt()
	{
		Blit(m_fakeScreenTarget, m_fakeScreenTargetSwap, null, "");

		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		var PixelEffect = m_PixelArt.Value;

		// Draw to m_fakeScreenTarget
		graphicsDevice.SetRenderTarget(m_fakeScreenTarget);
		graphicsDevice.Clear(Color.Transparent);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
			DepthStencilState.None,
			RasterizerState.CullNone);
		PixelEffect.Parameters["RenderTargetSize"].SetValue(RenderTargetSize);
		PixelEffect.Parameters["_PixelSize"].SetValue(1);
		PixelEffect.Parameters["_PixelRatio"].SetValue(1);
		PixelEffect.CurrentTechnique.Passes["Blend"].Apply();
		spriteBatch.Draw(m_fakeScreenTargetSwap, m_fakeScreenTarget.Bounds, Color.White);
		spriteBatch.End();
	}

	private void Edge()
	{
		Blit(m_fakeScreenTarget, m_fakeScreenTargetSwap, null, "");

		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;

		var EdgeEffect = m_Edge.Value;

		// Draw to m_fakeScreenTarget
		graphicsDevice.SetRenderTarget(m_fakeScreenTarget);
		graphicsDevice.Clear(Color.Transparent);
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp,
			DepthStencilState.None,
			RasterizerState.CullNone);
		EdgeEffect.Parameters["RenderTargetSize"].SetValue(RenderTargetSize);
		EdgeEffect.CurrentTechnique.Passes["Edge"].Apply();
		spriteBatch.Draw(m_fakeScreenTargetSwap, m_fakeScreenTarget.Bounds, Color.White);
		spriteBatch.End();
	}

	private void Blit(RenderTarget2D rt1, RenderTarget2D rt2, Effect effect, string pass)
	{
		Blit(rt1, rt2, effect, pass,
			BlendState.Opaque);
	}

	private void Blit(RenderTarget2D rt1, RenderTarget2D rt2, Effect effect, string pass, BlendState blendState)
	{
		Blit(rt1, rt2, effect, pass,
			blendState, SamplerState.PointClamp);
	}

	private void Blit(RenderTarget2D rt1, RenderTarget2D rt2, Effect effect, string pass, BlendState blendState,
		SamplerState samplerState)
	{
		Blit(rt1, rt2, effect, pass,
			blendState, samplerState, DepthStencilState.None);
	}

	private void Blit(RenderTarget2D rt1, RenderTarget2D rt2, Effect effect, string pass, BlendState blendState,
		SamplerState samplerState, DepthStencilState depthStencilState)
	{
		Blit(rt1, rt2, effect, pass,
			blendState, samplerState, depthStencilState, RasterizerState.CullNone);
	}

	private void Blit(RenderTarget2D rt1, RenderTarget2D rt2, Effect effect, string pass,
		BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState,
		RasterizerState rasterizerState)
	{
		var graphicsDevice = Main.graphics.GraphicsDevice;
		var spriteBatch = Main.spriteBatch;
		graphicsDevice.SetRenderTarget(rt2);
		graphicsDevice.Clear(Color.Transparent);

		SpriteSortMode spriteSortMode = SpriteSortMode.Deferred;
		if (effect != null)
		{
			spriteSortMode = SpriteSortMode.Immediate;
		}
		spriteBatch.Begin(spriteSortMode, blendState, samplerState,
			depthStencilState,
			rasterizerState);
		if (effect != null)
		{
			effect.Parameters["uInvImageSize"].SetValue(new Vector2(1.0f / rt1.Width,
								1.0f / rt1.Height));
			effect.CurrentTechnique.Passes[pass].Apply();
		}
		spriteBatch.Draw(rt1, rt2.Bounds, Color.White);
		spriteBatch.End();
	}

	public override void PostUpdateProjectiles()
	{
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			Player player = Main.LocalPlayer;
		}
		base.PostUpdateProjectiles();
	}
}