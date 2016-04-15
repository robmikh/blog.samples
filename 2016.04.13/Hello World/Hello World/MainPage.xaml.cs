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

            // We need to make sure we execute this after
            // the button as loaded, as we consume the
            // button's size. If it isn't loaded it will
            // report a size of 0,0 back to us.
            AnimatingButton.Loaded += (s, a) =>
            {
                InitComposition();
            };
        }

        private void InitComposition()
        {
            // The first step is to get a Compositor object. After that we'll create a 
            // SpriteVisual. A SpriteVisual is important here because we want to assign
            // content to it (in the form of a Brush).
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var visual = compositor.CreateSpriteVisual();

            // We'll set the size of the visual to be 150x150. We'll also set the offset
            // to be 50,50. Note that the offset is always relative to your parent. Finally,
            // we'll set the brush of the visual to a solid color brush filled with Red.
            visual.Size = new Vector2(150, 150);
            visual.Offset = new Vector3(50, 50, 0);
            visual.Brush = compositor.CreateColorBrush(Colors.Red);

            // This method will attach our visual to the Page.
            ElementCompositionPreview.SetElementChildVisual(this, visual);

            // Create an animation and a linear easing function. We don't want it to ease into
            // the final value in this case, so that's why we're not using the default easing
            // function.
            var animation = compositor.CreateScalarKeyFrameAnimation();
            var easing = compositor.CreateLinearEasingFunction();

            // We'll start the animation at 0 degrees and end it at 360 degrees. In other words
            // we're going to have it spin around. One rotation will take 3 seconds, but it will
            // repeat the spin forever.
            animation.InsertKeyFrame(0.0f, 0.0f);
            animation.InsertKeyFrame(1.0f, 360.0f, easing);
            animation.Duration = TimeSpan.FromMilliseconds(3000);
            animation.IterationBehavior = AnimationIterationBehavior.Forever;

            // CenterPoint determines the point the visual will rotate around, and because we want
            // the visual to rotate in place we set it to the center of the visual. Next we attach
            // the animation we created to the RotationAngleInDegrees property of the visual. 
            // StartAnimation takes a string, but you can use the nameof operator here to make your
            // life much easier. Thanks to @NicoVermeir for the tip on that one!
            visual.CenterPoint = new Vector3(visual.Size.X / 2.0f, visual.Size.Y / 2.0f, 0);
            visual.StartAnimation(nameof(visual.RotationAngleInDegrees), animation);

            // Next, let's get the visual that represents our button.
            var buttonVisual = ElementCompositionPreview.GetElementVisual(AnimatingButton);

            // Set the center point so that our button spins in place.
            buttonVisual.CenterPoint = new Vector3((float)AnimatingButton.ActualWidth / 2.0f, (float)AnimatingButton.ActualHeight / 2.0f, 0.0f);

            // Start the animation using the same animation we used above, animations are completely
            // reusable!
            buttonVisual.StartAnimation(nameof(buttonVisual.RotationAngleInDegrees), animation);
        }
    }
}
