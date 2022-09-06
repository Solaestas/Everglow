using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ReLogic.Content;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

namespace Everglow.Sources.Commons.Core.UI.UIElements
{
    internal class HorizontalScrollbar : BaseElement
    {
        private Texture2D uiScrollbarTexture;
        private UIImage inner;
        private float mouseX;
        private float wheelValue;
        private int whell = 0;
        private bool isMouseDown = false;
        private float alpha = 0f;
        public float WheelValue
        {
            get { return wheelValue; }
            set
            {
                if (value <= 1f && value >= 0f)
                    wheelValue = value;
            }
        }
        public HorizontalScrollbar(float wheelValue = 0f)
        {
            Info.Height = new PositionStyle(20f, 0f);
            Info.Top = new PositionStyle(-20f, 1f);
            Info.Width = new PositionStyle(-20f, 1f);
            Info.Left = new PositionStyle(10f, 0f);
            Info.LeftMargin.Pixel = 5f;
            Info.RightMargin.Pixel = 5f;
            Info.IsSensitive = true;
            uiScrollbarTexture = ModContent.Request<Texture2D>("Everglow/Sources/Commons/Core/UI/Images/HorizontalScrollbar", AssetRequestMode.ImmediateLoad).Value;
            WheelValue = wheelValue;
        }
        public override void LoadEvents()
        {
            base.LoadEvents();
            Events.OnLeftDown += element =>
            {
                if (!isMouseDown)
                {
                    mouseX = Main.mouseX;
                    isMouseDown = true;
                }
            };
            Events.OnLeftUp += element =>
            {
                isMouseDown = false;
            };
        }
        public override void OnInitialization()
        {
            base.OnInitialization();
            inner = new UIImage(ModContent.Request<Texture2D>("Everglow/Sources/Commons/Core/UI/Images/HorizontalScrollbarInner", AssetRequestMode.ImmediateLoad).Value, Color.White);
            inner.Info.Top.Pixel = -(inner.Info.Height.Pixel - Info.Height.Pixel) / 2f;
            Register(inner);
        }
        public override void Update(GameTime gt)
        {
            base.Update(gt);
            if (ParentElement == null)
                return;

            bool needCalculation = false;

            bool isMouseHover = ParentElement.GetCanHitBox().Contains(Main.MouseScreen.ToPoint());
            if ((isMouseHover || isMouseDown) && alpha < 1f)
                alpha += 0.01f;
            if ((!(isMouseHover || isMouseDown)) && alpha > 0f)
                alpha -= 0.01f;

            inner.ChangeColor(Color.White * alpha);

            MouseState state = Mouse.GetState();
            float width = Info.Size.X - 26f;
            if (!isMouseHover)
                whell = state.ScrollWheelValue;

            //if (isMouseHover && whell != state.ScrollWheelValue)
            //{
            //    inner.Info.Left.Pixel -= (float)(state.ScrollWheelValue - whell) / 10f;
            //    if (inner.Info.Left.Pixel > width)
            //        inner.Info.Left.Pixel = width;
            //    else if (inner.Info.Left.Pixel < 0)
            //        inner.Info.Top.Pixel = 0;
            //    whell = state.ScrollWheelValue;
            //    WheelValue = inner.Info.Left.Pixel / width;
            //    needCalculation = true;
            //}
            if (isMouseDown && mouseX != Main.mouseX)
            {
                inner.Info.Left.Pixel += (float)Main.mouseX - (float)mouseX;
                if (inner.Info.Left.Pixel > width)
                    inner.Info.Left.Pixel = width;
                else if (inner.Info.Left.Pixel < 0)
                    inner.Info.Left.Pixel = 0;
                WheelValue = inner.Info.Left.Pixel / width;
                mouseX = Main.mouseX;
                needCalculation = true;
            }

            if (needCalculation)
                Calculation();
        }
        protected override void DrawSelf(SpriteBatch sb)
        {
            sb.Draw(uiScrollbarTexture, new Rectangle(Info.HitBox.X - 12,
                Info.HitBox.Y + (Info.HitBox.Height - uiScrollbarTexture.Height) / 2, 12, uiScrollbarTexture.Height),
                new Rectangle(0, 0, 12, uiScrollbarTexture.Height), Color.White * alpha);

            sb.Draw(uiScrollbarTexture, new Rectangle(Info.HitBox.X,
                Info.HitBox.Y + (Info.HitBox.Height - uiScrollbarTexture.Height) / 2, Info.HitBox.Width, uiScrollbarTexture.Height),
                new Rectangle(12, 0, uiScrollbarTexture.Width - 24, uiScrollbarTexture.Height), Color.White * alpha);

            sb.Draw(uiScrollbarTexture, new Rectangle(Info.HitBox.X + Info.HitBox.Width,
                Info.HitBox.Y + (Info.HitBox.Height - uiScrollbarTexture.Height) / 2, 12, uiScrollbarTexture.Height),
                new Rectangle(uiScrollbarTexture.Width - 12, 0, 12, uiScrollbarTexture.Height), Color.White * alpha);
        }
    }
}
