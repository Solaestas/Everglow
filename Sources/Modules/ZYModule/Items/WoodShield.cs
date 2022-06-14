using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
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
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.damage = 10;
        Item.shootSpeed = 20;
    }
    public override bool? UseItem(Player player)
    {
        return true;
    }
}

internal class WoodShieldProj : BaseHeldProj<WoodShield>
{
    public uint rotRecord;
    public Rotation internalRotation;
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
        Projectile.penetrate = -1;
        RegisterState("Normal", NormalUpdate);
        RegisterState("Attack", AttackUpdate, AttackBegin, AttackEnd);
        RegisterState("Defend", DefendUpdate);
        RegisterState("Throw", ThrowUpdate, ThrowBegin, ThrowEnd);
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
        rotRecord |= (Owner.direction != LocalValue && LocalValue != 0 && Owner.direction != 0) ? 1u : 0u;
        LocalValue = Owner.direction;
        rotRecord <<= 1;
        if (player.ControlUseItem.Press)
        {
            uint temp = rotRecord;
            int count = 0;
            while (temp != 0)
            {
                if ((temp & 1u) == 1u)
                {
                    ++count;
                }
                temp >>= 1;
            }
            if (count > 2)
            {
                return GetStateID("Throw");
            }
            else
            {
                return GetStateID("Attack");
            }
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
        Projectile.friendly = true;
        Projectile.knockBack *= 3;
    }
    public int AttackUpdate()
    {
        Projectile.frame = 1;
        Player.CompositeArmStretchAmount stretch;
        Owner.itemTime = 2;
        //TODO 更换个更好的插值函数
        LocalValue = (float)Timer / Owner.itemTimeMax;
        LocalValue = MathUtils.SmoothStepMinus(0, 1.5f, LocalValue, 0.3f);
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
        if (Timer >= Owner.itemTimeMax * 1.5f)
        {
            return GetStateID("Normal");
        }
        return -1;
    }
    public void AttackEnd()
    {
        Projectile.friendly = false;
        Projectile.knockBack /= 3;
    }
    public void ThrowBegin()
    {
        int mul = 0;
        while(rotRecord != 0)
        {
            if((rotRecord & 1u) == 1u)
            {
                mul++;
            }
            rotRecord >>= 1;
        }
        var mouse = Owner.ToMouse();
        Projectile.frame = 2;
        Projectile.rotation = mouse.ToRotation();
        Projectile.direction = Math.Sign(mouse.X);
        Projectile.tileCollide = true;
        Projectile.velocity = mouse.NormalizeSafe() * item.Item.shootSpeed * Math.Max(1, mul * 0.2f);
        LocalValue = Projectile.damage;
        Projectile.damage = (int)(Projectile.damage * mul);
        internalRotation.Reset();
    }
    public int ThrowUpdate()
    {
        Owner.itemTime = 2;
        Projectile.friendly = true;
        Projectile.velocity += (Owner.Center - Projectile.Center).NormalizeSafe() * 0.5f;
        internalRotation += 0.05f;
        if(Projectile.velocity.Y < 2f)
        {
            Projectile.velocity.Y += 0.04f;
        }
        if(Timer > 60)
        {
            Projectile.tileCollide = false;
            Projectile.Center = MathUtils.Approach(Projectile.Center, Owner.Center, Math.Min((Timer - 60) * 0.2f, 20));
            if(Projectile.Center.Distance(Owner.Center) < 20)
            {
                return GetStateID("Normal");
            }
        }
        return -1;
    }
    public void ThrowEnd()
    {
        Projectile.damage = (int)LocalValue;
        Projectile.friendly = false;
        LocalValue = 0;
        Projectile.velocity *= 0;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.tileCollide = false;
        Timer = 60;
        Projectile.velocity *= 0;
        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WoodFurniture);
        return false;
    }
    public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
    {
        if (StateID == GetStateID("Throw"))
        {
            Timer = Math.Max(50, Timer);
        }
    }
    public override void OnHitPvp(Player target, int damage, bool crit)
    {
        if (StateID == GetStateID("Throw"))
        {
            Timer = Math.Max(50, Timer);
        }
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
            case 2:
                Projectile.hide = false;
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
            case 2:
                Main.EntitySpriteDraw(Asset.Value,
                    Projectile.Center - Main.screenPosition,
                    null, lightColor,
                    Projectile.rotation, Asset.Value.Size() / 2, new Vector2(1, 0.5f),
                    SpriteEffects.None, 0);
                break;
        }
        return false;
    }
}
