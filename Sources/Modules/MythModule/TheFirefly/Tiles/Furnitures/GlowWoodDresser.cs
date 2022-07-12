using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Commons.Core.EverglowUtils;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
    public class GlowWoodDresser : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileContainer[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicDresser[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = ModContent.DustType<BlueGlow>();
            AdjTiles = new int[] { TileID.Dressers };
            DresserDrop = ModContent.ItemType<Items.Furnitures.GlowWoodDresser>();

            // Names
            ContainerName.SetDefault("GlowWood Dresser");
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("GlowWood Dresser");
            AddMapEntry(new Color(0, 14, 175), name);

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[] { TileID.MagicalIceBlock };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override bool RightClick(int i, int j)
        {
            return FurnitureUtils.DresserRightClick();
        }

        public override void MouseOverFar(int i, int j)
        {
            string chestName = this.ContainerName.GetDefault();
            FurnitureUtils.DresserMouseFar<Items.Furnitures.GlowWoodDresser>(chestName);
        }

        public override void MouseOver(int i, int j)
        {
            string chestName = this.ContainerName.GetDefault();
            FurnitureUtils.DresserMouseOver<Items.Furnitures.GlowWoodDresser>(chestName);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 16, 32, DresserDrop);
            Chest.DestroyChest(x, y);
        }
    }
}