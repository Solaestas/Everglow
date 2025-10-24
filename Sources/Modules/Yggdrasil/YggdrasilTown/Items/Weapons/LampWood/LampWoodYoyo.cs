using Everglow.Commons.Templates.Weapons.Yoyos;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.LampWood;

public class LampWoodYoyo : YoyoItem
{
    public override void SetCustomDefaults()
    {
        Item.damage = 9;
        Item.rare = ItemRarityID.White;
        Item.value = Item.buyPrice(silver: 5);
        Item.shoot = ModContent.ProjectileType<Projectiles.Melee.LampWoodYoyo>();
    }
}