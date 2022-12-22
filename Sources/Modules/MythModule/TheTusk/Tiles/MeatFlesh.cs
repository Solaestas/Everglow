using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheTusk.Tiles
{
    public class MeatFlesh : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            MinPick = 200;
            DustType = 5;
            ItemDrop = ModContent.ItemType<Items.TuskFlesh>();
            AddMapEntry(new Color(219, 41, 47));
            HitSound = SoundID.Grass;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            /*if((int)(Main.time / 10) % 10 == 0)
            {
                Player player = Main.LocalPlayer;
                if ((player.Bottom - new Vector2(i * 16, j * 16)).Length() < 16)
                {
                    if (NPC.CountNPCS(ModContent.NPCType < NPCs.BloodTusk.LittleTusk>()) < 1)
                    {
                        int u = NPC.NewNPC(null, (int)player.Bottom.X, (int)player.Bottom.Y, ModContent.NPCType<NPCs.BloodTusk.LittleTusk>());
                        Main.npc[u].damage = 300;
                    }                     
                }
            }*/
        }
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {

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