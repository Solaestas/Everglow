namespace Everglow.Commons.Enums;

public enum CodeLayer
{
	None,
	PostSetupContent,

	//绘制
	PreDrawFilter,
	[Obsolete("换用PreDrawFilter")]
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