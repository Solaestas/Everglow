// 以后解决这个Config
using System.ComponentModel;
using Everglow.AssetReplace.UIReplace.Core;
using Terraria.ModLoader.Config;

namespace Everglow.AssetReplace;

public class AssetReplaceConfig : ModConfig
{
	/// <summary>
	/// 用于各个客户端的个性化设置
	/// </summary>
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[Header("TextureReplace")]
	[DefaultValue(TextureReplaceMode.Terraria)]
	[DrawTicks]
	public TextureReplaceMode TextureReplace;

	[Header("AudioReplace")]

	[DefaultValue(true)]
	public bool ItemPickSoundReplace;

	public override void OnChanged()
	{
		if ((int)TextureReplace >= 5)
		{
			TextureReplace = TextureReplaceMode.Terraria;
		}

		if (UIReplaceModule.IsLoaded)
		{
			UIReplaceModule.ReplaceTextures(TextureReplace);
		}

		base.OnChanged();
	}
}

public enum TextureReplaceMode
{
	Terraria,
	Default,
	EternalResolve,
	Everglow,
	Myth,
}