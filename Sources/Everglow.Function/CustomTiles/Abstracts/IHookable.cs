namespace Everglow.Commons.CustomTiles.Abstracts;

/// <summary>
/// Exposes the entity to which a hook can attach.
/// </summary>
public interface IHookable
{
	public void SetHookPosition(Projectile hook);
}