using Everglow.Commons.GroundLayer.Basics;

namespace Everglow.Commons.GroundLayer.LayerSupport
{
	public class GroundLayerManager : ModSystem
	{
		static GroundLayerManager()
		{
			On_Main.DoDraw_WallsTilesNPCs += On_Main_DoDraw_WallsTilesNPCs;
			On_Main.DrawLiquid += On_Main_DrawLiquid;
		}
		private static void On_Main_DrawLiquid(On_Main.orig_DrawLiquid orig, Main self, bool bg, int waterStyle, float Alpha, bool drawSinglePassLiquids)
		{
			orig(self, bg, waterStyle, Alpha, drawSinglePassLiquids);
			Instance.DrawForegroundLayers(Main.spriteBatch);
		}
		private static void On_Main_DoDraw_WallsTilesNPCs(On_Main.orig_DoDraw_WallsTilesNPCs orig, Main self)
		{
			Instance.DrawBackgroundLayers(Main.spriteBatch);
			orig(self);
		}
		public static GroundLayerManager Instance => ModContent.GetInstance<GroundLayerManager>();
		StructCollection<string, Layer> layerCollection = new();
		Vector3 CameraPos;
		public bool WaitLoadTexture = false;
		bool needResort;
		public bool AddLayer(string layerName, string layerTexturePath, Vector3 layerPos, int maxFrame = 1, int frameInterval = int.MaxValue)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				return false;
			}
			var layer = new Layer(layerName,layerTexturePath) 
			{ 
				Position = layerPos,
				MaxFrameCount = maxFrame,
				FrameInterval = frameInterval
			};
			return (needResort = layerCollection.Add(layer)) ? true : false;
		}
		public bool RemoveLayer(string layerName)
		{
			return layerCollection.Remove(layerName);
		}
		public void Clear()
		{
			layerCollection.Clear();
		}
		public bool GetLayer(string layerName, ref Layer layer)
		{
			return layerCollection.TryGet(layerName, ref layer);
		}
		public Vector3 GetCameraPos() => CameraPos;
		public void SetCameraPos(Vector3 cameraPos)
		{
			if (cameraPos.Z < 0)
			{
				throw new Exception("Can't Support Camera Set After Screen");
			}
			CameraPos = cameraPos;
		}
		private void Resort()
		{
			if(!needResort)
			{
				return;
			}
			layerCollection.Sort((i1, i2) => i1.Position.Z.CompareTo(i2.Position.Z));
			needResort = false;
		}
		public void DrawForegroundLayers(SpriteBatch sprite)
		{
			Resort();
			Layer[] layers = layerCollection.GetUpdateElements();
			for (int i = 0; i < layers.Length; i++)
			{
				Layer drawLayer = layers[i];
				if (drawLayer.Position.Z < 0)
				{
					continue;
				}
				if (drawLayer.Position.Z > CameraPos.Z)
				{
					return;
				}
				drawLayer.Draw(sprite, CameraPos, !WaitLoadTexture);
			}
		}
		public void DrawBackgroundLayers(SpriteBatch sprite)
		{
			Resort();
			Layer[] layers = layerCollection.GetUpdateElements();
			for (int i = 0; i < layers.Length; i++)
			{
				Layer drawLayer = layers[i];
				if (drawLayer.Position.Z > 0)
				{
					return;
				}
				drawLayer.Draw(sprite, CameraPos, !WaitLoadTexture);
			}
		}
		public override void OnWorldUnload()
		{
			Clear();
		}
		public override void PostUpdateEverything()
		{
			Instance.SetCameraPos(new(Main.Camera.Center, 1600));
		}
	}
	class TestCommand : ModCommand
	{
		public override string Command => "Layer";

		public override CommandType Type => CommandType.Chat;

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length == 0 || args[0] == "clear")
			{
				GroundLayerManager.Instance.Clear();
			}
			else if (args[0] == "new")
			{
				for (int i = 0; i < 100; i++)
				{
					GroundLayerManager.Instance.AddLayer("Test" + i, "Everglow/Commons/Textures/Noise_melting", new(Main.LocalPlayer.Center + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(3200), Main.rand.NextFloat(-1600, 1600)));
				}
			}
		}
	}
}