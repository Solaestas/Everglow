using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.Audio;

namespace Everglow.Sources.Modules.YggdrasilModule.Common.Elevator
{
    internal class YggdrasilElevator : DBlock
    {
        int ContinueUp = 1;
        int PauseTime = 0;
        int Statcooling = 0;
        bool LampOn = false;
        public override void AI()
        {
            size = new Vector2(96, 16);

            Vector2 TileCenter = Center / 16f;
            int TCX = (int)TileCenter.X;
            int TCY = (int)TileCenter.Y;
            int TCWidth = (int)(size.X / 32f);
            if (PauseTime > 0)
            {
                PauseTime--;
                velocity *= 0.9f;
                if (velocity.Length() < 0.01f)
                {
                    velocity *= 0;
                }
            }
            else
            {
                PauseTime = 0;
                if (velocity.Length() < 2f)
                {
                    velocity.Y += 0.1f * ContinueUp;
                }
                Statcooling--;
            }
            if (Statcooling > 0)
            {
                Statcooling--;
            }
            else
            {
                Statcooling = 0;
                if (PauseTime == 0)
                {
                    for (int dx = 0; dx < 5; dx++)
                    {
                        if (TCX - (TCWidth + dx) < Main.maxTilesX - 20 && TCX + (TCWidth + dx) < Main.maxTilesX - 20 && TCY + (1 - ContinueUp) * ContinueUp < Main.maxTilesY - 20 && TCY + (1 - ContinueUp) * ContinueUp > 20 && TCX + (TCWidth + dx) > 20 && TCX - (TCWidth + dx) > 20)
                        {
                            Tile tileleft = Main.tile[TCX - (TCWidth + dx), TCY + (1 - ContinueUp) * ContinueUp];
                            Tile tileright = Main.tile[TCX + (TCWidth + dx), TCY + (1 - ContinueUp) * ContinueUp];
                            if (tileleft.TileType == ModContent.TileType<Tiles.LiftLamp>() && tileleft.TileFrameY == 36)
                            {
                                PauseTime = 300;
                            }
                            if (tileright.TileType == ModContent.TileType<Tiles.LiftLamp>() && tileright.TileFrameY == 36)
                            {
                                PauseTime = 300;
                            }
                        }
                    }
                }
            }

            if (PauseTime == 2)
            {
                int Lamp = 0;
                int MaxLength = 1000;
                for (int dy = 3; dy < 1000; dy++)
                {
                    for (int dx = -2; dx < 3; dx++)
                    {
                        if (TCX + dx < Main.maxTilesX - 20 && TCY + dy * ContinueUp < Main.maxTilesY - 20 && TCY + dy * ContinueUp > 20 && TCX + dx > 20)
                        {
                            Tile tile0 = Main.tile[TCX + dx, TCY + dy * ContinueUp];
                            if (tile0.TileType == ModContent.TileType<Tiles.Winch>())
                            {
                                MaxLength = dy;
                                if (ContinueUp == -1)
                                {
                                    MaxLength -= 12;
                                }
                                break;
                            }
                            if (tile0.HasTile)
                            {
                                MaxLength = dy;
                                if (ContinueUp == -1)
                                {
                                    MaxLength -= 12;
                                }
                                break;
                            }
                        }
                    }
                    if (MaxLength != 1000)
                    {
                        break;
                    }
                }
                if (MaxLength > 3)
                {
                    for (int dy = 3; dy < MaxLength; dy++)
                    {
                        for (int dx = 0; dx < 5; dx++)
                        {
                            if (TCX - (TCWidth + dx) < Main.maxTilesX - 20 && TCX + (TCWidth + dx) < Main.maxTilesX - 20 && TCY + dy * ContinueUp < Main.maxTilesY - 20 && TCY + dy * ContinueUp > 20 && TCX + (TCWidth + dx) > 20 && TCX - (TCWidth + dx) > 20)
                            {
                                Tile tileleft = Main.tile[TCX - (TCWidth + dx), TCY + dy * ContinueUp];
                                Tile tileright = Main.tile[TCX + (TCWidth + dx), TCY + dy * ContinueUp];
                                if (tileleft.TileType == ModContent.TileType<Tiles.LiftLamp>() && tileleft.TileFrameY == 36)
                                {
                                    Lamp++;

                                }
                                if (tileright.TileType == ModContent.TileType<Tiles.LiftLamp>() && tileright.TileFrameY == 36)
                                {
                                    Lamp++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    PauseTime = 300;
                }
                if (Lamp == 0)
                {
                    ContinueUp *= -1;
                }
                Statcooling = 60;
            }
        }
        public override Color MapColor => new Color(122, 91, 79);
        public override void Draw()
        {
            if (position.X / 16f < Main.maxTilesX - 28 && position.Y / 16f < Main.maxTilesY - 28 && position.X / 16f > 28 && position.Y / 16f > 28)
            {
                Color drawc = Lighting.GetColor((int)(Center.X / 16f), (int)(Center.Y / 16f) - 3);
                Main.spriteBatch.Draw(YggdrasilContent.QuickTexture("Common/Elevator/SkyTreeLift"), position - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), drawc);
                Texture2D LiftFramework = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLightOff");

                if (LampOn)
                {
                    Lighting.AddLight((int)(position.X / 16f) + 1, (int)(position.Y / 16f) - 3, 1f, 0.8f, 0f);
                }

                Color drawcLampGlow = new Color(255, 255, 255, 0);

                Texture2D LiftLampOff = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLightOff");
                Texture2D LiftLampOn = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLampOn");
                Texture2D LiftLampGlow = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLampOnGlow");
                Texture2D LiftRopeTop = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftRope");
                Texture2D LiftRope = YggdrasilContent.QuickTexture("Common/Elevator/Textures/Rope");

                Main.spriteBatch.Draw(LiftFramework, Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(LiftRopeTop, Center - Main.screenPosition + new Vector2(0, -110), null, drawc, 0, new Vector2(48, 15), 1, SpriteEffects.None, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                List<Vertex2D> bars = new List<Vertex2D>();
                for (int f = 0; f < 1000; f++)
                {
                    Color drawcRope = Lighting.GetColor((int)(position.X / 16f) + 2, (int)((position.Y - f * 12) / 16f) - 7);

                    bars.Add(new Vertex2D(Center - Main.screenPosition + new Vector2(-4, -125 - f * 12), drawcRope, new Vector3(0, f % 2, 0)));
                    bars.Add(new Vertex2D(Center - Main.screenPosition + new Vector2(4, -125 - f * 12), drawcRope, new Vector3(1, f % 2, 0)));

                    int dx = 1;
                    if ((int)(position.X / 16f) + 2 + dx < Main.maxTilesX - 28 && (int)((position.Y - f * 12) / 16f) - 7 < Main.maxTilesY - 28 && (int)(position.X / 16f) + 2 + dx > 28 && (int)((position.Y - f * 12) / 16f) - 7 > 28)
                    {
                        Tile tile0 = Main.tile[(int)(position.X / 16f) + 2 + dx, (int)((position.Y - f * 12) / 16f) - 7];
                        if (tile0.TileType == ModContent.TileType<Tiles.Winch>() && tile0.HasTile)
                        {
                            break;
                        }
                    }
                }
                if (bars.Count > 2)
                {
                    Main.graphics.GraphicsDevice.Textures[0] = LiftRope;
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                if (!LampOn)
                {
                    Main.spriteBatch.Draw(LiftLampOff, Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
                }
                else
                {
                    Main.spriteBatch.Draw(LiftLampOn, Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(LiftLampGlow, Center - Main.screenPosition + new Vector2(0, -46), null, drawcLampGlow, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
                }
                Vector2 ButtomPosition = new Vector2(-11, -33) + Center;
                if ((Main.MouseWorld - ButtomPosition).Length() < 10)
                {
                    if (Main.SmartCursorIsUsed)
                    {
                        Texture2D LiftButtomHighLight = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellMiddleButtomHightLight");
                        if (LampOn)
                        {
                            LiftButtomHighLight = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellMiddleButtomOnHightLight");
                        }
                        Main.spriteBatch.Draw(LiftButtomHighLight, Center - Main.screenPosition + new Vector2(0, -46), null, Color.White, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
                    }
                    if (Main.mouseRight && Main.mouseRightRelease)
                    {
                        SoundEngine.PlaySound(SoundID.Unlock, ButtomPosition);
                        LampOn = !LampOn;
                    }
                }
            }
        }
        public override void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
        {
            base.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
        }
    }
    public class ElevatorSummonSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if(Main.mouseRight && Main.mouseRightRelease)
            {
               
            }
        }
    }
}