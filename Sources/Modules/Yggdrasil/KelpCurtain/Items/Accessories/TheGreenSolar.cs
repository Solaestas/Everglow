using Everglow.Yggdrasil.KelpCurtain.Buffs;
using Everglow.Yggdrasil.KelpCurtain.Cooldowns;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Accessories;

public class TheGreenSolar : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Accessories;

    public const int LifeMaxBonus = 20;
    public const int CooldownDuration = 600 * 60; // 600 s
    public const int BuffHealPerSecond = 5;
    public const int EffectRange = 5 * 16; // 5 blocks
    public const int OnFireDuration = 10 * 60;
    public const int DispelBuffLife = 200;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;

        Item.accessory = true;

        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 20);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statLifeMax2 += LifeMaxBonus; // Increase max life by 20.
        player.GetModPlayer<TheGreenSolarPlayer>().TheGreenSolarEnable = true;
    }

    public class TheGreenSolarPlayer : ModPlayer
    {
        public const int FrameCount = 4; // Number of frames for the Sunstone effect animation.

        public bool TheGreenSolarEnable { get; set; } = false;

        public int Frame { get; set; } = 0;

        public bool TheGreenSolarBuffActive => Player.HasBuff<TheGreenSolarBuff>();

        public override void ResetEffects()
        {
            TheGreenSolarEnable = false;
        }

        public override void UpdateEquips()
        {
            if (TheGreenSolarBuffActive)
            {
                if (Main.timeForVisualEffects % 15 == 0)
                {
                    Frame = ++Frame % FrameCount;
                }

                Player.lifeRegenTime = 0; // Disable natural life regeneration

                // Amend player life to 0, before heal player.
                Player.statLife = Math.Max(Player.statLife, 0);

                // Heal player every second.
                if (Main.timeForVisualEffects % 60 == 0)
                {
                    Player.Heal(BuffHealPerSecond);
                }

                // Disable controls naturally
                Player.webbed = true;
                Player.noFallDmg = true;
            }
        }

        public override void PostUpdate()
        {
            if (TheGreenSolarBuffActive)
            {
                // Add OnFire debuff to nearby NPCs.
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (!npc.friendly
                        && !npc.townNPC
                        && !npc.CountsAsACritter
                        && !npc.dontTakeDamage
                        && Vector2.Distance(npc.Center, Player.Center) <= EffectRange)
                    {
                        npc.AddBuff(BuffID.OnFire, OnFireDuration);
                    }
                }

                // Dispel the buff if player's life is greater than or equal to DispelBuffLife.
                var targetLife = Math.Min(Player.statLifeMax2, DispelBuffLife); // The field 'Player.statLifeMax2' must be referenced upon all update.
                if (Player.statLife >= targetLife)
                {
                    Player.ClearBuff(ModContent.BuffType<TheGreenSolarBuff>());
                }
                else
                {
                    Player.AddBuff(ModContent.BuffType<TheGreenSolarBuff>(), 2);
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (TheGreenSolarEnable)
            {
                if (!Player.HasCooldown<TheGreenSolarCooldown>())
                {
                    Player.AddBuff(ModContent.BuffType<TheGreenSolarBuff>(), 2);
                    Player.AddCooldown(TheGreenSolarCooldown.ID, CooldownDuration);
                    return false;
                }
            }

            return true;
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) => !TheGreenSolarBuffActive;

        public override bool CanBeHitByProjectile(Projectile projectile) => !TheGreenSolarBuffActive;

        public override bool CanBeTeleportedTo(Vector2 teleportPosition, string context) => !TheGreenSolarBuffActive;

        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (TheGreenSolarBuffActive && !Main.gameMenu)
            {
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    layer.Hide();
                }
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (TheGreenSolarBuffActive && !Main.gameMenu)
            {
                var stoneTex = ModAsset.SunstoneTest.Value;
                var frame = stoneTex.Frame(1, FrameCount, 0, Frame);
                var stoneColor = new Color(0.5f, 1f, 0f);
                Main.spriteBatch.Draw(stoneTex, drawInfo.drawPlayer.Bottom - Main.screenPosition, frame, stoneColor, 0f, new Vector2(frame.Width / 2, frame.Height), 1f, SpriteEffects.None, 0);

                var lightCoreTex = Commons.ModAsset.Point.Value;
                var lightCoreColor = new Color(0.2f, 0.5f, 0f, 0f);
                var lightCoreScale = 1.2f + 0.05f * MathF.Sin((float)Main.timeForVisualEffects * 0.02f);
                Main.spriteBatch.Draw(lightCoreTex, drawInfo.drawPlayer.Center - Main.screenPosition, null, lightCoreColor, 0f, lightCoreTex.Size() / 2, lightCoreScale, SpriteEffects.None, 0);

                // draw ring
                var drawCenter = drawInfo.drawPlayer.Center - Main.screenPosition;
                var vertices = new List<Vertex2D>();

                var time = (float)Main.timeForVisualEffects;
                var fixedValue = 0.18f;

                var texScaleA = 1.2f + 0.2f * fixedValue * MathF.Cos(time * 0.04f);
                var texScaleB = 1.2f + 0.2f * MathF.Sin(time * 0.05f) * MathF.Cos(time * 0.02f);

                var rotOffA = (time * 0.02f) % MathHelper.TwoPi;
                var rotOffB = (time * 0.03f) % MathHelper.TwoPi;

                for (int i = 0; i <= 180; i++)
                {
                    var posOffA = 12 * fixedValue * MathF.Sin(0.4f * fixedValue + i / 20f * MathHelper.TwoPi);
                    var offsetA = new Vector2(50 + posOffA, 0).RotatedBy(MathHelper.TwoPi * i / 180 + rotOffA);
                    vertices.Add(drawCenter + offsetA, lightCoreColor * 0.5f, new(i / 180f * texScaleA, 0, 0));

                    var posOffB = 24 * fixedValue * MathF.Sin(0.4f * fixedValue + i / 20f * MathHelper.TwoPi);
                    var offsetB = new Vector2(100 + posOffB, 0).RotatedBy(MathHelper.TwoPi * i / 180 + rotOffB);
                    vertices.Add(drawCenter + offsetB, lightCoreColor * 0.5f, new(i / 180f * texScaleB, 1, 0));
                }
                Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_6.Value;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
            }
        }
    }
}