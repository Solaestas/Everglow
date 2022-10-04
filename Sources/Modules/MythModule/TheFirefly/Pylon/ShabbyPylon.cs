using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Pylon
{
    internal class ShabbyPylon_TileEntity : TEModdedPylon
    {
    }

    internal class ShabbyPylon : ModPylon
    {
        public const int CrystalHorizontalFrameCount = 2;
        public const int CrystalVerticalFrameCount = 8;
        public const int CrystalFrameHeight = 64;
        public Asset<Texture2D> crystalTexture;
        public Asset<Texture2D> mapIcon;

        public override void Load()
        {
            crystalTexture = ModContent.Request<Texture2D>(Texture + "_Crystal");
            mapIcon = ModContent.Request<Texture2D>(Texture + "_MapIcon");
        }

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TEModdedPylon moddedPylon = ModContent.GetInstance<ShabbyPylon_TileEntity>();
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(moddedPylon.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(moddedPylon.Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            TileID.Sets.InteractibleByNPCs[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            AddToArray(ref TileID.Sets.CountsAsPylon);
            //ModTranslation pylonName = CreateMapEntryName();
            //AddMapEntry(Color.Transparent, pylonName);
        }

        public override int? IsPylonForSale(int npcType, Player player, bool isNPCHappyEnough)
        {
            return null;
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconEnabled = true;
            Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<ShabbyPylon_Item>();
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<ShabbyPylon_TileEntity>().Kill(i, j);
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 2, 3, ModContent.ItemType<ShabbyPylon_Item>());
        }

        public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData)
        {
            return Vector2.Distance(Main.LocalPlayer.Center, ModContent.GetInstance<ShabbyPylon_TileEntity>().Position.ToVector2() + new Vector2(24, 32)) <= 80;
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            DefaultDrawPylonCrystal(spriteBatch, i, j, crystalTexture, ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Pylon/CommonPylon_CrystalHighlight"), Vector2.Zero, Color.White, Color.Gray, 4, CrystalVerticalFrameCount);
        }

        public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
        {
            var FireflyPylon = ModContent.GetInstance<FireflyPylon_TileEntity>();
            if (FireflyPylon.IsDestoryed)
            {
                return;
            }
            else
            {
                Vector2 va = TileEntity.ByID[FireflyPylon.ID].Position.ToVector2() * 16 + new Vector2(32, 40);
                if (Vector2.Distance(va, Main.LocalPlayer.Center) > 80)
                {
                    return;
                }
                Vector2 vb = pylonInfo.PositionInTiles.ToVector2() * 16 + new Vector2(32, 40);
                if (Vector2.Distance(vb, Main.LocalPlayer.Center) > 80)
                {
                    return;
                }
            }
            bool mouseOver = DefaultDrawMapIcon(ref context, mapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
            DefaultMapClickHandle(mouseOver, pylonInfo, "Mods.MythMod.ItemName.ShabbyPylon_Item", ref mouseOverText);
        }
    }

    internal class ShabbyPylon_Item : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<ShabbyPylon>());
        }
    }
}