using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hello_World
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            AnimatingButton.Loaded += (s, a) =>
            {
                InitComposition();
            };
        }

        private void InitComposition()
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var visual = compositor.CreateSpriteVisual();

            visual.Size = new Vector2(150, 150);
            visual.Offset = new Vector3(50, 50, 0);
            visual.Brush = compositor.CreateColorBrush(Colors.Red);

            ElementCompositionPreview.SetElementChildVisual(this, visual);

            var animation = compositor.CreateScalarKeyFrameAnimation();
            var easing = compositor.CreateLinearEasingFunction();

            animation.InsertKeyFrame(0.0f, 0.0f);
            animation.InsertKeyFrame(1.0f, 360.0f, easing);
            animation.Duration = TimeSpan.FromMilliseconds(3000);
            animation.IterationBehavior = AnimationIterationBehavior.Forever;

            visual.CenterPoint = new Vector3(visual.Size.X / 2.0f, visual.Size.Y / 2.0f, 0);
            visual.StartAnimation("RotationAngleInDegrees", animation);

            var buttonVisual = ElementCompositionPreview.GetElementVisual(AnimatingButton);

            buttonVisual.CenterPoint = new Vector3((float)AnimatingButton.ActualWidth / 2.0f, (float)AnimatingButton.ActualHeight / 2.0f, 0.0f);

            buttonVisual.StartAnimation("RotationAngleInDegrees", animation);
        }
    }
}
