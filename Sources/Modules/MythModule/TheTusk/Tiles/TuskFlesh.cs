namespace Everglow.Sources.Modules.MythModule.TheTusk.Tiles
{
    public class TuskFlesh : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            MinPick = 300;
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
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i, j - 1].TileType != Type)
            {
                if (Main.tile[i, j].BottomSlope || Main.tile[i, j].LeftSlope || Main.tile[i, j].RightSlope || Main.tile[i, j].TopSlope || Main.tile[i, j].IsHalfBlock)
                {
                    return;
                }
                float SourX = i * 16 - Main.screenPosition.X;
                float SourY = j * 16 - Main.screenPosition.Y;
                float ScreenX = SourX / Main.screenWidth;
                float ScreenY = SourY / Main.screenHeight;
                float AVX = Main.LocalPlayer.velocity.X;
                float AVY = Main.LocalPlayer.velocity.Y;
                if (Main.gamePaused)
                {
                    AVX = 0;
                    AVY = 0;
                }
                /*for(int s = 0;s < 2;s++)
                {
                    float Dx = (float)(Math.Sin((i + s / 2d) / 2d + Main.time / 50d)) * 4;
                    spriteBatch.Draw(ModContent.Request<Texture2D>("MythMod/Tiles/BloodWave").Value, new Rectangle((int)(SourX + 192 + s * 8), (int)(SourY + 192 + Dx) - 2, 8, (int)(4 - Dx) + 2), new Rectangle((int)(((i + s / 2d) % 3d) * 16d), 0, 2, (int)(4 - Dx)), Lighting.GetColor(i, j));
                }*/
                //spriteBatch.Draw(Main.screenTarget, new Rectangle((int)(SourX), (int)(SourY) - 16, 16, 16), new Rectangle((int)(SourX), (int)(SourY), 16, 16), Color.White);
            }
        }
        /*public override void RandomUpdate(int i, int j)
        {
            if (!((Tile)Main.tile[i, j - 1]).HasTile)
            {
                if(Main.rand.Next(8) == 1)
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
        }*/
    }
}
