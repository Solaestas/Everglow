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

namespace MythMod.Walls
{
    public class 石榴石晶莹宝石块墙 : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[(int)base.Type] = true;
			this.DustType = 254;
            this.RegisterItemDrop(ModContent.ItemType<GarnetBrickWall>());
			var modTranslation = base.CreateMapEntryName();
            //modTranslation.SetDefault("石榴石晶莹宝石块墙");
			base.AddMapEntry(new Color(111, 9, 42), modTranslation);
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.14f;
            b = 0.34f;
            return;
        }
    }
}
