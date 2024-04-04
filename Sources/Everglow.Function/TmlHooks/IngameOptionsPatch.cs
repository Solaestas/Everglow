using System.Reflection;
using MonoMod.Cil;
using Terraria.Localization;
using Terraria.Social.Steam;

namespace Everglow.Commons.TmlHooks;

internal class IngameOptionsPatch : ILoadable
{
	public void Load(Mod mod)
	{
		Array.Resize(ref IngameOptions.leftScale, IngameOptions.leftScale.Length + 1);
		IL_IngameOptions.Draw += IL_IngameOptions_Draw;
		On_IngameOptions.DrawLeftSide += On_IngameOptions_DrawLeftSide;
	}

	private bool On_IngameOptions_DrawLeftSide(On_IngameOptions.orig_DrawLeftSide orig, SpriteBatch sb, string txt, int i, Microsoft.Xna.Framework.Vector2 anchor, Microsoft.Xna.Framework.Vector2 offset, float[] scales, float minscale, float maxscale, float scalespeed)
	{
		Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));
		if (IngameOptions._leftSideCategoryMapping.TryGetValue(i, out var value))
		{
			if (IngameOptions.category == value)
				color = Color.Gold;
		}
		var pos = anchor + offset * (1 + i);
		Vector2 vector = Utils.DrawBorderStringBig(sb, txt, pos, color, scales[i], 0.5f, 0.5f);

		if (!IngameOptions._canConsumeHover)
			return false;
		if (new Rectangle((int)(pos.X - vector.X / 2f), (int)(pos.Y - vector.Y / 2f - 20f), (int)vector.X, (int)(vector.Y + 10f)).Contains(Main.MouseScreen.ToPoint()))
		{
			IngameOptions._canConsumeHover = false;
			return true;
		}
		return false;
	}

	private static bool ShowButton()
	{
		if (!ModLoader.TryGetMod("SubWorldLibrary", out Mod sbl))
			return false;
		var sblSystemType = sbl.GetType().Assembly.GetType("SubworldLibrary.SubworldSystem");
		var current = sblSystemType.GetField("current", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		return current != null;
	}

	private static void DoSomething()
	{
		if (!ModLoader.TryGetMod("SubWorldLibrary", out Mod sbl))
			return;
		var sblSystemType = sbl.GetType().Assembly.GetType("SubworldLibrary.SubworldSystem");
		var current = sblSystemType.GetField("current", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		var cache = sblSystemType.GetField("cache", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		if (current != null && current == cache)
		{
			//SteamedWraps.StopPlaytimeTracking();
			//SystemLoader.PreSaveAndQuit();
			IngameOptions.Close();
			sblSystemType.GetMethod("BeginEntering", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { int.MinValue });
			//WorldGen.SaveAndQuit();
		}
	}

	private static void IL_IngameOptions_Draw(ILContext iLContext)
	{
		ILCursor cursor = new ILCursor(iLContext);
		if (cursor.TryGotoNext(x => x.MatchStloc(18)))
		{
			cursor.Index++;
			cursor.EmitCall(typeof(IngameOptionsPatch).GetMethod("ShowButton", BindingFlags.Static | BindingFlags.NonPublic));
			int flagIndex = cursor.Index;

			cursor.EmitLdloc(18);
			cursor.EmitLdcI4(1);
			cursor.EmitAdd();
			cursor.EmitStloc(18);

			cursor.EmitNop();
			var label = cursor.DefineLabel();
			label.Target = cursor.Instrs[cursor.Index];

			cursor.Index = flagIndex;
			cursor.EmitBrfalse(label);
		}
		cursor = new ILCursor(iLContext);
		if (cursor.TryGotoNext(x => x.MatchLdsfld<Lang>("inter") && x.Next != null && x.Next.MatchLdcI4(35)) &&
		cursor.TryGotoNext(x => x.MatchStloc(21) && x.Previous != null && x.Previous.MatchAdd()))
		{
			cursor.Index++;

			// 以下 IL 代码相当于插入
			/*
			if (IngameOptionsPatch.ShowButton())
			{
			 	if (IngameOptions.DrawLeftSide(sb, Language.GetTextValue(我日，好几把炫酷), num10, vector4, vector5, IngameOptions.leftScale))
				{
					IngameOptions.leftHover = num10;
					if (flag4)
					{
						IngameOptionsPatch.DoSomething();
					}
				}
			}
			num10++;
			*/

			// 如果 ShowButton 为 false
			// 相当于 if (IngameOptionsPatch.ShowButton())
			cursor.EmitCall(typeof(IngameOptionsPatch).GetMethod("ShowButton", BindingFlags.Static | BindingFlags.NonPublic));
			int flagIndex = cursor.Index;

			// 调用 DrawLeftSide ，如果其返回 false 则跳转到 自增参数索引为 21 的参数 段
			// 相当于 if (IngameOptions.DrawLeftSide(sb, Language.GetTextValue(我日，好几把炫酷), num10, vector4, vector5, IngameOptions.leftScale))
			cursor.EmitLdarg1();
			cursor.EmitLdstr("保存并退出到主菜单");
			cursor.EmitCall(typeof(Language).GetMethod("GetTextValue", new Type[] { typeof(string) }));
			cursor.EmitLdloc(21);
			cursor.EmitLdloc(19);
			cursor.EmitLdloc(20);
			cursor.EmitLdsfld(typeof(IngameOptions).GetField("leftScale", BindingFlags.Static | BindingFlags.Public));
			cursor.EmitLdcR4(0.7f);
			cursor.EmitLdcR4(0.8f);
			cursor.EmitLdcR4(0.01f);
			cursor.EmitCall(typeof(IngameOptions).GetMethod("DrawLeftSide"));
			int index = cursor.Index;

			// 设置 IngameOptions.leftHover 为参数索引为 21 的参数
			// 相当于 IngameOptions.leftHover = num10;
			// 作用为设置当前选中选项
			cursor.EmitLdloc(21);
			cursor.EmitStsfld(typeof(IngameOptions).GetField("leftHover", BindingFlags.Static | BindingFlags.Public));

			// 参数索引为 9 的参数不为 true，则跳到参数索引为 21 的参数自增段
			// 相当于 if (flag4)
			cursor.EmitLdloc(9);
			int index1 = cursor.Index;

			// 执行 IngameOptionsPatch.DoSomething 方法
			cursor.EmitCall(typeof(IngameOptionsPatch).GetMethod("DoSomething", BindingFlags.Static | BindingFlags.NonPublic));

			// 自增参数索引为 21 的参数
			// 相当于 num10++;
			cursor.EmitLdloc(21);
			var label = cursor.DefineLabel();
			label.Target = cursor.Instrs[cursor.Index];
			cursor.EmitLdcI4(1);
			cursor.EmitAdd();
			cursor.EmitStloc(21);

			// 衔接上文
			// 如果 IngameOptionsPatch.DrawLeftSide 不为 true，则跳到参数索引为 21 的参数自增段
			// 相当于检测鼠标是否悬浮于选项
			cursor.Index = index;
			cursor.EmitBrfalse(label);

			// 衔接上文
			// 如果参数索引为 9 的参数为 false，则跳到参数索引为 21 的参数自增段
			// 相当于检测鼠标是否按下
			cursor.Index = index1 + 1;
			cursor.EmitBrfalse(label);

			// 衔接上文
			// 如果 IngameOptionsPatch.ShowButton 为 false 则跳过选项绘制
			cursor.Index = flagIndex;
			cursor.EmitBrfalse(label);
		}
	}

	public void Unload()
	{
		Array.Resize(ref IngameOptions.leftScale, IngameOptions.leftScale.Length - 1);
		IL_IngameOptions.Draw -= IL_IngameOptions_Draw;
		On_IngameOptions.DrawLeftSide -= On_IngameOptions_DrawLeftSide;
	}
}