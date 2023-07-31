using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Everglow.Ocean.Projectiles.Ocean
{
	// Token: 0x0200057F RID: 1407
    public class 海蛇尾触手 : ModProjectile
	{
		// Token: 0x06001EC3 RID: 7875 RVA: 0x0000C81D File Offset: 0x0000AA1D
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("海蛇尾触手");
			Main.projFrames[base.Projectile.type] = 2;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0018A990 File Offset: 0x00188B90
		public override void SetDefaults()
		{
			base.Projectile.width = 4;
			base.Projectile.height = 4;
			base.Projectile.hostile = true;
			base.Projectile.ignoreWater = true;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = 1;
			base.Projectile.timeLeft = 100;
			this.CooldownSlot = 1;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0018AA00 File Offset: 0x00188C00
		public override void AI()
		{
            if (NPC.CountNPCS(ModContent.NPCType<Everglow.Ocean.NPCs.海蛇尾>()) < 1)
            {
                Projectile.timeLeft = 0;
            }
            if (Math.Abs(base.Projectile.velocity.X) + Math.Abs(base.Projectile.velocity.Y) < 16f)
			{
				base.Projectile.velocity *= 0.99f;
			}
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + 1.57f;
			base.Projectile.frameCounter++;
			if (base.Projectile.timeLeft % 15 == 0)
			{
				base.Projectile.frame++;
			}
			if (base.Projectile.frame > 1)
			{
				base.Projectile.frame = 0;
			}
			if (Projectile.timeLeft < 95)
            {
                Projectile.scale *= 0.975f;
            }
			if (Projectile.timeLeft < 40)
            {
                Projectile.hostile = false;
            }
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0000C861 File Offset: 0x0000AA61
		// Token: 0x06001EC8 RID: 7880 RVA: 0x0018AB58 File Offset: 0x00188D58
	}
}
