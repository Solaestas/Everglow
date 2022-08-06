﻿# 综述
VFX 即 Visual Effect（视觉特效），在当前的 VFX 系统中，VFX 的定义为即时绘制的带Shader的三角网格效果，我们可以规定这些三角网格效果应该经历哪些后处理过程。我们希望尽量将这些三角网格合批绘制以达到更好的性能。

# 组件介绍

## 接口
### IVisual 接口
用于提供所有类似 Visual 的实体接口，功能非常单一，包含每帧更新、绘制等方法。最重要的是 `IsActive` 属性。

### IVisualPipeline 接口
用于提供所有视觉效果实体的批量渲染流水线功能，主要是为了合批优化性能。同时，它的设计还提供了可复用性，同一种渲染方式的 VFX 可以共用一个 IVisualPipeline 实例。若要绑定一个 VFX 类型的 Pipeline，我们可以在 `VFXManager` 的构造函数里调用 `RegisterPipeline()` 来注册绑定信息。

## 管理器
### VFXManager
VFX 管理器用于管理 VFX 的产生、销毁、VFX关系的注册等。

### RenderTargetManager
RT 管理器用作 RT 的对象池，每次绘制前可以从该管理器获取一个或者多个与屏幕大小相等的 RT。一旦获取了 RT，在释放前你就拥有这个资源的绝对掌握权，但是使用结束时要记得释放，否则会造成生产过多的 RT 占用大量内存。

### VFXBatch
为了处理SpriteBatch的Draw与顶点绘制不好统一的问题，照着SpriteBatch的代码写了一个VFXBatch用来进行绘制

# 合批策略
一个VisualPipeline固定了当前绘制的BlendState，RasterizerState，Effect，同时可以决定每一个Visual绘制是否调用以及绘制调用前后的处理
目前暂时规定一个Visual绑定一个绘制层，每次Add一个Visual时会根据该Visual所需的Pipeline和Layer，插入到指定的红黑树中
需要多个Pipeline处理的Visual会先被第一个Pipeline处理后绘制到一个rt2D上，然后再由后面的Pipeline处理该rt2D，所以请勿多次进行顶点着色器的坐标系变换
缺点：额外处理过多导致性能较低，无法调整遮挡关系