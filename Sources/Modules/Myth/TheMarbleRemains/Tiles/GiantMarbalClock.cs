using System;
using Everglow.Myth.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Myth.TheMarbleRemains.Tiles
{
    public class GiantMarbalClock : ModTile
	{
        private float Pos = 280000;
        private float Tiome = 0;
        public override void SetStaticDefaults()
		{
			Main.tileSolid[(int)base.Type] = true;
			Main.tileMergeDirt[(int)base.Type] = false;
			Main.tileBlockLight[(int)base.Type] = true;
			Main.tileOreFinderPriority[(int)base.Type] = 700;
			this.MinPick = 500;
			this.DustType = 51;
            this.RegisterItemDrop(/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ ModContent.ItemType<Items.GiantMarbalClock>());
			var modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("遗迹大理石巨钟");
			base.AddMapEntry(new Color(30, 144, 255), modTranslation);
			this.MineResist = 5f;
			this.HitSound = SoundID.Tink;
			Main.tileSpelunker[(int)base.Type] = true;
            //modTranslation.AddTranslation(GameCulture.Chinese, "遗迹大理石巨钟");
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int I = Player.FindClosest(new Vector2(i * 16, j * 16), 1, 1);
            if(!Main.gamePaused)
            {
                Pos += Main.player[I].velocity.X;
                Tiome += 0.001f;
            }
            Color color = Lighting.GetColor(i,j);
            Color[,] colorP = new Color[150,150];
            
            /*color = Color.White;*/
            float P0 = Pos % 80 - 40;
            Vector2 v = new Vector2(P0, P0);
            float L = (float)Math.Sqrt(86 * 86 - v.Length() * v.Length()) - 18;
            Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/GiantMarbalClockBack"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Vector2.Zero + new Vector2(204, 204), new Rectangle(0, 0, 172, 172), color, 0f, new Vector2(86, 86), 1f, SpriteEffects.None, 0f);
            for (int x = 0; x < 150; x += 8)
            {
                for (int y = 0; y < 150; y += 8)
                {
                    if ((x - 75) * (x - 75) + (y - 75) * (y - 75) < 4000)
                    {
                        colorP[x, y] = Lighting.GetColor(i + x - 75, j + y - 75);
                        float S = (float)(Math.Sqrt(Math.Sqrt((x - 75) * (x - 75) + (y - 75) * (y - 75)) + 0.05f)) / 12f;
                        if (colorP[x, y] != new Color(0, 0, 0))
                        {
                            Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/GiantMarbalClockBackDot"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + new Vector2(204, 204) + new Vector2(x - 75, y - 75) * 1.2f, new Rectangle(0, 0, 16, 16), new Color(colorP[x, y].R * S / 255f, colorP[x, y].G * S / 255f, colorP[x, y].B * S / 255f, 0), 0, new Vector2(8, 8), 10f / (float)(Math.Sqrt(Math.Sqrt((x - 75) * (x - 75) + (y - 75) * (y - 75)) + 1)), SpriteEffects.None, 0f);
                            if ((x - 75) * (x - 75) + (y - 75) * (y - 75) < 1000)
                            {
                                float Ro = 0;
                                for (float u = colorP[x, y].R * S / 255f + colorP[x, y].G * S / 255f + colorP[x, y].B * S / 255f; u > 0; u -= 0.04f)
                                {
                                    Ro += 0.05f;
                                    Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/GiantMarbalClockBackDot"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + new Vector2(204, 204) - new Vector2(x - 75, y - 75).RotatedBy(Ro) * 2.4f, new Rectangle(0, 0, 16, 16), new Color(colorP[x, y].R * S / 255f, colorP[x, y].G * S / 255f, colorP[x, y].B * S / 255f, 0), 0, new Vector2(8, 8), 10f / (float)(Math.Sqrt(Math.Sqrt((x - 75) * (x - 75) + (y - 75) * (y - 75)) + 1)) * u, SpriteEffects.None, 0f);
                                    Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/GiantMarbalClockBackDot"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + new Vector2(204, 204) - new Vector2(x - 75, y - 75).RotatedBy(-Ro) * 2.4f, new Rectangle(0, 0, 16, 16), new Color(colorP[x, y].R * S / 255f, colorP[x, y].G * S / 255f, colorP[x, y].B * S / 255f, 0), 0, new Vector2(8, 8), 10f / (float)(Math.Sqrt(Math.Sqrt((x - 75) * (x - 75) + (y - 75) * (y - 75)) + 1)) * u, SpriteEffects.None, 0f);
                                }
                            }
                        }
                    }
                }
            }
            /*for (int z = 0; z < L; z += 6)
            {
                Main.spriteBatch.Draw(MythContent.QuickTexture("Tiles/GiantMarbalClockBackHalo"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + new Vector2(z / 1.4142135623731f, -z / 1.4142135623731f) + new Vector2(22, 22) + v + new Vector2(188, 188), new Rectangle(0, 0, 6, 24), color, (float)Math.PI * 0.75f, new Vector2(3, 3), 1f, SpriteEffects.None, 0f);
            }
            for (int z = 0; z < L; z += 6)
            {
                Main.spriteBatch.Draw(MythContent.QuickTexture("Tiles/GiantMarbalClockBackHalo"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) - new Vector2(z / 1.4142135623731f, -z / 1.4142135623731f) + new Vector2(22, 22) + v + new Vector2(188, 188), new Rectangle(0, 0, 6, 24), color, (float)Math.PI * 0.75f, new Vector2(3, 3), 1f, SpriteEffects.None, 0f);
            }*/
            Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/GiantMarbalClockC"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Vector2.Zero + new Vector2(204, 204), new Rectangle(0, 0, 172, 172), color, 0f, new Vector2(86, 86), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/HourPin"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Vector2.Zero + new Vector2(204, 204), new Rectangle(0, 0, 172, 172), color, Tiome / 12f, new Vector2(86, 86), 1f, SpriteEffects.None, 0f);
            Color[] colorU = new Color[50];
            Vector2 vH = new Vector2(32, 0).RotatedBy(Tiome / 12f + Math.PI / 2d);
            for (int e = 0;e < 5;e++)
            {
                colorU[e] = Lighting.GetColor(i + (int)(vH.X / 16f) * (e + 4), j + (int)(vH.Y / 16f) * (e + 4));
                colorU[e + 6] = Lighting.GetColor(i - (int)(vH.X / 16f) * (e + 4), j - (int)(vH.Y / 16f) * (e + 4));
            }
            float HR = 0;
            float HG = 0;
            float HB = 0;
            for (int f = 0;f < 12;f++)
            {
                HR += colorU[f].R;
                HG += colorU[f].G;
                HB += colorU[f].B;
            }
            Color C = new Color(HR / 1020f, HG / 1020f,HB / 1020f,0);
            Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/HourPinGlow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Vector2.Zero + new Vector2(204, 204), new Rectangle(0, 0, 172, 172), C, Tiome / 12f, new Vector2(86, 86), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/MinutePin"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Vector2.Zero + new Vector2(204, 204), new Rectangle(0, 0, 172, 172), color, Tiome, new Vector2(86, 86), 1f, SpriteEffects.None, 0f);
            Color[] colorV = new Color[50];
            Vector2 vM = new Vector2(0, -32).RotatedBy(Tiome / 12f + Math.PI / 2d);
            for (int e = 0; e < 5; e++)
            {
                colorV[e] = Lighting.GetColor(i + (int)(vM.X / 16f) * (e + 4), j + (int)(vM.Y / 16f) * (e + 4));
                colorV[e + 6] = Lighting.GetColor(i - (int)(vM.X / 16f) * (e + 4), j - (int)(vM.Y / 16f) * (e + 4));
            }
            float MR = 0;
            float MG = 0;
            float MB = 0;
            for (int f = 0; f < 12; f++)
            {
                MR += colorV[f].R;
                MG += colorV[f].G;
                MB += colorV[f].B;
            }
            Color C0 = new Color(MR / 1020f, MG / 1020f, MB / 1020f, 0);
            Main.spriteBatch.Draw(MythContent.QuickTexture("TheMarbleRemains/NPCs/Bosses/EvilBottle/Tiles/MinutePinGlow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + Vector2.Zero + new Vector2(204, 204), new Rectangle(0, 0, 172, 172), C0, Tiome, new Vector2(86, 86), 1f, SpriteEffects.None, 0f);
        }
    }
}
