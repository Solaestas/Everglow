using Terraria.ModLoader.IO;

namespace Everglow.Commons.Events
{
	public abstract class ModEvent : ModType
	{
		public sealed override void Register()
		{
			EventManager.Reigster(this);
		}
		/// <summary>
		/// 多个<see cref="ModEvent"/>处于激活状态时，按<see cref="SortRank"/>排序
		/// <br><see cref="SortRank"/>越大,越优先被执行</br>
		/// </summary>
		public float SortRank = 1;
		/// <summary>
		/// 此事件为后台事件(不显示事件)
		/// </summary>
		public bool IsBackground = true;
		/// <summary>
		/// 此事件是否处于启动状态
		/// <br>此属性将自动保存,重新进入世界后自动恢复</br>
		/// </summary>
		public bool Active { get;internal set; }
		/// <summary>
		/// 事件启动时被调用
		/// </summary>
		/// <param name="args">启动参数</param>
		public virtual void OnStart(params object[] args) { }
		/// <summary>
		/// 事件停止时被调用
		/// </summary>
		/// <param name="args">停止参数
		/// <br>当由<see cref="EventManager.Stop(Predicate{ModEvent}, object[])"/>停止时</br>,首个参数固定为谓语限定</param>
		public virtual void OnStop(params object[] args) { }
		public virtual void Update() { }
		/// <summary>
		/// 只在<see cref="IsBackground"/>为true时被调用
		/// </summary>
		/// <param name="sprite"></param>
		public virtual void Draw(SpriteBatch sprite) { }
		public virtual void SaveData(TagCompound tag) { }
		public virtual void LoadData(TagCompound tag) { }
		/// <summary>
		/// 修改生成池
		/// </summary>
		/// <param name="pool"></param>
		/// <param name="spawnInfo"></param>
		/// <returns>返回false以阻止其他事件继续修改</returns>
		public virtual bool EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) => false;
		/// <summary>
		/// 修改刷怪率
		/// </summary>
		/// <param name="player"></param>
		/// <param name="spawnrate"></param>
		/// <param name="maxspawn"></param>
		/// <returns>返回false以阻止其他事件继续修改</returns>
		public virtual bool EditSpawnRate(Player player, ref int spawnrate, ref int maxspawn) => false;
		/// <summary>
		/// 修改刷怪范围
		/// </summary>
		/// <param name="player"></param>
		/// <param name="spawnrangex"></param>
		/// <param name="spawnrangey"></param>
		/// <param name="saferangex"></param>
		/// <param name="saferangey"></param>
		/// <returns>返回false以阻止其他事件继续修改</returns>
		public virtual bool EditSpawnRange(Player player, ref int spawnrangex, ref int spawnrangey, ref int saferangex, ref int saferangey) => false;
	}
}
