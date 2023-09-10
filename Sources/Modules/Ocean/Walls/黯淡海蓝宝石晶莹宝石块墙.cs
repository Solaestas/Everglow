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
using Everglow.Ocean.Items;

namespace Everglow.Ocean.Walls
{
    public class 黯淡海蓝宝石晶莹宝石块墙 : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[(int)base.Type] = true;
			this.DustType = 156;
            this.RegisterItemDrop(ModContent.ItemType<AquamarineBrickWall>());
			var modTranslation = base.CreateMapEntryName();
            //modTranslation.SetDefault("黯淡海蓝宝石晶莹宝石块墙");
			base.AddMapEntry(new Color(0, 115, 118), modTranslation);
		}
    }
}
