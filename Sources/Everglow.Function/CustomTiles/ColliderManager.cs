using Everglow.Commons.CustomTiles.Abstracts;
using Everglow.Commons.CustomTiles.Core;
using Everglow.Commons.Enums;
using Everglow.Commons.Physics.DataStructures;

namespace Everglow.Commons.CustomTiles;

public class ColliderManager : ILoadable
{
	private const bool AllowOverflow = true;

	private const int Capacity = 100;

	public static bool EnableHook { get; set; } = true;

	public static bool Enable { get; private set; }

	public static ColliderManager Instance => ModContent.GetInstance<ColliderManager>();

	private List<RigidEntity> rigidbodies;

	public void Load(Mod mod)
	{
		rigidbodies = [];

		On_Collision.LaserScan += Collision_LaserScan;
		On_Collision.TileCollision += Collision_TileCollision;
		On_Collision.SolidCollision_Vector2_int_int += Collision_SolidCollision_Vector2_int_int;
		On_Collision.SolidCollision_Vector2_int_int_bool += Collision_SolidCollision_Vector2_int_int_bool;

		// TODO: Step hook.
		// On_Collision.StepUp += Collision_StepUp;
		Ins.HookManager.AddHook(CodeLayer.PostUpdateEverything, Update);
		Ins.HookManager.AddHook(CodeLayer.PostDrawTiles, Draw);
		Ins.HookManager.AddHook(CodeLayer.PostDrawMapIcons, DrawToMap);
	}

	public void Unload()
	{
		rigidbodies = null;

		On_Collision.LaserScan -= Collision_LaserScan;
		On_Collision.TileCollision -= Collision_TileCollision;
		On_Collision.SolidCollision_Vector2_int_int -= Collision_SolidCollision_Vector2_int_int;
		On_Collision.SolidCollision_Vector2_int_int_bool -= Collision_SolidCollision_Vector2_int_int_bool;
	}

	/// <summary>
	/// Adds the rigid entity to the collection, enabling the manager if it is not already enabled.
	/// <br/>Not recommended for use, call <see cref="Add{TRigidEntity}(Vector2)"/> instead.
	/// </summary>
	/// <remarks>
	/// If the collection exceeds its capacity, inactive entities may be removed to make room for the new entity.
	/// <br/>If no inactive entities are available and overflow is not allowed, the first entity in the collection will be replaced.
	/// </remarks>
	/// <param name="entity">The rigid entity to add to the collection.</param>
	private void Add(RigidEntity entity)
	{
		Enable = true;
		if (rigidbodies.Count >= Capacity)
		{
			int removedCount = rigidbodies.RemoveAll(rg => !rg.Active);
			if (removedCount == 0 && !AllowOverflow) // TODO: Add more intelligent removal strategy. Allow banning removal for some important entities.
			{
				rigidbodies[0] = entity;
				return;
			}
		}
		rigidbodies.Add(entity);
	}

	/// <summary>
	/// Adds the rigid entity to the collection, enabling the manager if it is not already enabled.
	/// <br/>Automatically creates an instance of the specified rigid entity type using <c>new()</c>.
	/// </summary>
	/// <remarks>
	/// If the collection exceeds its capacity, inactive entities may be removed to make room for the new entity.
	/// <br/>If no inactive entities are available and overflow is not allowed, the first entity in the collection will be replaced.
	/// </remarks>
	/// <param name="position">The initial position of the rigid entity.</param>
	public TRigidEntity Add<TRigidEntity>(Vector2 position)
		where TRigidEntity : RigidEntity, new()
	{
		var entity = new TRigidEntity()
		{
			Position = position,
		};

		Add(entity);

		entity.SetDefaults();
		entity.OnSpawn();

		return entity;
	}

	/// <summary>
	/// Tries to find the first rigid entity that intersects with the specified axis-aligned bounding box.
	/// <br/>If none is found, returns false and sets result to null.
	/// </summary>
	/// <param name="aabb"></param>
	/// <param name="result"></param>
	/// <returns></returns>
	public bool TryFirst(AABB aabb, out RigidEntity result)
	{
		foreach (var entity in rigidbodies)
		{
			if (entity.Intersect(aabb))
			{
				result = entity;
				return true;
			}
		}
		result = null;
		return false;
	}

	/// <summary>
	/// Indicates whether any rigid entity intersects with the specified axis-aligned bounding box.
	/// </summary>
	/// <param name="aabb"></param>
	/// <returns></returns>
	public bool Intersect(AABB aabb)
	{
		foreach (var entity in rigidbodies)
		{
			if (entity.Intersect(aabb))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Attempts to move the specified box by the given stride, detecting and reporting any collisions encountered during movement.
	/// </summary>
	/// <remarks>
	/// If a collision occurs, the stride may be adjusted to prevent overlapping with other entities.
	/// <br/>The box's final position reflects the actual movement after resolving collisions.
	/// </remarks>
	/// <param name="box">The box to move. Must not be null.</param>
	/// <param name="stride">The desired movement vector to apply to the box.</param>
	/// <returns>Details about each collision encountered in the order they occurred.</returns>
	public IEnumerable<CollisionResult> Move(IBox box, Vector2 stride)
	{
		var list = new List<CollisionResult>();
		foreach (var entity in rigidbodies)
		{
			if (box.Ignore(entity))
			{
				continue;
			}

			if (entity.Collision(box, stride, out var result))
			{
				stride = result.Stride;
				list.Add(result);
			}
		}

		box.Position += stride;
		return list;
	}

	/// <summary>
	/// Filters the elements based on a specified type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public IEnumerable<T> OfType<T>()
	{
		return rigidbodies.OfType<T>();
	}

	#region Game Hooks

	private void Update()
	{
		for (int i = 0; i < rigidbodies.Count; i++)
		{
			var rg = rigidbodies[i];
			if (rg.Active)
			{
				rg.Update();
			}
		}
	}

	private void Clear()
	{
		rigidbodies.Clear();
		Enable = false;
	}

	private void Draw()
	{
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		foreach (var tile in rigidbodies)
		{
			tile.Draw();
		}
		Main.spriteBatch.End();
	}

	private void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{
		foreach (var tile in rigidbodies)
		{
			tile.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
		}
	}

	// TODO: StepHook, when player try to move on the CustomTile, it should be stepped to the top.
	// private void Collision_StepUp(On_Collision.orig_StepUp orig, ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool holdsMatching = false, int specialChecksMode = 0)
	// {
	// orig(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY, gravDir, holdsMatching, specialChecksMode);
	// if (!Enable || !EnableHook)
	// {
	// CustomTileStepUp(ref position, ref velocity, width, height, ref gfxOffY, gravDir);
	// }
	// }

	// private void CustomTileStepUp(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float gfxOffY, int gravDir = 1)
	// {
	// float ascendValue = 0;
	// if (Instance is null || MathF.Abs(velocity.X) < 1e-5)
	// {
	// return;
	// }
	// float maxAscend = 20;
	// AABB entityBoxNow = new AABB(position, new Vector2(width, height));
	// AABB entityBoxNext = new AABB(position + velocity, new Vector2(width, height));
	// if (gravDir == 1)
	// {
	// foreach (var customTile in rigidbody.OfType<BoxEntity>())
	// {
	// if (customTile.Intersect(entityBoxNext) && !customTile.Intersect(entityBoxNow) && customTile.Box.Top > entityBoxNext.Bottom - maxAscend && entityBoxNext.Bottom - customTile.Box.Top > ascendValue)
	// {
	// ascendValue = entityBoxNext.Bottom - customTile.Box.Top;
	// }
	// }
	// if (ascendValue > 0)
	// {
	// position.Y -= ascendValue * 2;
	// gfxOffY += ascendValue * 2;
	// }
	// }
	// else
	// {
	// // up-side-down
	// foreach (var customTile in rigidbody.OfType<BoxEntity>())
	// {
	// if (Intersect(entityBoxNext) && !Intersect(entityBoxNow) && customTile.Box.Bottom < entityBoxNext.Top + maxAscend && customTile.Box.Bottom - entityBoxNext.Top > ascendValue)
	// {
	// ascendValue = customTile.Box.Bottom - entityBoxNext.Top;
	// }
	// }
	// if (ascendValue > 0)
	// {
	// position.Y += ascendValue;
	// gfxOffY -= ascendValue;
	// }
	// }
	// }
	#endregion

	#region Collision Hooks

	private void Collision_LaserScan(On_Collision.orig_LaserScan orig, Vector2 samplingPoint, Vector2 directionUnit, float samplingWidth, float maxDistance, float[] samples)
	{
		if (!Enable || !EnableHook)
		{
			orig(samplingPoint, directionUnit, samplingWidth, maxDistance, samples);
			return;
		}
		orig(samplingPoint, directionUnit, samplingWidth, maxDistance, samples);
		for (int i = 0; i < samples.Length; i++)
		{
			for (int j = 0; j < 20; j++)
			{
				float t = MathHelper.Lerp(0, samples[i], j / 20f);
				Vector2 p = samplingPoint + t * directionUnit;
				if (Intersect(new AABB(p - Vector2.One * 6, Vector2.One * 12)))
				{
					samples[i] = t;
					break;
				}
			}
		}
	}

	private bool Collision_SolidCollision_Vector2_int_int(On_Collision.orig_SolidCollision_Vector2_int_int orig, Vector2 Position, int Width, int Height)
	{
		if (!Enable || !EnableHook)
		{
			return orig(Position, Width, Height);
		}
		return orig(Position, Width, Height) || Intersect(new AABB(Position.X, Position.Y, Width, Height));
	}

	private bool Collision_SolidCollision_Vector2_int_int_bool(On_Collision.orig_SolidCollision_Vector2_int_int_bool orig, Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
	{
		if (!Enable || !EnableHook)
		{
			return orig(Position, Width, Height, acceptTopSurfaces);
		}
		return orig(Position, Width, Height, acceptTopSurfaces) || Intersect(new AABB(Position.X, Position.Y, Width, Height));
	}

	private Vector2 Collision_TileCollision(On_Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough, bool fall2, int gravDir)
	{
		Vector2 stride = orig(Position, Velocity, Width, Height, fallThrough, fall2, gravDir);
		if (EnableHook && Enable)
		{
			var box = new BoxImpl()
			{
				Position = Position,
				Size = new Vector2(Width, Height),
				Gravity = gravDir,
			};
			foreach (var entity in rigidbodies)
			{
				if (entity.Collision(box, stride, out var result))
				{
					stride = result.Stride;
				}
			}
			return stride;
		}
		return stride;
	}

	#endregion

	public class ColliderManagerSystem : ModPlayer
	{
		public override void OnEnterWorld()
		{
			if (Player.whoAmI == Main.myPlayer)
			{
				Instance.Clear();
			}
		}
	}
}