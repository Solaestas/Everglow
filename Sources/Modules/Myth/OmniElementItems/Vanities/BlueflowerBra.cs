using Terraria.Localization;

namespace Everglow.Sources.Modules.MythModule.OmniElementItems.Vanities
{
    //TODO Need Rewrite
    [AutoloadEquip(EquipType.Body)]
    public class BlueflowerBra : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 12;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.vanity = true;
            Item.accessory = true;
        }

        //public static void Load()
        //{
        //    On.Terraria.Main.DrawPlayers_AfterProjectiles += DrawHat;
        //    On.Terraria.Main.DrawPlayers_BehindNPCs += DrawDress;
        //}

        //public static void UnLoad()
        //{
        //    //On.Terraria.Main.DrawPlayers_AfterProjectiles -= DrawHat;
        //    //On.Terraria.Main.DrawPlayers_BehindNPCs -= DrawDress;
        //}

        //private static void DrawDress(On.Terraria.Main.orig_DrawPlayers_BehindNPCs orig, Terraria.Main self)
        //{
        //    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        //    for (int d = 0; d < Main.player.Length; d++)
        //    {
        //        for (int e = 0; e < Main.player[d].armor.Length; e++)
        //        {
        //            if (Main.player[d].armor[e].type == ModContent.ItemType<BlueflowerDress>())
        //            {
        //                Texture2D Dress = MythContent.QuickTexture("OmniElementItems/Vanities/BlueflowerDress_Dress");

        //                Main.spriteBatch.Draw(Dress, Main.player[d].MountedCenter - Main.screenPosition + new Vector2(0, Main.player[d].height * 0.4f + 12) + new Vector2(0, -Main.player[d].height * 0.4f).RotatedBy(Main.player[d].headRotation + Main.player[d].fullRotation) + new Vector2(0, 0), null, Lighting.GetColor((int)(Main.player[d].Center.X / 16f), (int)(Main.player[d].Center.Y / 16f) - 2), (Main.player[d].headRotation + Main.player[d].fullRotation), Dress.Size() / 2f, 1f, Main.player[d].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        //            }
        //        }
        //    }
        //    Main.spriteBatch.End();
        //}

        //private static void DrawHat(On.Terraria.Main.orig_DrawPlayers_AfterProjectiles orig, Terraria.Main self)
        //{
        //    orig(self);
        //    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        //    for (int d = 0; d < Main.player.Length; d++)
        //    {
        //        for (int e = 0; e < Main.player[d].armor.Length; e++)
        //        {
        //            if (Main.player[d].armor[e].type == ModContent.ItemType<BlueflowerCap>())
        //            {
        //                Texture2D Cap = MythContent.QuickTexture("OmniElementItems/Vanities/BlueflowerCap_Cap");
        //                Main.spriteBatch.Draw(Cap, Main.player[d].MountedCenter - Main.screenPosition + new Vector2(0, Main.player[d].height - 16) + new Vector2(0, -Main.player[d].height).RotatedBy(Main.player[d].headRotation + Main.player[d].fullRotation) + new Vector2(0, 0), null, Lighting.GetColor((int)(Main.player[d].Center.X / 16f), (int)(Main.player[d].Center.Y / 16f) - 2), (Main.player[d].headRotation + Main.player[d].fullRotation), Cap.Size() / 2f, 1f, Main.player[d].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        //            }
        //        }
        //    }
        //    Main.spriteBatch.End();
        //}
    }
}