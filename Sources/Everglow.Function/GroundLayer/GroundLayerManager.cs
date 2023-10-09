using ReLogic.Content;

namespace Everglow.Commons.GroundLayer;

public class GroundLayerManager : ILoadable
{
	public static GroundLayerManager Instance => ModContent.GetInstance<GroundLayerManager>();

	public Vector3 CameraPosition { get; private set; }

	public bool AddLayer(string layerName, Asset<Texture2D> texture, Vector3 layerPos)
	{
		if (layerCollection.ContainsKey(layerName))
		{
			return false;
		}
		var layer = new Layer(texture) { Position = layerPos };
		layerCollection.Add(layerName, layer);
		needResort = true;
		return true;
	}

	public void Clear()
	{
		layerCollection.Clear();
	}

	public void DrawBackgroundLayers(SpriteBatch sprite)
	{
		Resort();
		foreach (var drawLayer in layerCache)
		{
			if (drawLayer.Position.Z > 0)
			{
				return;
			}
			if (!drawLayer.Texture.IsLoaded)
			{
				continue;
			}
			var texture = drawLayer.Texture.Value;
			float f = drawLayer.Position.Z / CameraPosition.Z;

			int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPosition.X * f);
			int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPosition.X * f);
			int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPosition.Y * f);
			int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPosition.Y * f);

			Rectangle r1 = new((int)drawLayer.Position.X, (int)drawLayer.Position.Y, texture.Width, texture.Height);
			Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
			Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

			if (r3.Width == 0 || r3.Height == 0)
			{
				continue;
			}

			float f2 = CameraPosition.Z / (CameraPosition.Z - drawLayer.Position.Z);
			Rectangle r4 = new((int)(CameraPosition.X + (r3.X - CameraPosition.X) * f2),
				(int)(CameraPosition.Y + (r3.Y - CameraPosition.Y) * f2),
				(int)(r3.Width * f2),
				(int)(r3.Height * f2));

			sprite.Draw(texture,
				new Rectangle((int)(r4.X - Main.screenPosition.X), (int)(r4.Y - Main.screenPosition.Y), r4.Width, r4.Height),
				new Rectangle((int)(r3.X - drawLayer.Position.X), (int)(r3.Y - drawLayer.Position.Y), r3.Width, r3.Height),
				Color.White);
		}
	}

	public void DrawForegroundLayers(SpriteBatch sprite)
	{
		Resort();
		foreach (var drawLayer in layerCache)
		{
			if (drawLayer.Position.Z < 0)
			{
				continue;
			}
			if (drawLayer.Position.Z > CameraPosition.Z)
			{
				return;
			}
			if (!drawLayer.Texture.IsLoaded)
			{
				continue;
			}
			var texture = drawLayer.Texture.Value;
			float f = drawLayer.Position.Z / CameraPosition.Z;

			int leftX = (int)(Main.screenPosition.X * (1 - f) + CameraPosition.X * f);
			int rightX = (int)((Main.screenPosition.X + Main.screenWidth) * (1 - f) + CameraPosition.X * f);
			int topY = (int)(Main.screenPosition.Y * (1 - f) + CameraPosition.Y * f);
			int bottomY = (int)((Main.screenPosition.Y + Main.screenHeight) * (1 - f) + CameraPosition.Y * f);

			Rectangle r1 = new((int)drawLayer.Position.X, (int)drawLayer.Position.Y, texture.Width, texture.Height);
			Rectangle r2 = new(leftX, topY, rightX - leftX, bottomY - topY);
			Rectangle.Intersect(ref r1, ref r2, out Rectangle r3);

			if (r3.Width == 0 || r3.Height == 0)
			{
				continue;
			}

			float f2 = CameraPosition.Z / (CameraPosition.Z - drawLayer.Position.Z);
			Rectangle r4 = new((int)(CameraPosition.X + (r3.X - CameraPosition.X) * f2),
				(int)(CameraPosition.Y + (r3.Y - CameraPosition.Y) * f2),
				(int)(r3.Width * f2),
				(int)(r3.Height * f2));

			sprite.Draw(texture,
				new Rectangle((int)(r4.X - Main.screenPosition.X), (int)(r4.Y - Main.screenPosition.Y), r4.Width, r4.Height),
				new Rectangle((int)(r3.X - drawLayer.Position.X), (int)(r3.Y - drawLayer.Position.Y), r3.Width, r3.Height),
				Color.White);
		}
	}

	public Layer GetLayer(string layerName)
	{
		return layerCollection[layerName];
	}

	public void Load(Mod mod)
	{
		On_Main.DoDraw_WallsTilesNPCs += On_Main_DoDraw_WallsTilesNPCs;
		On_Main.DrawLiquid += On_Main_DrawLiquid;
		Ins.HookManager.AddHook(Enums.CodeLayer.PostExitWorld_Single, Clear);
	}

	public bool RemoveLayer(string layerName)
	{
		return layerCollection.Remove(layerName);
	}

	public void SetCameraPos(Vector3 cameraPos)
	{
		if (cameraPos.Z < 0)
		{
			throw new Exception("Can't Support Camera Set After Screen");
		}
		CameraPosition = cameraPos;
	}

	public bool TryGetLayer(string layerName, out Layer layer)
	{
		return layerCollection.TryGetValue(layerName, out layer);
	}

	public void Unload()
	{
		On_Main.DoDraw_WallsTilesNPCs -= On_Main_DoDraw_WallsTilesNPCs;
		On_Main.DrawLiquid -= On_Main_DrawLiquid;
	}

	private Layer[] layerCache;

	private Dictionary<string, Layer> layerCollection = new();

	private bool needResort;

	private void On_Main_DoDraw_WallsTilesNPCs(On_Main.orig_DoDraw_WallsTilesNPCs orig, Main self)
	{
		// 虽然我用ScaledPosition可能是错的，但是你用LocalPlayer.Center一定是错的
		SetCameraPos(new(Main.Camera.ScaledPosition, 1600));
		DrawBackgroundLayers(Main.spriteBatch);
		orig(self);
	}

	private void On_Main_DrawLiquid(On_Main.orig_DrawLiquid orig, Main self, bool bg, int waterStyle, float Alpha, bool drawSinglePassLiquids)
	{
		orig(self, bg, waterStyle, Alpha, drawSinglePassLiquids);
		DrawForegroundLayers(Main.spriteBatch);
	}

	private void Resort()
	{
		if (!needResort)
		{
			return;
		}
		layerCache = layerCollection.Values.OrderBy(layer => layer.Position.Z).ToArray();
		needResort = false;
	}
}