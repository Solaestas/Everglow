using Everglow.Sources.Commons.Core.Utils;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;
using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Pylon;

internal class ShabbyPylonTileEntity : TEModdedPylon
{
}

internal class ShabbyPylon : BaseModPylon<ShabbyPylonTileEntity>
{
    public override void PostSetDefaults()
    {
        DustType = DustID.Lead;
    }
    public override int DropItemType => ModContent.ItemType<ShabbyPylonItem>();

    public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData)
    {
        return Vector2.Distance(Main.LocalPlayer.Center, ModContent.GetInstance<ShabbyPylonTileEntity>().Position.ToVector2() + new Vector2(24, 32)) <= 80;
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        float offset;
        Color shadowColor;
        bool animation;
        if (PylonSystem.Instance.shabbyPylonEnable)
        {
            AscensionTimer = MathUtils.Approach(AscensionTimer, MaxAscensionTime, 1);
            float factor = (AscensionTimer / MaxAscensionTime);
            offset = DefaultVerticalOffset * factor;
            shadowColor = Color.White * 0.1f * factor;
            animation = true;
        }
        else
        {
            offset = 0;
            shadowColor = Color.Transparent;
            animation = false;
        }

        DrawModPylon(spriteBatch, i, j, crystalTexture, crystalHighlightTexture, new Vector2(0, offset), shadowColor, Color.Gray, 4, CrystalVerticalFrameCount, animation);
    }

    public float AscensionTimer = 0;
    public const float MaxAscensionTime = 100;
    public int AnimationTimer = 0;
    public const float MaxTime = 100f;

    public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
    {
        if (!PylonSystem.Instance.shabbyPylonEnable)
        {
            return;
        }

        if (Main.mapFullscreen && PylonSystem.Instance.firstEnableAnimation)
        {
            var firefly = TileEntity.ByPosition.FirstOrDefault(pair => pair.Value is FireflyPylonTileEntity);
            var target = firefly.Key.ToWorldCoordinates() - Main.ScreenSize.ToVector2() / 2;
            AnimationTimer++;

            //TODO 这里直接改screenPosition没用，需要一个改屏幕位置和阻止玩家操作的轮子
            Main.screenPosition = (AnimationTimer / MaxTime).Lerp(Main.screenPosition, target);
            if (AnimationTimer >= MaxTime)
            {
                PylonSystem.Instance.firstEnableAnimation = false;
            }
            return;
        }

        bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
        DefaultMapClickHandle(mouseOver, pylonInfo, "Mods.Everglow.ItemName.ShabbyPylonItem", ref mouseOverText);
    }
}

internal class ShabbyPylonItem : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<ShabbyPylon>());
    }
    public override bool CanUseItem(Player player)
    {
        ushort TileID = (ushort)ModContent.TileType<ShabbyPylon>();
        var position = Main.MouseWorld;
        var bottom = position.ToTileCoordinates();

        TileObject.CanPlace(bottom.X,bottom.Y,TileID,0,0,out var tileObject);
        TileObject.Place(tileObject);
        TileObjectData.CallPostPlacementPlayerHook(bottom.X, bottom.Y, TileID, 0, 0, 0, tileObject);
        return base.CanUseItem(player);
    }
}
internal class ShabbyPylonUpdate : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        if(npc.type is NPCID.BrainofCthulhu or NPCID.EaterofWorldsHead)
        {
            if (!PylonSystem.Instance.shabbyPylonEnable && !NPC.downedBoss2)
            {
                PylonSystem.Instance.shabbyPylonEnable = true;
                PylonSystem.Instance.firstEnableAnimation = true;
                Main.NewText(Language.GetTextValue("Mods.Everglow.Common.PylonSystem.ShabbyPylonRepairedTip"));
            }
        }
    }
}
internal class ScreenMovePlayer : ModPlayer
{
    public int AnimationTimer = 0;
    public const float MaxTime = 600f;
    public override void ModifyScreenPosition()
    {
        if (PylonSystem.Instance.firstEnableAnimation)
        {
            var firefly = TileEntity.ByPosition.FirstOrDefault(pair => pair.Value is ShabbyPylonTileEntity);
            if (firefly.Value != default)
            {
                var target = firefly.Key.ToWorldCoordinates() - Main.ScreenSize.ToVector2() / 2;
                AnimationTimer++;
                float Value = (1 - MathF.Cos(AnimationTimer / 60f * MathF.PI)) / 2f;
                if (AnimationTimer >= 60 && AnimationTimer < 540)
                {
                    Value = 1;
                }
                if (AnimationTimer >= 540)
                {
                    Value = (1 + MathF.Cos((AnimationTimer - 540) / 60f * MathF.PI)) / 2f;
                }
                Main.screenPosition = (Value).Lerp(Main.screenPosition, target);
                if (AnimationTimer >= MaxTime)
                {
                    PylonSystem.Instance.firstEnableAnimation = false;
                }
                Player.immune = true;
                Player.immuneTime = 2;
            }
        }
    }
}