using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Buffs
{
    public class OnMoth : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            for (int t = 0; t < 5; t++)
            {
                if (npc.buffType[t] == ModContent.BuffType<OnMoth>())
                {
                    if (npc.buffTime[t] < 10)
                    {
                        MothBuffTarget mothBuffTarget = new MothBuffTarget
                        {
                            MothStack = 0
                        };
                    }
                    break;
                }
            }
        }
    }

    public class MothBuffTarget : GlobalNPC
    {
        public int MothStack = 0;
        public override bool InstancePerEntity => true;

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (npc.HasBuff(ModContent.BuffType<OnMoth>()))
            {
                if (projectile.type == ModContent.ProjectileType<Projectiles.DarkFanFly>() || projectile.type == ModContent.ProjectileType<Projectiles.GlowingButterfly>())
                {
                    damage *= (int)(1.0f + (MothStack) / 10f);
                }
            }
            base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<OnMoth>()))
            {
                float Stre = 0;
                float Stre2 = 0;
                for (int t = 0; t < 5; t++)
                {
                    if (npc.buffType[t] == ModContent.BuffType<OnMoth>())
                    {
                        Stre = Math.Clamp((npc.buffTime[t] - 280) / 20f, 0, 1);
                        Stre2 = Math.Clamp((npc.buffTime[t]) / 120f, 0, 0.3f);
                        break;
                    }
                }
                int Index = MothStack;
                Texture2D Number = MythContent.QuickTexture("TheFirefly/Projectiles/GlowFanTex/" + Index.ToString());
                Texture2D Butterfly = MythContent.QuickTexture("TheFirefly/Projectiles/GlowFanTex/BlueFly");
                Texture2D ButterflyD = MythContent.QuickTexture("TheFirefly/Projectiles/GlowFanTex/BlueFlyD");
                if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
                {
                    spriteBatch.Draw(Butterfly, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(33, 33), 3f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(ButterflyD, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), 0, new Vector2(33, 33), 3f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(Number, npc.Center - Main.screenPosition, null, new Color(Stre2 * 2, Stre2 * 2, Stre2 * 2, 0), 0, new Vector2(Number.Width / 2f, Number.Height / 2f), 3f, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(Butterfly, npc.Center - Main.screenPosition, null, new Color(Stre, Stre, Stre, 0), 0, new Vector2(33, 33), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
                    spriteBatch.Draw(ButterflyD, npc.Center - Main.screenPosition, null, new Color(Stre2 * 0.5f, Stre2 * 0.5f, Stre2 * 0.5f, 0), 0, new Vector2(33, 33), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
                    spriteBatch.Draw(Number, npc.Center - Main.screenPosition, null, new Color(Stre2 * 2, Stre2 * 2, Stre2 * 2, 0), 0, new Vector2(Number.Width / 2f, Number.Height / 2f), 4f * npc.width * npc.height / 10300f * npc.scale * 2, SpriteEffects.None, 0f);
                }
            }
        }
    }
}