using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ExpressionsDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Populate our ListView with a bunch
            // of entries.
            var list = new List<String>();

            list.Add("Seattle");
            list.Add("New York");
            list.Add("San Francisco");
            list.Add("Paris");
            list.Add("London");
            list.Add("Berlin");
            list.Add("Moscow");
            list.Add("Beijing");
            list.Add("Tokyo");
            list.Add("Toronto");
            list.Add("Montreal");
            list.Add("Washington D.C.");
            list.Add("Los Angeles");
            list.Add("Chicago");
            list.Add("Huston");
            list.Add("Minneapolis");
            list.Add("Rome");
            list.Add("Dublin");
            list.Add("Sydney");
            list.Add("Madrid");
            list.Add("Seoul");
            list.Add("Athens");
            list.Add("Shanghai");
            list.Add("Singapore");
            list.Add("Miami");
            list.Add("Portland");
            list.Add("Boston");
            list.Add("Seattle");
            list.Add("New York");
            list.Add("San Francisco");
            list.Add("Paris");
            list.Add("London");
            list.Add("Berlin");
            list.Add("Moscow");
            list.Add("Beijing");
            list.Add("Tokyo");
            list.Add("Toronto");
            list.Add("Montreal");
            list.Add("Washington D.C.");
            list.Add("Los Angeles");
            list.Add("Chicago");
            list.Add("Huston");
            list.Add("Minneapolis");
            list.Add("Rome");
            list.Add("Dublin");
            list.Add("Sydney");
            list.Add("Madrid");
            list.Add("Seoul");
            list.Add("Athens");
            list.Add("Shanghai");
            list.Add("Singapore");
            list.Add("Miami");
            list.Add("Portland");
            list.Add("Boston");

            // Assign the list to our ListView
            MainListView.ItemsSource = list;

            // We need to make sure the ListView
            // is loaded before we can get the
            // ScrollViewer from it. You can also
            // comment out this portion to see what
            // the app looks like without Composition.
            MainListView.Loaded += (s, a) =>
            {
                InitComposition();
            };
        }

        private void InitComposition()
        {
            // Get our Compositor as well as the ScrollViewer that corresponds to
            // to our ListView. Then get the CompositionPropertySet that corresponds
            // to the ScrollViewer.
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var scrollViewer = MainListView.GetChildOfType<ScrollViewer>();
            var scrollerPropertySet = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scrollViewer);

            // Construct our parallax expression using the property set we got from the
            // ScrollViewer. Also we make sure to clamp the result so that the image doesn't
            // go off the screen/window.
            var expression = compositor.CreateExpressionAnimation("Clamp(scroller.Translation.Y * parallaxFactor, -608, 999)");
            expression.SetScalarParameter("parallaxFactor", 0.3f);
            expression.SetReferenceParameter("scroller", scrollerPropertySet);

            // Assign our expression to the visual that represents our BackgroundImage.
            var backgroundVisual = ElementCompositionPreview.GetElementVisual(BackgroundImage);
            backgroundVisual.StartAnimation("Offset.Y", expression);

            // Get the visual that represents our HeaderTextBlock and also set up part
            // of our expression.
            var textVisual = ElementCompositionPreview.GetElementVisual(HeaderTextBlock);
            String progress = "Clamp(visual.Offset.Y / -100.0, 0.0, 1.0)";

            // Create the expression and add in our progress string.
            var textExpression = compositor.CreateExpressionAnimation("Lerp(Vector3(0, 200, 0), Vector3(0, 0, 0), " + progress +")");
            textExpression.SetReferenceParameter("visual", backgroundVisual);

            // Assign our expression to the text visual.
            textVisual.StartAnimation("Offset", textExpression);
        }
    }
}
