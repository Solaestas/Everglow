namespace Everglow.Yggdrasil;

public class YggdrasilEnvironmentLightManager : ModSystem
{
	public Vector3 LightColor = Vector3.Zero;
	public float Alpha = 0f;
	public static YggdrasilScene LightingScene = YggdrasilScene.Null;

	public override void OnModLoad()
	{
	}

	// 目前环境光公式为 环境光[环境ID] + 原版光照 * 受日光影响的程度[环境ID]
	public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
	{
		if (YggdrasilWorld.InYggdrasil)
		{
			if (LightingScene >= 0)
			{
				LightScene scene = lightSceneConfig[LightingScene];
				Color ambientColor = new Color(scene.ambientColor);
				ambientColor = AddColor(tileColor * scene.dayLightFactor, ambientColor);
				LightColor = Vector3.Lerp(LightColor, ambientColor.ToVector3(), 0.5f);
				tileColor = new Color(LightColor);
				backgroundColor = tileColor;
			}
		}
	}

	/// <summary>
	/// 各个场景的光照设置
	/// </summary>
	public readonly Dictionary<YggdrasilScene, LightScene> lightSceneConfig = new Dictionary<YggdrasilScene, LightScene>()
	{
		{ YggdrasilScene.Null, new LightScene(new Vector3(0f, 0f, 0f), 0f) },
		{ YggdrasilScene.YggdrasilTown, new LightScene(new Vector3(0.1f, 0.1f, 0.2f), 0.2f) },
		{ YggdrasilScene.UnderTheOutPost, new LightScene(new Vector3(1f, 238 / 255f, 188 / 255f), 0.1f) },
		{ YggdrasilScene.LampWoodForest, new LightScene(new Vector3(0.1f, 0.1f, 0.2f), 0.1f) },
		{ YggdrasilScene.TwilightForest, new LightScene(new Vector3(1f, 238 / 255f, 188 / 255f), 0.1f) },
		{ YggdrasilScene.ChallengerJail, new LightScene(new Vector3(0.1f, 0.1f, 0.2f), 0f) },
		{ YggdrasilScene.KelpCurtain, new LightScene(new Vector3(1f, 238 / 255f, 188 / 255f), 0.9f) },
	};

	public override void PostUpdateEverything()
	{
		if (Alpha < 1)
		{
			Alpha += 0.02f;
		}
		else
		{
			Alpha = 1;
		}
	}

	private Color AddColor(Color color1, Color color2)
	{
		return new Color(color1.R + color2.R, color1.G + color2.G, color1.B + color2.B);
	}

	public struct LightScene
	{
		public Vector3 ambientColor;
		public float dayLightFactor;

		/// <param name="ambientColor">场景的环境光</param>
		/// <param name="dayLightFactor">场景受日光影响的程度</param>
		public LightScene(Vector3 ambientColor, float dayLightFactor)
		{
			this.ambientColor = ambientColor;
			this.dayLightFactor = dayLightFactor;
		}
	}
}