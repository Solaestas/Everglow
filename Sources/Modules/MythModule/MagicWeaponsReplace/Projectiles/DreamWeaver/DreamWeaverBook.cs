namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DreamWeaver
{
    internal class DreamWeaverBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            DustType = DustID.WaterCandle;
            ItemType = ModContent.ItemType<TheFirefly.Items.Weapons.DreamWeaver>();
            GlowPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverFrontGlow";
            BackGlowPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverBackGlow";
            BackTexPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverBack";
            PaperTexPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverPaper";
            FrontTexPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverFront";
            TexCoordTop = new Vector2(9, 0);
            TexCoordLeft = new Vector2(0, 34);
            TexCoordDown = new Vector2(21, 46);
            TexCoordRight = new Vector2(34, 9);
        }
        public override void SpecialAI()
        {
    
        }
    }
}