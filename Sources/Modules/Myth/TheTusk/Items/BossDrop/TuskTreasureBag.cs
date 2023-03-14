using Everglow.Myth.TheTusk.Items.Accessories;
using Everglow.Myth.TheTusk.Items.Weapons;
using Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.Items.BossDrop;

public class TuskTreasureBag : ModItem
{
	public override void SetDefaults()
	{
		Item.maxStack = 999;
		Item.consumable = true;
		Item.width = 32;
		Item.height = 32;
		Item.rare = ItemRarityID.Cyan;
		Item.expert = true;
	}

	public override bool CanRightClick()
	{
		return true;
	}
	//TODO 迁移
#if false
	[Obsolete]
	public override int BossBagNPC => ModContent.NPCType<BloodTusk>();
	[Obsolete]
	public override void OpenBossBag(Player player)
	{
		//player.TryGettingDevArmor();

		int h = Main.rand.Next(5);
		if (h == 0)
			player.QuickSpawnItem(null, ModContent.ItemType<ToothKnife>(), 1);
		if (h == 1)
			player.QuickSpawnItem(null, ModContent.ItemType<TuskLace>(), 1);
		if (h == 2)
			player.QuickSpawnItem(null, ModContent.ItemType<ToothStaff>(), 1);
		if (h == 3)
			player.QuickSpawnItem(null, ModContent.ItemType<ToothMagicBall>(), 1);
		if (h == 4)
			player.QuickSpawnItem(null, ModContent.ItemType<BloodyBoneYoyo>(), 1);
		if (h == 5)
			player.QuickSpawnItem(null, ModContent.ItemType<SpineGun>(), 1);
	}
#endif
	private int a = 0;
	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
	{
		Texture2D t = ModContent.Request<Texture2D>("Everglow/Myth/TheTusk/Items/BossDrop/TuskTreasureBag").Value;
		for (int i = 0; i < 4; i++)
		{
			Vector2 v = new Vector2(0, 8 * ((float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 4f)) * 0.3f + 0.7f)).RotatedBy((double)(Main.GlobalTimeWrappedHourly * 6.28318548f / 4f) + MathHelper.Pi * i / 2d);
			//spriteBatch.Draw(t, Item.Center, new Rectangle(0,0,32,32), new Color(100, 100, 100, 0), 0, new Vector2(16, 16), 3f, SpriteEffects.None, 1);
			Main.EntitySpriteDraw(t, Item.Center - Main.screenPosition + v, null, new Color(100, 100, 100, 0), 0, new Vector2(16, 16), 1f, SpriteEffects.None, 0);
		}
		if (!Main.gamePaused && a % 20 == 19)
		{
			a = 0;
			int num37 = Dust.NewDust(Item.Center + new Vector2(Main.rand.Next(-16, 6), 0), 0, 0, DustID.SilverCoin, 0f, 0f, 254, Color.White, 1f);
			Main.dust[num37].velocity = new Vector2(0, -Main.rand.NextFloat(0.3f, 0.9f));
			Main.dust[num37].rotation = 0;
			Main.dust[num37].noLight = true;
			Main.dust[num37].alpha = 240;
		}
		if (!Main.gamePaused)
		{
			a++;
			Lighting.AddLight((int)(Item.Center.X / 16f), (int)(Item.Center.Y / 16f), 0.5f, 0.15f, 0.0f);
		}

		return true;
	}
}
