using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using Everglow.Sources.Commons.Function.ObjectPool;

namespace Everglow.Sources.Commons.Core.VFX;

public class VFXManager : IModule
{
    /// <summary>
    /// Pipeline的Type
    /// </summary>
    private List<Type> pipelineTypes = new List<Type>();
    /// <summary>
    /// Pipeline的实例
    /// </summary>
    private List<IVisualPipeline> pipelineInstances = new List<IVisualPipeline>();
    /// <summary>
    /// 用绘制层 + 第一个调用的绘制层作为Key来储存List<IVisual>
    /// </summary>
    private Dictionary<CallOpportunity, Dictionary<PipelineIndex, List<IVisual>>> visuals =
        new Dictionary<CallOpportunity, Dictionary<PipelineIndex, List<IVisual>>>();
    /// <summary>
    /// 保存每一种Visual所需的Pipeline
    /// </summary>
    private Dictionary<Type, PipelineIndex> visualToPipeline = new Dictionary<Type, PipelineIndex>();
    /// <summary>
    /// RenderTarget池子
    /// </summary>
    private RenderTargetPool renderTargetPool = new RenderTargetPool();
    public string Name => "VFXManager";
    /// <summary>
    /// 获得一种Pipeline的下标，若没有此Pipeline便创建此Pipeline
    /// </summary>
    /// <param name="pipelineType"></param>
    /// <returns></returns>
    public int GetOrCreatePipeline(Type pipelineType)
    {
        if (pipelineTypes.Contains(pipelineType))
        {
            return pipelineTypes.IndexOf(pipelineType);
        }
        pipelineTypes.Add(pipelineType);
        pipelineInstances.Add((IVisualPipeline)Activator.CreateInstance(pipelineType));
        return pipelineTypes.Count - 1;
    }
    public void Register(IVisual visual)
    {
        Type type = visual.GetType();
        var pipelines = type.GetCustomAttribute<PipelineAttribute>().types;
        if (pipelines.Length == 0)
        {
            Debug.Fail("Not bind any pipeline");
            throw new Exception("Not bind any pipeline");
        }
        visualToPipeline.Add(type, new PipelineIndex(pipelineTypes.Select(i => GetOrCreatePipeline(i))));

    }
    public void Draw(CallOpportunity layer)
    {
        var visuals = this.visuals[layer];
        foreach(var (pipelineIndex, innerVisuals) in visuals)
        {
            
        }
    }
    /// <summary>
    /// 添加一个Visual实例
    /// </summary>
    /// <param name="visual"></param>
    public void Add(IVisual visual)
    {
        //将Visual实例加到对应绘制层与第一个Pipeline的位置
        if (!visuals.TryGetValue(visual.DrawLayer, out var value))
        {
            visuals[visual.DrawLayer] = value = new Dictionary<PipelineIndex, List<IVisual>>();
        }
        PipelineIndex index = visualToPipeline[visual.GetType()];
        if (!value.TryGetValue(index, out var list))
        {
            value[index] = list = new List<IVisual>();
        }

        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!list[i].Active)
            {
                list[i] = visual;
                return;
            }
        }
        list.Add(visual);
    }
    public void Load()
    {

    }

    public void Unload()
    {

    }


    private class Rt2DVisual : Visual
    {
        public override CallOpportunity DrawLayer => throw new InvalidOperationException("Don't use this manually!");

        public ResourceLocker<RenderTarget2D> locker;

        public override void Draw()
        {

        }
    }
}
