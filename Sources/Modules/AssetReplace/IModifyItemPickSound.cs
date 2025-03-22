using System.Text;
using Terraria.Audio;
using Terraria.ModLoader.Core;
using Hook = Everglow.AssetReplace.IModifyItemPickSound;

namespace Everglow.AssetReplace
{
	public interface IModifyItemPickSound
	{
		public static readonly GlobalHookList<GlobalItem> Hook = ItemLoader.AddModHook(GlobalHookList<GlobalItem>.Create(e => (e as Hook).ModifyItemPickSound));

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
		virtual void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound)
		{
		}

		/// <summary>
		/// 给 <seealso cref="ModItem"/> 添加自定义音效的快捷方法
		/// </summary>
		virtual SoundStyle? ModItemCustomPickSound() => null;

		public static void Invoke(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound)
		{
			if (item.ModItem is Hook)
			{
				(item.ModItem as Hook).ModifyItemPickSound(item, context, putIn, ref customSound, ref playOriginalSound);
				var pickSound = (item.ModItem as Hook).ModItemCustomPickSound();
				if (pickSound.HasValue)
				{
					playOriginalSound = false;
					SoundEngine.PlaySound(pickSound.Value);
				}
			}

			foreach (Hook g in Hook.Enumerate(item))
			{
				g.ModifyItemPickSound(item, context, putIn, ref customSound, ref playOriginalSound);
			}
		}
	}

	public static class PickSoundHelper
	{
		public static void ReadFromTxtFile(this Hook modifier, string fileName, out int[] itemIDs)
		{
			int everglowLength = nameof(Everglow).Length;
			string path = $"{modifier.GetType().Namespace.Replace('.', '/').Remove(0, everglowLength + 1)}/{fileName}.txt";
			List<int> ids = new();
			if (ModIns.Mod.FileExists(path))
			{
				using var stream = ModIns.Mod.GetFileStream(path);
				using var streamReader = new StreamReader(stream, Encoding.UTF8);

				string lastLine = string.Empty;
				string fileContents = streamReader.ReadLine();

				while (fileContents is not null)
				{
					if (int.TryParse(fileContents, out int id))
					{
						ids.Add(id);
					}

					// 星号表示从A到B的范围
					else if (int.TryParse(lastLine, out int lastID) && fileContents == "*")
					{
						fileContents = streamReader.ReadLine(); // 读取到下一行
						id = int.Parse(fileContents);
						for (int i = lastID + 1; i <= id; i++)
						{
							ids.Add(i);
						}
					}
					lastLine = fileContents;
					fileContents = streamReader.ReadLine();
				}
				itemIDs = ids.ToArray();

				stream.Close();
				return;
			}
			itemIDs = new int[1] { 0 };
		}
	}
}