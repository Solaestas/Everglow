namespace Everglow.Sources.Commons.Core.VFX.Visuals;

internal abstract class VisualProjectile : ModProjectile, IVisual
{
    public bool Active => Projectile.active && Main.projectile[Projectile.whoAmI] == Projectile;

    public virtual CallOpportunity DrawLayer => CallOpportunity.PostDrawProjectiles;

    public bool Visible => !Projectile.hide && VFXManager.InScreen(Projectile.position, ProjectileID.Sets.DrawScreenCheckFluff[Type]);

    public virtual void Draw()
    {
    }

    public override void SetDefaults()
    {
        if (!Main.gameMenu)
        {
            VFXManager.Instance.Add(this);
        }
    }

    public void Kill()
    {
        Projectile.Kill();
    }

    public void Update()
    {
    }
}