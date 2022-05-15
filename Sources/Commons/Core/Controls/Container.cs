using Everglow.Sources.Commons.Core.Controls.Flex;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Controls
{
    /// <summary>
    /// 容器类.
    /// <para>提供一组数据并令该容器可根据设备数据作出交互, 用于表示该对象在用户交互界面中的具象.</para>
    /// </summary>
    public abstract class Container : ILayout
    {
        /// <summary>
        /// 容器是否启用弹性布局?
        /// <para>若启用, 初始化时设置的子容器位置将失效, 子容器的位置将会由弹性布局样式自动决定.</para>
        /// </summary>
        public bool Flex = false;

        /// <summary>
        /// 弹性布局: 主轴方向.
        /// </summary>
        protected FlexDirection FlexDirection = FlexDirection.Row;

        /// <summary>
        /// 弹性布局: 换行方式.
        /// </summary>
        protected FlexWrap FlexWrap = FlexWrap.Nowrap;

        /// <summary>
        /// 弹性布局: 定义子容器在该容器主轴上的对齐方式.
        /// </summary>
        protected JustifyContent JustifyContent = JustifyContent.Center;

        /// <summary>
        /// 该容器位于其上级容器列表内的索引.
        /// </summary>
        public int OperatorIndex { get; internal set; }

        /// <summary>
        /// 该容器所属的上级容器.
        /// </summary>
        public Container ParentContainer { get; internal set; }

        /// <summary>
        /// 该容器所拥有的子容器项目.
        /// </summary>
        public List<Container> ContainerItems { get; private set; }

        /// <summary>
        /// 将具有指定引用的容器注册进该容器的列表内.
        /// </summary>
        /// <param name="container">具有指定引用的容器.</param>
        public void Register( Container container )
        {
            container.ParentContainer = this;
            ContainerItems.Add( container );
        }

        public float Width { get; set; }

        public float Height { get; set; }

        public Vector2 Size => new Vector2( Width, Height );

        public float LocationX { get; set; }

        public float LocationY { get; set; }

        public Vector2 Location => new Vector2( LocationX, LocationY );

        /// <summary>
        /// 获取该容器的基础矩形.
        /// </summary>
        public Rectangle BaseRectangle => new Rectangle( (int)LocationX, (int)LocationY, (int)Width, (int)Height );

        /// <summary>
        /// 启用该容器.
        /// </summary>
        public void Active( )
        {
            UpdateEnable = true;
            Visable = true;
            foreach ( Container item in ContainerItems )
            {
                item.Active( );
            }
        }

        /// <summary>
        /// 令该容器休眠.
        /// </summary>
        public void Dormancy( )
        {
            UpdateEnable = false;
            Visable = false;
            foreach ( Container item in ContainerItems )
                item.Dormancy( );
        }

        /// <summary>
        /// 返回该容器目前可最先交互的容器.
        /// </summary>
        /// <returns>如果寻找到非该容器之外的容器, 则返回寻找到的容器; 否则返回自己.</returns>
        public virtual Container SeekAt( )
        {
            Container target = null;
            for ( int sub = ContainerItems.Count - 1; sub >= 0; sub-- )
            {
                if ( ContainerItems[ sub ].SeekAt( ) == null )
                {
                    target = null;
                }
                else if ( ContainerItems[ sub ].SeekAt( ) != null )
                {
                    target = ContainerItems[ sub ].SeekAt( );
                    return target;
                }
            }
            if ( Interview )
            {
                return this;
            }
            return target;
        }

        /// <summary>
        /// 获取当前该容器是否允许进行交互的值.
        /// </summary>
        /// <returns>若是, 返回 <seealso href="true"/> , 否则返回 <seealso href="false"/>.</returns>
        public virtual bool GetInterviewState( )
        {
            if ( BaseRectangle.Contains( Main.MouseScreen.ToPoint( ) ) )
                return true;
            return false;
        }

        /// <summary>
        /// 发生容器的可交互状态发生改变时.
        /// </summary>
        public event Action OnInterviewStateChange;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器可交互状态更改时.
        /// </summary>
        public void DoInterviewStateChangeEvent( ) => OnInterviewStateChange.Invoke( );
        /// <summary>
        /// 在容器的可交互状态发生改变时执行.
        /// </summary>
        protected virtual void InterviewStateChangeEvent( )
        {
        }

        /// <summary>
        /// 发生在容器于可交互状态下, 鼠标左键单击时.
        /// </summary>
        public event Action OnMouseLeftClick;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器处于可交互状态下时, 鼠标左键单击时.
        /// </summary>
        public void DoMouseLeftClickEvet( ) => OnMouseLeftClick.Invoke( );
        /// <summary>
        /// 在容器于可交互状态下, 鼠标左键单击时执行.
        /// </summary>
        protected virtual void MouseLeftClickEvent( )
        {
        }

        /// <summary>
        /// 发生在容器于可交互状态下, 鼠标左键长按时.
        /// </summary>
        public event Action OnMouseLeftDown;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器处于可交互状态下时, 鼠标左键长按时.
        /// </summary>
        public void DoMouseLeftDownEvent( ) => OnMouseLeftDown.Invoke( );
        /// <summary>
        /// 在容器于可交互状态下, 鼠标左键长按时执行.
        /// </summary>
        protected virtual void MouseLeftDownEvent( )
        {
        }

        /// <summary>
        /// 发生在容器于可交互状态下, 鼠标左键抬起时.
        /// </summary>
        public event Action OnMouseLeftUp;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器处于可交互状态下时, 鼠标左键抬起时.
        /// </summary>
        public void DoMouseLeftUpEvent( ) => OnMouseLeftUp.Invoke( );
        /// <summary>
        /// 在容器于可交互状态下, 鼠标左键抬起时执行.
        /// </summary>
        protected virtual void MouseLeftUpEvent( )
        {
        }

        /// <summary>
        /// 发生在容器处于可交互状态下时.
        /// </summary>
        public event Action OnInterview;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器处于可交互状态下时.
        /// </summary>
        public void DoInterviewEvent( ) => OnInterview.Invoke( );
        /// <summary>
        /// 在容器处于可交互状态下时执行.
        /// </summary>
        protected virtual void InterviewEvent( )
        {
        }

        /// <summary>
        /// 发生在容器于可交互状态下, 鼠标右键单击时.
        /// </summary>
        public event Action OnMouseRightClick;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器处于可交互状态下时, 鼠标右键单击时.
        /// </summary>
        public void DoMouseRightClickEvent( ) => OnMouseRightClick.Invoke( );
        /// <summary>
        /// 在容器于可交互状态下, 鼠标右键单击时执行.
        /// </summary>
        protected virtual void MouseRightClickEvent( )
        {
        }

        /// <summary>
        /// 发生在容器于可交互状态下, 鼠标右键长按时.
        /// </summary>
        public event Action OnMouseRightDown;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器处于可交互状态下时, 鼠标右键长按时.
        /// </summary>
        public void DoMouseRightDownEvent( ) => OnMouseRightDown.Invoke( );
        /// <summary>
        /// 在容器于可交互状态下, 鼠标右键长按时执行.
        /// </summary>
        protected virtual void MouseRightDownEvent( )
        {
        }

        /// <summary>
        /// 发生在容器于可交互状态下, 鼠标右键抬起时.
        /// </summary>
        public event Action OnMouseRightUp;
        /// <summary>
        /// 由执行器调用: 执行自定义的操作于当前容器处于可交互状态下时, 鼠标右键抬起时.
        /// </summary>
        public void DoMouseRightUpEvent( ) => OnMouseRightUp.Invoke( );
        /// <summary>
        /// 在容器于可交互状态下, 鼠标右键抬起时执行.
        /// </summary>
        protected virtual void MouseRightUpEvent( )
        {
        }

        /// <summary>
        /// 执行该容器的初始化操作.
        /// </summary>
        public void DoIniti( )
        {
            OnMouseLeftClick += MouseLeftClickEvent;
            OnMouseLeftDown += MouseLeftDownEvent;
            OnMouseLeftUp += MouseLeftUpEvent;
            OnMouseRightClick += MouseRightClickEvent;
            OnMouseRightDown += MouseRightDownEvent;
            OnMouseRightUp += MouseRightUpEvent;
            OnInterview += InterviewEvent;
            OnInterviewStateChange += InterviewStateChangeEvent;
            InitializeContainer( );
            InitializeContainerItems( );
        }
        /// <summary>
        /// 执行该容器的子容器的 <see cref="DoIniti"/>, 于该容器本身的初始化执行的末尾执行.
        /// </summary>
        protected virtual void InitializeContainerItems()
        {
            for ( int count = 0; count < ContainerItems.Count ; count++ )
                ContainerItems[ count ].DoIniti( );
        }

        /// <summary>
        /// 执行于该容器进行初始化操作时.
        /// </summary>
        protected virtual void InitializeContainer( )
        {

        }

        /// <summary>
        /// 获取当前容器是否处于可交互状态的值.
        /// </summary>
        public bool Interview { get; private set; } = false;

        /// <summary>
        /// 获取当前容器在上一帧是否处于可交互状态的值.
        /// </summary>
        public bool InterviewLast { get; private set; } = false;

        bool _started = false;
        /// <summary>
        /// 性能优化可选项: 若设为<seealso href="true"/>, 该容器将执行 <seealso cref="DoUpdate"/>.
        /// <para>[!] 默认为 <seealso href="true"/> .</para>
        /// <para>[!] 于上级容器判断.</para>
        /// </summary>
        protected bool UpdateEnable { get; private set; } = true;
        /// <summary>
        /// 执行该容器的逻辑刷新.
        /// </summary>
        /// <param name="gameTime"></param>
        public void DoUpdate( )
        {
            if ( !_started )
            {
                _started = true;
                UpdateStart( );
            }
            this?.UpdateSelf( );
            InterviewLast = Interview;
            Interview = GetInterviewState( );
            this?.UpdateContainerItems( );
            if ( SeekAt( ).Interview )
                SeekAt( ).DoInterviewEvent( );
            if ( SeekAt( ).Interview != SeekAt( ).InterviewLast )
                SeekAt( ).DoInterviewStateChangeEvent( );
            if( Main.mouseLeft && Main.mouseLeftRelease )
                SeekAt( ).DoMouseLeftClickEvet( );
            if( Main.mouseLeft && !Main.mouseLeftRelease )
                SeekAt( ).DoMouseLeftDownEvent( );
            if ( !Main.mouseLeft && Main.mouseLeftRelease )
                SeekAt( ).DoMouseLeftUpEvent( );
            if ( Main.mouseRight && Main.mouseRightRelease )
                SeekAt( ).DoMouseRightClickEvent( );
            if ( Main.mouseRight && !Main.mouseRightRelease )
                SeekAt( ).DoMouseRightDownEvent( );
            if ( !Main.mouseRight && Main.mouseRightRelease )
                SeekAt( ).DoMouseRightUpEvent( );
            this?.PostUpdate( );
        }
        /// <summary>
        /// 执行于该容器最开始进行更新的第一帧.
        /// </summary>
        protected virtual void UpdateStart( )
        {

        }
        /// <summary>
        /// 执行于该容器进行交互检测前.
        /// <para>于该容器调用.</para>
        /// </summary>
        protected virtual void UpdateSelf( )
        {

        }
        /// <summary>
        /// 执行该容器的容器项目的  <seealso cref="DoUpdate"/>, 于 <seealso cref="UpdateSelf"/> 后调用.
        /// </summary>
        protected virtual void UpdateContainerItems( )
        {
            for ( int count = 0; count < ContainerItems.Count ; count++ )
                if ( ContainerItems[ count ].UpdateEnable )
                    ContainerItems[ count ].DoUpdate( );
        }
        /// <summary>
        /// 执行于该容器进行交互检测后.
        /// <para>于该容器调用.</para>
        /// </summary>
        public virtual void PostUpdate( )
        {

        }

        /// <summary>
        /// 性能优化可选项: 若设为 <seealso href="true"/>, 该容器将执行 <seealso cref="DoDraw"/>.
        /// <para>[!] 默认为 <seealso href="true"/> .</para>
        /// <para>[!] 于上级容器判断.</para>
        /// </summary>
        protected bool Visable { get; set; } = true;
        /// <summary>
        /// 执行该容器的纹理绘制.
        /// </summary>
        /// <param name="layerDepth">纹理合批优化参数.</param>
        public void DoDraw( )
        {
            this?.DrawSelf(  );
            this?.DrawContainerItems( );
            this?.PostDraw( );
        }
        /// <summary>
        /// 绘制于该容器的子容器绘制前.
        /// <para>于该容器调用.</para>
        /// </summary>
        protected virtual void DrawSelf( )
        {

        }
        /// <summary>
        /// 执行该容器的容器项目的  <seealso cref="DoDraw"/>, 于 <seealso cref="DrawSelf"/> 后调用.
        /// </summary>
        protected virtual void DrawContainerItems( )
        {
            for ( int count = 0; count < ContainerItems.Count; count++ )
                if ( ContainerItems[ count ].Visable )
                    ContainerItems[ count ].DoDraw( );
        }
        /// <summary>
        /// 绘制于该容器进行自身及其子容器的绘制后.
        /// </summary>
        public virtual void PostDraw( )
        {

        }

    }
}