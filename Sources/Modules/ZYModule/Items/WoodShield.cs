using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.Commons.Function.Base;

namespace Everglow.Sources.Modules.ZYModule.Items;

internal class WoodShield : BaseHeldItem<WoodShieldProj>
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.width = 32;
        Item.height = 32;
    }

}

internal class WoodShieldProj : BaseHeldProj<WoodShield>
{
    public override string Texture => Quick.ModulePath + "Items/WoodShield";
    public override void Initialize()
    {
        base.Initialize();
        Projectile.width = 32;
        Projectile.height = 32;
        RegisterState("Normal", NormalUpdate);
    }
    public override bool PreAI()
    {
        if(item != null && Owner.HeldItem?.ModItem != item)
        {
            Projectile.Kill();
        }
        Projectile.timeLeft = 2;
        return true;
    }
    public int NormalUpdate()
    {
        Projectile.Center = Owner.Center + new Vector2(10 * Owner.direction, 5);
        Projectile.frame = 0;
        Main.NewText(Owner.bodyFrame);
        return -1;
    }
    public int AttackUpdate()
    {
        return -1;
    }
    public int ThrowUpdate()
    {
        return -1;
    }
    public int DashAttackUpdate()
    {
        return -1;
    }
    public int DashThrowUpdate()
    {
        return -1;
    }
    public int DefendUpdate()
    {
        return -1;
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        switch(Projectile.frame)
        {
            case 0:
                overPlayers.Add(Projectile.whoAmI);
                Projectile.hide = true;
                break;
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        switch (Projectile.frame)
        {
            case 0:
                Main.EntitySpriteDraw(Asset.Value,
                    Projectile.position - Main.screenPosition + new Vector2(0, Owner.gfxOffY),
                    null, lightColor,
                    Projectile.rotation, Vector2.Zero, Vector2.One,
                    Owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                break;
        }
        return false;
    }
}
