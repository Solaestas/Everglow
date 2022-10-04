using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class MothEye : ModItem
    {
        FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 46;
            Item.value = 2000;
            Item.accessory = true;
            Item.rare = 2;
            //Item.vanity = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) *= 1.06f;

            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                player.maxTurrets += 1;
                player.wingTime += 0.20f;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeText1", "[c/7FC1FF:While in the Firefly biome:]\n[c/7FC1FF:- Increases sentry slots by 1]\n[c/7FC1FF:- Increases flight time by 20%]\n[c/7FC1FF:- All Firefly weapons deal 5% more damage]\n[c/7FC1FF:- Some Firefly-related items gain bonuses]"));
                tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeTextUnfinished", "[c/BA0022:This item is unfinished]"));
        /*        if (Language.ActiveCulture.Name == "zh-Hans")
                {
                    tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeText1zh", "[c/52A7FE:While in the Firefly biome:]\n[c/52A7FE:- Increases sentry slots by 1]\n[c/52A7FE:- Increases flight time by 10%]\n[c/52A7FE:- All Firefly weapons deal 5% more damage]\n[c/52A7FE:- Some Firefly-related items gain bonuses]"));
                    tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "ItemUnfinishedzh", "[c/BA0022:This item is unfinished]"));
                }
        */    }
            base.ModifyTooltips(tooltips);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
    /*        Vector2 slotSize = new Vector2(1f, 1f);
            position -= slotSize * Main.inventoryScale / 1f - frame.Size() * scale / 1f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 1f/* - texture.Size() * Main.inventoryScale / 2f;*/
    /*        Texture2D RArr = MythContent.QuickTexture("TheFirefly/Projectiles/FixCoin3AltLight");
            spriteBatch.Draw(RArr, drawPos, null, new Color(255, 255, 255, 50), 0f, new Vector2(8), scale * 1f, SpriteEffects.None, 0f);
            //ModContent.Request<Texture2D>("MythMod/UIImages/RightDFan").Value;
     */       return true;
        } // This currently has no effect. I want it to work so there is this glow in the back of the item in the inventory while in the firefly biome ~Setnour6
    }
}
