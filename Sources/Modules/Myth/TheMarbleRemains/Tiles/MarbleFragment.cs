using System;
using Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle;
using Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Myth.TheMarbleRemains.Tiles
{
    public class MarbleFragment : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[(int)base.Type] = true;
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            Main.tileWaterDeath[(int)base.Type] = false;
            this.MinPick = 190;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 192;
            TileObjectData.addTile((int)base.Type);
            var modTranslation = base.CreateMapEntryName();
            modTranslation.SetValue("碎片堆");
            base.AddMapEntry(new Color(146, 151, 176), modTranslation);
            base.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            //this.disableSmartCursor = true;
            this.AdjTiles = new int[]
            {
                4
            };
            //modTranslation.AddTranslation(GameCulture.Chinese, "碎片堆");
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2((float)i, (float)j) * 16f, 16, 16, 51, 0f, 0f, 1, default(Color), 1f);
            return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 2));
            Main.tile[i, j].frameX = (short)(num * 64);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.MarbleFragment>());
        }
        public override bool RightClick(int i, int j)//右击
        {
            Player player = Main.LocalPlayer;
            bool flag = true;
            for (int num66 = 0; num66 < 58; num66++)
            {
                if (player.inventory[num66].type == ModContent.ItemType<EvilFragment>() && player.inventory[num66].stack > 0)
                {
                    player.inventory[num66].stack--;
                    if (NPC.CountNPCS(ModContent.NPCType<EvilBottle>()) == 0)
                    {
                        NPC.NewNPC(null, (int)player.Center.X, (int)player.Center.Y - 200, ModContent.NPCType<EvilBottle>(), 0, 0, 0, 0, 255);
                    }
                }
            }
			return !NPC.AnyNPCs(ModContent.NPCType<EvilBottle>());
        }
        public override void MouseOver(int i, int j)
        {
            //CrystalEffectMain.Fragment = 2;
        }
    }
}
