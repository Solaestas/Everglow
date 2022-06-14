using Everglow.Sources.Modules.ZYModule.Commons.Core;
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
        Item.useTime = 60;
        Item.useAnimation = 60;
    }
    public override bool? UseItem(Player player)
    {
        return true;
    }
}

internal class WoodShieldProj : BaseHeldProj<WoodShield>
{
    //目测手臂帧图的位置
    public static readonly Vector2[] HoldOffset = new Vector2[]
    {
        new Vector2(-8, 6),
        new Vector2(-9, -10),
        new Vector2(4, -10),
        new Vector2(8, 3),
        new Vector2(6, 7),
        new Vector2(-8, -6),
        new Vector2(-4, 4),
        new Vector2(-7, 2),
        new Vector2(-7, 3),
        new Vector2(-6, 3),
        new Vector2(-6, 4),
        new Vector2(-6, 4),
        new Vector2(-5, 3),
        new Vector2(-4, 2),
        new Vector2(-2, 2),
        new Vector2(-1, 1),
        new Vector2(0, 1),
        new Vector2(-2, 3),
        new Vector2(-4, 3),
        new Vector2(-5, 3)
    };
    public override string Texture => Quick.ModulePath + "Items/WoodShield";
    public const int MaxLength = 20;
    public override void Initialize()
    {
        base.Initialize();
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.manualDirectionChange = true;
        Projectile.aiStyle = -1;
        RegisterState("Normal", NormalUpdate);
        RegisterState("Attack", AttackUpdate, AttackBegin);
        RegisterState("Defend", DefendUpdate);
    }
    public override bool PreAI()
    {
        if (Main.myPlayer == Owner.whoAmI && Owner.HeldItem?.ModItem != item)
        {
            item.projectile = null;
            Projectile.Kill();
        }
        Projectile.timeLeft = 2;
        return true;
    }
    public int NormalUpdate()
    {
        Vector2 offset = HoldOffset[Owner.bodyFrame.Y / Owner.bodyFrame.Height];
        Projectile.frame = 0;
        Projectile.Center = Owner.Center + new Vector2(offset.X * Owner.direction, offset.Y);
        var player = Owner.GetModPlayer<PlayerManager>();
        if (player.ControlUseItem.Press)
        {
            return GetStateID("Attack");
        }
        else if (player.ControlUseTile.Press)
        {
            return GetStateID("Defend");
        }
        return -1;
    }
    public void AttackBegin()
    {
        var mouse = Owner.ToMouse();
        Projectile.rotation = mouse.ToRot().Angle;
        Projectile.spriteDirection = Projectile.direction = Math.Sign(mouse.X);
    }
    public int AttackUpdate()
    {
        Projectile.frame = 1;
        Player.CompositeArmStretchAmount stretch;
        Owner.itemTime = 2;
        //TODO 更换个更好的插值函数
        LocalValue = (float)Timer / Owner.itemTimeMax;
        LocalValue = 0.4f / 0.09f * (LocalValue - 0.3f) * (LocalValue - 0.3f);
        if (LocalValue > 0.7f)
        {
            LocalValue = 1 - 1 / (1 / 0.3f - 0.7f + LocalValue);
        }
        stretch = LocalValue switch
        {
            < 0.25f => Player.CompositeArmStretchAmount.None,
            < 0.5f => Player.CompositeArmStretchAmount.Quarter,
            < 0.75f => Player.CompositeArmStretchAmount.ThreeQuarters,
            _ => Player.CompositeArmStretchAmount.Full
        };
        Projectile.Center = Owner.Center + new Vector2(LocalValue * MaxLength, 0).RotatedBy(Projectile.rotation);
        Owner.direction = Projectile.direction;
        Owner.SetCompositeArmFront(true, stretch, Projectile.rotation - MathHelper.PiOver2);
        if (Timer == Owner.itemTimeMax)
        {
            return GetStateID("Normal");
        }
        return -1;
    }
    public void ThrowBegin()
    {
        var mouse = Owner.ToMouse();
        Projectile.frame = 2;
        Projectile.rotation = mouse.ToRotation() - MathHelper.PiOver2;
        Projectile.direction = Math.Sign(mouse.X);
        Projectile.velocity = mouse.NormalizeSafe() * item.Item.shootSpeed;
        LocalValue = 0;
    }
    public int ThrowUpdate()
    {
        Owner.itemTime = 2;
        Projectile.penetrate = -1;
        Projectile.friendly = true;
        
        return -1;
    }
    public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
    {
        LocalValue = 1;
    }
    public override void OnHitPvp(Player target, int damage, bool crit)
    {
        LocalValue = 1;
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
        var mouse = Owner.ToMouse();
        Projectile.rotation = mouse.ToRot().Angle;
        Projectile.spriteDirection = Projectile.direction = Math.Sign(mouse.X);
        LocalValue = 1;
        Projectile.frame = 1;
        Owner.direction = Projectile.direction;
        Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        Projectile.Center = Owner.Center + new Vector2(18, 0).RotatedBy(Projectile.rotation) + new Vector2(-5 * Projectile.direction, -2);
        if (Owner.GetModPlayer<PlayerManager>().ControlUseTile.Press)
        {
            return -1;
        }
        return GetStateID("Normal");
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        switch (Projectile.frame)
        {
            case 0:
            case 1:
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
                    0, Vector2.Zero, Vector2.One,
                    Owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                break;
            case 1:
                float t = 1 - LocalValue;
                Main.EntitySpriteDraw(Asset.Value,
                    Projectile.Center - Main.screenPosition + new Vector2(0, Owner.gfxOffY),
                    null, lightColor,
                    Projectile.rotation, Asset.Value.Size() / 2, new Vector2(t > 0.5f ? 1 : (t + 0.5f), 1),
                    Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
                break;
        }
        return false;
    }
}
