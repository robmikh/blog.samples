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

            // Attempt to get our visual from our window content.
            var visual = 
                ElementCompositionPreview.GetElementChildVisual(Window.Current.Content);

            // OnNavigatedTo will be called every time we navigate to MainPage,
            // including the first time. Because of this, we aren't guaranteed to get 
            // a visual back from the call above. Make sure it's not null first.
            if (visual != null)
            {
                // Remove our visual from the window content and re-parent it to our
                // page.
                ElementCompositionPreview.SetElementChildVisual(Window.Current.Content, null);
                ElementCompositionPreview.SetElementChildVisual(this, visual);

                // Create an animation to animate the visual back to where it should
                // be on this page.
                var compositor = visual.Compositor;
                var animation = compositor.CreateVector3KeyFrameAnimation();
                animation.InsertKeyFrame(0.0f, visual.Offset);
                animation.InsertKeyFrame(1.0f, new Vector3(50.0f, 50.0f, 0.0f));
                animation.Duration = TimeSpan.FromMilliseconds(1500);

                // Start the animation.
                visual.StartAnimation(nameof(Visual.Offset), animation);
            }
        }

        private void InitComposition()
        {
            // Get our Compositor object.
            var compositor = 
                ElementCompositionPreview.GetElementVisual(this).Compositor;

            // Create a small visual that has a red CompositionColorBrush as its
            // content. Also set its offset and centerpoint for when we animate 
            // the rotation.
            var visual = compositor.CreateSpriteVisual();
            visual.Size = new Vector2(75.0f, 75.0f);
            visual.Offset = new Vector3(50.0f, 50.0f, 0.0f);
            visual.CenterPoint = new Vector3(visual.Size / 2.0f, 0.0f);
            visual.Brush = compositor.CreateColorBrush(Windows.UI.Colors.Red);

            // Use a linear easing function instead of the default.
            var easing = compositor.CreateLinearEasingFunction();
            // Animate our square to go in circles around its center forever.
            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(0.0f, 0.0f);
            animation.InsertKeyFrame(1.0f, 360.0f, easing);
            animation.IterationBehavior = AnimationIterationBehavior.Forever;
            animation.Duration = TimeSpan.FromMilliseconds(3000);

            // Animate our rotation.
            visual.StartAnimation(nameof(Visual.RotationAngleInDegrees), animation);

            // Attach our visual into the visual tree.
            ElementCompositionPreview.SetElementChildVisual(this, visual);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Remove the visual from the page and place it on the window content so that
            // it survives the page transition.
            var visual = ElementCompositionPreview.GetElementChildVisual(this);
            ElementCompositionPreview.SetElementChildVisual(this, null);
            ElementCompositionPreview.SetElementChildVisual(
                Window.Current.Content, 
                visual);

            // Navigate to the next page.
            Frame.Navigate(typeof(SecondaryPage));
        }
    }
}
