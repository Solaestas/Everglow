using Everglow.Sources.Modules.MythModule.Common.Coroutines;
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.Commons.Function.Base;

using Terraria.Audio;

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
    public float damageRate;//乘算
    public float damageReduce;//乘前减算
    public float damageImmune;//乘后减算
}
internal class WoodShieldProj : BaseHeldProj<WoodShield>
{
    public uint rotRecord;
    public Direction playerDirection;
    public Rotation internalRotation;
    public DefendData defendData;
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
    public bool IsDefending => GetStateID("Defend") == StateID;
    public int DefendTimer { get => (int)Projectile.ai[1]; set => Projectile.ai[1] = value; }
    public const int MaxLength = 18;
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
    }
    public override bool PreAI()
    {
        if (Main.myPlayer == Owner.whoAmI && (Owner.HeldItem?.ModItem != item || Owner.dead))
        {
            item.projectile = null;
            Projectile.Kill();
        }
        Projectile.timeLeft = 2;
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
        Vector2 offset = HoldOffset[Owner.bodyFrame.Y / Owner.bodyFrame.Height];
        Projectile.frame = 1;
        DefendTimer = (int)MathUtils.Approach(DefendTimer, 0, 1);
        Projectile.Center = Owner.Center + new Vector2(offset.X * Owner.direction, offset.Y);
        var player = Owner.GetModPlayer<PlayerManager>();
        rotRecord |= (Owner.GetControlDirectionH() != playerDirection && playerDirection != Direction.None) ? 1u : 0u;
        playerDirection = Owner.GetControlDirectionH();
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
        FaceTo(mouse);
        Projectile.friendly = true;
        Projectile.knockBack *= 3;
    }
    public int AttackUpdate()
    {
        ref float stretchRate = ref Projectile.localAI[0];
        Projectile.frame = 1;
        Player.CompositeArmStretchAmount stretch;
        Owner.itemTime = 2;
        //TODO 更换个更好的插值函数
        float factor = (float)Timer / Owner.itemTimeMax;
        if (factor < 0.3f)
        {
            stretchRate = MathUtils.Lerp(Math.Max(stretchRate, 0), -0.3f, factor / 0.3f);
        }
        else
        {
            stretchRate = MathUtils.Lerp(-0.3f, 1, (factor - 0.3f) / 0.7f);
        }
        stretch = stretchRate switch
        {
            < 0.25f => Player.CompositeArmStretchAmount.None,
            < 0.5f => Player.CompositeArmStretchAmount.Quarter,
            < 0.75f => Player.CompositeArmStretchAmount.ThreeQuarters,
            _ => Player.CompositeArmStretchAmount.Full
        };
        Projectile.Center = Owner.Center + new Vector2(stretchRate * MaxLength, 0).RotatedBy(Projectile.rotation);
        Owner.direction = Projectile.direction;
        Owner.SetCompositeArmFront(true, stretch, Projectile.rotation - MathHelper.PiOver2);
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
        ref float stretchRate = ref Projectile.localAI[0];
        stretchRate = 0;
    }
    public void ThrowBegin()
    {
        int mul = 0;
        while (rotRecord != 0)
        {
            if ((rotRecord & 1u) == 1u)
            {
                mul++;
            }
            rotRecord >>= 1;
        }
        var mouse = Owner.ToMouse();
        Projectile projectile = Projectile;
        projectile.frame = 2;
        projectile.ai[1] = projectile.rotation;//AI[1]记录投掷时的角度
        projectile.localAI[1] = Main.rand.NextBool() ? 1 : -1;//LocalAI[1]记录挥动方向，视觉效果不用同步
        FaceTo(mouse);
        projectile.tileCollide = true;
        projectile.velocity = mouse.NormalizeSafe() * item.Item.shootSpeed * Math.Max(1, mul * 0.2f);
        ref float oldDamage = ref projectile.localAI[0];
        oldDamage = projectile.damage;
        projectile.damage = (int)(projectile.damage * mul * 0.4f);
        internalRotation.Reset();
    }
    public int ThrowUpdate()
    {
        Player owner = Owner;
        Projectile projectile = Projectile;
        owner.itemTime = 2;
        if (Timer < 10f)
        {
            owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
                MathUtils.Lerp(projectile.ai[1] + 1f * projectile.localAI[1], projectile.ai[1] - 1f * projectile.localAI[1], Timer / 10f)
                - MathHelper.PiOver2);
        }
        projectile.friendly = true;
        projectile.velocity += (owner.Center - projectile.Center).NormalizeSafe() * 0.5f;
        projectile.rotation = (projectile.Center - owner.Center).ToRotation();
        internalRotation += 0.05f;
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
        ref float oldDamage = ref projectile.localAI[0];
        projectile.damage = (int)oldDamage;
        oldDamage = 0;
        projectile.friendly = false;
        projectile.velocity *= 0;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.tileCollide = false;
        Timer = 60;
        Projectile.velocity *= 0;
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
    public void DefendDamage(Entity entity, ref int damage)
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
    public void DashAttackBegin()
    {
        var mouse = Owner.ToMouse();
        FaceTo(mouse);
        //TODO SoundEngine

    }
    public int DashAttackUpdate()
    {

        return -1;
    }
    public int DashThrowUpdate()
    {
        return -1;
    }
    public void DefendBegin()
    {
        Owner.GetModPlayer<PlayerManager>().shield = this;
        ref float defendBoss = ref Projectile.localAI[1];
        Projectile.damage = item.Item.damage;
        rotRecord = 0;
        defendBoss = 0;
    }
    public int DefendUpdate()
    {
        var mouse = Owner.ToMouse();
        ref float stretchRate = ref Projectile.localAI[0];
        Projectile.rotation = mouse.ToRot().Angle;
        Projectile.spriteDirection = Projectile.direction = Math.Sign(mouse.X);
        stretchRate = 1;
        Projectile.frame = 1;
        ++DefendTimer;
        Owner.direction = Projectile.direction;
        Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        Projectile.Center = Owner.Center + new Vector2(MaxLength, 0).RotatedBy(Projectile.rotation) + new Vector2(-5 * Projectile.direction, -2);
        if (Owner.GetModPlayer<PlayerManager>().ControlUseTile.Press)
        {
            return -1;
        }
        return GetStateID("Normal");
    }
    public int PostDefendUpdate()
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
                Main.EntitySpriteDraw(Asset.Value,
                    Projectile.Center - Main.screenPosition + new Vector2(0, Owner.gfxOffY),
                    null, lightColor,
                    Projectile.rotation, Asset.Value.Size() / 2,
                    new Vector2(MathUtils.Clamp(0, 1, 1 - 0.5f * MathUtils.Clamp(0, 1, stretchRate)), 1),
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
