using Everglow.Sources.Commons.Core.Utils;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
    public class GlowWoodLamp : ModTile
    {
        private Asset<Texture2D> flameTexture;

        public override void SetStaticDefaults()
        {
            // Properties
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
            TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
            TileID.Sets.IsValidSpawnPoint[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            DustType = ModContent.DustType<BlueGlow>();
            AdjTiles = new int[] { TileID.Lamps };

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX); // this style already takes care of direction for us
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
            TileObjectData.addTile(Type);

            // Etc
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("GlowWood Lamp");
            AddMapEntry(new Color(0, 14, 175), name);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX < 18)
            {
                r = 0.1f;
                g = 0.9f;
                b = 1f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }

        public override void HitWire(int i, int j)
        {
            FurnitureUtils.LightHitwire(i, j, Type, 1, 3);
        }

        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 32, ModContent.ItemType<Items.Furnitures.GlowWoodLamp>());
        }
    }
}