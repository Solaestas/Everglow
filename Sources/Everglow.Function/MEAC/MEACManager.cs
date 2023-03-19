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
			Main.OnResolutionChanged += Main_OnResolutionChanged;
			On_FilterManager.EndCapture += FilterManager_EndCapture;
			ScreenWarp = ModAsset.ScreenWarp.Value;
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
	private void FilterManager_EndCapture(On_FilterManager.orig_EndCapture orig, FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
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
		bool flag = DrawWarp(Main.spriteBatch);

		if (flag)
		{
			graphicsDevice.SetRenderTarget(Main.screenTarget);
			graphicsDevice.Clear(Color.Transparent);
			graphicsDevice.Textures[1] = Main.screenTargetSwap;
			graphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			ScreenWarp.CurrentTechnique.Passes[0].Apply();
			ScreenWarp.Parameters["i"].SetValue(0.025f);//扭曲程度
			Main.spriteBatch.Draw(screen, Vector2.Zero, Color.White);
			Main.spriteBatch.End();
		}
		UseBloom(graphicsDevice);
		screen = null;
		renderTargets.Release();
		orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
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
	private bool DrawWarp(SpriteBatch sb)//扭曲层
	{
		bool flag = false;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		Effect KEx = Everglow.Commons.ModAsset.DrawWarp.Value;
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
	/// <summary>
	/// 往screen上保存原图
	/// </summary>
	/// <param name="graphicsDevice"></param>
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
		Ins.MainThread.AddTask(() =>
		{
			bloomTarget1?.Dispose();
			bloomTarget2?.Dispose();
		});
	}
}
