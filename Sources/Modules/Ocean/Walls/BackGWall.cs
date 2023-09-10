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
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Everglow.Ocean.Walls
{
    public class BackGWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            LocalizedText modTranslation = CreateMapEntryName();
            // modTranslation.SetDefault("");
            AddMapEntry(new Color(0, 0, 0, 255), modTranslation);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 vd = Main.screenPosition;
            Player player = Main.player[Main.myPlayer];
            //Color messageColor2 = Color.Red;
            //Color messageColor3 = Color.Green;
            //Main.NewText(Language.GetTextValue(player.position.X.ToString()), messageColor2);
            //Main.NewText(Language.GetTextValue(Main.screenPosition.X.ToString()), messageColor3);
            int Sealeval = 0;
            Vector2 vk = player.position - new Vector2(Main.screenWidth / 2f,Main.screenHeight / 2f);
            for (int X = -120; X <= Main.screenWidth * 1.5f; X += 16)
            {
                int Sea = 0;
                int Liq = 0;
                for (int Y = -120; Y <= Main.screenHeight * 1.5f; Y += 16)
                {
                    int w = 0;
                    for (int ix = -1; ix < 2; ix++)
                    {
                        for (int iy = -1; iy < 2; iy++)
                        {
                            if (Main.tile[(int)((vd.X + X) / 16f) + ix, (int)((vd.Y + Y) / 16f) + iy].LiquidAmount == byte.MaxValue && Main.tile[(int)((vd.X + X) / 16f) + ix, (int)((vd.Y + Y) / 16f) + iy].WallType <= 0)
                            {
                                w += 1;
                            }
                        }
                    }
                    if (w > 0)
                    {
                        Color color = Lighting.GetColor((int)((vd.X + X) / 16f), (int)((vd.Y + Y) / 16f));
                        //color.A = 0;
                        if (Sea == 0)
                        {
                            Sea += 1;
                            Sealeval = (int)Y;
                        }
                        if (Liq < 3)
                        {
                            Liq += 1;
                        }
                        if (Sealeval > 16)
                        {
                            if (Y + 120 >= Sealeval - 28 && Y + 120 <= Sealeval + 52)
                            {
                                int R = color.R;
                                float R0 = R;
                                R0 *= (Y - (Sealeval - 28)) / 80f;
                                R = (int)R0;
                                int G = color.G;
                                float G0 = G;
                                G0 *= (Y - (Sealeval - 28)) / 80f;
                                G = (int)G0;
                                int B = color.B;
                                float B0 = B;
                                B0 *= (Y - (Sealeval - 28)) / 80f;
                                B = (int)B0;
                                int A = color.A;
                                float A0 = A;
                                A0 *= (Y - (Sealeval - 28)) / 80f;
                                A = (int)A0;
                                if (color.R + color.G + color.B > 0)
                                {
                                    if (X + (int)(player.position.X * 1) % 1536 + 120 <= 1520)
                                    {
                                        spriteBatch.Draw(ModAsset.CoralFar.Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 1) % 1536 + 120, (int)Y + (int)(player.position.Y / 8f) - 600, 16, 16), new Color(R, G, B, A), 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(ModAsset.CoralFar.Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 1) % 1536 - 1536 + 120, (int)Y + (int)(player.position.Y / 8f) - 600, 16, 16), new Color(R, G, B, A), 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                    if (X + (int)(player.position.X * 0.6f) % 1536 + 120 <= 1520)
                                    {
                                        spriteBatch.Draw(ModContent.Request<Texture2D>("Backgrounds/CoralClose").Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 0.6f) % 1536 + 120, (int)Y + (int)(player.position.Y / 6f) - 1300, 16, 16), new Color(R, G, B, A), 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(ModContent.Request<Texture2D>("Backgrounds/CoralClose").Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 0.6f) % 1536 - 1536 + 120, (int)Y + (int)(player.position.Y / 6f) - 1300, 16, 16), new Color(R, G, B, A), 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                }
                            }
                            if (Y + 120> Sealeval + 52)
                            {
                                if (color.R + color.G + color.B > 0)
                                {
                                    if (X + (int)(player.position.X * 1) % 1536 + 120 <= 1520)
                                    {
                                        spriteBatch.Draw(ModAsset.CoralFar.Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 1) % 1536 + 120, (int)Y + (int)(player.position.Y / 8f) - 600, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(ModAsset.CoralFar.Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 1) % 1536 - 1536 + 120, (int)Y + (int)(player.position.Y / 8f) - 600, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                    if (X + (int)(player.position.X * 0.6f) % 1536 + 120 <= 1520)
                                    {
                                        spriteBatch.Draw(ModContent.Request<Texture2D>("Backgrounds/CoralClose").Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 0.6f) % 1536 + 120, (int)Y + (int)(player.position.Y / 6f) - 1300, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(ModContent.Request<Texture2D>("Backgrounds/CoralClose").Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 0.6f) % 1536 - 1536 + 120, (int)Y + (int)(player.position.Y / 6f) - 1300, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (color.R + color.G + color.B > 0)
                            {
                                if (X + (int)(player.position.X * 1) % 1536 + 120 <= 1520)
                                {
                                    spriteBatch.Draw(ModAsset.CoralFar.Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 1) % 1536 + 120, (int)Y + (int)(player.position.Y / 8f) - 600, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                }
                                else
                                {
                                    spriteBatch.Draw(ModAsset.CoralFar.Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 1) % 1536 - 1536 + 120, (int)Y + (int)(player.position.Y / 8f) - 600, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                }
                                if (X + (int)(player.position.X * 0.6f) % 1536 + 120 <= 1520)
                                {
                                    spriteBatch.Draw(ModContent.Request<Texture2D>("Backgrounds/CoralClose").Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 0.6f) % 1536 + 120, (int)Y + (int)(player.position.Y / 6f) - 1300, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                }
                                else
                                {
                                    spriteBatch.Draw(ModContent.Request<Texture2D>("Backgrounds/CoralClose").Value, new Vector2(X + 204, Y + 204), new Rectangle((int)X + (int)(player.position.X * 0.6f) % 1536 - 1536 + 120, (int)Y + (int)(player.position.Y / 6f) - 1300, 16, 16), color, 0f, new Vector2(8, 8), 1, SpriteEffects.None, 0f);
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
