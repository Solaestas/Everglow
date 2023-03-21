using Everglow.Sources.Commons.Core.VFX;
namespace Everglow.Sources.Modules.YggdrasilModule
{
    public interface IOcclusionProjectile
    {
        void DrawOcclusion(VFXBatch spriteBatch);
        void DrawEffect(VFXBatch spriteBatch);
    }
}
