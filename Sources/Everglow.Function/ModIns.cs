namespace Everglow.Commons;

public static class ModIns
{
	public static string ModCachePath;

	public static Mod Mod
	{
		get => _mod;
		set
		{
			_mod = value;
			if (value != null)
				ModCachePath = Path.Combine(Main.SavePath, "Mods", "ModDatas", _mod.Name);
			else
				ModCachePath = string.Empty;
		}
	}

	private static Mod _mod;

	public static event Action OnPostSetupContent;

	public static event Action OnUnload;

	public static void PostSetupContent() => OnPostSetupContent?.Invoke();

	public static void Unload() => OnUnload?.Invoke();
}