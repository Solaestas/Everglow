using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Profiler.Fody;

namespace Everglow.Sources.Modules.ZYModule.InvasionModule;

/// <summary>
/// TML有一个要做ModIvasion的PR，但是目前只是草稿
/// </summary>
internal class InvasionSystem : IModule
{
    public static Invasion CurrentInvasion { get; private set; }
    public string Name => "InvasionSystem";
    public static bool InvasionBegin<T>() where T : Invasion
    {
        if (Main.invasionType != 0)
        {
            return false;
        }
        var invasion = Everglow.ModuleManager.GetModule<T>();
        CurrentInvasion = invasion;
        invasion.active = true;
        invasion.Begin();
        return true;
    }
    public static void InvasionEnd()
    {
        Main.invasionProgressMode = Main.invasionType = Main.invasionSizeStart = Main.invasionSize = 0;
        if (CurrentInvasion is not null)
        {
            CurrentInvasion.active = false;
        }
    }
    public void Load()
    {
        On.Terraria.Main.DrawInterface_15_InvasionProgressBars += Main_DrawInterface_15_InvasionProgressBars;
        On.Terraria.Main.InvasionWarning += Main_InvasionWarning;
        ModContent.GetInstance<HookSystem>().AddMethod(UpdateInvasion, CallOpportunity.PostUpdateEverything);
        ModContent.GetInstance<HookSystem>().AddMethod(LoadInvasion, CallOpportunity.PostEnterWorld_Single);
    }

    private void Main_InvasionWarning(On.Terraria.Main.orig_InvasionWarning orig)
    {
        if (Main.invasionType > Invasion.VanillaCount)
        {
            return;
        }
        orig();
    }

    private void LoadInvasion()
    {
        if (Main.invasionType <= Invasion.VanillaCount)
        {
            CurrentInvasion = null;
            return;
        }
        foreach (var inv in Everglow.ModuleManager.FindModule<Invasion>())
        {
            if (inv.InvasionID == Main.invasionType)
            {
                CurrentInvasion = inv;
                inv.active = true;
                inv.progress = Main.invasionProgress;
                return;
            }
        }
    }
    private void UpdateInvasion()
    {
        if (CurrentInvasion is not null)
        {
            if (CurrentInvasion.active)
            {
                CurrentInvasion.Update();
            }
            else
            {
                Main.invasionProgressDisplayLeft = 0;
                Main.invasionProgressAlpha -= 0.05f;
                if (Main.invasionProgressAlpha <= 0)
                {
                    CurrentInvasion = null;
                    Main.invasionProgressAlpha = Main.invasionSize = Main.invasionSizeStart = Main.invasionType = Main.invasionProgressMode = 0;
                }
            }
        }
    }
    private void Main_DrawInterface_15_InvasionProgressBars(On.Terraria.Main.orig_DrawInterface_15_InvasionProgressBars orig)
    {
        if (CurrentInvasion is null)
        {
            orig();
            return;
        }
        else
        {
            CurrentInvasion.Draw();
        }
    }

    public void Unload()
    {
        CurrentInvasion = null;
    }
}
