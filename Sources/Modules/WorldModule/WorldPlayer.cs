using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Everglow.Sources.Modules.WorldModule
{
    /// <summary>
    /// 用来实现某个我觉得非常坏的但是万象想要的效果
    /// </summary>
    internal class WorldPlayer : ModPlayer
    {
        public override void SaveData(TagCompound tag)
        {
            //TODO 储存各个WorldHistory
        }
        public override void LoadData(TagCompound tag)
        {
            //TODO 读取WorldHistory
        }
    }
    //class LS:ModSystem
    //{
    //    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    //    {
    //        layers.Add(new LegacyGameInterfaceLayer("",
    //            ()=>
    //            {
    //                string context =
    //                    $"[Lighting.OffScreenTiles]{Lighting.OffScreenTiles}\n" +
    //                    $"[Main.mapFullscreen]{Main.mapFullscreen}\n" +
    //                    $"[Main.mapStyle]{Main.mapStyle}\n" +
    //                    $"[Main.mapFullscreenScale]{Main.mapFullscreenScale}\n" +
    //                    $"[Main.mapOverlayScale]{Main.mapOverlayScale}\n" +
    //                    $"[Main.mapMinimapScale]{Main.mapMinimapScale}\n" +
    //                    $"[Main.MapScale]{Main.MapScale}\n" +
    //                    $"[num5]{(Main.mapFullscreen ? "Main.mapFullscreenScale" : ((Main.mapStyle != 1) ? "Main.mapOverlayScale" : "Main.mapMinimapScale"))}\n";
    //                ChatManager.DrawColorCodedString(Main.spriteBatch,
    //                    FontAssets.MouseText.Value,
    //                    context,
    //                    Main.MouseScreen + new Vector2(16),
    //                    Color.White,
    //                    0,
    //                    Vector2.Zero,
    //                    Vector2.One);
    //                return true;
    //            }));
    //    }
    //}
    //class LSMapLayer:ModMapLayer
    //{
    //    public override void Draw(ref MapOverlayDrawContext context, ref string text)
    //    {
    //        text =
    //            $"{Main.mapFullscreenPos}\n" +
    //            $"{Main.mapFullscreenScale}" +
    //            $"{Main.mouseX:##.##},{Main.mouseY:##.##}\n" +
    //            $"{Main.MouseScreen}\n" +
    //            $"{Main.MouseWorld}\n";
    //    }
    //}
}
