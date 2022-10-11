using Everglow.Sources.Commons.Function.FeatureFlags;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using Terraria.Localization;

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
            Item.rare = ItemRarityID.Green;
            //Item.vanity = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.maxTurrets += 1;
            player.GetDamage(DamageClass.Summon) *= 1.06f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                tooltips.AddRange(new TooltipLine[]
                {
                    new(Everglow.Instance, "MothEyeText0", Language.GetTextValue("Mods.Everglow.ExtraItemTooltip.MothEyeText0")),
                    new(Everglow.Instance, "MothEyeText1", Language.GetTextValue("Mods.Everglow.ExtraItemTooltip.MothEyeText1")),
                    new(Everglow.Instance, "MothEyeText2", Language.GetTextValue("Mods.Everglow.ExtraItemTooltip.MothEyeText2")),
                    new(Everglow.Instance, "MothEyeText3", Language.GetTextValue("Mods.Everglow.ExtraItemTooltip.MothEyeText3")),
                    new(Everglow.Instance, "MothEyeText4", Language.GetTextValue("Mods.Everglow.ExtraItemTooltip.MothEyeText4")),
                }); // Using \n would cause spacing problems in the tooltip section (blank space underneath all tooltips). ~Setnour6
            }
            tooltips.Add(new TooltipLine(Everglow.Instance, "UnfinishedItem", Language.GetTextValue("Mods.Everglow.ExtraItemTooltip.UnfinishedItem")));
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_GlowOff");
                for (int x = 0; x < 8; x++)
                {
                    Vector2 v0 = new Vector2(0, 8 + 3f * (float)Main.timeForVisualEffects).RotatedBy(x / 4d * Math.PI);
                    spriteBatch.Draw(mEyeTex, position + v0 * 15, null, new Color(1f, 1f, 1f, 0), 0f, origin, scale, 0, 0f);
                }
                spriteBatch.Draw(mEyeTex, position, null, drawColor, 0f, origin, scale, 0, 0f);
            }
            else
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_GlowOn");
                for(int x = 0;x < 8;x++)
                {
                    Vector2 v0 = new Vector2(0, 6 + 2f * (float)(Math.Sin(Main.timeForVisualEffects * 0.1))).RotatedBy(x / 4d * Math.PI);
                    spriteBatch.Draw(mEyeTex, position + v0, null, new Color(0.2f, 0.2f, 0.2f, 0), 0f, origin, scale, 0, 0f);
                }
                spriteBatch.Draw(mEyeTex, position, null, drawColor, 0f, origin, scale, 0, 0f);
            }
        }
        //public override void EquipFrameEffects(Player player, EquipType type)
        //{
        //    if (fireflyBiome.IsBiomeActive(player))
        //    {
        //        Texture2D mEyeTex1 = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_Neck");
        //    }
        //    else
        //    {
        //        Texture2D mEyeTex2 = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_NeckOff");
        //        EquipTexture.Equals(mEyeTex2, type);
        //    }
        //}
        //TODO:DIDNOT FINISH Equipped Effect:Change texture in Firefly biome, fail.
    }
    class MothEyePlayer : ModPlayer
    {
        private FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (fireflyBiome.IsBiomeActive(Player))
            {
                for (int f = 0; f < Player.armor.Length; f++)
                {
                    if (Player.armor[f].type != ModContent.ItemType<MothEye>())
                    {
                        continue;
                    }
                    int[] FireflyWeapon =
                    {
                            ModContent.ItemType<Weapons.DarknessFan>(),
                            ModContent.ItemType<Weapons.DustOfCorrupt>(),
                            ModContent.ItemType<Weapons.EvilChrysalis>(),
                            ModContent.ItemType<Weapons.GlowBeadGun>(),
                            ModContent.ItemType<Weapons.MothYoyo>(),
                            ModContent.ItemType<Weapons.NavyThunder>(),
                            ModContent.ItemType<Weapons.PhosphorescenceGun>(),
                            ModContent.ItemType<Weapons.ScaleWingBlade>(),
                            ModContent.ItemType<Weapons.ShadowWingBow>()
                         };
                    if (Array.IndexOf(FireflyWeapon, item.type) != -1)
                    {
                        damage *= 1.05f;
                    }
                    break;
                }
            }
        }
    }
} //   TODO: Finish Item Equip Effects (Displays a different equip texture when in the Firefly Biome, See MothEye_Neck.png and MothEye_NeckOff.png
