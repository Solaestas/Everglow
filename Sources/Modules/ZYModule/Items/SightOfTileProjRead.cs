using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.ZYModule.Items
{
    internal class SightOfTileProjRead : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.itemTime = 5;
            player.itemAnimation = 5;
            Projectile.position = player.MountedCenter - new Vector2(17);
            if (Main.mouseLeft)
            {
                Projectile.timeLeft = 5;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 Vdr = Main.MouseWorld - Projectile.Center;
            
            Vdr = Vdr / Vdr.Length() * 7;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(Vdr.Y, Vdr.X) - Math.PI / 2d));
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/SightOfTileProjRead").Value;
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
            SpriteEffects S = SpriteEffects.None;
            if (Math.Sign(Vdr.X) == -1)
            {
                player.direction = -1;
            }
            else
            {
                player.direction = 1;
            }

            Main.spriteBatch.Draw(t, player.MountedCenter - Main.screenPosition + Vdr * 5f, null, color, (float)(Math.Atan2(Vdr.Y, Vdr.X) + Math.PI / 4d), t.Size() / 2f, Projectile.scale, S, 0f);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            MapIO mapIO = new MapIO((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16),1,1);
            int Count = 0;
            while (File.Exists("MapTiles" + Count.ToString() + ".mapio"))
            {
                Count++;
            }
            Count -= 1;
            mapIO.Read("MapTiles" + Count.ToString() + ".mapio");
            var it = mapIO.GetEnumerator();
            while (it.MoveNext())
            {
                WorldGen.SquareTileFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
                WorldGen.SquareWallFrame(it.CurrentCoord.X, it.CurrentCoord.Y);
            }
            base.Kill(timeLeft);
        }
    }
}
