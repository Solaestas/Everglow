using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Modules.ZYModule.Commons.Core;

namespace Everglow.Sources.Modules.ZYModule.TileModule;

internal class TileSystem : IModule
{

    private static List<IDynamicTile> dynamicTiles = new List<IDynamicTile>();
    public static IEnumerable<IDynamicTile> DynamicTiles => dynamicTiles;
    public static IEnumerable<DynamicTile> Tiles => dynamicTiles.Where(tile => tile is DynamicTile).Select(tile => tile as DynamicTile);
    public static IEnumerable<Block> Blocks => dynamicTiles.Where(tile => tile is Block).Select(tile => tile as Block);
    public static IEnumerable<IHookable> Hookables => dynamicTiles.Where(tile => tile is IHookable).Select(tile => tile as IHookable);
    public static IEnumerable<T> GetTiles<T>() where T : class => dynamicTiles.Where(tile => tile is T).Select(tile => tile as T);
    public static IDynamicTile GetStandTile(Entity entity)
    {
        if (entity is Player player)
        {
            return player.GetModPlayer<PlayerColliding>().standTile;
        }
        else if (entity is NPC npc)
        {
            return npc.GetGlobalNPC<NPCColliding>().standTile;
        }
        else if (entity is Projectile proj)
        {
            return proj.GetGlobalProjectile<ProjColliding>().standTile;
        }
        return null;
    }
    public static bool Enable => dynamicTiles.Count > 0;

    public string Name => "TileSystem";

    public void Load()
    {
        On.Terraria.Collision.SolidCollision_Vector2_int_int += Collision_SolidCollision_Vector2_int_int;
        On.Terraria.Collision.SolidCollision_Vector2_int_int_bool += Collision_SolidCollision_Vector2_int_int_bool;
        On.Terraria.Collision.LaserScan += Collision_LaserScan;
        On.Terraria.Collision.TileCollision += Collision_TileCollision;
        On.Terraria.Collision.SolidCollision_Vector2_int_int += Collision_SolidCollision_Vector2_int_int1;
        On.Terraria.Collision.SolidCollision_Vector2_int_int_bool += Collision_SolidCollision_Vector2_int_int_bool1;
        ModContent.GetInstance<HookSystem>().AddMethod(Update, CallOpportunity.PostUpdateEverything);
        ModContent.GetInstance<HookSystem>().AddMethod(Draw, CallOpportunity.PostDrawTiles);
        ModContent.GetInstance<HookSystem>().AddMethod(DrawToMap, CallOpportunity.PostDrawMapIcons);
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
    public static void AddTile(IDynamicTile tile)
    {
        tile.WhoAmI = dynamicTiles.Count;
        dynamicTiles.Add(tile);
    }
    /// <summary>
    /// 求出所有路径上的物块和碰撞信息
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="velocity"></param>
    /// <returns></returns>
    public static List<(IDynamicTile tile, Direction info)> MoveColliding(Entity entity, Vector2 move, bool fallthrough = false)
    {
        List<(IDynamicTile tile, Direction info)> list = new();
        var standTile = GetStandTile(entity);
        Vector2 oldpos = entity.position;
        if (standTile is not null)
        {
            standTile.StandingMoving(entity);
        }
        Vector2 delta = entity.position - oldpos;
        Vector2 trueVelocity = entity.velocity + delta;
        entity.position = oldpos;
        foreach (var tile in dynamicTiles)
        {
            Direction info;
            CRectangle tr = new CRectangle(entity.position, entity.Size);
            if (standTile != tile && (info = tile.MoveCollision(tr, ref trueVelocity, ref move, fallthrough)) != Direction.None)
            {
                list.Add((tile, info));
            }
        }
        entity.velocity = trueVelocity - delta;
        entity.position += Terraria.Collision.TileCollision(entity.position, move, entity.width, entity.height, fallthrough);
        return list;
    }
    public static bool MoveCollidingNoInfo(CRectangle rect, ref Vector2 velocity, bool fallthrough = false)
    {
        bool flag = false;
        Vector2 move = velocity;
        foreach (var tile in dynamicTiles)
        {
            Direction info;
            CRectangle tr = new CRectangle(rect.pos, rect.size);
            if ((info = tile.MoveCollision(tr, ref velocity, ref move, true)) != Direction.None)
            {
                flag = true;
            }
        }
        EnableDTCollision = false;
        rect.pos += Terraria.Collision.TileCollision(rect.pos, move, (int)rect.size.X, (int)rect.size.Y, fallthrough);
        EnableDTCollision = true;
        return flag;
    }
    public static bool Collision(ICollider collider, out IDynamicTile tile)
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
    public static bool Collision(ICollider collider)
    {
        return Collision(collider, out _);
    }
    public static void Update()
    {
        for (int i = 0; i < dynamicTiles.Count; i++)
        {
            var tile = dynamicTiles[i];
            if (!tile.Active)
            {
                dynamicTiles.RemoveAt(i--);
            }
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
    public static void DrawToMap()
    {
        var (mapTopLeft, mapX2Y2AndOff, mapRect, mapScale) = ModContent.GetInstance<HookSystem>().MapIconInfomation;
        foreach (var tile in dynamicTiles)
        {
            tile.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
        }
    }
    public static bool EnableDTCollision = true;
    private bool Collision_SolidCollision_Vector2_int_int1(On.Terraria.Collision.orig_SolidCollision_Vector2_int_int orig, Vector2 Position, int Width, int Height)
    {
        return orig(Position, Width, Height) || Collision(new CRectangle(Position.X, Position.Y, Width, Height));
    }
    private static Vector2 Collision_TileCollision(On.Terraria.Collision.orig_TileCollision orig, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough, bool fall2, int gravDir)
    {
        Vector2 result = orig(Position, Velocity, Width, Height, fallThrough, fall2, gravDir);
        if (EnableDTCollision)
        {
            var rect = new CRectangle(Position.X, Position.Y, Width, Height);
            if (MoveCollidingNoInfo(rect, ref result, fallThrough))
            {
                return rect.pos - Position;
            }
        }
        return result;
    }
    private static void Collision_LaserScan(On.Terraria.Collision.orig_LaserScan orig, Vector2 samplingPoint, Vector2 directionUnit, float samplingWidth, float maxDistance, float[] samples)
    {
        if (dynamicTiles.Count != 0)
        {
            orig(samplingPoint, directionUnit, samplingWidth, maxDistance, samples);
            for (int i = 0; i < samples.Length; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    float t = MathHelper.Lerp(0, samples[i], j / 50f);
                    Vector2 p = samplingPoint + t * directionUnit;
                    if (Collision(new CRectangle(p - Vector2.One, Vector2.One * 2)))
                    {
                        samples[i] = t;
                        break;
                    }
                }
            }
            return;
        }
        orig(samplingPoint, directionUnit, samplingWidth, maxDistance, samples);
    }
    private static bool Collision_SolidCollision_Vector2_int_int_bool(On.Terraria.Collision.orig_SolidCollision_Vector2_int_int_bool orig, Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
    {
        if (dynamicTiles.Count != 0)
        {
            var rect = new CRectangle(Position, new Vector2(Width, Height));
            return orig(Position, Width, Height, acceptTopSurfaces) || Collision(rect);
        }
        return orig(Position, Width, Height, acceptTopSurfaces);
    }
    private static bool Collision_SolidCollision_Vector2_int_int_bool1(On.Terraria.Collision.orig_SolidCollision_Vector2_int_int_bool orig, Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
    {
        return orig(Position, Width, Height, acceptTopSurfaces) || Collision(new CRectangle(Position.X, Position.Y, Width, Height));
    }
    private static bool Collision_SolidCollision_Vector2_int_int(On.Terraria.Collision.orig_SolidCollision_Vector2_int_int orig, Vector2 Position, int Width, int Height)
    {
        if (dynamicTiles.Count != 0)
        {
            var rect = new CRectangle(Position, new Vector2(Width, Height));
            return orig(Position, Width, Height) || Collision(rect);
        }
        return orig(Position, Width, Height);
    }

}
