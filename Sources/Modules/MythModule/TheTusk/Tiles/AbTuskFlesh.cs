namespace Everglow.Sources.Modules.MythModule.TheTusk.Tiles
{
    public class AbTuskFlesh : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<TuskFlesh>()] = true;
            Main.tileMerge[ModContent.TileType<TuskFlesh>()][Type] = true;
            Main.tileBlockLight[Type] = true;

            DustType = 5;
            ItemDrop = ModContent.ItemType<Items.TuskFlesh>();
            AddMapEntry(new Color(219, 41, 47));
            HitSound = SoundID.Grass;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        private int HasCheckPan = 0;
        private int RandomCheck = 0;
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (HasCheckPan < 3)
            {
                for (int x = -30; x < 31; x++)
                {
                    for (int y = 5; y < 300; y++)
                    {
                        if (Main.tile[i + x, j + y].TileType == (ushort)ModContent.TileType<Tiles.BloodyMossWheel>())
                        {
                            HasCheckPan += 1;
                            return;
                        }
                        if (Main.tile[i + x, j + y].TileType == (ushort)ModContent.TileType<Tiles.BloodyMossWheelFinished>())
                        {
                            HasCheckPan += 1;
                            return;
                        }
                    }
                }
                int CountCriW = 0;
                for (int y = 17; y < 300; y++)
                {
                    if (Main.tile[i, j + y].WallType == (ushort)ModContent.WallType<Walls.BloodyStoneWall>())
                    {
                        CountCriW++;
                        if (CountCriW > 2)
                        {
                            Main.tile[i, j + y + 12].TileType = (ushort)ModContent.TileType<Tiles.BloodyMossWheel>();
                            ((Tile)Main.tile[i, j + y + 12]).HasTile = true;
                            HasCheckPan += 1;
                            return;
                        }
                    }
                }
                HasCheckPan += 1;
            }
            if (RandomCheck <= 0)
            {
                if (NPC.CountNPCS(ModContent.NPCType<NPCs.Bosses.BloodTusk.BloodTusk>()) + NPC.CountNPCS(ModContent.NPCType<NPCs.Bosses.BloodTusk.TuskCooling>()) < 1)
                {
                    for (int x = -30; x < 31; x++)
                    {
                        for (int y = 5; y < 300; y++)
                        {
                            if (Main.tile[i + x, j + y].TileType == (ushort)ModContent.TileType<Tiles.BloodyMossWheelFinished>())
                            {
                                NPC.NewNPC(null, i * 16, j * 16, ModContent.NPCType<NPCs.Bosses.BloodTusk.BloodTusk>());
                                RandomCheck = 3600;
                                return;
                            }
                        }
                    }
                }
                RandomCheck = 180;
            }
            if (RandomCheck > 0)
            {
                RandomCheck--;
            }
        }
        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tile[i, j - 1].HasTile)
            {
                if (Main.rand.NextBool(8))
                {
                    switch (Main.rand.Next(1, 4))
                    {
                        case 1:
                            WorldGen.PlaceTile(i, j - 2, (ushort)ModContent.TileType<Tiles.StoneTuskSmall>());
                            short numz = (short)(Main.rand.Next(0, 12) * 24);
                            Main.tile[i, j - 2].TileFrameX = numz;
                            Main.tile[i, j - 1].TileFrameX = numz;
                            break;
                        case 2:
                            WorldGen.PlaceTile(i, j - 2, (ushort)ModContent.TileType<Tiles.StoneTuskSmall>());
                            short num = (short)(Main.rand.Next(0, 12) * 24);
                            Main.tile[i, j - 2].TileFrameX = num;
                            Main.tile[i, j - 1].TileFrameX = num;
                            break;
                        case 3:
                            WorldGen.PlaceTile(i, j - 3, (ushort)ModContent.TileType<Tiles.StoneTusk>());
                            short num1 = (short)(Main.rand.Next(0, 12) * 36);
                            Main.tile[i, j - 3].TileFrameX = num1;
                            Main.tile[i, j - 2].TileFrameX = num1;
                            Main.tile[i, j - 1].TileFrameX = num1;
                            break;
                    }
                }
            }
        }
    }
}
