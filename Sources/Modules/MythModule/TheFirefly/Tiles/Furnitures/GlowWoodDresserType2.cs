using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Commons.Core.Utils;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ObjectData;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
    public class GlowWoodDresserType2 : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileSolidTop[Type] = true;
            Main.tileContainer[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.BasicDresser[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            DustType = ModContent.DustType<BlueGlow>();
            AdjTiles = new int[] { TileID.Dressers };
            DresserDrop = ModContent.ItemType<Items.Furnitures.GlowWoodDresserType2>();
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

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
            FurnitureUtils.DresserMouseFar<Items.Furnitures.GlowWoodDresserType2>(chestName);
        }

        public override void MouseOver(int i, int j)
        {
            string chestName = this.ContainerName.GetDefault();
            FurnitureUtils.DresserMouseOver<Items.Furnitures.GlowWoodDresserType2>(chestName);
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
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodDresserType2Glow");
            spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(0.8f, 0.8f, 0.8f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

            base.PostDraw(i, j, spriteBatch);
        }
    }
}