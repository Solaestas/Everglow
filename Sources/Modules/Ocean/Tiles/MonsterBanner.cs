using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles
{
    // Token: 0x02000C85 RID: 3205
    public class MonsterBanner : ModTile
    {
        // Token: 0x0600406E RID: 16494 RVA: 0x00324FD4 File Offset: 0x003231D4
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16
            };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile((int)base.Type);
            this.DustType = -1;
            TileID.Sets.DisableSmartCursor[Type]/* tModPorter Note: Removed. Use TileID.Sets.DisableSmartCursor instead */ = true;
            LocalizedText modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("Banner");
            base.AddMapEntry(new Color(13, 88, 130), modTranslation);
            //modTranslation.AddTranslation(GameCulture.Chinese, "旗帜");
        }

        // Token: 0x0600406F RID: 16495 RVA: 0x003250A8 File Offset: 0x003232A8
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            string text;
            switch (frameX / 18)
            {
                case 0:
                    text = "MoonJellyfishBanner";
                    break;
                case 1:
                    text = "AlarmJellyfishBanner";
                    break;
                case 2:
                    text = "OceanSlimeBannerBanner";
                    break;
                case 3:
                    text = "AbyssSlimeBanner";
                    break;
                case 4:
                    text = "SpiceVolSlimeBanner";
                    break;
                case 5:
                    text = "LightfishBanner";
                    break;
                case 6:
                    text = "FlagfishBanner";
                    break;
                case 7:
                    text = "FloatStoneBanner";
                    break;
                case 8:
                    text = "BombshrimpBanner";
                    break;
                case 9:
                    text = "StSlimeBannerBanner";
                    break;
                case 10:
                    text = "TangyuanBanner";
                    break;
                case 11:
                    text = "SoulLanternBanner";
                    break;
                case 12:
                    text = "RedpackBanner";
                    break;
                case 13:
                    text = "LanternghostBanner";
                    break;
                case 14:
                    text = "SpringzombieBanner";
                    break;
                case 15:
                    text = "BakenMonsterBanner";
                    break;
                case 16:
                    text = "FlowerSpriteBanner";
                    break;
                case 17:
                    text = "MilkSlimeBanner";
                    break;
                case 18:
                    text = "LemonslimeBanner";
                    break;
                case 19:
                    text = "GrapeslimeBanner";
                    break;
                case 20:
                    text = "ChocolateSlimeBanner";
                    break;
                case 21:
                    text = "AppleSlimeBanner";
                    break;
                case 22:
                    text = "OrangeSlimeBanner";
                    break;
                case 23:
                    text = "PaperBatBanner";
                    break;
                default:
                    return;
            }
            Item.NewItem(null, i * 16, j * 16, 16, 48, base.Mod.Find<ModItem>(text).Type, 1, false, 0, false, false);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 16;
            Main.spriteBatch.Draw(Mod.GetTexture("Tiles/MonsterBannerGlow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, height), new Color(255, 255, 255, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
        // Token: 0x06004070 RID: 16496 RVA: 0x00325708 File Offset: 0x00323908
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player localPlayer = Main.LocalPlayer;
                string text;
                switch (Main.tile[i, j].TileFrameX / 18)
                {
                    case 0:
                        text = "海月水母";
                        break;
                    case 1:
                        text = "警报水母";
                        break;
                    case 2:
                        text = "海蓝史莱姆";
                        break;
                    case 3:
                        text = "深渊暗流史莱姆";
                        break;
                    case 4:
                        text = "尖刺火山史莱姆";
                        break;
                    case 5:
                        text = "灯笼鱼";
                        break;
                    case 6:
                        text = "旗鱼";
                        break;
                    case 7:
                        text = "火山浮石";
                        break;
                    case 8:
                        text = "枪虾";
                        break;
                    case 9:
                        text = "草莓史莱姆";
                        break;
                    case 10:
                        text = "弹弹汤圆";
                        break;
                    case 11:
                        text = "灯笼幽灵";
                        break;
                    case 12:
                        text = "封包轰炸机";
                        break;
                    case 13:
                        text = "鬼灯笼";
                        break;
                    case 14:
                        text = "吉祥僵尸";
                        break;
                    case 15:
                        text = "腊肠妖灵";
                        break;
                    case 16:
                        text = "兰花精灵";
                        break;
                    case 17:
                        text = "奶糖史莱姆";
                        break;
                    case 18:
                        text = "柠檬糖史莱姆";
                        break;
                    case 19:
                        text = "葡萄糖史莱姆";
                        break;
                    case 20:
                        text = "巧克力糖史莱姆";
                        break;
                    case 21:
                        text = "青苹果糖史莱姆";
                        break;
                    case 22:
                        text = "香橙糖史莱姆";
                        break;
                    case 23:
                        text = "剪纸蝙蝠";
                        break;
                    default:
                        return;
                }
                Main.SceneMetrics.NPCBannerBuff[base.Mod.Find<ModNPC>(text).Type] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }

        // Token: 0x06004071 RID: 16497 RVA: 0x00013B06 File Offset: 0x00011D06
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}
