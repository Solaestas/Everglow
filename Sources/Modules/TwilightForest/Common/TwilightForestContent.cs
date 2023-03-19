namespace Everglow.TwilightForest.Common;

public class TwilightForestContent
{
	/// <summary>
	/// 对于暮森模块专用的获取图片封装
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static Texture2D QuickTexture(string path)
	{
		return ModContent.Request<Texture2D>("Everglow/TwilightForest/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
	}

	/// <summary>
	/// 对于暮森模块专用的获取特效封装
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static Effect QuickEffect(string path)
	{
		return ModContent.Request<Effect>("Everglow/TwilightForest/" + path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
	}

	/// <summary>
	/// 对于暮森模块专用的获取音乐封装
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static int QuickMusic(string path)
	{
		Mod everglow = ModLoader.GetMod("Everglow");
		if (everglow != null)
			return MusicLoader.GetMusicSlot(everglow, "TwilightForest/Musics/" + path);
		else
		{
			return 0;
		}
	}
}