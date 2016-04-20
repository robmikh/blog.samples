using Robmikh.Util.CompositionImageLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageLoading
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

        private async void InitComposition()
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var imageLoader = ImageLoaderFactory.CreateImageLoader(compositor);

            var managedSurface = await imageLoader.CreateManagedSurfaceFromUriAsync(new Uri("ms-appx:///Assets/tripphoto1.jpg"));
            var brush = compositor.CreateSurfaceBrush(managedSurface.Surface);

            var visual = compositor.CreateSpriteVisual();
            visual.Size = managedSurface.Size.ToVector2();
            visual.Offset = new Vector3(50, 50, 0);
            visual.Brush = brush;

            ElementCompositionPreview.SetElementChildVisual(this, visual);
        }
    }
}
