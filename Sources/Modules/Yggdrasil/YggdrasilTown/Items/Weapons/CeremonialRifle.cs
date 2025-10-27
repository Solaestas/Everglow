using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class CeremonialRifle : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.RangedWeapons;

    public override void SetDefaults()
    {
        Item.width = 90;
        Item.height = 20;

        Item.DamageType = DamageClass.Ranged;
        Item.damage = 39;
        Item.knockBack = 6f;
        Item.crit = 8;

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.UseSound = SoundID.Item40;
        Item.useTime = Item.useAnimation = 35;
        Item.noMelee = true;

        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 3);

        Item.useAmmo = AmmoID.Bullet;
        Item.shoot = ProjectileID.Bullet;
        Item.shootSpeed = 16;
    }

    public override void HoldItem(Player player)
    {
        player.GetModPlayer<CeremonialRiflePlayer>().HoldingCeremonyRifle = true;
    }

    public class CeremonialRiflePlayer : ModPlayer
    {
        public const int CrossHairCountMax = 3;
        public const float DamageMultiplication = 1.5f;

        public bool HoldingCeremonyRifle { get; set; } = false;

        public int CrossHairCount { get; set; } = 0;

        public override void ResetEffects()
        {
            if (Player.HeldItem.type != ModContent.ItemType<CeremonialRifle>())
            {
                HoldingCeremonyRifle = false;
                CrossHairCount = 0;
            }
        }

        public override void PostUpdate()
        {
            if (CrossHairCount > 0)
            {
                Player.AddBuff(ModContent.BuffType<CrossHair>(), 10);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (CrossHairCount > 0)
            {
                modifiers.DisableCrit();
                modifiers.FinalDamage *= DamageMultiplication;
                CrossHairCount--;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (HoldingCeremonyRifle && hit.Crit)
            {
                CrossHairCount = CrossHairCountMax;
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (CrossHairCount <= 0)
            {
                return;
            }

            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, CrossHairCount.ToString(), Player.Bottom - Main.screenPosition, Color.White);
        }
    }
}