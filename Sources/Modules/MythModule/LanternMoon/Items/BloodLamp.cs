using Everglow.Sources.Modules.MythModule.LanternMoon.LanternCommon;
using Terraria.Localization;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Items
{
    public class BloodLamp : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }

        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.noUseGraphic = true;
            Item.width = 38;
            Item.height = 60;
            Item.rare = 2;
            Item.scale = 1;
            Item.useStyle = 4;
            Item.useTurn = true;
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.value = 10000;
        }
        public override bool? UseItem(Player player)
        {
            if(Main.dayTime)
            {
                return false;
            }
            for(int x = 0;x < Main.maxProjectiles;x++)
            {
                if(Main.projectile[x].type == ModContent.ProjectileType<Projectiles.BloodLampProj>() && Main.projectile[x].active)
                {
                    return false;
                }
            }
            LanternMoonProgress LanterMoon = ModContent.GetInstance<LanternMoonProgress>();         
            if (!LanterMoon.OnLanternMoon && !Main.dayTime && !Main.snowMoon && !Main.pumpkinMoon)
            {
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + new Vector2(12, 0) * player.direction, new Vector2(16, 0) * player.direction, ModContent.ProjectileType<Projectiles.BloodLampProj>(), 0, 0, player.whoAmI);
                LanterMoon.OnLanternMoon = true;
                LanterMoon.Point = 0;
                LanterMoon.WavePoint = 0;
                Color messageColor = new Color(175, 75, 255);
                Color messageColor1 = Color.PaleGreen;
                Main.NewText(Language.GetTextValue("Lantern Moon is raising..."), messageColor1);
                Main.NewText(Language.GetTextValue("Wave 1:"), messageColor);
                return true;
            }
            else
            {
                return false;
            }
        }
        /*public override void AddRecipes()
        {
            CreateRecipe()
               .AddIngredient(344, 1)
               .AddIngredient(ItemID.Torch, 1)
               .AddIngredient(ModContent.ItemType<Items.Flowers.RedFlame>(), 8)
               .AddTile(26)
               .Register();
        }*/
    }
}
