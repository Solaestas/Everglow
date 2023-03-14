using Everglow.Myth.Common;

namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Buffs
{
	public class ShadowSupervisor : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int LuckyTarget = Main.rand.Next(200);
			if (LuckyTarget == npc.whoAmI)
				return;
			NPC target = Main.npc[LuckyTarget];
			if (target.active)
			{
				int i = target.FindBuffIndex(ModContent.BuffType<ShadowSupervisor>());
				if (i != -1)
				{
					if (!target.dontTakeDamage)
					{
						if (!target.friendly)
						{
							if ((target.Center - npc.position).Length() < 600)
							{
								int x = (int)Main.timeForVisualEffects;
								Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.AmbiguousLine>(), 40, 0, Main.myPlayer, x, 0);
								Projectile.NewProjectile(npc.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.AmbiguousLine>(), 40, 0, Main.myPlayer, x, 1/*ai1 = 1才绘制*/);
								ScreenShaker Gsplayer = Main.player[Main.myPlayer].GetModPlayer<ScreenShaker>();
								Gsplayer.FlyCamPosition = new Vector2(0, 2).RotatedByRandom(6.283);
								npc.buffTime[buffIndex] -= 120;
							}
						}
					}
				}
			}
		}
	}

	public class ShadowSupervisorTarget : GlobalNPC
	{

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (npc.HasBuff(ModContent.BuffType<ShadowSupervisor>()))
			{
				Texture2D Mark = MythContent.QuickTexture("MiscItems/Weapons/Slingshots/Buffs/ShadowSupervisorMark");

				if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
					spriteBatch.Draw(Mark, npc.Center - Main.screenPosition, null, Color.White, 0, Mark.Size() / 2f, 0.3f, SpriteEffects.None, 0f);
				else
				{
					spriteBatch.Draw(Mark, npc.Center - Main.screenPosition, null, Color.White, 0, Mark.Size() / 2f, 4f * npc.width * npc.height / 10300f * npc.scale * 0.3f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}