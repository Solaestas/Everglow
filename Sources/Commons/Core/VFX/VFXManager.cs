using System.Collections;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Commons.Core.VFX.Base;
using Everglow.Sources.Commons.Core.VFX.Interfaces;
using Everglow.Sources.Commons.Function.ObjectPool;

namespace Everglow.Sources.Commons.Core.VFX;
[ProfilerMeasure]
public class VFXManager : IModule
{
    private class VisualCollection : IEnumerable<IVisual>
    {
        private const int FLUSH_COUNT = 50;
        private SortedSet<IVisual> visuals = new SortedSet<IVisual>(compare);
        /// <summary>
        /// 比较器
        /// </summary>
        private static VisualComparer compare = new VisualComparer();
        public IEnumerator<IVisual> GetEnumerator()
        {
            return visuals.GetEnumerator();
        }
        public void Add(IVisual visual)
        {
            if (visuals.Count % FLUSH_COUNT == 0)
            {
                Flush();
            }
            visuals.Add(visual);
        }
        public void Flush()
        {
            int b = visuals.RemoveWhere(visual => !visual.Active);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private class VisualComparer : Comparer<IVisual>
        {
            /// <summary>
            /// 按照Type从小到大排序，允许重复
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public override int Compare(IVisual x, IVisual y)
            {
                if(x == y)
                {
                    return 0;
                }
                var diff = x.Type - y.Type;
                return diff == 0 ? x.GetHashCode() - y.GetHashCode() : diff;
            }
        }
    }
    private class PipelineIndexComparer : Comparer<PipelineIndex>
    {
        /// <summary>
        /// 按照Depth从大到小排序，允许Depth相同，保证Rt2DVisual不会被添加到已经遍历过的Collection里面
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override int Compare(PipelineIndex x, PipelineIndex y)
        {
            if (x.Equals(y))
            {
                return 0;
            }
            int diff = y.GetDepth() - x.GetDepth();
            return diff == 0 ? x.GetHashCode() - y.GetHashCode() : diff;
        }
    }
    private PipelineIndexComparer pipelineIndexComparer = new PipelineIndexComparer();
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
    private Dictionary<CallOpportunity, SortedList<PipelineIndex, VisualCollection>> visuals =
        new Dictionary<CallOpportunity, SortedList<PipelineIndex, VisualCollection>>();
    /// <summary>
    /// 保存每一种Visual所需的Pipeline
    /// </summary>
    private List<PipelineIndex> requiredPipeline = new List<PipelineIndex>();
    /// <summary>
    /// 保存每一种Visual的Type
    /// </summary>
    private Dictionary<Type, int> visualTypes = new Dictionary<Type, int>()
    {
        [typeof(Rt2DVisual)] = int.MaxValue
    };
    /// <summary>
    /// RenderTarget池子
    /// </summary>
    public RenderTargetPool renderTargetPool = new RenderTargetPool();
    /// <summary>
    /// GraphicsDevice引用
    /// </summary>
    private GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
    /// <summary>
    /// 当前使用RenderTarget的Index
    /// </summary>
    private bool rt2DIndex = true;
    private const bool DefaultRt2DIndex = true;
    /// <summary>
    /// 当前的RenderTarget
    /// </summary>
    public RenderTarget2D CurrentRenderTarget => rt2DIndex ? Main.screenTarget : Main.screenTargetSwap;
    /// <summary>
    /// 下一个RenderTarget
    /// </summary>
    public RenderTarget2D NextRenderTarget => rt2DIndex ? Main.screenTargetSwap : Main.screenTarget;
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
            throw new Exception("No pipeline attribute");
        }
    }
    public void SwitchRenderTarget() => rt2DIndex = !rt2DIndex;
    public void Draw(CallOpportunity layer)
    {
        var visuals = this.visuals[layer];
        bool needToSwitch = false;
        foreach (var (pipelineIndex, innerVisuals) in visuals)
        {
            var visibles = innerVisuals.Where(v => v.Visible && v.Active);
            if (!visibles.Any())
            {
                continue;
            }

            if (pipelineIndex.next == null && needToSwitch)
            {
                graphicsDevice.SetRenderTarget(NextRenderTarget);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                Main.spriteBatch.Draw(CurrentRenderTarget, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
                SwitchRenderTarget();
                needToSwitch = false;
            }

            if (pipelineIndex.next != null && Main.targetSet)
            {
                var rt2D = new Rt2DVisual(renderTargetPool.GetRenderTarget2D());
                graphicsDevice.SetRenderTarget(rt2D.locker.Resource);
                graphicsDevice.Clear(Color.Transparent);
                visuals[pipelineIndex.next].Add(rt2D);
                needToSwitch = true;
            }


            var pipeline = pipelineInstances[pipelineIndex.index];
            pipeline.BeginRender();
            pipeline.Render(visibles);
            pipeline.EndRender();
        }

        if (CurrentRenderTarget != Main.screenTarget)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            graphicsDevice.SetRenderTarget(Main.screenTarget);
            Main.spriteBatch.Draw(CurrentRenderTarget, Vector2.Zero, Color.White);
            rt2DIndex = DefaultRt2DIndex;
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
            visuals[visual.DrawLayer] = value = new SortedList<PipelineIndex, VisualCollection>(pipelineIndexComparer);
        }
        PipelineIndex index = requiredPipeline[visual.Type];
        if (!value.TryGetValue(index, out var list))
        {
            value[index] = list = new VisualCollection();

            //将后续Index的List也初始化
            var next = index.next;
            while (next != null)
            {
                if (value.ContainsKey(next))
                {
                    break;
                }
                value[next] = new VisualCollection();
                next = next.next;
            }
        }

        list.Add(visual);
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
            List<PipelineIndex> waitToDelete = new List<PipelineIndex>();
            foreach (var (key, list) in visuals)
            {
                list.Flush();

                if (!list.Any())
                {
                    waitToDelete.Add(key);
                }
            }
            //删除没有元素的List
            foreach (var key in waitToDelete)
            {
                visuals.Remove(key);
            }

            //重新添加有Next的List
            List<PipelineIndex> waitToAdd = new List<PipelineIndex>();
            foreach (var (key, _) in visuals)
            {
                var next = key.next;
                while (next != null)
                {
                    waitToAdd.Add(next);
                    next = next.next;
                }
            }

            foreach (var key in waitToAdd)
            {
                if (!visuals.ContainsKey(key))
                {
                    visuals[key] = new VisualCollection();
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
    public static readonly CallOpportunity[] drawLayers = new CallOpportunity[]
    {
        CallOpportunity.PostDrawFilter,
        CallOpportunity.PostDrawProjectiles,
        CallOpportunity.PostDrawTiles,
        CallOpportunity.PostDrawDusts,
        CallOpportunity.PostDrawBG,
        CallOpportunity.PostDrawPlayers,
        CallOpportunity.PostDrawNPCs
    };

    public void Load()
    {
        Instance = this;
        spriteBatch = new VFXBatch(graphicsDevice);
        foreach (var layer in drawLayers)
        {
            visuals[layer] = new SortedList<PipelineIndex, VisualCollection>(pipelineIndexComparer);
            if (layer is CallOpportunity.PostDrawNPCs or CallOpportunity.PostDrawBG)
            {
                Everglow.HookSystem.AddMethod(() =>
                {
                    Main.spriteBatch.End();
                    Draw(layer);
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                        SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone,
                        null, Main.GameViewMatrix.TransformationMatrix);
                }, layer, $"VFX {layer}");
            }
            else
            {
                Everglow.HookSystem.AddMethod(() => Draw(layer), layer, $"VFX {layer}");
            }
        }
        Everglow.HookSystem.AddMethod(Update, CallOpportunity.PostUpdateEverything, "VFX Update");
    }
    public void Unload()
    {
        Everglow.MainThreadContext.AddTask(() =>
        {
            foreach (var pipeline in pipelineInstances)
            {
                pipeline.Unload();
            }
            spriteBatch?.Dispose();
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
            spriteBatch.BindTexture(locker.Resource).Draw(Vector2.Zero, Color.White);
            //虽然这个Rt2D会被延迟绘制，但是目前假设在Render到EndRender期间不会再次申请Rt2D，所以直接释放了
            locker.Release();
        }
    }
}
