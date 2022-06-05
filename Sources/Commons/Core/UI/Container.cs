using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.UI
{
    /// <summary>
    /// 容器类.
    /// <para>提供一组数据并令该容器可根据设备数据作出交互, 用于表示该对象在用户交互界面中的具象.</para>
    /// </summary>
    public class Container
    {
        /// <summary>
        /// 指示该容器是否可被 <seealso cref="SeekAt"/> 检索到.
        /// </summary>
        public bool CanSeek = true;

        public Container( )
        {
            Events = new ContainerEvents( this );
            ContainerElement = new ContainerElement( this );
            ContainerPointer = new ContainerPointer( this );
            ContainerItems = new List<Container>( );
        }

        /// <summary>
        /// 该容器所拥有的事件.
        /// </summary>
        public ContainerEvents Events { get; protected set; }

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
        /// 获取该容器的容器树..
        /// </summary>
        /// <returns>该容器的容器树.</returns>
        public List<Container> GetContainerTree( )
        {
            List<Container> result = new List<Container>( );
            result.Add( this );
            for ( int count = 0; count < ContainerItems.Count; count++ )
                result.Concat( ContainerItems[ count ].GetContainerTree( ) );
            return result;
        }

        /// <summary>
        /// 获取该容器的目前处于启用状态下的容器的树.
        /// </summary>
        /// <returns>该容器的已启用的容器的树.</returns>
        public List<Container> GetActiveContainerTree( )
        {
            List<Container> result = new List<Container>( );
            if ( UpdateEnable )
                result.Add( this );
            for ( int sub = 0; sub < ContainerItems.Count; sub++ )
                if ( ContainerItems[ sub ].UpdateEnable )
                    result.Concat( ContainerItems[ sub ].GetActiveContainerTree( ) );
            return result;
        }

        public ContainerPointer ContainerPointer;

        /// <summary>
        /// 将具有指定引用的容器注册进该容器的列表内.
        /// </summary>
        /// <param name="container">具有指定引用的容器.</param>
        public void Register( Container container )
        {
            container.ParentContainer = this;
            ContainerItems.Add( container );
        }

        public ContainerElement ContainerElement;

        public Vector2 Size => new Vector2( ContainerElement.Width, ContainerElement.Height );

        public Vector2 Location => new Vector2( ContainerElement.LocationX, ContainerElement.LocationY );

        /// <summary>
        /// 获取该容器的基础矩形.
        /// </summary>
        public Rectangle BaseRectangle => new Rectangle( (int)ContainerElement.LocationX, (int)ContainerElement.LocationY, (int)ContainerElement.Width, (int)ContainerElement.Height );

        /// <summary>
        /// 指示该容器是否启用剪裁功能.
        /// </summary>
        public bool EnableScissor = false;

        /// <summary>
        /// 指示该容器在启用剪裁功能的情况下是否也启用着色器应用功能.
        /// </summary>
        public bool EnableScissorShader = false;

        /// <summary>
        /// 指示该容器的矩形剪裁范围.
        /// </summary>
        public Rectangle ScissorRectangle;

        /// <summary>
        /// 表示该控件的移动方法.
        /// </summary>
        public IMoveContainerFunction MoveFunction { get; protected set; }

        /// <summary>
        /// 表示该控件的缩放方法.
        /// </summary>
        public IScaleContainerFunction ScaleFunction { get; protected set; }

        /// <summary>
        /// 返回该容器目前可最先交互的容器.
        /// </summary>
        /// <returns>如果寻找到非该容器之外的容器, 则返回寻找到的容器; 否则返回自己.</returns>
        public virtual Container SeekAt( )
        {
            Container target = null;
            for ( int sub = 0; sub < ContainerItems.Count; sub++ )
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
            if ( GetInterviewState( ) )
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
            if ( ScissorRectangle.Contains( new Point( Mouse.GetState( ).X, Mouse.GetState( ).Y ) ) && BaseRectangle.Contains( new Point( Mouse.GetState().X , Mouse.GetState().Y )) )
            {
                Main.LocalPlayer.mouseInterface = true;
                return true;
            }
            return false;
        }

        public void DoInitialize( )
        {
            InitializeContainer( );
            InitializeContainerItems( );
        }

        /// <summary>
        /// 执行于该容器进行初始化操作时.
        /// </summary>
        protected virtual void InitializeContainer( )
        {

        }

        /// <summary>
        /// 执行该容器的子容器的 <see cref="DoInitialize"/>, 于该容器本身的初始化执行的末尾执行.
        /// </summary>
        protected virtual void InitializeContainerItems( )
        {
            for ( int count = 0; count < ContainerItems.Count; count++ )
                ContainerItems[ count ].DoInitialize( );
        }

        bool _started = false;
        /// <summary>
        /// 性能优化可选项: 若设为<seealso href="true"/>, 该容器将执行 <seealso cref="DoUpdate"/>.
        /// <para>[!] 默认为 <seealso href="true"/> .</para>
        /// <para>[!] 于上级容器判断.</para>
        /// </summary>
        protected virtual bool UpdateEnable { get; } = true;

        /// <summary>
        /// 对每帧需要重置的数据进行重置.
        /// <para>[!] 该方法的执行同样受 <see cref="UpdateEnable"/> 控制.</para>
        /// </summary>
        public void DoReset( )
        {
            ResetUpdate( );
            for ( int count = 0; count < ContainerItems.Count; count++ )
                if ( ContainerItems[ count ].UpdateEnable )
                    ContainerItems[ count ].DoReset( );
        }

        /// <summary>
        /// 重写该方法以自定义对每帧需要重置的数据进行重置的操作.
        /// <para>[!] 它将先于 <see cref="DoUpdate"/> 执行.</para>
        /// <para>[!] 该方法的执行同样受 <see cref="UpdateEnable"/> 控制.</para>
        /// </summary>
        protected virtual void ResetUpdate( )
        {

        }

        /// <summary>
        /// 执行该容器的逻辑刷新.
        /// </summary>
        public void DoUpdate( )
        {
            if ( !_started )
            {
                _started = true;
                UpdateStart( );
            }
            if ( Events.Droping && ContainerSystem.LeftClickContainer == this )
                ContainerElement.SetLocation( new Vector2( Mouse.GetState( ).X, Mouse.GetState( ).Y ) - Events.SelectPoint );
            this?.UpdateSelf( );
            this?.UpdateContainerItems( );
            ScissorRectangle = BaseRectangle;
            if ( ParentContainer != null && ParentContainer.CanSeek )
                ScissorRectangle = ParentContainer.BaseRectangle;
            if ( ParentContainer != null && ParentContainer.CanSeek && ParentContainer.EnableScissor )
                EnableScissor = true;
            SetLayerout( ref ContainerElement );
            ContainerElement.UpdateElement( );
            MoveFunction?.UpdateLocation( ContainerElement );
            if ( MoveFunction != null )
                ContainerElement.SetLocation( Location.X + MoveFunction.VelocityX, Location.Y + MoveFunction.VelocityY );
            ScaleFunction?.UpdateScale( ContainerElement );
            this?.PostUpdate( );
        }
        /// <summary>
        /// 执行于该容器最开始进行更新的第一帧.
        /// </summary>
        protected virtual void UpdateStart( )
        {
        }
        /// <summary>
        /// 重写该函数以进行对容器元素主体的数值调整.
        /// </summary>
        protected virtual void SetLayerout( ref ContainerElement containerElement )
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
            for ( int count = 0; count < ContainerItems.Count; count++ )
                if ( ContainerItems[ count ].UpdateEnable )
                    ContainerItems[ count ].DoUpdate( );
        }
        /// <summary>
        /// 执行于该容器进行交互检测后.
        /// <para>于该容器调用.</para>
        /// </summary>
        protected virtual void PostUpdate( )
        {
        }

        /// <summary>
        /// 性能优化可选项: 若设为 <seealso href="true"/>, 该容器将执行 <seealso cref="DoDraw"/>.
        /// <para>[!] 默认为 <seealso href="true"/> .</para>
        /// <para>[!] 于上级容器判断.</para>
        /// </summary>
        protected virtual bool Visable { get; } = true;
        /// <summary>
        /// 执行该容器的纹理绘制.
        /// </summary>
        public void DoDraw( )
        {
            if( EnableScissor )
            {
                Main.spriteBatch.End( );
                RasterizerState OverflowHiddenRasterizerState = new RasterizerState
                {
                    CullMode = CullMode.None,
                    ScissorTestEnable = true
                };
                Main.spriteBatch.GraphicsDevice.ScissorRectangle = ScissorRectangle;
                Main.spriteBatch.GraphicsDevice.RasterizerState = OverflowHiddenRasterizerState;
                if( !EnableScissorShader )
                    Main.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null );
                else
                    Main.spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null );
            }
            this?.DrawSelf( );
            this?.DrawContainerItems( );
            this?.PostDraw( );
            if ( EnableScissor )
            {
                Main.spriteBatch.End( );
                Main.spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null );
            }
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
            for ( int count = ContainerItems.Count - 1; count >= 0; count-- )
                if ( ContainerItems[ count ].Visable )
                    ContainerItems[ count ].DoDraw( );
        }
        /// <summary>
        /// 绘制于该容器进行自身及其子容器的绘制后.
        /// </summary>
        protected virtual void PostDraw( )
        {

        }

    }
}