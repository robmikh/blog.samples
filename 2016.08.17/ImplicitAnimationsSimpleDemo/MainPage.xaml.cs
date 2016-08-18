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

namespace ImplicitAnimationsSimpleDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Compositor _compositor;
        private SpriteVisual _visual;
        private bool _isOnRight;

        public MainPage()
        {
            this.InitializeComponent();

            InitComposition();
        }

        private void InitComposition()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            _visual = _compositor.CreateSpriteVisual();
            _visual.Size = new Vector2(75.0f);
            _visual.Offset = new Vector3(50.0f, 50.0f, 0.0f);
            _visual.CenterPoint = new Vector3(_visual.Size / 2.0f, 0.0f);
            _visual.Brush = _compositor.CreateColorBrush(Colors.Red);

            ElementCompositionPreview.SetElementChildVisual(this, _visual);

            var offsetAnimation = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.Target = nameof(Visual.Offset);
            offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(1500);

            var rotationAnimation = _compositor.CreateScalarKeyFrameAnimation();
            rotationAnimation.Target = nameof(Visual.RotationAngleInDegrees);
            rotationAnimation.InsertKeyFrame(0.0f, 0.0f);
            rotationAnimation.InsertKeyFrame(1.0f, 360.0f);
            rotationAnimation.Duration = TimeSpan.FromMilliseconds(1500);

            var animationGroup = _compositor.CreateAnimationGroup();
            animationGroup.Add(offsetAnimation);
            animationGroup.Add(rotationAnimation);

            var implicitAnimations = _compositor.CreateImplicitAnimationCollection();
            implicitAnimations[nameof(Visual.Offset)] = animationGroup;

            _visual.ImplicitAnimations = implicitAnimations;
        }

        private void OffsetButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isOnRight)
            {
                _visual.Offset = new Vector3(350.0f, 50.0f, 0.0f);
            }
            else
            {
                _visual.Offset = new Vector3(50.0f, 50.0f, 0.0f);
            }

            _isOnRight = !_isOnRight;
        }
    }
}
