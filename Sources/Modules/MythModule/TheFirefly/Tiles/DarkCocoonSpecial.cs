namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class DarkCocoonSpecial : ModTile//��������ħ��
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<DarkCocoon>()] = true;
            MinPick = 17500;
            DustType = 191;
            ItemDrop = ModContent.ItemType<Items.DarkCocoon>();
            AddMapEntry(new Color(17, 16, 17));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (NPC.CountNPCS(ModContent.NPCType<NPCs.Bosses.EvilPack>()) < 1)
            {
                NPC.NewNPC(null, i * 16, j * 16 + 244, ModContent.NPCType<NPCs.Bosses.EvilPack>());
            }
        }
    }
}