using Humanizer;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReLogic.Content;
using Terraria;

namespace Everglow.Sources.Commons.Core.UI.UIElements
{
    internal class VerticalScrollbar : BaseElement
    {
        private Texture2D uiScrollbarTexture;
        private UIImage inner;
        private float mouseY;
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
                if (inner != null)
                {
                    float height = Info.Size.Y - 26f;
                    inner.Info.Top.Pixel = height * wheelValue;
                }
            }
        }
        public float WhellValueMult = 1f;
        public VerticalScrollbar(float wheelValue = 0f)
        {
            Info.Width = new PositionStyle(20f, 0f);
            Info.Left = new PositionStyle(-20f, 1f);
            Info.Height = new PositionStyle(-20f, 1f);
            Info.Top = new PositionStyle(10f, 0f);
            Info.TopMargin.Pixel = 5f;
            Info.ButtomMargin.Pixel = 5f;
            Info.IsSensitive = true;
            uiScrollbarTexture = ModContent.Request<Texture2D>("Everglow/Sources/Commons/Core/UI/Images/VerticalScrollbar", AssetRequestMode.ImmediateLoad).Value;
            WheelValue = wheelValue;
        }
        public override void LoadEvents()
        {
            base.LoadEvents();
            Events.OnLeftDown += element =>
            {
                if (!isMouseDown)
                {
                    mouseY = Main.mouseY;
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
            inner = new UIImage(ModContent.Request<Texture2D>("Everglow/Sources/Commons/Core/UI/Images/VerticalScrollbar", AssetRequestMode.ImmediateLoad).Value, Color.White);
            inner.Info.Left.Pixel = -(inner.Info.Width.Pixel - Info.Width.Pixel) / 2f;
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
            float height = Info.Size.Y - 26f;
            if (!isMouseHover)
                whell = state.ScrollWheelValue;

            if (isMouseHover && whell != state.ScrollWheelValue)
            {
                inner.Info.Top.Pixel -= (float)(state.ScrollWheelValue - whell) / 30f * WhellValueMult;
                if (inner.Info.Top.Pixel > height)
                    inner.Info.Top.Pixel = height;
                else if (inner.Info.Top.Pixel < 0)
                    inner.Info.Top.Pixel = 0;
                whell = state.ScrollWheelValue;
                WheelValue = inner.Info.Top.Pixel / height;
                needCalculation = true;
            }
            if (isMouseDown && mouseY != Main.mouseY)
            {
                inner.Info.Top.Pixel += (float)Main.mouseY - (float)mouseY;
                if (inner.Info.Top.Pixel > height)
                    inner.Info.Top.Pixel = height;
                else if (inner.Info.Top.Pixel < 0)
                    inner.Info.Top.Pixel = 0;
                WheelValue = inner.Info.Top.Pixel / height;
                mouseY = Main.mouseY;
                needCalculation = true;
            }

            if (needCalculation)
                Calculation();
        }
        protected override void DrawSelf(SpriteBatch sb)
        {
            sb.Draw(uiScrollbarTexture, new Rectangle(Info.HitBox.X + (Info.HitBox.Width - uiScrollbarTexture.Width) / 2,
                Info.HitBox.Y - 12, uiScrollbarTexture.Width, 12),
                new Rectangle(0, 0, uiScrollbarTexture.Width, 12), Color.White * alpha);

            sb.Draw(uiScrollbarTexture, new Rectangle(Info.HitBox.X + (Info.HitBox.Width - uiScrollbarTexture.Width) / 2,
                Info.HitBox.Y, uiScrollbarTexture.Width, Info.HitBox.Height),
                new Rectangle(0, 12, uiScrollbarTexture.Width, uiScrollbarTexture.Height - 24), Color.White * alpha);

            sb.Draw(uiScrollbarTexture, new Rectangle(Info.HitBox.X + (Info.HitBox.Width - uiScrollbarTexture.Width) / 2,
                Info.HitBox.Y + Info.HitBox.Height, uiScrollbarTexture.Width, 12),
                new Rectangle(0, uiScrollbarTexture.Height - 12, uiScrollbarTexture.Width, 12), Color.White * alpha);
        }
    }
}
