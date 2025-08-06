using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Weapons.Melee;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Tools;

public class MiningPowerPickaxe : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Tools;

    public const int Pick = 59;
    public const int PickTileMax = 8;
    public const int SearchTileMax = 50;
    public const int SearchTileRange = 24 * 16; // 24 Blocks Range
    public const int ChargeMax = 400;
    public const int ChargeCost = 5;

    public int Charge { get; set; } = 0;

    private float ChargeProgress => Charge / (float)ChargeMax;

    private string ChargeProgressText => $"{Charge}/{ChargeMax}";

    public override void SetDefaults()
    {
        Item.width = 56;
        Item.height = 50;

        Item.pick = Pick;
        Item.tileBoost = 1;

        Item.DamageType = DamageClass.Melee;
        Item.damage = 28;
        Item.knockBack = 2f;

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 5; // The 'useAnimation' of pickaxe must less than useTime, to avoid duplicate call of UseItem() returning true.
        Item.useTime = 25;
        Item.autoReuse = true;
        Item.useTurn = true;

        Item.noUseGraphic = true;
        Item.channel = true;
        Item.noMelee = true;

        Item.shoot = ModContent.ProjectileType<MiningPowerPickaxe_Proj>();
        Item.shootSpeed = 10;

        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 3);
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        tooltips.Add(new TooltipLine(Mod, "Charge", Language.GetTextValue($"Charge: {ChargeProgressText}"))
        {
            OverrideColor = Color.LimeGreen,
        });
    }

    public override void HoldItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            // When mouse right is held.
            if (MouseUtils.MouseRight.IsHeld)
            {
                // Charge the pickaxe.
                if (++Charge > ChargeMax)
                {
                    Charge = ChargeMax;
                }
            }
        }
    }

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        var tileType = Main.tile[Player.tileTargetX, Player.tileTargetY].type;
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai2: tileType);
        return false;
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        // Draw charge text under player.
        if (Main.LocalPlayer.HeldItem.ModItem == this)
        {
            DrawChargeText(spriteBatch);
        }
    }

    private void DrawChargeText(SpriteBatch spriteBatch)
    {
        var stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, ChargeProgressText, Vector2.One);
        var drawPos = Main.LocalPlayer.Bottom - Main.screenPosition;
        var textColor = new Color(
            MathHelper.Lerp(1f, Color.LimeGreen.R / 255f * (0.7f + ChargeProgress * 0.3f), ChargeProgress),
            Color.LimeGreen.G / 255f * (0.9f + ChargeProgress * 0.2f),
            Color.LimeGreen.B / 255f)
            * (0.9f + 0.1f * MathF.Sin((float)Main.timeForVisualEffects * 0.04f));
        var sBS = GraphicsUtils.GetState(spriteBatch).Value;
        spriteBatch.End();
        spriteBatch.Begin(sBS);

        spriteBatch.transformMatrix = Main.GameViewMatrix.ZoomMatrix;
        spriteBatch.DrawString(FontAssets.MouseText.Value, ChargeProgressText, drawPos, textColor, 0, new Vector2(stringSize.X * 0.5f, -stringSize.Y * 0.5f), 1f, SpriteEffects.None, 0);

        spriteBatch.End();
        spriteBatch.Begin(sBS);
    }
}