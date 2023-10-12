using Everglow.Commons.Enums;

namespace Everglow.Commons.Collider;

public class ColliderManager : ILoadable
{
	public const float AirSpeed = 0.001f;

	public static bool EnableHook { get; set; } = true;

	public static bool Enable { get; private set; }

	public static ColliderManager Instance => ModContent.GetInstance<ColliderManager>();

	public void Add(RigidEntity entity)
	{
		Enable = true;
		if (rigidbody.Count > Capacity)
		{
			int count = rigidbody.RemoveAll(tile => !tile.Active);
			if (count == 0 && !AllowOverflow)
			{
				rigidbody[0] = entity;
				return;
			}
		}
		rigidbody.Add(entity);
	}

	public void Clear()
	{
		rigidbody.Clear();
		Enable = false;
	}

	public void Draw()
	{
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		foreach (var tile in rigidbody)
		{
			tile.Draw();
		}
		Main.spriteBatch.End();
	}

	public void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{
		foreach (var tile in rigidbody)
		{
			tile.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
		}
	}

	public bool First(AABB aabb, out RigidEntity result)
	{
		foreach (var entity in rigidbody)
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

	public bool Intersect(AABB aabb)
	{
		foreach(var entity in rigidbody)
		{
			if (entity.Intersect(aabb))
			{
				return true;
			}
		}
		return false;
	}

	public void Load(Mod mod)
	{
		On_Collision.LaserScan += Collision_LaserScan;
		On_Collision.TileCollision += Collision_TileCollision;
		On_Collision.SolidCollision_Vector2_int_int += Collision_SolidCollision_Vector2_int_int;
		On_Collision.SolidCollision_Vector2_int_int_bool += Collision_SolidCollision_Vector2_int_int_bool;
		Ins.HookManager.AddHook(CodeLayer.PostUpdateEverything, Update);
		Ins.HookManager.AddHook(CodeLayer.PostDrawTiles, Draw);
		Ins.HookManager.AddHook(CodeLayer.PostDrawMapIcons, DrawToMap);
		Ins.HookManager.AddHook(CodeLayer.PostExitWorld_Single, Clear);
	}

	public IEnumerable<CollisionResult> Move(IBox box, Vector2 stride)
	{
		var list = new List<CollisionResult>();
		foreach (var entity in rigidbody)
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

	public IEnumerable<T> OfType<T>()
	{
		return rigidbody.OfType<T>();
	}

	public void Unload()
	{
		On_Collision.LaserScan -= Collision_LaserScan;
		On_Collision.TileCollision -= Collision_TileCollision;
		On_Collision.SolidCollision_Vector2_int_int -= Collision_SolidCollision_Vector2_int_int;
		On_Collision.SolidCollision_Vector2_int_int_bool -= Collision_SolidCollision_Vector2_int_int_bool;
	}

	public void Update()
	{
		for (int i = 0; i < rigidbody.Count; i++)
		{
			var tile = rigidbody[i];
			if (tile.Active)
			{
				tile.Update();
			}
		}
	}

	private const bool AllowOverflow = true;

	private const int Capacity = 100;

	private List<RigidEntity> rigidbody = new();

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
			foreach(var entity in rigidbody)
			{
				if(entity.Collision(box, stride, out var result))
				{
					stride = result.Stride;
				}
			}
			return stride;
		}
		return stride;
	}
}