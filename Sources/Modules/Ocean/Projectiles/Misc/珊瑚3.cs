using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Everglow.Ocean.MiscImplementation;
using Terraria;
using Terraria.GameContent.Generation;
using Everglow.Ocean.Tiles;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Everglow.Ocean.Projectiles.Ocean
{
    public class 珊瑚3 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("珊瑚");
		}
		public override void SetDefaults()
		{
			base.Projectile.width = 2;
			base.Projectile.height = 2;
			base.Projectile.friendly = true;
			base.Projectile.alpha = 255;
			base.Projectile.timeLeft = 600;
			base.Projectile.penetrate = 1;
            Projectile.extraUpdates = (int)2f;
			base.Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
		}
		public override void AI()
		{
            Projectile.velocity.Y += 0.15f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(70, 600);
            target.AddBuff(69, 600);
        }
        public override void Kill(int timeLeft)
        {
            if (Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 + 1].TileType == 396 && Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].TileType == 396 && Main.tile[(int)Projectile.position.X / 16 + 1, (int)Projectile.position.Y / 16 + 1].TileType == 396 && Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 2].TileType <= 396 && Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].TileType <= 396)
            {
                WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.伞房叶状珊瑚>(), true, false, -1, 0);
            }
        }
    }
}
