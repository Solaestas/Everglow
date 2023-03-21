﻿using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class MothEye : ModItem
    {
        FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
        public static Player LocalOwner => Main.LocalPlayer;
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
            player.GetModPlayer<MothEyePlayer>().MothEyeEquipped = true;
            player.maxMinions += 1;
            player.maxTurrets += 1;
            player.GetDamage(DamageClass.Summon) *= 1.06f;
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                player.manaSickReduction += 4;
                player.manaCost -= 0.05f;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                tooltips.AddRange(new TooltipLine[]
                {
                    new(Everglow.Instance, "MothEyeText0", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeText0")),
                    new(Everglow.Instance, "MothEyeText1", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeText1")),
                    new(Everglow.Instance, "MothEyeText2", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeText2")),
                    new(Everglow.Instance, "MothEyeText3", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeText3")),
                    new(Everglow.Instance, "MothEyeText4", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeText4")),
                }); // Using \n would cause spacing problems in the tooltip section (blank space underneath all tooltips). ~Setnour6
            }
            else if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                tooltips.Add(new TooltipLine(Everglow.Instance, "MothEyeCriteriaText", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeCriteriaText")));
            }
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer) && Main.hardMode)
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_GlowOn");
                for (int x = 0; x < 8; x++)
                {
                    Vector2 v0 = new Vector2(0, 6 + 2f * (float)(Math.Sin(Main.timeForVisualEffects * 0.1))).RotatedBy(x / 4d * Math.PI);
                    spriteBatch.Draw(mEyeTex, position + v0, null, new Color(0.2f, 0.2f, 0.2f, 0), 0f, origin, scale, 0, 0f);
                }
                spriteBatch.Draw(mEyeTex, position, null, drawColor, 0f, origin, scale, 0, 0f);
            }
            else
            {
                Texture2D mEyeTex = MythContent.QuickTexture("TheFirefly/Items/Accessories/MothEye_GlowOff");
                for (int x = 0; x < 8; x++)
                {
                    Vector2 v0 = new Vector2(0, 8 + 3f * (float)Main.timeForVisualEffects).RotatedBy(x / 4d * Math.PI);
                    spriteBatch.Draw(mEyeTex, position + v0 * 15, null, new Color(1f, 1f, 1f, 0), 0f, origin, scale, 0, 0f);
                }
                spriteBatch.Draw(mEyeTex, position, null, drawColor, 0f, origin, scale, 0, 0f);
            }
        }

        //public override void UpdateInventory(Player player) //KEEP FOR REFERENCE
        //{
        //    bool hasMothEye = false;
        //    foreach (var item in player.armor)
        //    {
        //        if (item.type == ModContent.ItemType<Items.Accessories.MothEye>())
        //        {
        //            hasMothEye = true;
        //            break;
        //        }
        //    }
        //    base.UpdateInventory(player);
        //}

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
        FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
        public bool MothEyeEquipped;

        public override void ResetEffects()
        {
            MothEyeEquipped = false;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
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
                            ModContent.ItemType<Weapons.DreamWeaver>(), // no MothEye effect
                            ModContent.ItemType<Weapons.DustOfCorrupt>(), // no MothEye effect
                            ModContent.ItemType<Weapons.EvilChrysalis>(),
                            ModContent.ItemType<Weapons.FlowLightMissile>(), // no MothEye effect
                            ModContent.ItemType<Weapons.GlowBeadGun>(), // no MothEye effect
                            ModContent.ItemType<Weapons.GlowWoodSword>(),
                            ModContent.ItemType<Weapons.MothYoyo>(),
                            ModContent.ItemType<Weapons.NavyThunder>(), // no MothEye effect
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
        //public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit) //Example use of MothEyeEquipped
        //{
        //    if (MothEyeEquipped == true)
        //    {
        //        target.AddBuff(BuffID.Bleeding, 300);
        //    }
        //    base.OnHitNPC(item, target, damage, knockback, crit);
        //}
    }
} //   TODO: Finish Item Equip Effects (Displays a different equip texture when in the Firefly Biome, See MothEye_Neck.png and MothEye_NeckOff.png