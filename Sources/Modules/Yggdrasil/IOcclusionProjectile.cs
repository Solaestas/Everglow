using Everglow.Sources.Commons.Core.VFX;

namespace Everglow.Yggdrasil
{
	public interface IOcclusionProjectile
	{
		void DrawOcclusion(VFXBatch spriteBatch);
		void DrawEffect(VFXBatch spriteBatch);
	}
}
