using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Witherbark;

[AutoloadEquip(EquipType.Body)]
public class WitherbarkBreastPlate : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Armor;

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;

        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 0, 60, 0);

        Item.defense = 2;
    }

    public override void UpdateEquip(Player player)
    {
        player.whipRangeMultiplier += 0.2f;
    }

    public abstract class WitherbarkBreasCloakLayer : PlayerDrawLayer
    {
        public virtual int XOffsetValue => 10;

        public abstract int YOffsetValue { get; }

        public abstract Texture2D CloakTexture { get; }

        public sealed override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(ModIns.Mod, nameof(WitherbarkBreastPlate), EquipType.Body);

        public override void Draw(ref PlayerDrawSet drawInfo)
        {
            // Direction and gravity adaption.
            float xOffset = -XOffsetValue * drawInfo.drawPlayer.direction;
            float yOffset = -YOffsetValue * drawInfo.drawPlayer.gravDir;
            float rotation = drawInfo.drawPlayer.gravDir > 0 ? 0f : MathHelper.Pi;
            SpriteEffects effect = drawInfo.drawPlayer.direction * drawInfo.drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            var position = drawInfo.Center + new Vector2(xOffset, yOffset) - Main.screenPosition;
            position = new Vector2((int)position.X, (int)position.Y);
            Rectangle frame = drawInfo.drawPlayer.legFrame;
            var drawData = new DrawData(CloakTexture, position, frame, drawInfo.colorArmorBody, rotation, new Vector2(frame.Width * 0.5f, 0), 1f, effect, 0);
            drawInfo.DrawDataCache.Add(drawData);
        }
    }

    public class WitherbarkBreasCloakLayer_Front : WitherbarkBreasCloakLayer
    {
        public override int YOffsetValue => 25;

        public override Texture2D CloakTexture => ModAsset.WitherbarkBreastPlate_CloakFront.Value;

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);
    }

    public class WitherbarkBreasCloakLayer_Back : WitherbarkBreasCloakLayer
    {
        public override int YOffsetValue => 29;

        public override Texture2D CloakTexture => ModAsset.WitherbarkBreastPlate_CloakBack.Value;

        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);
    }
}