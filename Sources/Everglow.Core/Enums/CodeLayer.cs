namespace Everglow.Commons.Enums;

public enum CodeLayer
{
	None,

	//绘制
	PostDrawFilter,

	PostDrawTiles,
	PostDrawProjectiles,
	PostDrawDusts,
	PostDrawNPCs,
	PostDrawPlayers,
	PostDrawMapIcons,
	PostDrawBG,

	//加载
	PostUpdateEverything,
	PostUpdateProjectiles,
	PostUpdatePlayers,
	PostUpdateNPCs,
	PostUpdateDusts,
	PostUpdateInvasions,
	PostEnterWorld_Single,
	PostEnterWorld_Server,
	PostExitWorld_Single,
	ResolutionChanged
}