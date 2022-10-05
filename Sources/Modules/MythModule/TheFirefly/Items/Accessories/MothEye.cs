using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class MothEye : ModItem
    {
        private FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();

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
                player.wingTime += 0.20f; // For some reason, this makes it as if the player has featherfall when they hold the jump button and their wing time ends. Hopefully we fix it. ~Setnour6
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeTextUnfinished", "[c/BA0022:This item is unfinished]"));
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeText1", "[c/7FC1FF:While in the Firefly biome:]\n[c/7FC1FF:- Increases sentry slots by 1]\n[c/7FC1FF:- Increases flight time by 20%]\n[c/7FC1FF:- All Firefly weapons deal 5% more damage]\n[c/7FC1FF:- Some Firefly-related items gain bonuses]"));

                //if (Language.ActiveCulture.Name == "zh-Hans")
                //{
                //    tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeText1zh", "[c/52A7FE:While in the Firefly biome:]\n[c/52A7FE:- Increases sentry slots by 1]\n[c/52A7FE:- Increases flight time by 10%]\n[c/52A7FE:- All Firefly weapons deal 5% more damage]\n[c/52A7FE:- Some Firefly-related items gain bonuses]"));
                //    tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "ItemUnfinishedzh", "[c/BA0022:This item is unfinished]"));
                //}
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 slotSize = new Vector2(52f, 52f);
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale / 2f*/;
            if (!fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEyeOff");
                Texture2D eyeInd = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_IndicationOff"); //Keep for testing purposes (Use debug in config)
                spriteBatch.Draw(eyeInd, drawPos + new Vector2(27.8f) * scale, null, new Color(255, 255, 255, 250), 0f, new Vector2(8), scale * 1.4f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(mEyeTex, position + Utils.Size(mEyeTex) / 2.6f, null, drawColor, 0f, Utils.Size(mEyeTex) / 2.7f, scale * 1.1f, 0, 0f);
                // UNFINISHED
            }
            else
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye");
                Texture2D eyeInd = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_IndicationOn");
                spriteBatch.Draw(eyeInd, drawPos + new Vector2(27.8f) * scale, null, new Color(255, 255, 255, 250), 0f, new Vector2(8), scale * 1.4f, SpriteEffects.None, 0f);
            }
            //ModContent.Request<Texture2D>("MythMod/UIImages/RightDFan").Value;
            return true; //Without a return type, error CS0161 appears ~ Setnour6
        } // This currently has no effect. I want it to work so there is this glow in the back of the item in the inventory while in the firefly biome ~Setnour6
    }
}