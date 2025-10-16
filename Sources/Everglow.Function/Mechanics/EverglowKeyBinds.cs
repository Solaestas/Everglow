using Microsoft.Xna.Framework.Input;

namespace Everglow.Commons.Mechanics;

public class EverglowKeyBinds : ModSystem
{
	public static ModKeybind ArmorSetBonusHotKey { get; private set; }

	public static ModKeybind AccessorySkillKey { get; private set; }

	public override void Load()
	{
		ArmorSetBonusHotKey = KeybindLoader.RegisterKeybind(Mod, nameof(ArmorSetBonusHotKey), Keys.Z);
		AccessorySkillKey = KeybindLoader.RegisterKeybind(Mod, nameof(AccessorySkillKey), Keys.C);
	}

	public override void Unload()
	{
		ArmorSetBonusHotKey = null;
		AccessorySkillKey = null;
	}
}