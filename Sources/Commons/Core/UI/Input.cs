using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Everglow.Sources.Commons.Core.UI
{
    /// <summary>
    /// 可从中获取输入信息的 <seealso cref="Input"/> 类.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// 键盘上的键位数字数组.
        /// </summary>
        public static Keys[ ] Digits = new Keys[ ]
        {
            Keys.D1,
            Keys.D2,
            Keys.D3,
            Keys.D4,
            Keys.D5,
            Keys.D6,
            Keys.D7,
            Keys.D8,
            Keys.D9,
            Keys.D0,
        };

        /// <summary>
        /// 由 <seealso cref="Mouse.GetState( )"/> 获取本帧的鼠标状态.
        /// </summary>
        public static MouseState MouseState { get; private set; }

        /// <summary>
        /// 由 <seealso cref="MouseState"/> 获取上一帧的 <seealso cref="Mouse.GetState( )"/>.
        /// </summary>
        public static MouseState MouseStateLast { get; private set; } = new MouseState( );

        /// <summary>
        /// 获取鼠标在屏幕上的位置.
        /// </summary>
        public static Vector2 MousePosition
        { get { return new Vector2( Mouse.GetState( ).X, Mouse.GetState( ).Y ); } }

        /// <summary>
        /// 获取鼠标是否进行了左键单击操作的值.
        /// </summary>
        public static bool MouseLeftClick
        { get { return MouseState.LeftButton == ButtonState.Pressed && MouseStateLast.LeftButton == ButtonState.Released; } }

        /// <summary>
        /// 获取鼠标是否进行了左键长按操作的值.
        /// </summary>
        public static bool MouseLeftDown
        { get { return MouseState.LeftButton == ButtonState.Pressed && MouseStateLast.LeftButton == ButtonState.Pressed; } }

        /// <summary>
        /// 获取鼠标是否进行了释放左键操作的值.
        /// </summary>
        public static bool MouseLeftUp
        { get { return MouseState.LeftButton == ButtonState.Released && MouseStateLast.LeftButton == ButtonState.Pressed; } }

        /// <summary>
        /// 表示鼠标是否进行了右键单击操作的值.
        /// </summary>
        public static bool MouseRightClick
        { get { return MouseState.RightButton == ButtonState.Pressed && MouseStateLast.RightButton == ButtonState.Released; } }

        /// <summary>
        /// 获取鼠标是否进行了右键长按操作的值.
        /// </summary>
        public static bool MouseRightDown
        { get { return MouseState.RightButton == ButtonState.Pressed && MouseStateLast.RightButton == ButtonState.Pressed; } }

        /// <summary>
        /// 表示鼠标是否进行了释放右键操作的值.
        /// </summary>
        public static bool MouseRightUp
        { get { return MouseState.RightButton == ButtonState.Released && MouseStateLast.RightButton == ButtonState.Pressed; } }

        /// <summary>
        /// 表示鼠标是否进行了双键释放操作的值.
        /// </summary>
        public static bool MouseReleased
        {
            get
            {
                return
                    MouseState.LeftButton == ButtonState.Released &&
                    MouseStateLast.LeftButton == ButtonState.Released &&
                    MouseState.RightButton == ButtonState.Released &&
                    MouseStateLast.RightButton == ButtonState.Released;
            }
        }

        /// <summary>
        /// 表示鼠标滑轮进行了向下滑操作的值.
        /// </summary>
        public static bool SlideDown
        { get { return MouseState.ScrollWheelValue < MouseStateLast.ScrollWheelValue; } }

        /// <summary>
        /// 表示鼠标滑轮进行了上滑操作的值.
        /// </summary>
        public static bool SlideUp
        { get { return MouseState.ScrollWheelValue > MouseStateLast.ScrollWheelValue; } }

        /// <summary>
        /// 表示鼠标进行了左键双击操作的值.
        /// </summary>
        public static bool MouseDoubleLeftClick { get; set; } = false;

        // 用于计算左键双击的计时器.
        private static int _doubleLeftClickTimer = 30;

        /// <summary>
        /// 由 <seealso cref="Keyboard.GetState( )"/> 获取本帧的键盘状态.
        /// </summary>
        public static KeyboardState KeyboardState = new KeyboardState( );

        /// <summary>
        /// 由 <seealso cref="KeyboardState"/> 获取上一帧的 <seealso cref="Keyboard.GetState( )"/>.
        /// </summary>
        public static KeyboardState KeyboardStateLast = new KeyboardState( );

        internal static void GetInformationFromDevice( )
        {
            MouseStateLast = MouseState;
            MouseState = Mouse.GetState( );
            KeyboardStateLast = KeyboardState;
            KeyboardState = Keyboard.GetState( );
            if ( MouseLeftUp && _doubleLeftClickTimer <= 0 && !MouseDoubleLeftClick )
            {
                _doubleLeftClickTimer = 30;
            }
            else if ( MouseLeftUp && _doubleLeftClickTimer > 0 )
            {
                MouseDoubleLeftClick = true;
            }
            else if ( _doubleLeftClickTimer <= 0 )
                MouseDoubleLeftClick = false;
            if ( _doubleLeftClickTimer > 0 )
                _doubleLeftClickTimer--;
        }

        /// <summary>
        /// 将上一帧与这一帧的鼠标状态相统一.
        /// </summary>
        public static void ResetMouseState( )
        {
            MouseStateLast = Mouse.GetState( );
            MouseState = Mouse.GetState( );
        }

        /// <summary>
        /// 判断键盘上的某个键位是否被单击.
        /// </summary>
        /// <param name="keys">键.</param>
        /// <returns>如若是, 返回 <seealso href="true"/>, 否则返回 <seealso href="false"/>.</returns>
        public static bool KeyClick( Keys keys )
        {
            return KeyboardState.IsKeyDown( keys ) && KeyboardStateLast.IsKeyUp( keys );
        }

        /// <summary>
        /// 判断键盘上的某个键位是否被按下后松开.
        /// </summary>
        /// <param name="keys">键.</param>
        /// <returns>如若是, 返回 <seealso href="true"/>, 否则返回 <seealso href="false"/>.</returns>
        public static bool KeyUp( Keys keys )
        {
            return KeyboardState.IsKeyUp( keys ) && KeyboardStateLast.IsKeyDown( keys );
        }

    }
}