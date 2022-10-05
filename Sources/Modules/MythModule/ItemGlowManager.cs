using Everglow.Sources.Commons.Core.ModuleSystem;
using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule;

public class ItemGlowManager : IModule
{
    private static Dictionary<int, short> glowMapping = new Dictionary<int, short>();
    private static List<Asset<Texture2D>> glowMasks = new List<Asset<Texture2D>>();
    private short begin = 0;
    public string Name => "ItemGlowManager";
    public static void AutoLoadItemGlow(ModItem modItem)
    {
        glowMapping[modItem.Type] = (short)glowMapping.Count;
        glowMasks.Add(ModContent.Request<Texture2D>($"Everglow/Sources/Modules/MythModule/Glows/{modItem.GetType().Name}_glow"));
    }
    public static short GetItemGlow(ModItem modItem)
    {
        return glowMapping.TryGetValue(modItem.Type, out var i) ? i : (short)0;
    }
    public void Load()
    {
        begin = (short)TextureAssets.GlowMask.Length;
        foreach (var key in glowMapping.Keys.ToArray())
        {
            glowMapping[key] += begin;
        }
        TextureAssets.GlowMask = TextureAssets.GlowMask.Concat(glowMasks).ToArray();
    }

    public void Unload()
    {
        TextureAssets.GlowMask = TextureAssets.GlowMask[0..begin].Concat(TextureAssets.GlowMask[(begin + glowMasks.Count)..]).ToArray();
    }
}
