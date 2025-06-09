using System;
using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Armors.Ruin;

[AutoloadEquip(EquipType.Head)]
public class RuinMask : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.width = 18;
		Item.height = 18;

		Item.defense = 1;

		Item.value = Item.buyPrice(gold: 1);
		Item.rare = ItemRarityID.Gray;
	}

	public override void UpdateEquip(Player player)
	{
		player.maxMinions += 1;
		player.statManaMax2 += 40;
	}

	public override bool IsArmorSet(Item head, Item body, Item legs)
	{
		return body.type == ModContent.ItemType<RuinMagicRobe>() && legs.type == ModContent.ItemType<RuinLeggings>();
	}

	public override void UpdateArmorSet(Player player)
	{
		player.maxMinions += 1;
		player.GetModPlayer<RuinSetPlayer>().RuinSetEnable = true;
	}

	public class RuinSetPlayer : ModPlayer
	{
		public bool RuinSetEnable { get; set; } = false;

		public bool RuinSetBuffActive => RuinSetEnable && Player.HasBuff<RuinSetBuff>();

		public override void ResetEffects()
		{
			RuinSetEnable = false;
		}

		public override void UpdateEquips()
		{
			if (RuinSetEnable)
			{
				if (MouseUtils.MouseMiddle.IsClicked && Player.HasBuff<RuinSetCooldown>())
				{
					Player.AddBuff(ModContent.BuffType<RuinSetBuff>(), 30 * 60);
					Player.AddBuff(ModContent.BuffType<RuinSetCooldown>(), 120 * 60);
				}
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			// TODO : Bug, the layer of glow is under head.
			if (RuinSetEnable)
			{
				Vector2 helmetOffset = drawInfo.helmetOffset;
				Vector2 pos = helmetOffset + new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.headPosition + drawInfo.headVect;
				pos += new Vector2(30 * MathF.Sin((float)Main.timeForVisualEffects * 0.05f), 0);
				DrawData item = new DrawData(ModAsset.RuinMask_Head_glow.Value, pos, drawInfo.drawPlayer.bodyFrame, new Color(1f, 1f, 1f, 0), drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0.5f);
				item.shader = drawInfo.cHead;
				drawInfo.DrawDataCache.Add(item);
			}
			base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
		}
	}
}