using Terraria.GameContent.Drawing;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.TileHelper;

/// <summary>
/// 为物块提供一个自定义FlameData的接口 <br/>
/// 注意： <br/>
/// - 仅用于 <see cref="FurnitureUtils.SwingObjectFluentDraw"/> <br/>
/// - 必须同时继承 <see cref="ITileFluentlyDrawn"/> 接口
/// </summary>
public interface ITileFlameData
{
	TileDrawing.TileFlameData GetTileFlameData(int tileX, int tileY, int type, int tileFrameY);
}
