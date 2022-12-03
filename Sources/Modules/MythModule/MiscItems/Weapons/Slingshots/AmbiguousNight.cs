namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Slingshots
{
    public class AmbiguousNight : SlingshotItem
    {
        //TODO:迷夜窥视 命中后给怪物施加幻夜印记 幻夜印记：有两个同时被此印记标记的怪物靠近之后，产生梦魇织丝，对两个怪物造成伤害，并且此buff持续时间-2s。
        public override void SetDef()
        {
            Item.damage = 54;
            Item.crit = 8;
            ProjType = ModContent.ProjectileType<Projectiles.AmbiguousNight>();
            Item.width = 40;
            Item.height = 32;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }
    }
}