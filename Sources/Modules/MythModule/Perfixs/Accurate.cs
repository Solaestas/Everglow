namespace Everglow.Sources.Modules.MythModule.Prefixs
{
    /// <summary>
    /// 功能尚未完善
    /// </summary>
    public class Accurate : ModPrefix
    {
        public override PrefixCategory Category => PrefixCategory.Accessory;
        public override void AutoStaticDefaults()
        {
            base.AutoStaticDefaults();
        }
        public override void ValidateItem(Item item, ref bool invalid)
        {
            base.ValidateItem(item, ref invalid);
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 0.1025f;
        }
        public override void Apply(Item item)
        {
            item.FindOwner(item.whoAmI);
            base.Apply(item);
        }
        public override float RollChance(Item item)
        {
            return 0f;
        }
    }
}
