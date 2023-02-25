using Everglow.Commons.CustomTile.Collide;
using Everglow.Commons.CustomTile.DataStructures;
using Everglow.Commons.CustomTile.EntityColliding;
using Everglow.Commons.CustomTile.Tiles;
using Everglow.Commons.Enums;

namespace Everglow.Commons.CustomTile;

public class TileSystem : ILoadable
{
	public static float AirSpeed => 0.001f;
	private static List<DynamicTile> dynamicTiles = new();
	public static IEnumerable<DynamicTile> DynamicTiles => dynamicTiles;
	public static IEnumerable<DBlock> Blocks => dynamicTiles.Where(tile => tile is DBlock).Select(tile => tile as DBlock);
	public static IEnumerable<IHookable> Hookables => dynamicTiles.Where(tile => tile is IHookable).Select(tile => tile as IHookable);
	public static IEnumerable<T> GetTiles<T>() where T : class => dynamicTiles.Where(tile => tile is T).Select(tile => tile as T);
	public static bool Enable => dynamicTiles.Count > 0;
	public void Load(Mod mod)
	{
		On_Collision.LaserScan += Collision_LaserScan;
		On_Collision.TileCollision += Collision_TileCollision;
		On_Collision.SolidCollision_Vector2_int_int += Collision_SolidCollision_Vector2_int_int;
		On_Collision.SolidCollision_Vector2_int_int_bool += Collision_SolidCollision_Vector2_int_int_bool;
		Ins.HookManager.AddHook(CodeLayer.PostUpdateEverything, Update);
		Ins.HookManager.AddHook(CodeLayer.PostDrawTiles, Draw);
		Ins.HookManager.AddHook(CodeLayer.PostDrawMapIcons, DrawToMap);
	}
	public void Unload()
	{
		dynamicTiles = null;
	}
	public static void Clear()
	{
		foreach (var tile in dynamicTiles)
		{
			tile.Kill();
		}
		dynamicTiles.Clear();
	}
	public static DynamicTile GetTile(int whoAmI) => dynamicTiles[whoAmI];
	/// <summary>
	/// 增加一个Tile，不会进行联机同步，需要手动发包
	/// </summary>
	/// <param name="tile"></param>
	public static void AddTile(DynamicTile tile)
	{
		int index = dynamicTiles.FindIndex(tile => !tile.Active);
		if (index == -1)
		{
			tile.WhoAmI = dynamicTiles.Count;
			dynamicTiles.Add(tile);
		}
		else
		{
			tile.WhoAmI = index;
			dynamicTiles[index] = tile;
		}
	}
	/// <summary>
	/// 求出所有路径上的物块和碰撞信息
	/// </summary>
	/// <param name="rect"></param>
	/// <param name="velocity"></param>
	/// <returns></returns>
	public static List<(DynamicTile tile, Direction info)> MoveCollision(EntityHandler handler, Vector2 move, bool ignorePlats = false)
	{
		List<(DynamicTile tile, Direction info)> list = new();
		AABB aabb = handler.HitBox;
		foreach (var tile in dynamicTiles)
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
	public static bool MoveCollision(ref AABB aabb, ref Vector2 velocity, bool fallthrough = false)
	{
		bool flag = false;
		Vector2 move = velocity;
		foreach (var tile in dynamicTiles)
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
	public static bool Collision(Collider collider, out DynamicTile tile)
	{
		foreach (var c in dynamicTiles)
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
	public static bool Collision(Collider collider)
	{
		foreach (var c in dynamicTiles)
		{
			if (c.Collision(collider))
				return true;
		}
		return false;
	}
	public static void Update()
	{
		for (int i = 0; i < dynamicTiles.Count; i++)
		{
			var tile = dynamicTiles[i];
			if (!tile.Active)
				dynamicTiles.RemoveAt(i--);
			else
			{
				tile.Update();
			}
		}
	}
	public static void Draw()
	{
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		foreach (var tile in dynamicTiles)
		{
			tile.Draw();
		}
		Main.spriteBatch.End();
	}
	public static void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{
		foreach (var tile in dynamicTiles)
		{
			tile.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
		}
	}
	public static bool EnableCollisionHook = true;
	private static Vector2 Collision_TileCollision(On_Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough, bool fall2, int gravDir)
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
	private static void Collision_LaserScan(On_Collision.orig_LaserScan orig, Vector2 samplingPoint, Vector2 directionUnit, float samplingWidth, float maxDistance, float[] samples)
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
	private static bool Collision_SolidCollision_Vector2_int_int_bool(On_Collision.orig_SolidCollision_Vector2_int_int_bool orig, Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
	{
		if (!Enable || !EnableCollisionHook)
			return orig(Position, Width, Height, acceptTopSurfaces);
		return orig(Position, Width, Height, acceptTopSurfaces) || Collision(new CAABB(new AABB(Position.X, Position.Y, Width, Height)));
	}
	private static bool Collision_SolidCollision_Vector2_int_int(On_Collision.orig_SolidCollision_Vector2_int_int orig, Vector2 Position, int Width, int Height)
	{
		if (!Enable || !EnableCollisionHook)
			return orig(Position, Width, Height);
		return orig(Position, Width, Height) || Collision(new CAABB(new AABB(Position.X, Position.Y, Width, Height)));
	}

}
