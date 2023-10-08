using Everglow.Commons.GroundLayer.Basics;

namespace Everglow.Commons.GroundLayer.LayerSupport
{
	public class GroundLayerManager : ModSystem
	{
		public static GroundLayerManager Instance => ModContent.GetInstance<GroundLayerManager>();
		StructCollection<Layer> layerCollection = new();
		Vector3 CameraPos;
		public bool WaitLoadTexture = false;
		Dictionary<string, int> layerMap = new();
		public bool AddLayer(string layerName, string layerTexturePath, Vector3 layerPos)
		{
			if (layerMap.ContainsKey(layerName))
			{
				return false;
			}
			var layer = new Layer(layerTexturePath) { Position = layerPos };
			if (layerCollection.Add(layer))
			{
				layerMap[layerName] = layer.UniqueID;
				return true;
			}
			return false;
		}
		public bool RemoveLayer(string layerName)
		{
			if (!layerMap.TryGetValue(layerName, out int id))
			{
				return false;
			}
			return layerCollection.Remove(id);
		}
		public bool GetLayer(string layerName, ref Layer layer)
		{
			if (layerMap.TryGetValue(layerName, out int id))
			{
				layer = layerCollection[id];
				return true;
			}
			layer = default;
			return false;
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
		public void DrawForegroundLayers(SpriteBatch sprite)
		{
			Layer[] layers = layerCollection.GetUpdateElements();
			Array.Sort(layers, (l1, l2) => l1.Position.Z.CompareTo(l2.Position.Z));
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
				if (!drawLayer.PrePareDraw(out Texture2D texture, !WaitLoadTexture))
				{
					continue;
				}
				float f = drawLayer.Position.Z / CameraPos.Z;

				int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPos.X * f);
				int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPos.X * f);
				int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPos.Y * f);
				int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPos.Y * f);

				Rectangle r1 = new((int)drawLayer.Position.X, (int)drawLayer.Position.Y, texture.Width, texture.Height);
				Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
				Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

				if (r3.Width == 0 || r3.Height == 0)
				{
					continue;
				}

				float f2 = CameraPos.Z / (CameraPos.Z - drawLayer.Position.Z);
				Rectangle r4 = new((int)(CameraPos.X + (r3.X - CameraPos.X) * f2),
					(int)(CameraPos.Y + (r3.Y - CameraPos.Y) * f2),
					(int)(r3.Width * f2),
					(int)(r3.Height * f2));

				sprite.Draw(texture,
					new Rectangle((int)(r4.X - Main.screenPosition.X), (int)(r4.Y - Main.screenPosition.Y), r4.Width, r4.Height),
					new Rectangle((int)(r3.X - drawLayer.Position.X), (int)(r3.Y - drawLayer.Position.Y), r3.Width, r3.Height),
					Color.White with { A = (byte)(r4.Intersects(Main.LocalPlayer.getRect()) ? 63 : 255) });
			}
		}
		public void DrawBackgroundLayers(SpriteBatch sprite)
		{
			Layer[] layers = layerCollection.GetUpdateElements();
			Array.Sort(layers, (l1, l2) => l1.Position.Z.CompareTo(l2.Position.Z));
			for (int i = 0; i < layers.Length; i++)
			{
				Layer drawLayer = layers[i];
				if (drawLayer.Position.Z > 0)
				{
					return;
				}
				if (!drawLayer.PrePareDraw(out var texture, !WaitLoadTexture))
				{
					continue;
				}
				float f = drawLayer.Position.Z / CameraPos.Z;

				int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPos.X * f);
				int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPos.X * f);
				int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPos.Y * f);
				int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPos.Y * f);

				Rectangle r1 = new((int)drawLayer.Position.X, (int)drawLayer.Position.Y, texture.Width, texture.Height);
				Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
				Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

				if (r3.Width == 0 || r3.Height == 0)
				{
					continue;
				}

				float f2 = CameraPos.Z / (CameraPos.Z - drawLayer.Position.Z);
				Rectangle r4 = new((int)(CameraPos.X + (r3.X - CameraPos.X) * f2),
					(int)(CameraPos.Y + (r3.Y - CameraPos.Y) * f2),
					(int)(r3.Width * f2),
					(int)(r3.Height * f2));

				sprite.Draw(texture,
					new Rectangle((int)(r4.X - Main.screenPosition.X), (int)(r4.Y - Main.screenPosition.Y), r4.Width, r4.Height),
					new Rectangle((int)(r3.X - drawLayer.Position.X), (int)(r3.Y - drawLayer.Position.Y), r3.Width, r3.Height),
					Color.White with { A = (byte)(r4.Intersects(Main.LocalPlayer.getRect()) ? 63 : 255) });
			}
		}
		public override void PostDrawTiles()
		{
			SetCameraPos(new(Main.LocalPlayer.Center, 1600));
			DrawForegroundLayers(Main.spriteBatch);
		}
		public override void OnWorldUnload()
		{
			layerMap.Clear();
			layerCollection.Clear();
		}
	}
	class TestPlayer : ModPlayer
	{
		public override void OnEnterWorld()
		{
			GroundLayerManager.Instance.AddLayer("Test1", "Everglow/Commons/Textures/Noise_melting", new(Player.Center, 400));
			GroundLayerManager.Instance.AddLayer("Test2", "Everglow/Commons/Textures/Noise_melting", new(Player.Center, 800));
			GroundLayerManager.Instance.AddLayer("Test3", "Everglow/Commons/Textures/Noise_melting", new(Player.Center, 1200));
		}
	}
}