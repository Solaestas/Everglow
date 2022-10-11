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
            player.GetDamage(DamageClass.Summon) *= 1.06f;
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
            if (fireflyBiome.IsBiomeActive(player))
            {
                player.maxTurrets += 1;
                player.wingTime += 0.20f; // For some reason, this makes it as if the player has featherfall when they hold the jump button and their wing time ends. Hopefully we fix it. ~Setnour6

                for (int f = 0; f < player.inventory.Length; f++)
                {
                    if (Array.IndexOf(FireflyWeapon, player.inventory[f].type) != -1)
                    {
                        float damageMult = getPrefixDamage(player.inventory[f].prefix);
                        float DamageNow = player.inventory[f].OriginalDamage * damageMult;
                        //player.inventory[f].damage = (int)(DamageNow * 1.05);
                    }
                }
            }
            else
            {
                for (int f = 0; f < player.inventory.Length; f++)
                {
                    if (Array.IndexOf(FireflyWeapon, player.inventory[f].type) != -1)
                    {
                        float damageMult = getPrefixDamage(player.inventory[f].prefix);
                        float DamageNow = player.inventory[f].OriginalDamage * damageMult;
                        //player.inventory[f].damage = (int)(DamageNow);
                    }
                }
            }
            
        }
        private float getPrefixDamage(int PrefixType)
        {
            float damageMult = 1f;
            if(PrefixType < 85)
            {
                switch (PrefixType)
                {
                    case 3:
                        damageMult = 1.05f;
                        break;
                    case 4:
                        damageMult = 1.1f;
                        break;
                    case 5:
                        damageMult = 1.15f;
                        break;
                    case 6:
                        damageMult = 1.1f;
                        break;
                    case 81:
                        damageMult = 1.15f;
                        break;
                    case 8:
                        damageMult = 0.85f;
                        break;
                    case 10:
                        damageMult = 0.85f;
                        break;
                    case 12:
                        damageMult = 1.05f;
                        break;
                    case 13:
                        damageMult = 0.9f;
                        break;
                    case 16:
                        damageMult = 1.1f;
                        break;
                    case 20:
                        damageMult = 1.1f;
                        break;
                    case 21:
                        damageMult = 1.1f;
                        break;
                    case 82:
                        damageMult = 1.15f;
                        break;
                    case 22:
                        damageMult = 0.85f;
                        break;
                    case 25:
                        damageMult = 1.15f;
                        break;
                    case 58:
                        damageMult = 0.85f;
                        break;
                    case 26:
                        damageMult = 1.1f;
                        break;
                    case 28:
                        damageMult = 1.15f;
                        break;
                    case 83:
                        damageMult = 1.15f;
                        break;
                    case 30:
                        damageMult = 0.9f;
                        break;
                    case 31:
                        damageMult = 0.9f;
                        break;
                    case 32:
                        damageMult = 1.1f;
                        break;
                    case 34:
                        damageMult = 1.1f;
                        break;
                    case 35:
                        damageMult = 1.15f;
                        break;
                    case 52:
                        damageMult = 0.9f;
                        break;
                    case 84:
                        damageMult = 1.17f;
                        break;
                    case 37:
                        damageMult = 1.1f;
                        break;
                    case 53:
                        damageMult = 1.1f;
                        break;
                    case 55:
                        damageMult = 1.05f;
                        break;
                    case 59:
                        damageMult = 1.15f;
                        break;
                    case 60:
                        damageMult = 1.15f;
                        break;
                    case 39:
                        damageMult = 0.7f;
                        break;
                    case 40:
                        damageMult = 0.85f;
                        break;
                    case 41:
                        damageMult = 0.9f;
                        break;
                    case 57:
                        damageMult = 1.18f;
                        break;
                    case 43:
                        damageMult = 1.1f;
                        break;
                    case 46:
                        damageMult = 1.07f;
                        break;
                    case 50:
                        damageMult = 0.8f;
                        break;
                    case 51:
                        damageMult = 1.05f;
                        break;
                }
            }
            return damageMult;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Everglow.Instance, "UnfinishedItem", Language.GetTextValue("Mods.Everglow.ExtraItemTooltip.UnfinishedItem")));
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
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

            return true;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_GlowOff");
                spriteBatch.Draw(mEyeTex, position, null, drawColor, 0f, origin, scale, 0, 0f);
            }
            else
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_GlowOn");
                spriteBatch.Draw(mEyeTex, position, null, drawColor, 0f, origin, scale, 0, 0f);
            }
        }
        //public override void EquipFrameEffects(Player player, EquipType type)
        //{
        //    if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
        //    {
        //        Texture2D mEyeTex2 = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEyeOff");
        //    }
        //    base.EquipFrameEffects(player, type);
        //} UNFINISHED. Please help
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
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
        }
    }
} //   TODO: Finish Item Equip Effects (Displays a different equip texture when in the Firefly Biome, See MothEye_Neck.png and MothEye_NeckOff.png
