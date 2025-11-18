using Everglow.EternalResolve.Items.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Bosses.Projectiles
{
    public class CrutchRapier_Hostile : Rapier_Hostile
    {
        public override void SetDefaults()
        {
            Color = new Color(155, 162, 164);
            base.SetDefaults();
			MaxOldAttackUnitCount = 4;
			OldShade = 0.3f;
			Shade = 0.2f;
			ShadeMultiplicative_Modifier = 0.64f;
			ScaleMultiplicative_Modifier = 1;
			OldLightColorValue = 1f;
			LightColorValueMultiplicative_Modifier = 0.4f;
			AttackLength = 1.05f;
			AttackEffectWidth = 0.4f;
			itemType = ModContent.ItemType<CrutchBayonet>();
		}

	}
}