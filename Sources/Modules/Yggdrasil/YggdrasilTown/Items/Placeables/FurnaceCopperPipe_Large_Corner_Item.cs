using Everglow.Yggdrasil.YggdrasilTown.Tiles.FurnaceTiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class FurnaceCopperPipe_Large_Corner_Item : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

    public int State = 0;

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<FurnaceCopperPipe_Large_Corner>());
        Item.width = 16;
        Item.height = 16;
    }

    public override void HoldItem(Player player)
    {
        if (Main.mouseRight && Main.mouseRightRelease)
        {
            State += 1;
            State %= 4;
        }
        Item.placeStyle = GetPlaceStyle();
    }

    public int ConnectUp(Tile tile)
    {
        if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_V>() && tile.TileFrameY == 36)
        {
            return tile.TileFrameX % 36;
        }
        return -1;
    }

    public int ConnectDown(Tile tile)
    {
        if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_V>() && tile.TileFrameY == 0)
        {
            return tile.TileFrameX % 36;
        }
        return -1;
    }

    public int ConnectLeft(Tile tile)
    {
        if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_H>() && tile.TileFrameX % 54 == 36)
        {
            return tile.TileFrameY;
        }
        return -1;
    }

    public int ConnectRight(Tile tile)
    {
        if (tile.TileType == ModContent.TileType<FurnaceCopperPipe_Large_H>() && tile.TileFrameX % 54 == 0)
        {
            return tile.TileFrameY;
        }
        return -1;
    }

    public int GetPlaceStyle()
    {
        var center = Main.MouseWorld.ToTileCoordinates();
        int style = State;

        var upTile = Main.tile[center + new Point(0, -2)];
        var downTile = Main.tile[center + new Point(0, 2)];
        var leftTile = Main.tile[center + new Point(-2, 0)];
        var rightTile = Main.tile[center + new Point(2, 0)];
        if (ConnectRight(rightTile) == 18 && ConnectDown(downTile) == 18)
        {
            style = 0;
        }
        if (ConnectLeft(leftTile) == 18 && ConnectDown(downTile) == 0)
        {
            style = 1;
        }
        if (ConnectRight(rightTile) == 0 && ConnectUp(upTile) == 18)
        {
            style = 2;
        }
        if (ConnectLeft(leftTile) == 0 && ConnectUp(upTile) == 0)
        {
            style = 3;
        }
        return style;
    }
}