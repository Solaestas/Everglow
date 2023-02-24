namespace Everglow.ZYModule.Commons.Core;

public static class EnumUtils
{
	public static T[] GetEnums<T>() where T : Enum
	{
		return (from t in new object[] { typeof(T).GetEnumValues() } select (T)t).ToArray();
	}
}
