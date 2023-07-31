using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Capture;


namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000E8F RID: 3727
    public class 青纽扣珊瑚 : ModTile
	{
		private float num5 = 0;
		private int num6 = 0;
		// Token: 0x060045D6 RID: 17878 RVA: 0x0027A6F0 File Offset: 0x002788F0
		public override void SetStaticDefaults()
		{
			Main.tileSolid[(int)base.Type] = true;
			Main.tileMergeDirt[(int)base.Type] = true;
			Main.tileBlendAll[(int)base.Type] = true;
			Main.tileBlockLight[(int)base.Type] = true;
			Main.tileShine2[(int)base.Type] = true;
			Main.tileOreFinderPriority[(int)base.Type] = 1300;
			this.MinPick = 200;
			this.DustType = 183;
			this.HitSound = 21;
			this.soundStyle/* tModPorter Note: Removed. Integrate into HitSound */ = 2;
            this.RegisterItemDrop(ModContent.ItemType<Everglow.Ocean.Items.CyanZoanthid>());
			Main.tileSpelunker[(int)base.Type] = true;
			LocalizedText modTranslation = base.CreateMapEntryName();
			base.AddMapEntry(new Color(37, 91, 67), modTranslation);
            // modTranslation.SetDefault("");
            // modTranslation.AddTranslation(GameCulture.Chinese, "");
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			num5 += 0.3f;
			LocalizedText modTranslation = base.CreateMapEntryName();
			base.AddMapEntry(new Color(159 * (int)(Math.Sin(num5 / 10000f) * 0.5 + 1), 101, 196), modTranslation);
			if (!Lighting.NotRetro)
			{
				return;
			}
			int num = (int)Main.tile[i, j].TileFrameX;
			int num2 = (int)Main.tile[i, j].TileFrameY;
			int num3 = i % 1;
			int num4 = j % 1;
			num3 *= 288;
			num4 *= 270;
			num += num3;
			num2 += num4;
			Texture2D texture = ModContent.Request<Texture2D>("Everglow/Ocean/Tiles/Ocean/青纽扣珊瑚Glow");
			Vector2 position = new Vector2((float)(i * 16) - Main.screenPosition.X + (float)this.GetDrawOffset(), (float)(j * 16) - Main.screenPosition.Y + (float)this.GetDrawOffset());
			if (CaptureManager.Instance.IsCapturing)
			{
				position = new Vector2((float)(i * 16) - Main.screenPosition.X, (float)(j * 16) - Main.screenPosition.Y);
			}
			spriteBatch.Draw(texture, position, new Rectangle?(new Rectangle(num, num2, 18, 18)), new Color(55,55,55,0), 0f, new Vector2(0f, 0f), 1f, SpriteEffects.None, 0f);
		}
		// Token: 0x06004041 RID: 16449 RVA: 0x00324478 File Offset: 0x00322678
		public override void NearbyEffects(int i, int j, bool closer)
		{
		}
		// Token: 0x0600401A RID: 16410 RVA: 0x003228F4 File Offset: 0x00320AF4
		private int GetDrawOffset()
		{
			int num;
			if ((float)Main.screenWidth < 1664f)
			{
				num = 193;
			}
			else
			{
				num = (int)(-0.5f * (float)Main.screenWidth + 1025f);
			}
			return num - 1;
		}
	}
}
