using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;

namespace MythMod.Items.UnderSea
{
    public class AncientMysteryMirror : ModItem
    {
        private bool num = true;
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("古老的暗镜");
            base.Tooltip.SetDefault("Teleport you to your mouse's mirrorpoint of you");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "古老的暗镜");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "连通另一个世界的入口");
        }
        public override void SetDefaults()
        {
            base.item.melee = false;
            base.item.width = 32;
            base.item.height = 32;
            base.item.useTime = 25;
            base.item.useAnimation = 25;
            base.item.useTurn = true;
            base.item.useStyle = 1;
            base.item.value = 5000;
            base.item.UseSound = SoundID.Item1;
            base.item.autoReuse = true;
            base.item.rare = 6;
        }
        public override void AddRecipes()
        {

        }
        public override bool AltFunctionUse(Player player)
        { return true; }
        public override bool CanUseItem(Player player)
        {
            MythPlayer modPlayer = player.GetModPlayer<MythPlayer>();
            if (player.altFunctionUse == 2)
            {
            }
            else
            {
                for (int i = 0; i < 45; i++)
                {
                    Dust.NewDust(new Vector2((float)player.position.X, (float)player.position.Y), 32, 96, 131, 0f, 0f, 0, default(Color), 0.8f);
                }
                player.position = player.position - new Vector2(Main.mouseX - Main.screenWidth / 2f, Main.mouseY - Main.screenHeight / 2f);
            }
            return base.CanUseItem(player);
        }
    }
}
