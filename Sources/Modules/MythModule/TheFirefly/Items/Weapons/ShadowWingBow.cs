﻿using Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class ShadowWingBow : ModItem
    {
        FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
        public override void SetStaticDefaults()
        {
            ItemGlowManager.AutoLoadItemGlow(this);
        }

        public override void SetDefaults()
        {
            Item.glowMask = ItemGlowManager.GetItemGlow(this);
            Item.width = 46;
            Item.height = 82;
            Item.rare = ItemRarityID.Green;

            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item1;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 22;
            Item.knockBack = 5f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool CanUseItem(Player player)
        {
            if (base.CanUseItem(player))
            {
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.ShadowWingBow>()] <= 0)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.ShadowWingBow>(), (int)(damage * 0.65f), knockback, player.whoAmI, type, Item.useAnimation);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (MothEye.LocalOwner != null && MothEye.LocalOwner.TryGetModPlayer(out MothEyePlayer mothEyePlayer))
            {
                if (mothEyePlayer.MothEyeEquipped && fireflyBiome.IsBiomeActive(Main.LocalPlayer) && Main.hardMode)
                {
                    tooltips.AddRange(new TooltipLine[]
                    {
                        new(Everglow.Instance, "MothEyeBonusText", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MothEyeBonusText")),
                        new(Everglow.Instance, "MothEyeBowBonus", Language.GetTextValue("Mods.Everglow.ExtraTooltip.FireflyItems.MEyeBonusTextMothBow")),
                    });
                }
            }
        }
    }
}