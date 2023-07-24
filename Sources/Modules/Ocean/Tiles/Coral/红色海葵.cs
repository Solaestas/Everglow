﻿using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
using Terraria.ObjectData;

namespace MythMod.Tiles.Ocean
{
	public class 红色海葵 : ModTile
	{
        private float num = 0;
        private int num2 = 0;
        private bool num3 = false;
        private bool flag2 = false;
        public override void SetDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                18
            };
            TileObjectData.newTile.CoordinateWidth = 40;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile((int)base.Type);
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("");
            base.AddMapEntry(new Color(36, 100, 100), modTranslation);
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			return true;
		}
        public override void NearbyEffects(int i, int j, bool closer)
        {
            bool flag = false;
            Player player = Main.player[Main.myPlayer];
            if((player.Center - new Vector2(i * 16, j * 16)).Length() < 70)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            if (flag)
            {
                if((int)(Main.time / 5) % 2 == 0 && Main.tile[i, j].frameX < 360)
                {
                    Main.tile[i, j].frameX += 40;
                }
            }
            if (Main.tile[i, j].frameX >= 40 && !flag)
            {
                if ((int)(Main.time / 5) % 3 == 0 && Main.tile[i, j].frameX > 0)
                {
                    Main.tile[i, j].frameX -= 40;
                }
            }
            if(Main.tile[i, j + 1].type == base.mod.TileType("红色海葵"))
            {
                Main.tile[i, j + 1].frameX = Main.tile[i, j].frameX;
            }
            if (Main.tile[i, j - 1].type == base.mod.TileType("红色海葵"))
            {
                Main.tile[i, j - 1].frameX = Main.tile[i, j].frameX;
            }
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, base.mod.ItemType("RedSeaAnemone"));
        }
    }
}
