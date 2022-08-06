using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using Everglow.Sources.Commons.Function.ObjectPool;

namespace Everglow.Sources.Commons.Core.VFX;
[ProfilerMeasure]
public class VFXManager : IModule
{
    private class VisualCompare : Comparer<IVisual>
    {
        public override int Compare(IVisual x, IVisual y)
        {
            return x.Type - y.Type;
        }
    }
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
    private VisualCompare compare = new VisualCompare();
    private bool rt2DIndex = true;
    /// <summary>
    /// 当前的RenderTarget，取值为screenTarget或者screenTargetSwap
    /// </summary>
    public RenderTarget2D CurrentRenderTarget => rt2DIndex ? Main.screenTarget : Main.screenTargetSwap;
    /// <summary>
    /// 不是当前的RenderTarget，取值为screenTarget或者screenTargetSwap
    /// </summary>
    public RenderTarget2D NextRenderTarget => rt2DIndex ? Main.screenTarget : Main.screenTargetSwap;
    /// <summary>
    /// 代替SpriteBatch，可以用来处理顶点绘制
    /// </summary>
    public static VFXBatch spriteBatch;
    public static VFXManager Instance { get; private set; }
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
        IVisualPipeline pipeline = (IVisualPipeline)Activator.CreateInstance(pipelineType);
        pipeline.Load();
        pipelineInstances.Add(pipeline);
        return pipelineTypes.Count - 1;
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
    public void SwitchRenderTarget() => rt2DIndex = !rt2DIndex;
    public void Draw(CallOpportunity layer)
    {
        var visuals = this.visuals[layer];
        foreach (var (pipelineIndex, innerVisuals) in visuals)
        {
            Main.NewText(innerVisuals.Count);
            if (pipelineIndex.next != null)
            {
                var rt2D = new Rt2DVisual(renderTargetPool.GetRenderTarget2D());
                graphicsDevice.SetRenderTarget(rt2D.locker.Resource);
                graphicsDevice.Clear(Color.Transparent);
                if (!visuals.TryGetValue(pipelineIndex.next, out var list))
                {
                    visuals[pipelineIndex.next] = list = new List<IVisual>();
                }
                list.Add(rt2D);
            }

            var pipeline = pipelineInstances[pipelineIndex.index];
            pipeline.BeginRender();
            pipeline.Render(innerVisuals.Where(v => v.Visible));
            pipeline.EndRender();

            if (pipelineIndex.next != null)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                graphicsDevice.SetRenderTarget(NextRenderTarget);
                Main.spriteBatch.Draw(CurrentRenderTarget, Vector2.Zero, Color.White);
                rt2DIndex = !rt2DIndex;
                Main.spriteBatch.End();
            }
        }

        if (CurrentRenderTarget != Main.screenTarget)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            graphicsDevice.SetRenderTarget(NextRenderTarget);
            Main.spriteBatch.Draw(CurrentRenderTarget, Vector2.Zero, Color.White);
            rt2DIndex = !rt2DIndex;
            Main.spriteBatch.End();
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
        PipelineIndex index = requiredPipeline[visual.Type] ?? throw new InvalidOperationException("Not bind any pipeline");
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

        //红黑树好难……暂时就先这样了
        list.Add(visual);
        list.Sort(compare);
    }
    public void Update()
    {
        foreach (var visuals in visuals.Values)
        {
            foreach (var list in visuals.Values)
            {
                foreach (var visual in list)
                {
                    if (visual.Active)
                    {
                        visual.Update();
                    }
                }
            }
        }
    }
    public void Flush()
    {
        foreach (var visuals in visuals.Values)
        {
            foreach (var (key, list) in visuals)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!list[i].Active)
                    {
                        list.RemoveAt(i--);
                    }
                }
            }

        }
    }
    public void Clear()
    {
        foreach (var visuals in visuals.Values)
        {
            visuals.Clear();
        }
    }
    public void Load()
    {
        Instance = this;
        spriteBatch = new VFXBatch(graphicsDevice);
        visuals[CallOpportunity.PostDrawEverything] = new Dictionary<PipelineIndex, List<IVisual>>();
        visuals[CallOpportunity.PostDrawProjectiles] = new Dictionary<PipelineIndex, List<IVisual>>();
        visuals[CallOpportunity.PostDrawTiles] = new Dictionary<PipelineIndex, List<IVisual>>();
        visuals[CallOpportunity.PostDrawDusts] = new Dictionary<PipelineIndex, List<IVisual>>();
        visuals[CallOpportunity.PostDrawBG] = new Dictionary<PipelineIndex, List<IVisual>>();
        visuals[CallOpportunity.PostDrawPlayers] = new Dictionary<PipelineIndex, List<IVisual>>();
        visuals[CallOpportunity.PostDrawNPCs] = new Dictionary<PipelineIndex, List<IVisual>>();
        Everglow.HookSystem.AddMethod(() => Draw(CallOpportunity.PostDrawEverything), CallOpportunity.PostDrawEverything,
            "VFX PostDrawEverything");
        Everglow.HookSystem.AddMethod(() => Draw(CallOpportunity.PostDrawProjectiles), CallOpportunity.PostDrawProjectiles,
            "VFX PostDrawProjectile");
        Everglow.HookSystem.AddMethod(() => Draw(CallOpportunity.PostDrawTiles), CallOpportunity.PostDrawTiles,
            "VFX PostDrawTile");
        Everglow.HookSystem.AddMethod(() => Draw(CallOpportunity.PostDrawDusts), CallOpportunity.PostDrawDusts,
            "VFX PostDrawDust");
        Everglow.HookSystem.AddMethod(() => Draw(CallOpportunity.PostDrawBG), CallOpportunity.PostDrawBG,
            "VFX PostDrawBG");
        Everglow.HookSystem.AddMethod(() => Draw(CallOpportunity.PostDrawPlayers), CallOpportunity.PostDrawPlayers,
            "VFX PostDrawPlayer");
        Everglow.HookSystem.AddMethod(() => Draw(CallOpportunity.PostDrawNPCs), CallOpportunity.PostDrawNPCs,
            "VFX PostDrawNPCs");
    }
    public void Unload()
    {
        Everglow.MainThreadContext.AddTask(() =>
        {
            foreach (var pipeline in pipelineInstances)
            {
                pipeline.Unload();
            }
            spriteBatch.Dispose();
        });
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
            //绘制一次后变移除
            Active = false;
            locker.Release();
        }
    }
}
