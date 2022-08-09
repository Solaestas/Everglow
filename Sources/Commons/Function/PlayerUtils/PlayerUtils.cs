using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Sources.Commons.Function.PlayerUtils
{
    internal static class PlayerUtils
    {
        /// <summary>
        /// 造成一次无法被闪避的伤害
        /// </summary>
        /// <param name="player">受到伤害的玩家</param>
        /// <param name="from">伤害的直接来源，可能是NPC或者Projectile</param>
        /// <param name="damage">基础伤害</param>
        /// <param name="pvp">是否为PVP造成的伤害</param>
        public static void ApplyDamage(this Player player, Entity from, int damage, bool pvp)
        {
            int direction = player.Center.X > from.Center.X ? -1 : 1;
            bool crit = false;
            int realDamage;

            damage = Main.DamageVar(damage, -player.luck);

            int banner;
            if (from is NPC npc)
            {
                banner = Item.NPCtoBanner(npc.BannerID());
                if (banner > 0 && player.HasNPCBannerBuff(banner))
                {
                    var bannerEffect = ItemID.Sets.BannerStrength[Item.BannerToItem(banner)];
                    damage = (!Main.expertMode) ?
                        ((int)(damage * bannerEffect.NormalDamageReceived)) :
                        ((int)(damage * bannerEffect.ExpertDamageReceived));
                }

                if (Main.myPlayer == player.whoAmI && !npc.dontTakeDamage)
                {
                    float thorns = player.turtleThorns ? 2 : player.thorns;
                    if (thorns > 0)
                    {
                        int thornsDamage = (int)(damage * thorns);
                        player.ApplyDamageToNPC(npc, Math.Min(thornsDamage, 1000), 10, -direction, false);
                    }
                    if (player.cactusThorns)
                    {
                        player.ApplyDamageToNPC(npc, Main.masterMode ? 45 : (Main.expertMode ? 30 : 15), 10, -direction, false);
                    }
                }

                if (player.resistCold && npc.coldDamage)
                {
                    damage = (int)(damage * 0.7f);
                }

                NPCLoader.ModifyHitPlayer(npc, player, ref damage, ref crit);
                PlayerLoader.ModifyHitByNPC(player, npc, ref damage, ref crit);
                realDamage = (int)player.Hurt(PlayerDeathReason.ByNPC(npc.whoAmI), damage, direction, false, false, crit);
                if (realDamage > 0 && !player.dead)
                {
                    player.StatusFromNPC(npc);
                }
                NPCLoader.OnHitPlayer(npc, player, damage, crit);
                PlayerLoader.OnHitByNPC(player, npc, damage, crit);
            }
            else if (from is Projectile proj)
            {
                banner = proj.bannerIdToRespondTo;
                if (banner > 0 && player.HasNPCBannerBuff(banner))
                {
                    var bannerEffect = ItemID.Sets.BannerStrength[Item.BannerToItem(banner)];
                    damage = (!Main.expertMode) ?
                        ((int)(damage * bannerEffect.NormalDamageReceived)) :
                        ((int)(damage * bannerEffect.ExpertDamageReceived));
                }

                if (player.resistCold && proj.coldDamage)
                {
                    damage = (int)(damage * 0.7f);
                }

                float multiple = Main.GameModeInfo.EnemyDamageMultiplier;
                if (Main.GameModeInfo.IsJourneyMode)
                {
                    CreativePowers.DifficultySliderPower power = CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>();
                    if (power.GetIsUnlocked())
                    {
                        multiple = power.StrengthMultiplierToGiveNPCs;
                    }
                }
                damage = (int)(damage * multiple);
                ProjectileLoader.ModifyHitPlayer(proj, player, ref damage, ref crit);
                PlayerLoader.ModifyHitByProjectile(player, proj, ref damage, ref crit);
                realDamage = (int)player.Hurt(PlayerDeathReason.ByProjectile(pvp ? proj.owner : -1, proj.whoAmI), damage, direction, pvp, false, crit);
                if (realDamage > 0)
                {
                    proj.StatusPlayer(player.whoAmI);
                }
                ProjectileLoader.OnHitPlayer(proj, player, realDamage, crit);
                PlayerLoader.OnHitByProjectile(player, proj, realDamage, crit);
            }

        }
    }
}
