using Terraria.Graphics.Effects;

namespace Everglow.Commons.MEAC;

internal class MEACManager : ILoadable
{
	private RenderTarget2D screen = null, bloomTarget1 = null, bloomTarget2 = null;

	private Effect ScreenWarp;

	public void Load(Mod mod)
	{
		if (!Main.dedServ)
		{
			var hookManager = Ins.HookManager;
			hookManager.AddHook(Enums.CodeLayer.ResolutionChanged, Main_OnResolutionChanged);
			hookManager.AddHook(Enums.CodeLayer.PreDrawFilter, FilterManager_EndCapture);
			ScreenWarp = ModAsset.ScreenWarp.Value;
			ModIns.OnUnload += () =>
			{
				Ins.MainThread.AddTask(() =>
				{
					bloomTarget1?.Dispose();
					bloomTarget2?.Dispose();
				});
			};
		}
	}

	private void CreateRender(Vector2 v)
	{
		bloomTarget1 = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)v.X / 3, (int)v.Y / 3);
		bloomTarget2 = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)v.X / 3, (int)v.Y / 3);
	}

	private void Main_OnResolutionChanged(Vector2 obj)
	{
		CreateRender(obj);
	}

	private bool HasBloom()
	{
		bool flag = false;
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.ModProjectile is IBloomProjectile ModProj)
					flag = true;
			}
		}
		return flag;
	}

	private void UseBloom(GraphicsDevice graphicsDevice)
	{
		if (HasBloom())
		{
			Effect Bloom = ModAsset.Bloom1.Value;

			//保存原图
			graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
			graphicsDevice.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
			Main.spriteBatch.End();

			//在screen上绘制发光部分
			graphicsDevice.SetRenderTarget(screen);
			graphicsDevice.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			DrawBloom(Main.spriteBatch);
			Main.spriteBatch.End();

			//取样

			graphicsDevice.SetRenderTarget(bloomTarget2);
			graphicsDevice.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Bloom.CurrentTechnique.Passes[0].Apply();//取亮度超过m值的部分
			Bloom.Parameters["m"].SetValue(0.5f);
			Main.spriteBatch.Draw(screen, new Rectangle(0, 0, Main.screenWidth / 3, Main.screenHeight / 3), Color.White);
			Main.spriteBatch.End();

			//处理

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			Bloom.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) / 3f);
			Bloom.Parameters["uRange"].SetValue(1.5f);//范围
			Bloom.Parameters["uIntensity"].SetValue(0.97f);//发光强度
			for (int i = 0; i < 2; i++)//交替使用两个RenderTarget2D，进行多次模糊
			{
				Bloom.CurrentTechnique.Passes["GlurV"].Apply();//横向
				graphicsDevice.SetRenderTarget(bloomTarget1);
				graphicsDevice.Clear(Color.Transparent);
				Main.spriteBatch.Draw(bloomTarget2, Vector2.Zero, Color.White);

				Bloom.CurrentTechnique.Passes["GlurH"].Apply();//纵向
				graphicsDevice.SetRenderTarget(bloomTarget2);
				graphicsDevice.Clear(Color.Transparent);
				Main.spriteBatch.Draw(bloomTarget1, Vector2.Zero, Color.White);
			}
			Main.spriteBatch.End();

			graphicsDevice.SetRenderTarget(Main.screenTarget);
			graphicsDevice.Clear(Color.Transparent);

			//叠加
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
			Main.spriteBatch.Draw(bloomTarget2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
			Main.spriteBatch.End();
		}
	}

	private void FilterManager_EndCapture()
	{
		// 直接从RT池子里取
		var renderTargets = Ins.RenderTargetPool.GetRenderTarget2DArray(1);
		screen = renderTargets.Resource[0];
		if (bloomTarget1 == null)
			CreateRender(new Vector2(Main.screenWidth, Main.screenHeight));
		GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;

		GetOrig(graphicsDevice);
		graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
		graphicsDevice.Clear(Color.Transparent);
		bool flag = DrawWarp();
		if (flag)
		{
			graphicsDevice.SetRenderTarget(Main.screenTarget);
			graphicsDevice.Clear(Color.Transparent);
			graphicsDevice.Textures[1] = Main.screenTargetSwap;
			graphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			ScreenWarp.Parameters["strength"].SetValue(0.025f);//扭曲程度
			ScreenWarp.CurrentTechnique.Passes["KScreen0"].Apply();

			Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
			Main.spriteBatch.End();
			GetOrig(graphicsDevice);
			graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
			graphicsDevice.Clear(Color.Transparent);
			screen = renderTargets.Resource[0];
		}


		bool flag2 = DrawWarp_Style2();
		if (flag2)
		{
			graphicsDevice.SetRenderTarget(Main.screenTarget);
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0, 0));
			graphicsDevice.Textures[1] = Main.screenTargetSwap;
			graphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			ScreenWarp.Parameters["strength"].SetValue(0.25f);//扭曲程度
			ScreenWarp.CurrentTechnique.Passes["KScreen1"].Apply();

			Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
			Main.spriteBatch.End();
		}
		UseBloom(graphicsDevice);
		screen = null;
		renderTargets.Release();
	}

	private void DrawBloom(SpriteBatch sb)//发光层
	{
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.ModProjectile is IBloomProjectile ModProj)
					ModProj.DrawBloom();
			}
		}
	}

	private bool DrawWarp()//扭曲层
	{
		bool flag = false;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		Effect KEx = ModAsset.DrawWarp.Value;
		KEx.Parameters["uTransform"].SetValue(Main.Transform * Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		KEx.CurrentTechnique.Passes[0].Apply();
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.ModProjectile is IWarpProjectile ModProj2)
				{
					flag = true;
					ModProj2.DrawWarp(Ins.Batch);
				}
			}
		}
		Ins.Batch.End();
		return flag;
	}
	private bool DrawWarp_Style2()//扭曲层
	{
		bool flag = false;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		Effect ef = ModAsset.DrawWarp.Value;
		ef.Parameters["uTransform"].SetValue(Main.Transform * Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1));
		ef.CurrentTechnique.Passes["Trail1"].Apply();
		foreach (Projectile proj in Main.projectile)
		{
			if (proj.active)
			{
				if (proj.ModProjectile is IWarpProjectile_warpStyle2 modProj2)
				{
					flag = true;
					modProj2.DrawWarp(Ins.Batch);
				}
			}
		}
		Ins.Batch.End();
		return flag;
	}

	/// <summary>
	/// 往screen上保存原图
	/// </summary>
	/// <param name="graphicsDevice"> </param>
	private void GetOrig(GraphicsDevice graphicsDevice)
	{
		graphicsDevice.SetRenderTarget(screen);
		graphicsDevice.Clear(Color.Transparent);
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
		Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
		Main.spriteBatch.End();
	}

	public void Unload()
	{
	}
}