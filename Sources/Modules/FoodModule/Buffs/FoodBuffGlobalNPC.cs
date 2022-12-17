using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Everglow.Resources.NPCList.EventNPCs;
using Everglow.Sources.Modules.FoodModule.Buffs;
using Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs;
using Steamworks;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuffGlobalNPC : GlobalNPC
    {
        public bool isservant = false;
        public override bool InstancePerEntity => true;
        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            Player player = Main.LocalPlayer;
            if (crit)
            {
                damage *= (FoodBuffModPlayer.CritDamage + 1) / 2f;
            }
            if (player != null && player.active && !player.dead)
            {
                if (Main.bloodMoon && BloodMoonNPCs.vanillaBloodMoonNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().PurpleHooterBuff)
                {
                    damage *= 5;
                }
                if (Main.snowMoon && FrostMoonNPCs.vanillaFrostMoonNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().RedWineBuff)
                {
                    damage *= 5;
                }
                if (Main.pumpkinMoon && PumpkinMoonNPCs.vanillaPumpkinMoonNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().QuinceMarryBuff)
                {
                    damage *= 5;
                }
                if (Main.eclipse && EclipseNPCs.vanillaEclipseNPCs.Contains(npc.type) && player.GetModPlayer<FoodBuffModPlayer>().SunriseBuff)
                {
                    damage *= 5;
                }
                if (npc.GetGlobalNPC<FoodBuffGlobalNPC>().isservant && player.GetModPlayer<FoodBuffModPlayer>().TricolourBuff)
                {
                    damage *= 5;
                }
                if (player.GetModPlayer<FoodBuffModPlayer>().StrawberryBuff)
                {
                    damage  =  Math.Clamp(Math.Log10((double)1 / ((npc.Center - player.Center).Length() / 1000) + 10), 1, 1.25f)* damage;
                }
                if (player.GetModPlayer<FoodBuffModPlayer>().StrawberryIcecreamBuff)
                {
                    damage = Math.Clamp(Math.Log((double)1 / ((npc.Center - player.Center).Length() / 100) + 2.5), 1, 1.33f) * damage;
                }
                /*if (灯笼月 && PiercoldWindBuff)
             {
                 damage *= 5;
             }*/
            }

            return base.StrikeNPC(npc, ref damage, defense, ref knockback, hitDirection, ref crit);
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (source is EntitySource_Parent)
            {
                EntitySource_Parent parentSource = source as EntitySource_Parent;
                if (parentSource.Entity is NPC && (parentSource.Entity as NPC).boss && !npc.boss)
                {
                    isservant = true;
                }
            }
        }
    }
}
