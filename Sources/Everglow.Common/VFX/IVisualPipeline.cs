using Everglow.Core.VFX.Visuals;

namespace Everglow.Core.VFX.Pipelines
{
	public interface IPipeline
	{
		/// <summary>
		/// 批量渲染同一种，或者同一类 VFX
		/// </summary>
		/// <param name="visuals"></param>
		public void Render(IEnumerable<IVisual> visuals);

		/// <summary>
		/// 用于加载Effect等资源
		/// </summary>
		public void Load();

		/// <summary>
		/// 用于释放资源
		/// </summary>
		public void Unload();
	}
}