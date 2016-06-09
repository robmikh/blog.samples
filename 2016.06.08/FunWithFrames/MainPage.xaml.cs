using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

namespace FunWithFrames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            InitComposition();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var visual = ElementCompositionPreview.GetElementChildVisual(Window.Current.Content);

            if (visual != null)
            {
                ElementCompositionPreview.SetElementChildVisual(Window.Current.Content, null);
                ElementCompositionPreview.SetElementChildVisual(this, visual);

                var compositor = visual.Compositor;
                var animation = compositor.CreateVector3KeyFrameAnimation();
                animation.InsertKeyFrame(0.0f, visual.Offset);
                animation.InsertKeyFrame(1.0f, new Vector3(50.0f, 50.0f, 0.0f));
                animation.Duration = TimeSpan.FromMilliseconds(1500);

                visual.StartAnimation(nameof(Visual.Offset), animation);
            }
        }

        private void InitComposition()
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            var visual = compositor.CreateSpriteVisual();
            visual.Size = new Vector2(75.0f, 75.0f);
            visual.Offset = new Vector3(50.0f, 50.0f, 0.0f);
            visual.CenterPoint = new Vector3(visual.Size / 2.0f, 0.0f);
            visual.Brush = compositor.CreateColorBrush(Windows.UI.Colors.Red);

            var easing = compositor.CreateLinearEasingFunction();
            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(0.0f, 0.0f);
            animation.InsertKeyFrame(1.0f, 360.0f, easing);
            animation.IterationBehavior = AnimationIterationBehavior.Forever;
            animation.Duration = TimeSpan.FromMilliseconds(3000);

            visual.StartAnimation(nameof(Visual.RotationAngleInDegrees), animation);

            ElementCompositionPreview.SetElementChildVisual(this, visual);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var visual = ElementCompositionPreview.GetElementChildVisual(this);
            ElementCompositionPreview.SetElementChildVisual(this, null);
            ElementCompositionPreview.SetElementChildVisual(Window.Current.Content, visual);

            Frame.Navigate(typeof(SecondaryPage));
        }
    }
}
