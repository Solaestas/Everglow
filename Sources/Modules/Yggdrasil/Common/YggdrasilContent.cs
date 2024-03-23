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
			return MusicLoader.GetMusicSlot(everglow, "Yggdrasil/Musics/" + path);
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// 对于天穹模块专用的Glowmask获取
	/// </summary>
	/// <param name="modItem"></param>
	/// <returns></returns>
	public static short SetStaticDefaultsGlowMask(ModItem modItem)
	{
		if (!Terraria.Main.dedServ)
		{
			var glowMasks = new ReLogic.Content.Asset<Texture2D>[TextureAssets.GlowMask.Length + 1];
			for (int i = 0; i < TextureAssets.GlowMask.Length; i++)
			{
				glowMasks[i] = TextureAssets.GlowMask[i];
			}
			glowMasks[glowMasks.Length - 1] = ModContent.Request<Texture2D>("Everglow/Yggdrasil/Glows/" + modItem.GetType().Name + "_glow");
			TextureAssets.GlowMask = glowMasks;
			return (short)(glowMasks.Length - 1);
		}
		else
		{
			return 0;
		}
	}
}
