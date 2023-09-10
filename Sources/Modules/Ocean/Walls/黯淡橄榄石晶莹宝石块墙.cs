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

namespace Everglow.Ocean.Walls
{
    public class 黯淡橄榄石晶莹宝石块墙 : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[(int)base.Type] = true;
			this.DustType = 163;
            this.RegisterItemDrop(ModContent.ItemType<Items.DarkOlivineBrickWall>());
			var modTranslation = base.CreateMapEntryName();
            //modTranslation.SetDefault("黯淡橄榄石晶莹宝石块墙");
			base.AddMapEntry(new Color(43, 75, 0), modTranslation);
		}
    }
}
