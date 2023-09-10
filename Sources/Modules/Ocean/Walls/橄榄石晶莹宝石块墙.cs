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
    public class 橄榄石晶莹宝石块墙 : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[(int)base.Type] = true;
			this.DustType = 163;
			this.RegisterItemDrop(ModContent.ItemType<Ocean.Items.DarkOlivineBrickWall>());
			var modTranslation = base.CreateMapEntryName();
            //modTranslation.SetDefault("橄榄石晶莹宝石块墙");
			base.AddMapEntry(new Color(43, 75, 0), modTranslation);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.375f / 2f;
            g = 0.975f / 2f;
            b = 0f;
            return;
        }
    }
}
