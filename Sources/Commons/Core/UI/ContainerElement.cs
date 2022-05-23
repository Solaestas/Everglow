using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.UI
{
    /// <summary>
    /// 存储容器主体属性的结构.
    /// </summary>
    public struct ContainerElement
    {
        Container Container;

        public float LocationX { get; private set; }

        public float LocationY { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        /// <summary>
        /// 容器相对于其父容器的百分比横坐标; 若该属性为 -1, 则选择使用 <seealso cref="LocationX"/>.
        /// <para>[!] 在 <seealso cref="LocationX"/> 具有值、且该属性不为 -1 的情况下, </para>
        /// <para>[!] 优先使用 <seealso cref="LocationX"/> 进行横坐标的设置.</para>
        /// </summary>
        public float PercentageX { get; private set; }

        /// <summary>
        /// 容器相对于其父容器的百分比纵坐标; 若该属性为 -1, 则选择使用 <seealso cref="LocationY"/>.
        /// <para>[!] 在 <seealso cref="LocationY"/> 具有值、且该属性不为 -1 的情况下, </para>
        /// <para>[!] 优先使用 <seealso cref="LocationY"/> 进行纵坐标的设置.</para>
        /// </summary>
        public float PercentageY { get; private set; }

        /// <summary>
        /// 容器相对于其父容器的百分比宽度; 若该属性为 -1, 则选择使用 <seealso cref="Width"/>.
        /// <para>[!] 在 <seealso cref="Width"/> 具有值、且该属性不为 -1 的情况下, </para>
        /// <para>[!] 优先使用 <seealso cref="Width"/> 进行宽度的设置.</para>
        /// </summary>
        public float PercentageWidth { get; private set; }

        /// <summary>
        /// 容器相对于其父容器的百分比高度; 若该属性为 -1, 则选择使用 <seealso cref="Height"/>.
        /// <para>[!] 在 <seealso cref="Height"/> 具有值、且该属性不为 -1 的情况下, </para>
        /// <para>[!] 优先使用 <seealso cref="Height"/> 进行高度的设置.</para>
        /// </summary>
        public float PercentageHeight { get; private set; }

        /// <summary>
        /// 外边框宽度.
        /// </summary>
        public float MarginWidth { get; private set; }

        /// <summary>
        /// 外边框高度.
        /// </summary>
        public float MarginHeight { get; private set; }

        /// <summary>
        /// 内边框宽度.
        /// </summary>
        public float PaddingWidth { get; private set; }

        /// <summary>
        /// 内边框高度.
        /// </summary>
        public float PaddingHeight { get; private set; }

        public ContainerElement( Container container )
        {
            Container = container;
            PaddingWidth = 0;
            PaddingHeight = 0;
            MarginWidth = 0;
            MarginHeight = 0;
            Width = 0;
            Height = 0;
            LocationX = 0;
            LocationY = 0;
            PercentageX = 0;
            PercentageY = 0;
            PercentageWidth = 0;
            PercentageHeight = 0;
        }

        public void Reset( )
        {
            PaddingWidth = 0;
            PaddingHeight = 0;
            MarginWidth = 0;
            MarginHeight = 0;
            Width = 0;
            Height = 0;
            LocationX = 0;
            LocationY = 0;
            PercentageX = -1;
            PercentageY = -1;
            PercentageWidth = -1;
            PercentageHeight = -1;
        }

        public void UpdateElement( )
        {
            if ( Container.ParentContainer == null )
                return;
            if ( LocationX == 0 && PercentageX != -1 )
                LocationX = PercentageX * Container.ParentContainer.ContainerElement.Width + Container.ParentContainer.ContainerElement.LocationX;
            if ( LocationY == 0 && PercentageY != -1 )
                LocationY = PercentageX * Container.ParentContainer.ContainerElement.Height + Container.ParentContainer.ContainerElement.LocationY;
            if ( Width == 0 && PercentageWidth != -1 )
                Width = PercentageWidth * Container.ParentContainer.ContainerElement.Width;
            if ( Height == 0 && PercentageHeight != -1 )
                Height = PercentageHeight * Container.ParentContainer.ContainerElement.Height;
        }

        /// <summary>
        /// 设置外边框.
        /// </summary>
        /// <param name="width">外边框宽度.</param>
        /// <param name="height">外边框高度.</param>
        public void SetMargin( float width, float height )
        {
            MarginWidth = width;
            MarginHeight = height;
        }

        /// <summary>
        /// 设置内边框.
        /// </summary>
        /// <param name="width">内边框宽度.</param>
        /// <param name="height">内边框高度.</param>
        public void SetPadding( float width, float height )
        {
            PaddingWidth = width;
            PaddingHeight = height;
        }

        /// <summary>
        /// 设置容器相对于父容器的位置.
        /// </summary>
        /// <param name="x">相对于父容器的横坐标.</param>
        /// <param name="y">相对于父容器的纵坐标.</param>
        public void SetLocation( float x, float y )
        {
            LocationX = x;
            LocationY = y;
        }

        /// <summary>
        /// 设置容器相对于父容器的位置.
        /// </summary>
        /// <param name="location">相对于父容器的坐标.</param>
        public void SetLocation( Vector2 location )
        {
            LocationX = location.X;
            LocationY = location.Y;
        }

        /// <summary>
        /// 设置大小.
        /// </summary>
        /// <param name="width">宽度.</param>
        /// <param name="height">高度.</param>
        public void SetSize( float width, float height )
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 设置百分比大小.
        /// </summary>
        /// <param name="width">相对于父容器的宽度.</param>
        /// <param name="height">相对于父容器的高度.</param>
        public void SetPercentageSize( float width, float height )
        {
            PercentageWidth = width;
            PercentageHeight = height;
        }

        /// <summary>
        /// 设置布局.
        /// </summary>
        /// <param name="x">相对于父容器的横坐标.</param>
        /// <param name="y">相对于父容器的纵坐标.</param>
        /// <param name="width">宽度.</param>
        /// <param name="height">高度.</param>
        public void SetLayerout( float x, float y, float width, float height )
        {
            LocationX = x;
            LocationY = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 设置百分比布局.
        /// </summary>
        /// <param name="x">相对于父容器的百分比横坐标.</param>
        /// <param name="y">相对于父容器的百分比纵坐标.</param>
        /// <param name="width">相对于父容器的百分比宽度.</param>
        /// <param name="height">相对于父容器的百分比高度.</param>
        public void SetPercentageLayerout( float x, float y, float width, float height )
        {
            PercentageX = x;
            PercentageY = y;
            PercentageWidth = width;
            PercentageHeight = height;
        }
    }
}