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
    /// 指示顶层容器的事件.
    /// <para>[!] 一般情况下, 该事件类由 <seealso cref="ContainerSystem"/> 调用.</para>
    /// </summary>
    public class ContainerEvents
    {
        /// <summary>
        /// 指示该容器是否启用拖动功能.
        /// </summary>
        public bool Drop = false;

        /// <summary>
        /// 若启用拖动功能, 记录当前容器是否正处于拖动状态.
        /// </summary>
        public bool Droping = false;

        /// <summary>
        /// 若启用拖动功能, 在第一帧按下时记录鼠标在容器内的位置.
        /// </summary>
        public Vector2 SelectPoint;

        /// <summary>
        /// 用于计算输入动作: 确定开始交互与结束交互的容器是否为同一个.
        /// </summary>
        private bool _clickRecordSeek = false;

        /// <summary>
        /// 用于计算输入动作: 确定开始交互与结束交互的容器是否为同一个.
        /// </summary>
        private bool _pressedRecordSeek = false;

        /// <summary>
        /// 指示该容器是否会被父容器的指针寻找到.
        /// </summary>
        public bool CanGetForPointer = false;

        public Container Container { get; private set; }

        public ContainerEvents( Container container )
        {
            Container = container;
            OnMouseLeftClick += MouseLeftClickEvent;
            OnMouseLeftDown += MouseLeftDownEvent;
            OnMouseLeftUp += MouseLeftUpEvent;
            OnMouseRightClick += MouseRightClickEvent;
            OnMouseRightDown += MouseRightDownEvent;
            OnMouseRightUp += MouseRightUpEvent;
            OnInterview += InterviewEvent;
            OnInterviewStateChange += InterviewStateChangeEvent;
        }

        /// <summary>
        /// 获取当前容器是否处于可交互状态的值.
        /// </summary>
        public bool Interview { get; private set; } = false;

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
        public void DoMouseLeftClickEvent( )
        {
            _clickRecordSeek = true;
            if ( Drop )
            {
                Droping = true;
                SelectPoint = new Vector2( Mouse.GetState( ).X, Mouse.GetState( ).Y ) - Container.Location;
            }
            OnMouseLeftClick.Invoke( );
        }
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
        public void DoMouseLeftDownEvent( )
        {
            _pressedRecordSeek = true;
            OnMouseLeftDown.Invoke( );
        }
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
        public void DoMouseLeftUpEvent( )
        {
            _clickRecordSeek = false;
            _pressedRecordSeek = false;
            OnMouseLeftUp.Invoke( );
        }
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
        public void DoMouseRightClickEvent( ) 
        {
            _clickRecordSeek = true;
            OnMouseRightClick.Invoke( ); 
        }
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
        public void DoMouseRightDownEvent( ) 
        {
            _pressedRecordSeek = true;
            OnMouseRightDown.Invoke( );
        }
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
        public void DoMouseRightUpEvent( )
        {
            _clickRecordSeek = false;
            _pressedRecordSeek = false;
            OnMouseRightUp.Invoke( );
        }
        /// <summary>
        /// 在容器于可交互状态下, 鼠标右键抬起时执行.
        /// </summary>
        protected virtual void MouseRightUpEvent( )
        {
        }

        public virtual void Update( )
        {
            if ( Main.mouseLeftRelease && Drop && Droping )
                Droping = false;
            Interview = Container.GetInterviewState( );
            if ( Interview )
                DoInterviewEvent( );
            if ( Input.MouseLeftClick )
                DoMouseLeftClickEvent( );
            else if ( Input.MouseLeftDown && _clickRecordSeek )
                DoMouseLeftDownEvent( );
            else if ( Input.MouseLeftUp && _pressedRecordSeek )
                DoMouseLeftUpEvent( );
            if ( Input.MouseRightClick )
                DoMouseRightClickEvent( );
            else if ( Input.MouseRightDown && _clickRecordSeek )
                DoMouseRightDownEvent( );
            else if ( Input.MouseRightUp && _pressedRecordSeek )
                DoMouseRightUpEvent( );

        }
    }
}