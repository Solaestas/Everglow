using Everglow.Commons.Netcode;

namespace Everglow.Commons;

public static class ModIns
{
	public static Mod Mod { get; set; }

	public static PacketResolver PacketResolver { get; set; }

	public static event Action OnPostSetupContent;

	public static event Action OnUnload;

	public static void PostSetupContent() => OnPostSetupContent?.Invoke();

	public static void Unload() => OnUnload?.Invoke();
}