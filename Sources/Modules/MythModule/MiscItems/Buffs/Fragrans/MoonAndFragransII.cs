﻿namespace Everglow.Sources.Modules.MythModule.MiscItems.Buffs.Fragrans
{
	public class MoonAndFragransII : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}
