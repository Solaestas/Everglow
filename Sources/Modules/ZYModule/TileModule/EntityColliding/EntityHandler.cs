using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

namespace Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

internal enum AttachType
{
    None,
    Stand,
    Grab
}
internal abstract class EntityHandler
{
    public abstract Entity GetEntity();
    public Vector2 position;
    public Vector2 velocity;
    public DynamicTile attachTile;
    public AttachType attachType;
    public Direction attachDir;
    public AABB HitBox => new AABB(position, GetEntity().Size);
}
internal abstract class EntityHandler<TEntity> : EntityHandler where TEntity : Entity
{
    private TEntity entity;
    public override Entity GetEntity() => entity;
    public TEntity Entity => entity;
    protected EntityHandler(TEntity entity)
    {
        this.entity = entity;
        position = entity.position;
        velocity = entity.velocity;
        attachTile = null;
    }

    public virtual bool CanAttach() => true;
    public virtual Direction Ground => Direction.Bottom;
    public virtual void OnCollision(DynamicTile tile, Direction dir) { }
    public virtual void OnAttach() { }
    public virtual void OnLeave() { }
    public virtual void Update()
    {

        Vector2 move = entity.position - position + (attachTile?.Velocity ?? Vector2.Zero);
        velocity = entity.velocity + (attachTile?.Velocity ?? Vector2.Zero);

        DynamicTile newAttach = null;
        if (attachTile != null && !attachTile.Active)
        {
            attachTile = null;
        }
        foreach (var (tile, dir) in TileSystem.MoveCollision(this, move))
        {
            OnCollision(tile, dir);
            if (dir == Direction.Bottom && CanAttach())
            {
                newAttach = tile;
                attachType = AttachType.Stand;
                attachDir = dir;
            }
        }
        //处理站在方块上
        if (newAttach == null)
        {
            if (attachTile != null)
            {
                attachTile.Leave(entity);
                OnLeave();
                attachTile = null;
                attachType = AttachType.None;
            }
        }
        else if (attachTile != newAttach)
        {
            attachTile?.Leave(entity);
            attachTile = newAttach;
            newAttach.Stand(entity, true);
        }
        else if (attachTile != null && attachTile == newAttach)
        {
            newAttach.Stand(entity, false);
        }

        //同步位置和速度
        entity.position = position;
        entity.velocity = velocity - (attachTile?.Velocity ?? Vector2.Zero);
        if (attachTile != null)
        {
            OnAttach();
        }

    }
}
