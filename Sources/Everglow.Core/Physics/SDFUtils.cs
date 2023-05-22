using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Physics
{
    internal class SDFUtils
    {
        public static Vector3 CalculateTileSDF(Vector2 pos)
        {
            int tileX = (int)Math.Floor(pos.X / 16);
            int tileY = (int)Math.Floor(pos.Y / 16);

            if (tileX < 0 || tileX >= Main.maxTilesX || tileY < 0 || tileY >= Main.maxTilesY)
            {
                return new Vector3(16, 0, 0);
            }

            bool inner = false;
            if (Main.tile[tileX, tileY].HasTile)
            {
                var tile = Main.tile[tileX, tileY];
                var collider = SDFUtils.ExtractColliderFromTile(tile, tileX, tileY);
                if (collider != null && collider.Contains(pos))
                {
                    inner = true;
                }
            }

            Vector3 targetSDF = new Vector3(16, 0, 0);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (tileX + j < 0 || tileX + j >= Main.maxTilesX || tileY + i < 0 || tileY + i >= Main.maxTilesY)
                    {
                        continue;
                    }
                    var tile = Main.tile[tileX + j, tileY + i];
                    ICollider2D collider = SDFUtils.ExtractColliderFromTile(tile, tileX + j, tileY + i, inner);

                    if (collider != null)
                    {
                        var sdf = collider.GetSDFWithGradient(pos);
                        if (sdf.X < targetSDF.X)
                        {
                            targetSDF = sdf;
                        }
                        //// Is solid block
                        //if (tile.HasTile && Main.tileSolid[tile.TileType])
                        //{
                        //    if (sdf.X < solidSDF.X)
                        //    {
                        //        solidSDF = sdf;
                        //    }
                        //}
                        //else
                        //{
                        //    if (sdf.X < airSDF.X)
                        //    {
                        //        airSDF = sdf;
                        //    }
                        //}
                    }
                }
            }

            if (inner)
            {
                return new Vector3(-targetSDF.X, targetSDF.Y, targetSDF.Z);
            }
            else
            {
                return targetSDF;
            }
        }

        public static ICollider2D ExtractColliderFromTile(in Tile tile, int i, int j, bool inverse = false)
        {
            if (!tile.HasTile || !Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType])
            {
                if (inverse)
                {
                    return new AABBCollider2D()
                    {
                        Center = new Vector2(i * 16 + 8, j * 16 + 8),
                        Size = new Vector2(8, 8)
                    };
                }
                return null;
            }

            if (tile.BlockType == BlockType.Solid || tile.BlockType == BlockType.HalfBlock)
            {
                if (tile.IsHalfBlock)
                {
                    return inverse ? new AABBCollider2D()
                    {
                        Center = new Vector2(i * 16 + 8, j * 16 + 4),
                        Size = new Vector2(8, 4)
                    }
                    : new AABBCollider2D()
                    {
                        Center = new Vector2(i * 16 + 8, j * 16 + 12),
                        Size = new Vector2(8, 4)
                    };
                }
                else
                {
                    return inverse ? null : new AABBCollider2D()
                    {
                        Center = new Vector2(i * 16 + 8, j * 16 + 8),
                        Size = new Vector2(8, 8)
                    };
                }
            }
            else
            {
                Vector2 dirLine = Vector2.One;
                if (tile.Slope == SlopeType.SlopeUpRight)
                {
                    dirLine = -dirLine;
                }
                else if (tile.Slope == SlopeType.SlopeDownRight)
                {
                    dirLine = new Vector2(1, -1);
                }
                else if (tile.Slope == SlopeType.SlopeUpLeft)
                {
                    dirLine = new Vector2(-1, 1);
                }
                // 斜坡只要线方向相反就是反形状
                return new TileTriangleCollider2D()
                {
                    Center = new Vector2(i * 16 + 8, j * 16 + 8),
                    Size = new Vector2(8, 8),
                    LineDir = inverse ? -dirLine : dirLine
                };
            }
        }
    }
}
