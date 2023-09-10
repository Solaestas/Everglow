//using Terraria.Localization;

//namespace Everglow.Ocean.Projectiles.Misc
//{
//    public class Transport : ModProjectile
//    {
//        public override void SetStaticDefaults()
//        {
//            //DisplayName.SetDefault("Transport");
//            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "传送效果");
//        }
//        public override void SetDefaults()
//        {
//            Projectile.width = 0;
//            Projectile.height = 0;
//            Projectile.aiStyle = -1;
//            Projectile.friendly = false;
//            Projectile.hostile = false;
//            Projectile.ignoreWater = true;
//            Projectile.tileCollide = false;
//            Projectile.timeLeft = 120;
//        }
//        public override void AI()
//        {
//            Projectile.Center = Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
//            if (Projectile.timeLeft < 60)
//            {
//                Common.Systems.SubWorld.Tran = true;
//                /*if (Filters.Scene["MythMod:Vortex"].IsActive())
//                {
//                    //Filters.Scene.Deactivate("MythMod:Vortex");
//                }*/
//            }
//            if (Projectile.timeLeft > 110)
//            {
//                /*if (!Filters.Scene["MythMod:Vortex"].IsActive())
//                {
//                    //Filters.Scene.Activate("MythMod:Vortex");
//                }*/
//            }
//        }
//        public override Color? GetAlpha(Color lightColor)
//        {
//            return new Color?(new Color(0, 0, 0, 0));
//        }
//        private Effect ef;
//    }
//}