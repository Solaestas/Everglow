using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using MythMod.Items.Walls;

namespace Everglow.Ocean.Walls
{
    public class 黯淡石榴石晶莹宝石块墙 : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[(int)base.Type] = true;
			this.DustType = 254;
            this.RegisterItemDrop(ModContent.ItemType<DarkGarnetBrickWall>());
			var modTranslation = base.CreateMapEntryName();
            //modTranslation.SetDefault("黯淡石榴石晶莹宝石块墙");
			base.AddMapEntry(new Color(111, 9, 42), modTranslation);
		}
    }
}
