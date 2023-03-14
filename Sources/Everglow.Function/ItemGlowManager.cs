using Everglow.Commons;
using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.Myth;

/// <summary>
/// 通过GlobalItem实现自动加载Glow
/// </summary>
public class ItemGlowManager : GlobalItem
{
	private Dictionary<int, short> glowMapping = new Dictionary<int, short>();
	private List<Asset<Texture2D>> glowMasks = new List<Asset<Texture2D>>();
	private short begin = 0;

	public override void Load()
	{
		ModIns.OnPostSetupContent += LoadGlowMasks;
	}
	public void LoadGlowMasks()
	{
		begin = (short)TextureAssets.GlowMask.Length;
		// 只读，线程安全
		var asset = ModIns.Mod.Assets._assets;
		short index = 0;
		foreach(var item in ModIns.Mod.GetContent<ModItem>())
		{
			var path = item.Texture + "_glow";
			if(asset.TryGetValue(path, out var glow) && glow is Asset<Texture2D> texture)
			{
				glowMasks.Add(texture);
				glowMapping.Add(item.Type, (short)(begin + index));
				index++;
			}
		}
		TextureAssets.GlowMask = TextureAssets.GlowMask.Concat(glowMasks).ToArray();
	}
	public override void SetDefaults(Item item)
	{
		if(glowMapping.TryGetValue(item.type, out var index))
		{
			item.glowMask = glowMapping[index];
		}
	}
	public override void Unload()
	{
		ModIns.OnPostSetupContent -= LoadGlowMasks;
		TextureAssets.GlowMask = TextureAssets.GlowMask[..begin].Concat(TextureAssets.GlowMask[(begin + glowMasks.Count)..]).ToArray();
	}
}