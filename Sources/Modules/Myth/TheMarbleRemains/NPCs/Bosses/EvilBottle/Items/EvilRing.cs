using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Items
{
	public class EvilRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("怨念之戒");
			// base.Tooltip.SetDefault("");
			//base.DisplayName.AddTranslation(GameCulture.Chinese, "怨念之戒");
            // Tooltip.SetDefault("封印着仇恨与痛苦\n所有伤害提升27%,暴击增加14%\n生命将缓缓流逝");
		}
		public override void SetDefaults()
		{
			base.Item.width = 42;
			base.Item.height = 32;
            base.Item.rare = 3;
            base.Item.value = 100000;
			base.Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            //MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            player.lifeRegen -= 6;
            player.arrowDamage *= 1.27f;
            player.GetDamage(DamageClass.Melee) *= 1.27f;
            player.bulletDamage *= 1.27f;
            player.GetDamage(DamageClass.Magic) *= 1.27f;
            player.GetDamage(DamageClass.Summon) *= 1.27f;
            player.GetDamage(DamageClass.Throwing) *= 1.27f;
            player.GetCritChance(DamageClass.Ranged) += 14;
            player.GetCritChance(DamageClass.Generic) += 14;
            player.GetCritChance(DamageClass.Magic) += 14;
        }
    }
}
