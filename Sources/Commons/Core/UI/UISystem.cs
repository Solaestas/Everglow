using Everglow.Sources.Commons.Core.UI;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace Everglow.Sources.Commons.Core.UI
{
    internal class UISystem : ModSystem
    {
        public static EverglowUISystem EverglowUISystem
        {
            get => Instance.system;
        }
        public static UISystem Instance 
        { 
            get => instance; 
        }
        private EverglowUISystem system;
        private static UISystem instance;
        private Point ScreenSize;
        public UISystem()
        {
            system = new EverglowUISystem();
            instance = this;
        }
        public override void Load()
        {
            base.Load();
            system.Load();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            if (ScreenSize != Main.ScreenSize)
            {
                ScreenSize = Main.ScreenSize;
                system.Calculation();
            }
            system.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "Everglow: Everglow UI System",
                    delegate
                    {
                        system.Draw(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
