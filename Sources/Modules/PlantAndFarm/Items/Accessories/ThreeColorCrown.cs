using Everglow.PlantAndFarm.Items.Materials;

namespace Everglow.PlantAndFarm.Items.Accessories
{
	public class ThreeColorCrown : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Three-colored Wreath");
			//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "三色花环");
			//Tooltip.SetDefault("Hitting enemies randomly grants you one of the three effects below for 7s\nRed:Inceasing damage by 22%, increasing crit chance by 11%\nViolet:Immunity to most debuffs\nBlue:Increasing max mana by 300\nIt has a 30s CD\n'Pray for not being so lucky'");
			//Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "击中敌人后随机获得以下三种效果之一,持续7秒\n红:伤害增加22%，暴击率增加11%\n紫:免疫绝大多数减益效果\n蓝:魔力上限增加300\n有30秒CD\n'求别太幸运'");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 26;
			Item.value = 3724;
			Item.accessory = true;
			Item.rare = 3;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//MythPlayer.ThreeColorCrown = 2;
			//if (MythPlayer.ThreeColorCrownBuff1 > 0)
			//{
			player.GetDamage(DamageClass.Generic) *= 1.22f;
			player.GetCritChance(DamageClass.Generic) += 11;
			if (Main.rand.Next(6) == 0)
			{
				int R = 255;
				int G = 0;
				int B = 0;
				int num90 = Dust.NewDust(player.Center - new Vector2(37.5f, 27.5f), 60, 80, ModContent.DustType<Dusts.PFMBuff>(), 0f, 0f, 0, new Color(R, G, B, 0), Main.rand.NextFloat(0.4f, 0.8f));
				Main.dust[num90].noGravity = true;
				Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(-6f, -3f));
			}
			//}
			//if (MythPlayer.ThreeColorCrownBuff2 > 0)
			//{
			for (int i = 0; i < player.buffType.Length; i++)
			{
				if (Main.debuff[player.buffType[i]])
				{
					player.buffImmune[i] = true;
					player.ClearBuff(player.buffType[i]);
				}

			}
			if (Main.rand.Next(3) == 0)
			{
				int R = 160;
				int G = 0;
				int B = 160;
				int num90 = Dust.NewDust(player.Center - new Vector2(37.5f, 27.5f), 60, 80, ModContent.DustType<Dusts.PFMBuff>(), 0f, 0f, 0, new Color(R, G, B, 0), Main.rand.NextFloat(0.4f, 0.8f));
				Main.dust[num90].noGravity = true;
				Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(-6f, -3f));
			}
			//}
			//if (MythPlayer.ThreeColorCrownBuff3 > 0)
			//{
			player.statManaMax2 += 300;
			//if (MythPlayer.ThreeColorCrownBuff3 >= 419)
			//{
			player.statMana = player.statManaMax2;
			//}
			if (Main.rand.Next(3) == 0)
			{
				int R = 0;
				int G = 0;
				int B = 255;
				int num90 = Dust.NewDust(player.Center - new Vector2(37.5f, 27.5f), 60, 80, ModContent.DustType<Dusts.PFMBuff>(), 0f, 0f, 0, new Color(R, G, B, 0), Main.rand.NextFloat(0.4f, 0.8f));

				Main.dust[num90].noGravity = true;
				Main.dust[num90].velocity = new Vector2(0, Main.rand.NextFloat(-6f, -3f));
			}
			//}
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<WindMoveSeed>(), 15)
				.AddIngredient(ModContent.ItemType<PurplePhantom>(), 24)
				.AddTile(304)
				.Register();
		}
	}
}
