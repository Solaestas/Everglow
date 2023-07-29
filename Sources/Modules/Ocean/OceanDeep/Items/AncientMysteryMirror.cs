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

namespace Everglow.Ocean.Items.UnderSea
{
    public class AncientMysteryMirror : ModItem
    {
        private bool num = true;
        public override void SetStaticDefaults()
        {
            // // base.DisplayName.SetDefault("古老的暗镜");
            // base.Tooltip.SetDefault("Teleport you to your mouse's mirrorpoint of you");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "古老的暗镜");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "连通另一个世界的入口");
        }
        public override void SetDefaults()
        {
            base.Item.melee = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
            base.Item.width = 32;
            base.Item.height = 32;
            base.Item.useTime = 25;
            base.Item.useAnimation = 25;
            base.Item.useTurn = true;
            base.Item.useStyle = 1;
            base.Item.value = 5000;
            base.Item.UseSound = SoundID.Item1;
            base.Item.autoReuse = true;
            base.Item.rare = 6;
        }
        public override void AddRecipes()
        {

        }
        public override bool AltFunctionUse(Player player)
        { return true; }
        public override bool CanUseItem(Player player)
        {
            OceanContentPlayer modPlayer = player.GetModPlayer<OceanContentPlayer>();
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
