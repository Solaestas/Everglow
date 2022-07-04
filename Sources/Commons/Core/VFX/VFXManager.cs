using Everglow.Sources.Commons.Core.ModuleSystem;
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
    private Dictionary<CallOpportunity, Dictionary<int, List<IVisual>>> visuals =
        new Dictionary<CallOpportunity, Dictionary<int, List<IVisual>>>();
    /// <summary>
    /// 保存每一种Visual所需的Pipeline
    /// </summary>
    private Dictionary<Type, List<int>> visualToPipeline = new Dictionary<Type, List<int>>();
    /// <summary>
    /// RenderTarget池子
    /// </summary>
    private RenderTargetPool renderTargetPool = new RenderTargetPool();
    public string Name => "VFXManager";
    public void Register(IVisual visual)
    {
        Type type = visual.GetType();
        var list = visualToPipeline[type] = new List<int>();
        foreach (var pipelineType in type.GetCustomAttribute<PipelineAttribute>().types)
        {
            if (!pipelineTypes.Contains(pipelineType))
            {
                var pipeline = (IVisualPipeline)Activator.CreateInstance(pipelineType);
                list.Add(pipelineInstances.Count);
                pipelineInstances.Add(pipeline);
                pipelineTypes.Add(pipelineType);
            }
            else
            {
                list.Add(pipelineTypes.IndexOf(pipelineType));
            }
        }
    }
    public void Draw(CallOpportunity layer)
    {
        var visuals = this.visuals[layer];
        foreach (var (pipelineIndex, list) in visuals)
        {
            var pipeline = pipelineInstances[pipelineIndex];

            foreach(var visual in list)
            {
                if(!visual.Active)
                {
                    continue;
                }

            }
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
            visuals[visual.DrawLayer] = value = new Dictionary<int, List<IVisual>>();
        }
        //第一个所需Pipeline的Index
        int index = visualToPipeline[visual.GetType()][0];
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
}
