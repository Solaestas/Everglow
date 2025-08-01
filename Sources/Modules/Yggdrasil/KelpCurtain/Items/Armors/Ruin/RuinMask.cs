using Everglow.Commons.Mechanics.Cooldown;
using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Cooldowns;
using Everglow.Yggdrasil.KelpCurtain.Items.Weapons.Ruin;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;

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
		public const int BuffDuration = 30 * 60;
		public const int CooldownDuration = 120 * 60;

		// behind -> front -> behind -> front -> behind = 5 * 15 frames
		public const int AnimationDuration = 75;
		public const int AnimationSwingInterval = 30;

		public bool RuinSetEnable { get; set; } = false;

		public bool RuinSetBuffActive => RuinSetEnable && Player.HasBuff<RuinSetBuff>();

		public int RuinSetBuffTimer { get; set; }

		public override void ResetEffects()
		{
			if (!RuinSetEnable)
			{
				RuinSetBuffTimer = 0;
				Player.ClearBuff(ModContent.BuffType<RuinSetBuff>());
			}

			RuinSetEnable = false;
		}

		public override void FrameEffects()
		{
			if (RuinSetEnable)
			{
				if (Player.HeldItem.type == ModContent.ItemType<WoodlandWraithStaff>())
				{
					Player.armorEffectDrawShadow = true;
					Player.armorEffectDrawOutlines = Player.HasBuff<RuinSetBuff>();
				}

				if (RuinSetBuffTimer > 0)
				{
					Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Player.direction * MathF.Cos(RuinSetBuffTimer / (float)AnimationSwingInterval * MathHelper.TwoPi) + MathHelper.Pi);
				}
			}
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (MouseUtils.MouseMiddle.IsClicked)
			{
				if (RuinSetEnable
					&& Player.HeldItem.type == ModContent.ItemType<WoodlandWraithStaff>()
					&& !Player.HasCooldown<RuinSetCooldown>())
				{
					if (Player.whoAmI == Main.myPlayer)
					{
						Player.AddBuff(ModContent.BuffType<RuinSetBuff>(), BuffDuration);
						Player.AddCooldown(RuinSetCooldown.ID, CooldownDuration);
						RuinSetBuffTimer = AnimationDuration;
						Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Player.velocity, ModContent.ProjectileType<WoodlandWraithStaff_SetAnimation>(), 0, 0, Player.whoAmI);
					}
				}
			}

			if (RuinSetBuffTimer > 0)
			{
				if (RuinSetBuffTimer % AnimationSwingInterval == AnimationSwingInterval / 2)
				{
					SoundEngine.PlaySound(SoundID.Item20, Player.Center);
				}

				RuinSetBuffTimer--;
			}
		}

		public override bool CanUseItem(Item item) => RuinSetEnable ? RuinSetBuffTimer <= 0 : true;

		public override void PostUpdate()
		{
			// Constraint for vanities, accessories and miscs effects that can change player visible equipments.
			if (Player.face == -1
				&& Player.head == EquipLoader.GetEquipSlot(Mod, nameof(RuinMask), EquipType.Head)
				&& Player.body == EquipLoader.GetEquipSlot(Mod, nameof(RuinMagicRobe), EquipType.Body)
				&& Player.legs == EquipLoader.GetEquipSlot(Mod, nameof(RuinLeggings), EquipType.Legs))
			{
				GenerateEyeGlowDust();
			}
		}

		/// <summary>
		/// Generate dusts as eye glow. Same as vanilla accessory <see cref="ItemID.Yoraiz0rDarkness"/>.
		/// <para/> This method is copied from <see cref="Player.Yoraiz0rEye"/> with some modifications.
		/// </summary>
		private void GenerateEyeGlowDust()
		{
			int offsetsPlayerHeadgearIndex = Player.bodyFrame.Y / 56;
			if (offsetsPlayerHeadgearIndex >= Main.OffsetsPlayerHeadgear.Length)
			{
				offsetsPlayerHeadgearIndex = 0;
			}
			var frameOffset = Main.OffsetsPlayerHeadgear[offsetsPlayerHeadgearIndex];
			var dustMaskOffset = new Vector2(-1f, 1.5f);
			Vector2 vector = frameOffset + dustMaskOffset;
			vector *= Player.Directions;

			Vector2 vector2 = new Vector2(Player.width / 2, Player.height / 2) + vector + (Player.MountedCenter - Player.Center);
			Player.sitting.GetSittingOffsetInfo(Player, out var posOffset, out var seatAdjustment);
			vector2 += posOffset + new Vector2(0f, seatAdjustment);

			if (Player.mount.Active && Player.mount.Type == 52)
			{
				vector2.X += 14f * (float)Player.direction;
				vector2.Y -= 2f * Player.gravDir;
			}

			float y = -11.5f * Player.gravDir;
			Vector2 vector3 = new Vector2(3 * Player.direction - ((Player.direction == 1) ? 1 : 0), y) + Vector2.UnitY * Player.gfxOffY + vector2;
			Vector2 vector4 = new Vector2(3 * Player.shadowDirection[1] - ((Player.direction == 1) ? 1 : 0), y) + vector2;
			Vector2 vector5 = Vector2.Zero;
			if (Player.mount.Active && Player.mount.Cart)
			{
				int num2 = Math.Sign(Player.velocity.X);
				if (num2 == 0)
				{
					num2 = Player.direction;
				}

				vector5 = new Vector2(MathHelper.Lerp(0f, -8f, Player.fullRotation / ((float)Math.PI / 4f)), MathHelper.Lerp(0f, 2f, Math.Abs(Player.fullRotation / ((float)Math.PI / 4f)))).RotatedBy(Player.fullRotation);
				if (num2 == Math.Sign(Player.fullRotation))
				{
					vector5 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(Player.fullRotation / ((float)Math.PI / 4f)));
				}
			}

			if (Player.fullRotation != 0f)
			{
				vector3 = vector3.RotatedBy(Player.fullRotation, Player.fullRotationOrigin);
				vector4 = vector4.RotatedBy(Player.fullRotation, Player.fullRotationOrigin);
			}

			float num3 = 0f;
			Vector2 vector6 = Player.position + vector3 + vector5;
			Vector2 vector7 = Player.oldPosition + vector4 + vector5;
			vector7.Y -= num3 / 2f;
			vector6.Y -= num3 / 2f;

			DelegateMethods.v3_1 = Main.hslToRgb(Main.rgbToHsl(Player.eyeColor).X, 1f, 0.5f).ToVector3() * 0.5f * 1f;
			if (Player.velocity != Vector2.Zero)
			{
				Utils.PlotTileLine(Player.Center, Player.Center + Player.velocity * 2f, 4f, DelegateMethods.CastLightOpen);
			}
			else
			{
				Utils.PlotTileLine(Player.Left, Player.Right, 4f, DelegateMethods.CastLightOpen);
			}

			int dustCount = (int)Vector2.Distance(vector6, vector7) / 3 + 1;
			if (Vector2.Distance(vector6, vector7) % 3f != 0f)
			{
				dustCount++;
			}

			// Generate dusts.
			for (int i = 1; i <= dustCount; i++)
			{
				Dust obj = Main.dust[Dust.NewDust(Player.Center, 0, 0, DustID.TheDestroyer)];
				obj.position = Vector2.Lerp(vector7, vector6, i / (float)dustCount);
				obj.noGravity = true;
				obj.velocity = Vector2.Zero;
				obj.scale = 0.8f;
				obj.shader = GameShaders.Armor.GetSecondaryShader(Player.cYorai, Player);
			}
		}
	}
}