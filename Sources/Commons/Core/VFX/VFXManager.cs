using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using Everglow.Sources.Commons.Function.ObjectPool;

namespace Everglow.Sources.Commons.Core.VFX;
[ProfilerMeasure]
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
    private Dictionary<CallOpportunity, Dictionary<PipelineIndex, SortedList<int ,IVisual>>> visuals =
        new Dictionary<CallOpportunity, Dictionary<PipelineIndex, SortedList<int, IVisual>>>();
    /// <summary>
    /// 保存每一种Visual所需的Pipeline
    /// </summary>
    private List<PipelineIndex> requiredPipeline = new List<PipelineIndex>();
    /// <summary>
    /// 保存每一种Visual的Type
    /// </summary>
    private Dictionary<Type, int> visualTypes = new Dictionary<Type, int>();
    /// <summary>
    /// RenderTarget池子
    /// </summary>
    private RenderTargetPool renderTargetPool = new RenderTargetPool();
    /// <summary>
    /// GraphicsDevice引用
    /// </summary>
    private GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
    private bool rt2DIndex = false;
    private RenderTarget2D CurrentRenderTarget => rt2DIndex ? Main.screenTarget : Main.screenTargetSwap;
    private RenderTarget2D NextRenderTarget => rt2DIndex ? Main.screenTarget : Main.screenTargetSwap;
    
    public static VFXBatch spriteBatch;
    /// <summary>
    /// 名称
    /// </summary>
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
    /// <summary>
    /// 切换当前RenderTarget
    /// </summary>
    private void SwitchRenderTarget()
    {
        graphicsDevice.SetRenderTarget(NextRenderTarget);
        graphicsDevice.Clear(Color.Transparent);
        rt2DIndex = !rt2DIndex;
    }
    /// <summary>
    /// 获得Visual的Type
    /// </summary>
    /// <param name="visual"></param>
    /// <returns></returns>
    public int GetVisualType(IVisual visual)
    {
        return visualTypes[visual.GetType()];
    }
    /// <summary>
    /// 注册一个Visual
    /// </summary>
    /// <param name="visual"></param>
    /// <exception cref="Exception">该Visual未绑定任何Pipeline</exception>
    public void Register(IVisual visual)
    {
        Type type = visual.GetType();
        visualTypes.Add(type, requiredPipeline.Count);
        if (type.IsDefined(typeof(PipelineAttribute)))
        {
            var pipelines = type.GetCustomAttribute<PipelineAttribute>().types;
            if (pipelines.Length == 0)
            {
                Debug.Fail("Not bind any pipeline");
                throw new Exception("Not bind any pipeline");
            }
            requiredPipeline.Add(new PipelineIndex(pipelines.Select(i => GetOrCreatePipeline(i))));
        }
        else
        {
            requiredPipeline.Add(null);
        }
    }
    public void Draw(CallOpportunity layer)
    {
        var visuals = this.visuals[layer];
        foreach(var (pipelineIndex, innerVisuals) in visuals)
        {
            if(pipelineIndex.next != null)
            {
                var rt2D = new Rt2DVisual(renderTargetPool.GetRenderTarget2D());
                graphicsDevice.SetRenderTarget(rt2D.locker.Resource);
                graphicsDevice.Clear(Color.Transparent);
                if(!visuals.TryGetValue(pipelineIndex.next, out var list))
                {
                    visuals[pipelineIndex.next] = list = new SortedList<int, IVisual>();
                }
                list.Add(rt2D.Type, rt2D);
            }

            var pipeline = pipelineInstances[pipelineIndex.index];
            pipeline.BeginRender();
            pipeline.Render(innerVisuals.Values);
            pipeline.EndRender();

            if (pipelineIndex.next != null)
            {
                SwitchRenderTarget();
                //TODO Draw
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
            visuals[visual.DrawLayer] = value = new Dictionary<PipelineIndex, SortedList<int, IVisual>>();
        }
        PipelineIndex index = requiredPipeline[visual.Type] ?? throw new InvalidOperationException("Not bind any pipeline");
        if (!value.TryGetValue(index, out var list))
        {
            value[index] = list = new SortedList<int, IVisual>();
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
        list.Add(visual.Type, visual);
    }
    public void Load()
    {
        spriteBatch = new VFXBatch(graphicsDevice);
    }
    public void Unload()
    {

    }

    [DontAutoLoad]
    private class Rt2DVisual : Visual
    {
        public override CallOpportunity DrawLayer => throw new InvalidOperationException("Don't use this manually!");

        public ResourceLocker<RenderTarget2D> locker;

        public Rt2DVisual(ResourceLocker<RenderTarget2D> locker)
        {
            this.locker = locker;
        }

        public override void Draw()
        {

        }
    }
}
