using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CursedFlames;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    public class Odd8Ring : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = 1375;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Odd8RingEquiper o8RE = player.GetModPlayer<Odd8RingEquiper>();
            o8RE.Odd8Enable = true;
        }
    }
    class Odd8RingEquiper : ModPlayer
    {
        public bool Odd8Enable = false;
        public override void ResetEffects()
        {
            Odd8Enable = false;
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(Odd8Enable)
            {
                damage = Math.Max(8, damage);
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Odd8Enable)
            {
                damage = Math.Max(8, damage);
            }
        }
        public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
        {
            if (Odd8Enable)
            {
                damage = Math.Max(8, damage);
            }
        }
        public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
        {
            if (Odd8Enable)
            {
                damage = Math.Max(8, damage);
            }
        }
    }
}
