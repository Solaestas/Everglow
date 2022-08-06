using Everglow.Sources.Commons.Core.Coroutines;
using Everglow.Sources.Commons.Core.DataStructures;
using Everglow.Sources.Commons.Function.PlayerUtils;
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.Commons.Function.Base;

using Terraria.Audio;

namespace Everglow.Sources.Modules.ZYModule.Items;

internal class WoodShield : BaseHeldItem<WoodShieldProj>
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("WoodShield");
        Tooltip.SetDefault($"左键攻击，右键防御\n\"只是一面普通的盾牌，不会有人想用盾牌去打人吧\"");
    }
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.width = 32;
        Item.height = 32;
        Item.useTime = 40;
        Item.useAnimation = 40;
        Item.knockBack = 5;
        Item.damage = 10;
        Item.shootSpeed = 20;
    }
    public override bool? UseItem(Player player)
    {
        return true;
    }
}
public struct DefendData
{
    /// <summary>
    /// 乘算
    /// </summary>
    public float damageRate;
    /// <summary>
    /// 乘前减算
    /// </summary>
    public float damageReduce;
    /// <summary>
    /// 乘后减算
    /// </summary>
    public float damageImmune;
}
internal class WoodShieldProj : BaseHeldProj<WoodShield>
{
    public uint rotRecord;
    public Direction playerDirection;
    public DefendData defendData;
    public float oldDamage;
    public int doubleClickTimer;
    public int dashCoolDown;
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
    public bool IsDefending => GetStateID("Defend") == StateID || GetStateID("Dash") == StateID;
    public int DefendTimer { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
    public const int MaxLength = 18;
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Shield");
    }
    public override void Initialize()
    {
        base.Initialize();
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.manualDirectionChange = true;
        Projectile.aiStyle = -1;
        Projectile.penetrate = -1;
        defendData = new DefendData()
        {
            damageRate = 1,
            damageImmune = 1,
            damageReduce = 5,
        };
        RegisterState("Normal", NormalUpdate);
        RegisterState("Attack", AttackUpdate, AttackBegin, AttackEnd);
        RegisterState("Defend", DefendUpdate, DefendBegin);
        RegisterState("Throw", ThrowUpdate, ThrowBegin, ThrowEnd);
        RegisterState("PostDefend", PostDefendUpdate, null, null, PostDefendCoroutine);
        RegisterState("Dash", DashUpdate, DashBegin, DashEnd, DashCoroutine);
    }
    public void Using()
    {
        ref float stretchRate = ref Projectile.localAI[0];
        Projectile.frame = 1;
        Owner.direction = Projectile.direction;
        Player.CompositeArmStretchAmount stretch;
        stretch = stretchRate switch
        {
            < 0.25f => Player.CompositeArmStretchAmount.None,
            < 0.5f => Player.CompositeArmStretchAmount.Quarter,
            < 0.75f => Player.CompositeArmStretchAmount.ThreeQuarters,
            _ => Player.CompositeArmStretchAmount.Full
        };
        Owner.SetCompositeArmFront(true, stretch, Projectile.rotation - MathHelper.PiOver2);
        Projectile.Center = Owner.Center
            + new Vector2(stretchRate * MaxLength, 0).RotatedBy(Projectile.rotation)
            + new Vector2(-5 * Projectile.direction, -2).RotatedBy(Owner.fullRotation);

    }
    public void Holding()
    {
        Projectile.frame = 0;
        Projectile.direction = Owner.direction;
        Vector2 offset = HoldOffset[Owner.bodyFrame.Y / Owner.bodyFrame.Height].RotatedBy(Owner.fullRotation);
        offset.X *= Projectile.direction;
        Projectile.Center = Owner.Center + offset;
    }
    public int GetDirectionSwitchCount()
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
        return count;
    }
    public override bool PreAI()
    {
        if (Main.myPlayer == Owner.whoAmI && (Owner.HeldItem?.ModItem != item || Owner.dead))
        {
            item.projectile = null;
            Projectile.Kill();
        }
        Projectile.timeLeft = 2;
        dashCoolDown = (int)MathUtils.Approach(dashCoolDown, 0, 1);
        return true;
    }
    public void NormalBegin()
    {
        playerDirection = Owner.GetControlDirectionH();
    }
    public int NormalUpdate()
    {
        ref float stretchRate = ref Projectile.localAI[0];
        stretchRate = MathUtils.Approach(stretchRate, 0, 0.07f);
        DefendTimer = (int)MathUtils.Approach(DefendTimer, 0, 1);
        Holding();
        Direction now = Owner.GetControlDirectionH();
        if (now != Direction.None && now != playerDirection)
        {
            rotRecord |= 1u;
            playerDirection = now;
        }
        rotRecord <<= 1;

        var player = Owner.GetModPlayer<PlayerManager>();
        if (player.ControlLeft.DoubleClick)
        {
            doubleClickTimer = -10;
        }
        else if (player.ControlRight.DoubleClick)
        {
            doubleClickTimer = 10;
        }
        doubleClickTimer = (int)MathUtils.Approach(doubleClickTimer, 0, 1);
        if (player.ControlUseItem.Press)
        {
            var angle = Owner.ToMouse().ToRotation();
            ref float dashDirection = ref Projectile.localAI[1];
            if (GetDirectionSwitchCount() > 2)
            {
                return GetStateID("Throw");
            }
            else if (dashCoolDown == 0 && ((doubleClickTimer < 0 && angle is > MathHelper.PiOver4 * 3 or < MathHelper.PiOver4 * 3)
                || (doubleClickTimer > 0 && angle is > -MathHelper.PiOver4 and < MathHelper.PiOver4)))
            {
                Projectile.rotation = angle;
                return GetStateID("Dash");
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
        FaceTo(mouse);
        Projectile.friendly = true;
        Projectile.knockBack *= 3;
    }
    public int AttackUpdate()//localAI[0] -> stretchRate
    {
        ref float stretchRate = ref Projectile.localAI[0];
        Projectile.frame = 1;
        Owner.itemTime = 2;
        //TODO 更换个更好的插值函数
        float factor = (float)Timer / Owner.itemTimeMax;
        stretchRate = factor switch
        {
            < 0.4f => MathUtils.Approach(stretchRate, -0.3f, 0.3f),
            < 0.5f => -0.3f,
            _ => MathUtils.Approach(stretchRate, 1, 0.2f)
        };
        Using();
        if (Timer >= Owner.itemTimeMax)
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
        int mul = GetDirectionSwitchCount();
        var mouse = Owner.ToMouse();
        Projectile projectile = Projectile;
        projectile.frame = 2;
        projectile.localAI[0] = projectile.rotation;//AI[1]记录投掷时的角度
        projectile.localAI[1] = Main.rand.NextBool() ? 1 : -1;//LocalAI[1]记录挥动方向，视觉效果不用同步
        FaceTo(mouse);
        projectile.tileCollide = true;
        projectile.velocity = mouse.NormalizeSafe() * item.Item.shootSpeed * Math.Max(1, mul * 0.2f);
        oldDamage = projectile.damage;
        projectile.damage = (int)(projectile.damage * mul * 0.4f);
        rotRecord = 0;
    }
    public int ThrowUpdate()//localAI[0] -> swingRotation localAI[1] -> swingDirection
    {
        Player owner = Owner;
        Projectile projectile = Projectile;
        owner.itemTime = 2;
        if (Timer < 10f)
        {
            owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
                MathUtils.Lerp(projectile.localAI[0] + 1f * projectile.localAI[1],
                projectile.localAI[0] - 1f * projectile.localAI[1], Timer / 10f)
                - MathHelper.PiOver2);
        }
        projectile.friendly = true;
        projectile.velocity += (owner.Center - projectile.Center).NormalizeSafe() * 0.5f;
        projectile.rotation = (projectile.Center - owner.Center).ToRotation();
        if (projectile.velocity.Y < 2f)
        {
            projectile.velocity.Y += 0.04f;
        }
        if (Timer > 60)
        {
            projectile.tileCollide = false;
            projectile.Center = MathUtils.Approach(projectile.Center, owner.Center, Math.Min((Timer - 60) * 0.2f, 20));
            if (projectile.Center.Distance(owner.Center) < 20)
            {
                return GetStateID("Normal");
            }
        }
        return -1;
    }
    public void ThrowEnd()
    {
        Projectile projectile = Projectile;
        projectile.damage = (int)oldDamage;
        projectile.friendly = false;
        projectile.velocity *= 0;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.tileCollide = false;
        Timer = 50;
        Vector2 dir = Vector2.Normalize(Owner.Center - Projectile.Center);
        Projectile.velocity = Math.Abs(Vector2.Dot(dir, Projectile.velocity)) * dir;
        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WoodFurniture);
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
    public int CalculateDamage(int damage)
    {
        return (int)((damage - defendData.damageReduce) * defendData.damageRate - defendData.damageImmune);
    }
    public void DefendDamage(Entity entity, ref int damage)// localAI[1] -> DefendBoss
    {
        var player = Owner;
        float knockBackRate = 1;
        float playerKnockBackRate = 1;
        if (DefendTimer <= 10)
        {
            DefendTimer = 60;
            player.immuneTime = 30;
            knockBackRate = 1.5f;
            playerKnockBackRate = 0.5f;
            damage = 0;
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, player.Center);
        }
        else
        {
            player.immuneTime = 30;
            damage = CalculateDamage(damage);
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, player.Center);
        }
        if (entity is NPC npc)
        {
            ref float defendBoss = ref Projectile.localAI[1];
            if ((npc.width > 60 && npc.height > 60) || npc.boss)
            {
                defendBoss = npc.Center.X > player.Center.X ? -1 : 1;
                entity.velocity += new Vector2(knockBackRate * Projectile.knockBack * 0.5f, 0).RotatedBy(Projectile.rotation) + player.velocity * 0.5f;
                if (!player.noKnockback)
                {
                    player.velocity -= new Vector2(playerKnockBackRate * 4, 0).RotatedBy(Projectile.rotation);
                }
            }
            else
            {
                entity.velocity += new Vector2(knockBackRate * Projectile.knockBack * 2, 0).RotatedBy(Projectile.rotation) + player.velocity;
                if (!player.noKnockback)
                {
                    player.velocity -= new Vector2(playerKnockBackRate * 2, 0).RotatedBy(Projectile.rotation);
                }
            }
        }
        else
        {
            player.velocity -= new Vector2(playerKnockBackRate, 0).RotatedBy(Projectile.rotation);
        }
        player.noKnockback = true;
        StateID = GetStateID("PostDefend");
    }
    public void DashBegin()
    {
        doubleClickTimer = 0;
        Projectile.damage *= 4;
        Projectile.knockBack *= 2;
        Projectile.friendly = true;
        defendData.damageRate /= 5;
        dashCoolDown = 90;
    }
    public int DashUpdate()//localAI[0] -> stretchRate
    {
        ref float stretchRate = ref Projectile.localAI[0];
        Rotation rot = Projectile.rotation;
        stretchRate = 1;
        float proj = MathUtils.Projection(rot.XAxis, Projectile.velocity);
        Owner.noKnockback = true;
        Owner.velocity = Math.Max(proj, Math.Max(12, Owner.accRunSpeed * 3)) * rot.XAxis;
        Using();
        if (Timer > 9)
        {
            return GetStateID("Normal");
        }
        return -1;
    }
    public void DashEnd()
    {
        Projectile.damage /= 4;
        Projectile.knockBack /= 2;
        Projectile.friendly = false;
        defendData.damageRate *= 5;
    }
    public IEnumerator<ICoroutineInstruction> DashCoroutine()
    {
        yield return new WaitForFrames(15);
        float len = Owner.velocity.Length();
        if (len == 0)
        {
            yield break;
        }
        Owner.velocity = Math.Min(len, Owner.accRunSpeed) * Vector2.Normalize(Owner.velocity);
        yield break;
    }
    public void DefendBegin()
    {
        Owner.GetModPlayer<PlayerManager>().shield = this;
        ref float defendBoss = ref Projectile.localAI[1];
        Projectile.damage = item.Item.damage;
        rotRecord = 0;
        defendBoss = 0;
    }
    public int DefendUpdate()//localAI[0] -> stretchRate
    {
        ref float stretchRate = ref Projectile.localAI[0];
        stretchRate = 1;
        ++DefendTimer;
        FaceTo(Owner.ToMouse());
        Using();
        if (Owner.GetModPlayer<PlayerManager>().ControlUseTile.Press)
        {
            return -1;
        }
        return GetStateID("Normal");
    }
    public int PostDefendUpdate()//localAI[1] -> defendBoss
    {
        ref float defendBoss = ref Projectile.localAI[1];
        Owner.direction = Projectile.direction;
        Projectile.Center = Owner.Center + new Vector2(MaxLength, 0).RotatedBy(Projectile.rotation) + new Vector2(-5 * Projectile.direction, -2);
        Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        if (Timer > 10)
        {
            return GetStateID("Normal");
        }
        if ((defendBoss == 1 && Owner.controlRight) || (defendBoss == -1 && Owner.controlLeft))
        {
            Owner.velocity.X += 5 * defendBoss;
            defendBoss = 0;
        }
        return -1;
    }
    public IEnumerator<ICoroutineInstruction> PostDefendCoroutine()
    {
        Projectile.damage *= 3;
        yield return new WaitForFrames(20);
        Projectile.damage /= 3;
        yield break;
    }
    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        width = (int)(width * 0.7f);
        height = (int)(height * 0.7f);
        return true;
    }
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        if (Projectile.frame != 2)
        {
            return targetHitbox.Intersects(projHitbox);
        }

        ref float stretchRate = ref Projectile.localAI[0];
        float factor = 1 - 0.5f * MathUtils.Clamp(0, 1, stretchRate);
        float point = 0;
        Vector2 vec = new Vector2(0, Projectile.height / 2f).RotatedBy(Projectile.rotation);
        return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
            Projectile.Center + vec,
            Projectile.Center - vec,
            Projectile.width * factor * 0.4f,
            ref point);
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
                ref float stretchRate = ref Projectile.localAI[0];
                float factor = 1 - 0.5f * MathUtils.Clamp(0, 1, stretchRate);
                Main.EntitySpriteDraw(Asset.Value,
                    Projectile.Center - Main.screenPosition + new Vector2(0, Owner.gfxOffY),
                    null, lightColor,
                    Projectile.rotation,
                    Asset.Value.Size() / 2,
                    new Vector2(factor, 1),
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
