using System;
using Everglow.Myth.Common;
using Everglow.Myth.Misc.Projectiles.Weapon.Ranged.Slingshots;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Myth.TheMarbleRemains.NPCs.Bosses.EvilBottle.Projectiles
{
    public class EvilSlingshot : SlingshotProjectile
	{
		//public override void SetStaticDefaults()
		//{
  //          // base.DisplayName.SetDefault("妖火弹球");
		//}
		//public override void SetDefaults()
		//{
		//	base.Projectile.width = 20;
		//	base.Projectile.height = 20;
		//	base.Projectile.friendly = true;
		//	base.Projectile.DamageType = DamageClass.Melee;
		//	base.Projectile.penetrate = 15;
		//	base.Projectile.aiStyle = 1;
		//	base.Projectile.timeLeft = 1200;
  //          base.Projectile.hostile = false;
  //          Projectile.penetrate = 1;

  //      }

		public override void SetDef()
		{
			ShootProjType = ModContent.ProjectileType<EvilSlingshotAmmo>();
			SlingshotLength = 8;
			SplitBranchDis = 8;
		}
		public override void DrawString()
		{
			base.DrawString();
		Player player = Main.player[Projectile.owner];
		Color drawColor = new Color(160, 20, 200, 255);
		float DrawRot = Projectile.rotation - MathF.PI / 4f;
		Vector2 HeadCenter = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot);
		if (player.direction == -1)
			HeadCenter = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d);
		HeadCenter += Projectile.Center - Main.screenPosition;
		Vector2 SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Projectile.Center - Main.MouseWorld;
		Vector2 SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot) + Vector2.Normalize(SlingshotStringHead) * Power * 0.2625f;
		if (player.direction == -1)
		{
			SlingshotStringHead = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Projectile.Center - Main.MouseWorld;
			SlingshotStringTail = new Vector2(SlingshotLength, -SlingshotLength).RotatedBy(DrawRot + Math.PI / 2d) + Vector2.Normalize(SlingshotStringHead) * Power * 0.2625f;
		}
		SlingshotStringTail += Projectile.Center - Main.screenPosition;
		Vector2 Head1 = HeadCenter + HeadCenter.RotatedBy(Math.PI / 8 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
		Vector2 Head2 = HeadCenter - HeadCenter.RotatedBy(Math.PI / 8 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
		if (player.direction == -1)
		{
			Head1 = HeadCenter + HeadCenter.RotatedBy(Math.PI / 8 * 5 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
			Head2 = HeadCenter - HeadCenter.RotatedBy(Math.PI / 8 * 5 + DrawRot).SafeNormalize(Vector2.Zero) * SplitBranchDis;
		}
		var dColor = Color.Lerp(drawColor, new Color(120, 20, 160, 160), Power / 130f + 0.1f);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		DrawTexLine(Head1, SlingshotStringTail, 1, dColor, MythContent.QuickTexture("Misc/Projectiles/Weapon/Ranged/Slingshots/String"));
		DrawTexLine(Head2, SlingshotStringTail, 1, dColor, MythContent.QuickTexture("Misc/Projectiles/Weapon/Ranged/Slingshots/String"));
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		}
		//public override void AI()
		//{
		//          int r = Dust.NewDust(new Vector2(base.Projectile.Center.X, base.Projectile.Center.Y) + base.Projectile.velocity * 1.5f + new Vector2(-9, -4), 0, 0, 27, 0, 0, 0, default(Color), 1f);
		//          Main.dust[r].velocity.X = 0;
		//          Main.dust[r].velocity.Y = 0;
		//          Main.dust[r].noGravity = true;
		//      }
		//      public override void Kill(int timeLeft)
		//      {
		//      }
		//      public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		//      {
		//          target.AddBuff(153, 900);
		//      }
		//      public int projTime = 15;
	}
}
