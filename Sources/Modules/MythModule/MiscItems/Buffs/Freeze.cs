﻿namespace Everglow.Sources.Modules.MythModule.MiscItems.Buffs
{
    public class Freeze : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
    }
}
