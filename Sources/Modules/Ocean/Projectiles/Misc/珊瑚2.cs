using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MythMod.MiscImplementation;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using MythMod.Tiles;
using Terraria.ModLoader;

namespace MythMod.Projectiles.Ocean
{
	// Token: 0x0200054E RID: 1358
    public class 珊瑚2 : ModProjectile
	{
		// Token: 0x06001DC0 RID: 7616 RVA: 0x0000C2EA File Offset: 0x0000A4EA
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("珊瑚");
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0017F224 File Offset: 0x0017D424
		public override void SetDefaults()
		{
			base.projectile.width = 2;
			base.projectile.height = 2;
			base.projectile.friendly = true;
			base.projectile.alpha = 255;
			base.projectile.timeLeft = 600;
			base.projectile.penetrate = 1;
            projectile.extraUpdates = (int)2f;
			base.projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0017F28C File Offset: 0x0017D48C
		public override void AI()
		{
            projectile.velocity.Y += 0.15f;
        }
		// Token: 0x06001DC3 RID: 7619 RVA: 0x0017F570 File Offset: 0x0017D770
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(70, 600);
            target.AddBuff(69, 600);
        }
        public override void Kill(int timeLeft)
        {
            if (Main.tile[(int)projectile.position.X / 16 - 2, (int)projectile.position.Y / 16 + 1].type == 396 && Main.tile[(int)projectile.position.X / 16 + 2, (int)projectile.position.Y / 16 + 1].type == 396 && Main.tile[(int)projectile.position.X / 16 + 2, (int)projectile.position.Y / 16 - 1].type <= 396 && Main.tile[(int)projectile.position.X / 16 - 2, (int)projectile.position.Y / 16 - 1].type <= 396)
            {
                WorldGen.PlaceTile((int)projectile.position.X / 16 - 2, (int)projectile.position.Y / 16 - 2, (ushort)mod.TileType("巨大鹿角珊瑚"), true, false, -1, 0);
            }
        }
    }
}
