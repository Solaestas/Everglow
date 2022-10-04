using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;

namespace Everglow.Sources.Modules.MythModule.Common.WeaponEffects
{
    public class StringDraw
    {
        public static void Load()
        {
            On.Terraria.Main.DrawGore += DrawString;
        }

        public static void UnLoad()
        {
            //On.Terraria.Main.DrawGore -= DrawString;
        }

        private static void DrawString(On.Terraria.Main.orig_DrawGore orig, Terraria.Main self)
        {
            for (int d = 0; d < Main.projectile.Length; d++)
            {
                if (Main.projectile[d].active)
                {
                    if (Main.projectile[d].type == ModContent.ProjectileType<ShadowWingBow>())
                    {
                        Player player = Main.player[Main.projectile[d].owner];
                        //Texture2D TexString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowString0");
                        Texture2D TrueString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowString");
                        int TotalLength = 34;//满弦长
                        int TotalWidth = 6;//弦宽
                        int BreakLength = 10;//断点长
                        if (player.direction == -1)
                        {
                            BreakLength = 24;
                            TrueString = MythContent.QuickTexture("TheFirefly/Items/Weapons/ShadowWingBowStringFlip");
                        }
                        float b0 = Math.Clamp((Main.projectile[d].timeLeft - 5) / 2f, 0, 60);
                        float b1 = b0 / 60f;
                        float b2 = b1;
                        float b3 = b2 * b2;
                        float DelX = -11;
                        float DelY = -12;
                        if (player.direction == -1)
                        {
                            DelY = -22;
                        }
                        Vector2 DragVertex0 = Main.projectile[d].Center + new Vector2(DelX, DelY).RotatedBy(Main.projectile[d].rotation - Math.PI * 0.25);
                        Vector2 DragVertex1 = Main.projectile[d].Center + new Vector2(DelX - 6 * b3, DelY + BreakLength).RotatedBy(Main.projectile[d].rotation - Math.PI * 0.25);//手拉力点
                        Vector2 DragVertex2 = Main.projectile[d].Center + new Vector2(DelX, DelY + TotalLength).RotatedBy(Main.projectile[d].rotation - Math.PI * 0.25);
                        Vector2 line0 = DragVertex1 - DragVertex0;
                        Vector2 line2 = DragVertex1 - DragVertex2;
                        Vector2 DrawPoint0 = (DragVertex1 + DragVertex0) / 2f;
                        Vector2 DrawPoint2 = (DragVertex1 + DragVertex2) / 2f;
                        float Rot0 = (float)(Math.Atan2(line0.Y, line0.X));
                        float Rot2 = (float)(Math.Atan2(line2.Y, line2.X));
                        float Len0 = line0.Length();
                        float Len2 = line2.Length();
                        SpriteEffects se0 = SpriteEffects.FlipVertically;
                        SpriteEffects se2 = SpriteEffects.None;
                        Color drawColor = Lighting.GetColor((int)Main.projectile[d].Center.X / 16, (int)(Main.projectile[d].Center.Y / 16));
                        //int ColS = (int)((Main.projectile[d].timeLeft - 5) * 3 / 2f + 50f);
                        Main.spriteBatch.Draw(TrueString, DrawPoint0 - Main.screenPosition, new Rectangle(0, 0, TotalWidth, BreakLength)/*第一段长度*/, drawColor, Rot0 + (float)(Math.PI * 0.5), new Vector2(TotalWidth / 2f, BreakLength / 2f), new Vector2(1f, Len0 / BreakLength), se0, 0);
                        Main.spriteBatch.Draw(TrueString, DrawPoint2 - Main.screenPosition, new Rectangle(0, BreakLength, TotalWidth, TotalLength - BreakLength)/*第二段长度*/, drawColor, Rot2 + (float)(Math.PI * 0.5), new Vector2(TotalWidth / 2f, (TotalLength - BreakLength) / 2f), new Vector2(1f, Len2 / (TotalLength - BreakLength)), se2, 0);
                        //Main.spriteBatch.Draw(TexString, Main.projectile[d].Center - Main.screenPosition, null, drawColor, Main.projectile[d].rotation - (float)(Math.PI * 0.25), new Vector2(TexString.Width / 2f, TexString.Height / 2f), 1f, se, 0);
                        //Main.spriteBatch.Draw(TexString, Main.projectile[d].Center - Main.screenPosition, null, new Color(ColS, ColS, ColS, 0), Main.projectile[d].rotation - (float)(Math.PI * 0.25), new Vector2(TexString.Width / 2f, TexString.Height / 2f), 1f, se, 0);
                    }
                }
            }
        }
    }
}