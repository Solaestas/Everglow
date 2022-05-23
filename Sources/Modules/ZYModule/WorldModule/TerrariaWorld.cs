using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.UI;

namespace Everglow.Sources.Modules.ZY.WorldModule;

internal class TerrariaWorld : World
{
    public TerrariaWorld()
    {
    }

    public TerrariaWorld(WorldFileData data) : base(data)
    {
    }

    public override string WorldName => "Terraria";

    public override uint Version => (uint)(Main.WorldGeneratorVersion & 0x00_00_00_00_FF_FF_FF_FFul);

    public override void EnterWorld(UIMouseEvent evt, UIElement listeningElement)
    {
        data.SetAsActive();
        SetBaseWorldData();
        SoundEngine.PlaySound(SoundID.MenuOpen);
        Main.GetInputText("");
        Task.Run(() =>
        {
            if (!File.Exists(data.Path))
            {
                Main.menuMode = 888;
                Main.MenuUI.SetState(new UIWorldLoad());
                Main.AutogenProgress.Value = 0;
                WorldGen.clearWorld();
                Main.spawnTileX = DefaultSpawnPoint.X;
                Main.spawnTileY = DefaultSpawnPoint.Y;
                UIWorldCreation.ProcessSpecialWorldSeeds(data.SeedText);
                WorldGen.GenerateWorld(data.Seed, null);
                WorldFile.SaveWorld(Main.ActiveWorldFileData.IsCloudSave, true);
            }
            Main.menuMode = 10;
            typeof(UIWorldListItem).GetMethod("PlayGame", BindingFlags.NonPublic | BindingFlags.Instance)
                .CreateDelegate<UIElement.MouseEvent>(Everglow.ModuleManager.GetModule<WorldSystem>().dataToUI[data])
                .Invoke(evt, listeningElement);

        });
    }
}
