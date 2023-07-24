using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MythMod.Projectiles.Ocean
{
	// Token: 0x0200057F RID: 1407
    public class 海蛇尾触手 : ModProjectile
	{
		// Token: 0x06001EC3 RID: 7875 RVA: 0x0000C81D File Offset: 0x0000AA1D
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海蛇尾触手");
			Main.projFrames[base.projectile.type] = 2;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0018A990 File Offset: 0x00188B90
		public override void SetDefaults()
		{
			base.projectile.width = 4;
			base.projectile.height = 4;
			base.projectile.hostile = true;
			base.projectile.ignoreWater = true;
			base.projectile.tileCollide = false;
			base.projectile.penetrate = 1;
			base.projectile.timeLeft = 100;
			this.cooldownSlot = 1;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0018AA00 File Offset: 0x00188C00
		public override void AI()
		{
            if (NPC.CountNPCS(mod.NPCType("海蛇尾")) < 1)
            {
                projectile.timeLeft = 0;
            }
            if (Math.Abs(base.projectile.velocity.X) + Math.Abs(base.projectile.velocity.Y) < 16f)
			{
				base.projectile.velocity *= 0.99f;
			}
			base.projectile.rotation = (float)Math.Atan2((double)base.projectile.velocity.Y, (double)base.projectile.velocity.X) + 1.57f;
			base.projectile.frameCounter++;
			if (base.projectile.timeLeft % 15 == 0)
			{
				base.projectile.frame++;
			}
			if (base.projectile.frame > 1)
			{
				base.projectile.frame = 0;
			}
			if (projectile.timeLeft < 95)
            {
                projectile.scale *= 0.975f;
            }
			if (projectile.timeLeft < 40)
            {
                projectile.hostile = false;
            }
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0000C861 File Offset: 0x0000AA61
		// Token: 0x06001EC8 RID: 7880 RVA: 0x0018AB58 File Offset: 0x00188D58
	}
}
