using Everglow.Commons.CustomTiles.Collide;
using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.CustomTiles.EntityColliding;
using Everglow.Commons.CustomTiles.Tiles;
using Everglow.Commons.Enums;
using On.Terraria;

namespace Everglow.Commons.CustomTiles;

public class TileSystem
{
	public const float AirSpeed = 0.001f;

	public static bool EnableCollisionHook = true;

	private const int GCTime = 600;

	private static TileSystem _instance;

	private Queue<int> _freeIndices = new Queue<int>();

	private List<CustomTile> _tiles = new();

	private int _updateCount = 0;

	private TileSystem()
	{
		On_Collision.LaserScan += Collision_LaserScan;
		On_Collision.TileCollision += Collision_TileCollision;
		On_Collision.SolidCollision_Vector2_int_int += Collision_SolidCollision_Vector2_int_int;
		On_Collision.SolidCollision_Vector2_int_int_bool += Collision_SolidCollision_Vector2_int_int_bool;
		Ins.HookManager.AddHook(CodeLayer.PostUpdateEverything, Update);
		Ins.HookManager.AddHook(CodeLayer.PostDrawTiles, Draw);
		Ins.HookManager.AddHook(CodeLayer.PostDrawMapIcons, DrawToMap);
	}

	public static TileSystem Instance => _instance ??= new TileSystem();

	public static bool Enable { get; private set; }

	public IEnumerable<CustomTile> Tiles => _tiles;

	/// <summary>
	/// 增加一个Tile，不会进行联机同步，需要手动发包
	/// </summary>
	/// <param name="tile"> </param>
	public void AddTile(CustomTile tile)
	{
		Enable = true;
		if (_freeIndices.TryDequeue(out var index))
		{
			tile.WhoAmI = index;
			_tiles[index] = tile;
		}
		else
		{
			tile.WhoAmI = _tiles.Count;
			_tiles.Add(tile);
		}
	}

	public void Clear()
	{
		foreach (var tile in _tiles)
		{
			tile.Kill();
		}
		_tiles.Clear();
	}

	public bool Collision(Collider collider, out CustomTile tile)
	{
		foreach (var c in _tiles)
		{
			if (c.Collision(collider))
			{
				tile = c;
				return true;
			}
		}
		tile = null;
		return false;
	}

	public bool Collision(Collider collider)
	{
		foreach (var c in _tiles)
		{
			if (c.Collision(collider))
				return true;
		}
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

	public CustomTile GetTile(int whoAmI) => _tiles[whoAmI];

	public IEnumerable<T> GetTiles<T>() => _tiles.OfType<T>();

	/// <summary>
	/// 求出所有路径上的物块和碰撞信息
	/// </summary>
	/// <param name="rect"> </param>
	/// <param name="velocity"> </param>
	/// <returns> </returns>
	public List<(CustomTile tile, Direction info)> MoveCollision(EntityHandler handler, Vector2 move, bool ignorePlats = false)
	{
		List<(CustomTile tile, Direction info)> list = new();
		AABB aabb = handler.HitBox;
		foreach (var tile in _tiles)
		{
			Direction info = tile.MoveCollision(aabb, ref handler.trueVelocity, ref move, ignorePlats);
			if (info != Direction.None)
			{
				list.Add((tile, info));
				tile.OnCollision(aabb, info);
			}
		}
		handler.position += Terraria.Collision.TileCollision(handler.position, move, handler.GetEntity().width, handler.GetEntity().height, ignorePlats);
		return list;
	}

	public bool MoveCollision(ref AABB aabb, ref Vector2 velocity, bool fallthrough = false)
	{
		bool flag = false;
		Vector2 move = velocity;
		foreach (var tile in _tiles)
		{
			Direction info;
			if ((info = tile.MoveCollision(aabb, ref velocity, ref move, true)) != Direction.None)
				flag = true;
		}
		EnableCollisionHook = false;
		aabb.position += Terraria.Collision.TileCollision(aabb.position, move, (int)aabb.size.X, (int)aabb.size.Y, fallthrough);
		EnableCollisionHook = true;
		return flag;
	}

	public void Update()
	{
		if (_updateCount++ > GCTime)
		{
			_updateCount = 0;
			int count = _tiles.RemoveAll(tile => !tile.Active);
			if (count != 0)
			{
				for (int i = 0; i < _tiles.Count; i++)
				{
					_tiles[i].WhoAmI = i;
				}
			}
			Enable = _tiles.Count > 0;
		}

		_freeIndices.Clear();
		for (int i = 0; i < _tiles.Count; i++)
		{
			var tile = _tiles[i];
			if (tile.Active)
			{
				tile.Update();
			}
			else
			{
				_freeIndices.Enqueue(i);
			}
		}
	}

	private void Collision_LaserScan(On_Collision.orig_LaserScan orig, Vector2 samplingPoint, Vector2 directionUnit, float samplingWidth, float maxDistance, float[] samples)
	{
		if (!Enable || !EnableCollisionHook)
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
				if (Collision(new CAABB(new AABB(p - Vector2.One, Vector2.One * 2))))
				{
					samples[i] = t;
					break;
				}
			}
		}
	}

	private bool Collision_SolidCollision_Vector2_int_int(On_Collision.orig_SolidCollision_Vector2_int_int orig, Vector2 Position, int Width, int Height)
	{
		if (!Enable || !EnableCollisionHook)
			return orig(Position, Width, Height);
		return orig(Position, Width, Height) || Collision(new CAABB(new AABB(Position.X, Position.Y, Width, Height)));
	}

	private bool Collision_SolidCollision_Vector2_int_int_bool(On_Collision.orig_SolidCollision_Vector2_int_int_bool orig, Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
	{
		if (!Enable || !EnableCollisionHook)
			return orig(Position, Width, Height, acceptTopSurfaces);
		return orig(Position, Width, Height, acceptTopSurfaces) || Collision(new CAABB(new AABB(Position.X, Position.Y, Width, Height)));
	}

	private Vector2 Collision_TileCollision(On_Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough, bool fall2, int gravDir)
	{
		Vector2 result = orig(Position, Velocity, Width, Height, fallThrough, fall2, gravDir);
		if (EnableCollisionHook && Enable)
		{
			var rect = new AABB(Position.X, Position.Y, Width, Height);
			if (MoveCollision(ref rect, ref result, fallThrough))
				return rect.position - Position;
		}
		return result;
	}
}