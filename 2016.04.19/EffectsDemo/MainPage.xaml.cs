using Microsoft.Graphics.Canvas.Effects;
using Robmikh.Util.CompositionImageLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Effects;
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

namespace EffectsDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // It's a good practice to keep these in a 
        // place where it is easy to re-use them. ImageLoader
        // is a relatively heavy object if you make too
        // many of them.
        private Compositor _compositor;
        private IImageLoader _imageLoader;
        private CompositionEffectFactory _effectFactory;

        public MainPage()
        {
            this.InitializeComponent();

            InitComposition();
        }

        private async void InitComposition()
        {
            // Store our Compositor and create our ImageLoader.
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _imageLoader = ImageLoaderFactory.CreateImageLoader(_compositor);

            // Setup our effect definition. First is the CompositeEffect that will take
            // our sources and produce the intersection of the images (because we selected
            // the DestinationIn mode for the effect). Next we take our CompositeEffect 
            // and make it the source of our next effect, the InvertEffect. This will take 
            // the intersection image and invert the colors. Finally we take that combined 
            // effect and put it through a HueRotationEffect, were we can adjust the colors
            // using the Angle property (which we will animate below). 
            IGraphicsEffect graphicsEffect = new HueRotationEffect
            {
                Name = "hueEffect",
                Angle = 0.0f,
                Source = new InvertEffect
                {
                    Source = new CompositeEffect
                    {
                        Mode = Microsoft.Graphics.Canvas.CanvasComposite.DestinationIn,
                        Sources =
                        {
                            new CompositionEffectSourceParameter("image"),
                            new CompositionEffectSourceParameter("mask")
                        }
                    }
                }
            };
            // Create our effect factory using the effect definition and mark the Angle
            // property as adjustable/animatable.
            _effectFactory = _compositor.CreateEffectFactory(graphicsEffect, new string[] { "hueEffect.Angle" });

            // Create MangedSurfaces for both our base image and the mask we'll be using. 
            // The mask is a transparent image with a white circle in the middle. This is 
            // important since the CompositeEffect will use just the circle for the 
            // intersectionsince the rest is transparent.
            var managedImageSurface = await _imageLoader.CreateManagedSurfaceFromUriAsync(new Uri("ms-appx:///Assets/tripphoto1.jpg"));
            var managedMaskSurface = await _imageLoader.CreateManagedSurfaceFromUriAsync(new Uri("ms-appx:///Assets/CircleMask.png"));

            // Create brushes from our surfaces.
            var imageBrush = _compositor.CreateSurfaceBrush(managedImageSurface.Surface);
            var maskBrush = _compositor.CreateSurfaceBrush(managedMaskSurface.Surface);

            // Create an setup our effect brush.Assign both the base image and mask image
            // brushes as source parameters in the effect (with the same names we used in
            // the effect definition). If we wanted, we could create many effect brushes
            // and use different images in all of them.
            var effectBrush = _effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("image", imageBrush);
            effectBrush.SetSourceParameter("mask", maskBrush);

            // All that's left is to create a visual, assign the effect brush to the Brush
            // property, and attach it into the tree...
            var visual = _compositor.CreateSpriteVisual();

            visual.Size = managedImageSurface.Size.ToVector2();
            visual.Offset = new Vector3(50, 50, 0);

            visual.Brush = effectBrush;

            ElementCompositionPreview.SetElementChildVisual(this, visual);

            // ... but wait! There's more! We're going to animate the Angle property
            // to get a really trippy color effect on our masked inverted image.
            AnimateEffect(effectBrush);
        }

        private void AnimateEffect(CompositionEffectBrush effectBrush)
        {
            // Create a scalar keyframe animation (since the Angle property
            // is just a single float). Additionally we don't want any easing
            // for our animation, so we create a simple linear easing function.
            var animation = _compositor.CreateScalarKeyFrameAnimation();
            var easing = _compositor.CreateLinearEasingFunction();

            // The Angle property in a HueRotation effect has a range of 0 - 2 * Pi, so
            // that's what we'll animate between. Make sure we set our easing function.
            animation.InsertKeyFrame(0.0f, 0.0f);
            animation.InsertKeyFrame(1.0f, (float)(2.0 * Math.PI), easing);

            // This animation will go on forever and one pass will take about 4 seconds.
            animation.IterationBehavior = AnimationIterationBehavior.Forever;
            animation.Duration = TimeSpan.FromMilliseconds(4000);

            // Start the animation using the same name that we specified when compiling the
            // effect above when creating our effect factory.
            effectBrush.StartAnimation("hueEffect.Angle", animation);
        }
    }
}
