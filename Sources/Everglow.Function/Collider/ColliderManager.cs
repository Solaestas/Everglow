using Everglow.Commons.Enums;

namespace Everglow.Commons.Collider;

public class ColliderManager : ILoadable
{
	public const float AirSpeed = 0.001f;

	private const int Capacity = 100;

	private const bool AllowOverflow = true;

	public static bool EnableHook = true;

	private List<RigidEntity> _tiles = new();

	public void Load(Mod mod)
	{
		//On_Collision.LaserScan += Collision_LaserScan;
		On_Collision.TileCollision += Collision_TileCollision;
		//On_Collision.SolidCollision_Vector2_int_int += Collision_SolidCollision_Vector2_int_int;
		//On_Collision.SolidCollision_Vector2_int_int_bool += Collision_SolidCollision_Vector2_int_int_bool;
		Ins.HookManager.AddHook(CodeLayer.PostUpdateEverything, Update);
		Ins.HookManager.AddHook(CodeLayer.PostDrawTiles, Draw);
		Ins.HookManager.AddHook(CodeLayer.PostDrawMapIcons, DrawToMap);
	}

	public void Unload()
	{
		On_Collision.LaserScan -= Collision_LaserScan;
		On_Collision.TileCollision -= Collision_TileCollision;
		On_Collision.SolidCollision_Vector2_int_int -= Collision_SolidCollision_Vector2_int_int;
		On_Collision.SolidCollision_Vector2_int_int_bool -= Collision_SolidCollision_Vector2_int_int_bool;
	}

	public static ColliderManager Instance => ModContent.GetInstance<ColliderManager>();

	public static bool Enable { get; private set; }

	public IEnumerable<RigidEntity> Tiles => _tiles;

	public void Add(RigidEntity tile)
	{
		Enable = true;
		if (_tiles.Count > Capacity)
		{
			int count = _tiles.RemoveAll(tile => !tile.Active);
			if (count == 0 && !AllowOverflow)
			{
				_tiles[0] = tile;
				return;
			}
		}
		_tiles.Add(tile);
	}

	public void Clear()
	{
		_tiles.Clear();
	}

	public bool First(AABB aabb, out RigidEntity tile)
	{
		foreach (var entity in _tiles)
		{
			if (entity.Intersect(aabb))
			{
				tile = entity;
				return true;
			}
		}
		tile = null;
		return false;
	}

	public void Draw()
	{
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		foreach (var tile in _tiles)
		{
			tile.Draw();
		}
		Main.spriteBatch.End();
	}

	public void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{
		foreach (var tile in _tiles)
		{
			tile.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
		}
	}

	public IEnumerable<T> OfType<T>()
	{
		return _tiles.OfType<T>();
	}

	public IEnumerable<CollisionResult> Move(IBox box, Vector2 stride)
	{
		var list = new List<CollisionResult>();
		if(stride == Vector2.Zero)
		{
			stride += box.Gravity * CollisionUtils.Epsilon * Vector2.UnitY;
		}
		foreach (var tile in _tiles)
		{
			if (box.Ignore(tile))
			{
				continue;
			}
			if (tile.Collision(box, stride, out var result))
			{
				stride = result.Stride;
				list.Add(result);
			}
		}
		box.Position += stride;
		return list;
	}

	public void Update()
	{
		for (int i = 0; i < _tiles.Count; i++)
		{
			var tile = _tiles[i];
			if (tile.Active)
			{
				tile.Update();
			}
		}
	}

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
				if (Intersect(new AABB(p - Vector2.One, Vector2.One * 2)))
				{
					samples[i] = t;
					break;
				}
			}
		}
	}

	public bool Intersect(AABB aabb)
	{
		return First(aabb, out _);
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
		return !Enable || !EnableHook
			? orig(Position, Width, Height, acceptTopSurfaces)
			: orig(Position, Width, Height, acceptTopSurfaces) || Intersect(new AABB(Position.X, Position.Y, Width, Height));
	}

	private Vector2 Collision_TileCollision(On_Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough, bool fall2, int gravDir)
	{
		Vector2 result = orig(Position, Velocity, Width, Height, fallThrough, fall2, gravDir);
		if (EnableHook && Enable)
		{
			var box = new BoxImpl()
			{
				Position = Position,
				Size = new Vector2(Width, Height),
			};
			Move(box, Velocity);
			return box.Position - Position;
		}
		return result;
	}
}