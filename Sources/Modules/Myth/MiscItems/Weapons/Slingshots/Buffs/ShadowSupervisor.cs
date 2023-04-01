using Everglow.Myth.Common;
namespace Everglow.Myth.MiscItems.Weapons.Slingshots.Buffs;

public class ShadowSupervisor : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		Dust index = Dust.NewDustDirect(npc.position - new Vector2(4), npc.width, npc.height, DustID.WaterCandle, 0f, 0f, 0, default, Main.rand.NextFloat(0.6f, 2.3f));
		index.velocity = new Vector2(0, Main.rand.NextFloat(0.5f, 2f)).RotatedByRandom(6.283) + new Vector2(0, -Main.rand.NextFloat(1.1f, 2f));
		index.noGravity = true;
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
		//if (npc.HasBuff(ModContent.BuffType<ShadowSupervisor>()))
		//{
		//	Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.AnisotropicWrap, RasterizerState.CullNone);
		//	Vector2 v0 = new Vector2(0, 40).RotatedBy(Main.time * 0.00);
		//	Vector2 v1 = new Vector2(0, 40).RotatedBy(Main.time * 0.00 + Math.PI / 3d * 2);
		//	Vector2 v2 = new Vector2(0, 40).RotatedBy(Main.time * 0.00 + Math.PI / 3d * 4);

		//	Vector2 v0_0 = v0 + new Vector2(0, -7).RotatedBy(Main.time * 0.08);
		//	Vector2 v0_1 = v0 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI / 2d);
		//	Vector2 v0_2 = v0 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI);
		//	Vector2 v0_3 = v0 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI / 2d * 3);

		//	Vector2 v1_0 = v1 + new Vector2(0, -7).RotatedBy(Main.time * 0.08);
		//	Vector2 v1_1 = v1 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI / 2d);
		//	Vector2 v1_2 = v1 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI);
		//	Vector2 v1_3 = v1 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI / 2d * 3);

		//	Vector2 v2_0 = v2 + new Vector2(0, -7).RotatedBy(Main.time * 0.08);
		//	Vector2 v2_1 = v2 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI / 2d);
		//	Vector2 v2_2 = v2 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI);
		//	Vector2 v2_3 = v2 + new Vector2(0, -7).RotatedBy(Main.time * 0.08 + Math.PI / 2d * 3);


		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v0_0 - Main.screenPosition, npc.Center + v1_0 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v0_1 - Main.screenPosition, npc.Center + v1_1 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v0_2 - Main.screenPosition, npc.Center + v1_2 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v0_3 - Main.screenPosition, npc.Center + v1_3 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);


		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v1_0 - Main.screenPosition, npc.Center + v2_0 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v1_1 - Main.screenPosition, npc.Center + v2_1 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v1_2 - Main.screenPosition, npc.Center + v2_2 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v1_3 - Main.screenPosition, npc.Center + v2_3 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);


		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v2_0 - Main.screenPosition, npc.Center + v0_0 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v2_1 - Main.screenPosition, npc.Center + v0_1 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v2_2 - Main.screenPosition, npc.Center + v0_2 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);
		//	MythUtils.DrawTexLine(Ins.Batch, npc.Center + v2_3 - Main.screenPosition, npc.Center + v0_3 - Main.screenPosition, Color.White, Color.White, ModAsset.FogTraceShade5xDark.Value);

		//	MythUtils.DrawTexCircle(Ins.Batch, 80, 40, Color.White, npc.Center - Main.screenPosition, ModAsset.FogTraceShade5xDark.Value);
		//	Ins.Batch.End();
		//}
	}
}