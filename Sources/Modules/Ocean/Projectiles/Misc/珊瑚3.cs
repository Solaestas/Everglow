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
    public class 珊瑚3 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("珊瑚");
		}
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
		public override void AI()
		{
            projectile.velocity.Y += 0.15f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(70, 600);
            target.AddBuff(69, 600);
        }
        public override void Kill(int timeLeft)
        {
            if (Main.tile[(int)projectile.position.X / 16 - 1, (int)projectile.position.Y / 16 + 1].type == 396 && Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16 + 1].type == 396 && Main.tile[(int)projectile.position.X / 16 + 1, (int)projectile.position.Y / 16 + 1].type == 396 && Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16 + 2].type <= 396 && Main.tile[(int)projectile.position.X / 16 - 1, (int)projectile.position.Y / 16 - 1].type <= 396)
            {
                WorldGen.PlaceTile((int)projectile.position.X / 16 - 1, (int)projectile.position.Y / 16 - 1, (ushort)mod.TileType("伞房叶状珊瑚"), true, false, -1, 0);
            }
        }
    }
}
