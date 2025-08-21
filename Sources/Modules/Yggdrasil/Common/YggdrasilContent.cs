using Terraria.GameContent;

namespace Everglow.Yggdrasil.Common;

public class YggdrasilContent
{
	/// <summary>
	/// 对于天穹模块专用的获取音乐封装
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static int QuickMusic(string path)
	{
		Mod everglow = ModLoader.GetMod("Everglow");
		if (everglow != null)
		{
			return MusicLoader.GetMusicSlot(everglow, path);
		}
		else
		{
			return 0;
		}
	}
}