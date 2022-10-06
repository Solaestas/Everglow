using Terraria.ObjectData;
using Everglow.Sources.Commons.Core.Utils;
using Everglow.Sources.Modules.YggdrasilModule.Common.Utils;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Terraria.Enums;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
    public class SideHangingLantern : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16
            };
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.Tree | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
            TileObjectData.addAlternate(1);

            TileObjectData.addTile(Type);
            DustType = DustID.DynastyWood;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(151, 31, 32), modTranslation);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 16, ModContent.ItemType<Items.SideHangingLantern>());
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                var tile = Main.tile[i, j];
                foreach (Player player in Main.player)
                {
                    if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
                    {
                        if (!TileSpin.TileRotation.ContainsKey(((i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18))))
                        {
                            TileSpin.TileRotation.Add(((i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18)), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18)].X;
                            rot = TileSpin.TileRotation[((i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18))].Y;
                            float mass = 24f;
                            float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
                            if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
                            {
                                TileSpin.TileRotation[((i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18))] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove(((i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18)));
                            }
                        }
                    }
                    if (Main.tile[i, j].WallType == 0)
                    {
                        if (!TileSpin.TileRotation.ContainsKey(((i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18))))
                        {
                            TileSpin.TileRotation.Add(((i - (tile.TileFrameY % 36) / 18, j - (tile.TileFrameY % 54) / 18)), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
                        }
                    }
                }
            }
        }
        public override void HitWire(int i, int j)
        {
            FurnitureUtils.LightHitwire(i, j, Type, 1, 5, 54);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tPostTexture = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/SideHangingLantern_Post");
            Rectangle rt = new Rectangle(i * 16, j * 16, 16, 16);
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            rt.X -= (int)(Main.screenPosition.X - zero.X);
            rt.Y -= (int)(Main.screenPosition.Y - zero.Y);
            Tile tile = Main.tile[i, j];

            spriteBatch.Draw(tPostTexture, rt, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), Lighting.GetColor(i, j));

            if ((tile.TileFrameY % 54) == 0 && (tile.TileFrameX == 0 || tile.TileFrameX == 36))
            {
                TileSpin tileSpin = new TileSpin();
                tileSpin.Update(i, j);
                Texture2D tex = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/SideHangingLantern_Lantern");
                int FrameX = 0;
                if(tile.TileFrameY == 54)
                {
                    FrameX = 26;
                }
                tileSpin.DrawRotatedTile(i, j, tex, new Rectangle(FrameX, 0, 26, 36), new Vector2(13, 0), 16, 6);
                if (tile.TileFrameX == 0)
                {
                    Lighting.AddLight(i, j, 1f, 0.45f, 0.15f);
                    Lighting.AddLight(i, j + 1, 1f, 0.45f, 0.15f);
                }
            }
            return false;
        }
    }
}
