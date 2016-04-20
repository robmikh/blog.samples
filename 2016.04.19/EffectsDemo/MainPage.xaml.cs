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
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _imageLoader = ImageLoaderFactory.CreateImageLoader(_compositor);

            IGraphicsEffect graphicsEffect = new InvertEffect
            {
                Name = "invertEffect",
                Source = new CompositeEffect
                {
                    Mode = Microsoft.Graphics.Canvas.CanvasComposite.DestinationIn,
                    Sources =
                    {
                        new CompositionEffectSourceParameter("image"),
                        new CompositionEffectSourceParameter("mask")
                    }
                }
            };
            _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

            var managedImageSurface = await _imageLoader.CreateManagedSurfaceFromUriAsync(new Uri("ms-appx:///Assets/tripphoto1.jpg"));
            var managedMaskSurface = await _imageLoader.CreateManagedSurfaceFromUriAsync(new Uri("ms-appx:///Assets/CircleMask.png"));

            var imageBrush = _compositor.CreateSurfaceBrush(managedImageSurface.Surface);
            var maskBrush = _compositor.CreateSurfaceBrush(managedMaskSurface.Surface);

            var effectBrush = _effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("image", imageBrush);
            effectBrush.SetSourceParameter("mask", maskBrush);

            var visual = _compositor.CreateSpriteVisual();

            visual.Size = managedImageSurface.Size.ToVector2();
            visual.Offset = new Vector3(50, 50, 0);

            visual.Brush = effectBrush;

            ElementCompositionPreview.SetElementChildVisual(this, visual);
        }
    }
}
