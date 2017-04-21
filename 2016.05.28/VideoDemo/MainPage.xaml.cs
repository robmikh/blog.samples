using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Graphics.Canvas.Effects;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Pickers;
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

namespace VideoDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Windows.Media.Playback
        private MediaPlayer _player;
        private MediaPlayerSurface _mediaSurface;

        // Windows.UI.Composition
        private Compositor _compositor;
        private SpriteVisual _visual;
        private CompositionSurfaceBrush _videoBrush;
        private CompositionEffectBrush _effectBrush;

        public MainPage()
        {
            this.InitializeComponent();

            // Create our MediaPlayer
            _player = new MediaPlayer();
            _player.PlaybackSession.NaturalVideoSizeChanged += (sender, args) =>
            {
                sender.MediaPlayer.SetSurfaceSize(new Size(sender.NaturalVideoWidth, sender.NaturalVideoHeight));
            };
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Pause();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _player.Play();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private async void OpenFile()
        {
            // Create a file picker so the user can select their own video.
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            picker.ViewMode = PickerViewMode.Thumbnail;
            // To support more file types, just add them here.
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".avi");

            var file = await picker.PickSingleFileAsync();
            // If the user has selected a file to play, create a MediaSource
            // and play it.
            if (file != null)
            {
                var source = MediaSource.CreateFromStorageFile(file);
                PlaySource(source);
            }
        }

        private void PlaySource(MediaSource source)
        {
            // Create our MediaPlaybackItem.
            var mediaItem = new MediaPlaybackItem(source);

            // Set it as the source to our MediaPlayer and start playing.
            _player.Source = mediaItem;
            _player.Play();

            // Make sure our Composition related members are created.
            EnsureComposition();
            EnsureVideoSurface();
        }

        private void EnsureComposition()
        {
            if (_compositor == null)
            {
                // Get our Compositor and create a SpriteVisual.
                _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
                _visual = _compositor.CreateSpriteVisual();

                // Set the size to the size of the page, and make sure to update it whenever
                // the page's size changes.
                _visual.Size = new Vector2((float)ActualWidth, (float)ActualHeight);
                SizeChanged += (s, a) =>
                {
                    _visual.Size = new Vector2((float)ActualWidth, (float)ActualHeight);
                };

                // Attach our visual to the tree.
                ElementCompositionPreview.SetElementChildVisual(this, _visual);

                // Create our HueRotation effect.
                var graphicsEffect = new HueRotationEffect
                {
                    Name = "HueRotation",
                    Angle = 0.0f,
                    Source = new CompositionEffectSourceParameter("Video")
                };

                var effectFactory = _compositor.CreateEffectFactory(
                    graphicsEffect, 
                    new string[] { "HueRotation.Angle" });

                _effectBrush = effectFactory.CreateBrush();

                // Apply our effect brush to our visual.
                _visual.Brush = _effectBrush;
            }
        }

        private void EnsureVideoSurface()
        {
            if (_mediaSurface == null)
            {
                // Create a surface from our MediaPlayer.
                _mediaSurface = _player.GetSurface(_compositor);
                var surface = _mediaSurface.CompositionSurface;

                // Make a brush out of it and apply it to our effect.
                _videoBrush = _compositor.CreateSurfaceBrush(surface);
                _videoBrush.Stretch = CompositionStretch.Uniform;
                _videoBrush.HorizontalAlignmentRatio = 0.5f;
                _videoBrush.VerticalAlignmentRatio = 0.5f;
                _effectBrush.SetSourceParameter("Video", _videoBrush);

                // Enable our button now that we have everything set up.
                EffectButton.IsEnabled = true;
            }
        }

        private void EffectButton_Click(object sender, RoutedEventArgs e)
        {
            if (EffectButton.IsChecked == true)
            {
                // Create and start an animation that goes on forever.
                var easing = _compositor.CreateLinearEasingFunction();

                var animation = _compositor.CreateScalarKeyFrameAnimation();
                animation.InsertKeyFrame(0.0f, 0.0f);
                animation.InsertKeyFrame(1.0f, 2.0f * (float)Math.PI, easing);
                animation.Duration = TimeSpan.FromMilliseconds(5000);
                animation.IterationBehavior = AnimationIterationBehavior.Forever;

                _effectBrush.StartAnimation("HueRotation.Angle", animation);
            }
            else
            {
                // Settings the value directly will stop any animations that were running
                // on that property.
                _effectBrush.Properties.InsertScalar("HueRotation.Angle", 0.0f);
            }
        }
    }
}
