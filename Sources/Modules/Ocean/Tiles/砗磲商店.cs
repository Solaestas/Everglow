using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
	// Token: 0x02000EAA RID: 3754
    public class 砗磲商店 : ModTile
	{
        // Token: 0x06004652 RID: 18002 RVA: 0x0027C814 File Offset: 0x0027AA14
        private int A = 0;
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[(int)base.Type] = true;
			Main.tileContainer[(int)base.Type] = true;
			Main.tileShine2[(int)base.Type] = true;
			Main.tileShine[(int)base.Type] = 1200;
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
            this.MinPick = 2147483647;
            Main.tileOreFinderPriority[(int)base.Type] = 575;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Height = 4;
            TileObjectData.newTile.Width = 7;
            TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
                16,
                16,
                18
			};
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[]
			{
				127
			};
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile((int)base.Type);
			LocalizedText modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("砗磲商店");
			base.AddMapEntry(new Color(247, 145, 156), modTranslation);
			this.DustType = 155;
			TileID.Sets.DisableSmartCursor[Type]/* tModPorter Note: Removed. Use TileID.Sets.DisableSmartCursor instead */ = true;
			this.AdjTiles = new int[]
			{
				21
			};
            TileID.Sets.BasicChest[Type]/* tModPorter Note: Removed. Override DefaultContainerName and use TileID.Sets.BasicChest instead */ = true;
            //// modTranslation.AddTranslation(GameCulture.Chinese, "砗磲商店");
		}
        // Token: 0x06004653 RID: 18003 RVA: 0x0027286C File Offset: 0x00270A6C

		// Token: 0x06004654 RID: 18004 RVA: 0x00012DBC File Offset: 0x00010FBC

		// Token: 0x06004655 RID: 18005 RVA: 0x002728E0 File Offset: 0x00270AE0
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
		}
        //public override void NearbyEffects(int i, int j, bool closer)
        //{
        //   int num5 = (int)Player.FindClosest(new Vector2(i * 16, j * 16), 1, 1);
        //   if (closer && NPC.CountNPCS(ModContent.NPCType<Everglow.Ocean.NPCs.砗磲商店>()) < 1 && (new Vector2(i * 16, j * 16) - Main.player[num5].Center).Length() < 2500)
        //   {
        //       NPC.NewNPC((int)i * 16, (int)j * 16, ModContent.NPCType<Everglow.Ocean.NPCs.砗磲商店>(), 0, 0f, 0f, 0f, 0f, 255);
        //   }
        //}
        // Token: 0x06004656 RID: 18006 RVA: 0x00272914 File Offset: 0x00270B14
        public override bool RightClick(int i, int j) // TODO: Test this
		{
            Player localPlayer = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
            int num = i;
            int num2 = j;
            if (tile.TileFrameX % 126 != 0)
            {
                num--;
            }
            if (tile.TileFrameY != 0)
            {
                num2--;
            }
            if (Main.netMode != 1)
            {
                int num3 = Chest.FindChest(num, num2);
                if (num3 >= 0)
                {
                    Main.stackSplit = 600;
                    if (num3 == localPlayer.chest)
                    {
                        localPlayer.chest = -1;
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else
                    {
                        localPlayer.chest = num3;
                        Main.playerInventory = true;
                        Main.recBigList = false;
                        localPlayer.chestX = num;
                        localPlayer.chestY = num2;
                        SoundEngine.PlaySound((localPlayer.chest < 0) ? 10 : 12, -1, -1, 1, 1f, 0f);
                    }
                    Recipe.FindRecipes();
                }
                return false;
            }
            if (num == localPlayer.chestX && num2 == localPlayer.chestY && localPlayer.chest >= 0)
            {
                localPlayer.chest = -1;
                Recipe.FindRecipes();
                SoundEngine.PlaySound(SoundID.MenuClose);
                return false;
            }
            NetMessage.SendData(31, -1, -1, null, num, (float)num2, 0f, 0f, 0, 0, 0);
            Main.stackSplit = 600;
			return true;
        }
	}
}
