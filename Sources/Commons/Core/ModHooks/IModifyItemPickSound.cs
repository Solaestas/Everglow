using Terraria.Audio;
using Terraria.ModLoader.Core;
using Hook = Everglow.Sources.Commons.Core.ModHooks.IModifyItemPickSound;

namespace Everglow.Sources.Commons.Core.ModHooks
{
    public interface IModifyItemPickSound
	{
		public static readonly HookList<GlobalItem> Hook = ItemLoader.AddModHook(new HookList<GlobalItem>(typeof(Hook).GetMethod(nameof(ModifyItemPickSound))));

		/// <summary>
		/// 用于替换掉从物品槽拿起/放下物品的音效（不会在服务器运行）
		/// </summary>
		/// <param name="item">物品实例</param>
		/// <param name="context">物品槽ID，详见 <seealso cref="Terraria.UI.ItemSlot.Context"/> </param>
		/// <param name="putIn">
		/// 是否正在放入某个物品槽
		/// <br>如果物品槽内物品和手持物品一致，为 <see langword="true"/>，即放入</br>
		/// <br>如果右键点击，只会是 <see langword="false"/></br>
		/// </param>
		/// <param name="customSound"> <seealso cref="SoundStyle"/> 音效实例</param>
		/// <param name="playOriginalSound">是否播放原版音效</param>
		void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound);

		public static void Invoke(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound) {
			(item.ModItem as Hook)?.ModifyItemPickSound(item, context, putIn, ref customSound, ref playOriginalSound);

			foreach (Hook g in Hook.Enumerate(item)) {
				g.ModifyItemPickSound(item, context, putIn, ref customSound, ref playOriginalSound);
			}
		}
	}
}
