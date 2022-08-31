using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items
{
    public class MothScaleDust : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.width = 20;
            Item.height = 12;
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GlowingFirefly>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
