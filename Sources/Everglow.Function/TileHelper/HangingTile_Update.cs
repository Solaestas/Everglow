using Everglow.Commons.Physics.MassSpringSystem;

namespace Everglow.Commons.TileHelper;

public class HangingTile_Update : ModSystem
{
	/// <summary>
	/// 物块质点系统
	/// </summary>
	public static MassSpringSystem HangingTileMassSpringSystem = new MassSpringSystem();
	public static EulerSolver HangingTileEulerSolver = new EulerSolver(8);
	public static PBDSolver HangingTilePBDSolver = new PBDSolver(8);

	public override void PostUpdateEverything()
	{
		HangingTileMassSpringSystem = new MassSpringSystem();
		foreach (var HangingTile in TileLoader.tiles.OfType<HangingTile>())
		{
			foreach (var rope in HangingTile.RopesOfAllThisTileInTheWorld.Values)
			{
				HangingTileMassSpringSystem.AddMassSpringMesh(rope);
			}
		}
		HangingTileEulerSolver.Step(HangingTileMassSpringSystem, 1);
	}

	public override void OnWorldLoad()
	{
		foreach (var HangingTile in TileLoader.tiles.OfType<HangingTile>())
		{
			HangingTile.RopesOfAllThisTileInTheWorld.Clear();
		}
		base.OnWorldLoad();
	}

	public override void OnWorldUnload()
	{
		foreach (var HangingTile in TileLoader.tiles.OfType<HangingTile>())
		{
			HangingTile.RopesOfAllThisTileInTheWorld.Clear();
		}
		base.OnWorldUnload();
	}
}