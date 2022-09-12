using Everglow.Sources.Commons.Core.UI.UIElements;

namespace Everglow.Sources.Modules.ExampleModule.UI
{
    internal class ExampleContainer : ContainerElement
    {
        public override void OnInitialization()
        {
            base.OnInitialization();
            Info.IsVisible = false;

            UIPanel panel = new UIPanel();
            panel.Info.Width.SetValue(0f, 0.4f);
            panel.Info.Height.SetValue(0f, 0.4f);
            panel.Info.Left.SetValue(0f, 0.3f);
            panel.Info.Top.SetValue(0f, 0.3f);
            Register(panel);

            UIContainerPanel containerPanel = new UIContainerPanel();
            containerPanel.Info.Width.SetValue(-20f, 1f);
            containerPanel.Info.Height.SetValue(-20f, 1f);
            panel.Register(containerPanel);

            HorizontalScrollbar horizontalScrollbar = new HorizontalScrollbar();
            panel.Register(horizontalScrollbar);
            containerPanel.SetHorizontalScrollbar(horizontalScrollbar);

            VerticalScrollbar verticalScrollbar = new VerticalScrollbar();
            panel.Register(verticalScrollbar);
            containerPanel.SetVerticalScrollbar(verticalScrollbar);

            UIImage image = new UIImage(ModContent.Request<Texture2D>("Everglow/icon", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, Color.White);
            image.Info.Width.SetValue(0f, 2f);
            image.Style = UIImage.CalculationStyle.LockAspectRatioMainWidth;
            containerPanel.AddElement(image);
        }
    }
}
