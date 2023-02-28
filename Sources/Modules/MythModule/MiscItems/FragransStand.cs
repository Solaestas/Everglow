//using Terraria.Localization;

//namespace Everglow.Sources.Modules.MythModule.MiscItems
//{
//    public class FragransStand : ModItem
//    {
//        public override void SetStaticDefaults()
//        { //TODO: Localization needed
//            DisplayName.SetDefault("Moon Fragrans");
//            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "天外之月");
//            Tooltip.SetDefault("Right Click will consume 120 fragrans soul to start a random prize");
//            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "右键点击消耗300金桂之魂启动一次抽奖");
//			ItemGlowManager.AutoLoadItemGlow(this);
//		}
//        public static short GetGlowMask = 0;
//        public override void SetDefaults()
//        {
//			Item.glowMask = ItemGlowManager.GetItemGlow(this);
//			Item.width = 30;
//            Item.height = 50;
//            Item.useAnimation = 20;
//            Item.useTime = 20;
//            Item.maxStack = 99;
//            Item.rare = 9;
//            Item.value = Item.sellPrice(0, 20, 0, 0);
//            Item.useAnimation = 15;
//            Item.useTime = 10;
//            Item.useStyle = 1;
//            Item.consumable = true;
//            Item.useTurn = true;
//            Item.autoReuse = true;
//            Item.createTile = ModContent.TileType<MiscItems.Tiles.FragransStand>();
//        }
//        public override bool CanUseItem(Player player)
//        {
//            //if (ModLoader.GetMod("MythModBeta") != null)
//            //{
//            //    return true;
//            //}
//            //return File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFra.json");
//            return true;
//        }
//        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
//        {
//            //Texture2D texture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/Ban").Value;
//            //Vector2 slotSize = new Vector2(52f, 52f);
//            //position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
//            //Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale / 2f*/;
//            //float alpha = 0.8f;
//            //Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
//            //if (!File.Exists(Main.WorldPath + Main.LocalPlayer.name + "MoonFra.json"))
//            //{
//            //    spriteBatch.Draw(texture, drawPos, null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
//            //}
//        }
//    }
//}
