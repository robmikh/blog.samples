using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SimpleColorPicker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var list = CreateColorItems();

            MainGridView.ItemsSource = list;
        }

        private List<ColorItem> CreateColorItems()
        {
            var list = new List<ColorItem>();

            list.Add(new ColorItem(Color.FromArgb(255, 255, 185, 0)));
            list.Add(new ColorItem(Color.FromArgb(255, 231, 72, 86)));
            list.Add(new ColorItem(Color.FromArgb(255, 0, 120, 215)));
            list.Add(new ColorItem(Color.FromArgb(255, 0, 153, 188)));
            list.Add(new ColorItem(Color.FromArgb(255, 122, 117, 116)));
            list.Add(new ColorItem(Color.FromArgb(255, 118, 118, 118)));
            list.Add(new ColorItem(Color.FromArgb(255, 255, 140, 0)));
            list.Add(new ColorItem(Color.FromArgb(255, 232, 17, 35)));
            list.Add(new ColorItem(Color.FromArgb(255, 0, 99, 177)));
            list.Add(new ColorItem(Color.FromArgb(255, 45, 125, 154)));
            list.Add(new ColorItem(Color.FromArgb(255, 93, 90, 88)));
            list.Add(new ColorItem(Color.FromArgb(255, 76, 74, 72)));
            list.Add(new ColorItem(Color.FromArgb(255, 247, 99, 12)));
            list.Add(new ColorItem(Color.FromArgb(255, 234, 0, 94)));
            list.Add(new ColorItem(Color.FromArgb(255, 142, 140, 216)));
            list.Add(new ColorItem(Color.FromArgb(255, 0, 183, 195)));
            list.Add(new ColorItem(Color.FromArgb(255, 104, 118, 138)));
            list.Add(new ColorItem(Color.FromArgb(255, 105, 121, 126)));
            list.Add(new ColorItem(Color.FromArgb(255, 202, 80, 16)));
            list.Add(new ColorItem(Color.FromArgb(255, 195, 0, 82)));
            list.Add(new ColorItem(Color.FromArgb(255, 107, 105, 214)));
            list.Add(new ColorItem(Color.FromArgb(255, 3, 131, 135)));
            list.Add(new ColorItem(Color.FromArgb(255, 81, 92, 107)));
            list.Add(new ColorItem(Color.FromArgb(255, 74, 84, 89)));
            list.Add(new ColorItem(Color.FromArgb(255, 218, 59, 1)));
            list.Add(new ColorItem(Color.FromArgb(255, 227, 0, 140)));
            list.Add(new ColorItem(Color.FromArgb(255, 135, 100, 184)));
            list.Add(new ColorItem(Color.FromArgb(255, 0, 178, 148)));
            list.Add(new ColorItem(Color.FromArgb(255, 86, 124, 115)));
            list.Add(new ColorItem(Color.FromArgb(255, 100, 124, 100)));
            list.Add(new ColorItem(Color.FromArgb(255, 239, 105, 80)));
            list.Add(new ColorItem(Color.FromArgb(255, 191, 0, 119)));
            list.Add(new ColorItem(Color.FromArgb(255, 116, 77, 169)));
            list.Add(new ColorItem(Color.FromArgb(255, 1, 133, 116)));
            list.Add(new ColorItem(Color.FromArgb(255, 72, 104, 96)));
            list.Add(new ColorItem(Color.FromArgb(255, 82, 94, 84)));
            list.Add(new ColorItem(Color.FromArgb(255, 209, 52, 56)));
            list.Add(new ColorItem(Color.FromArgb(255, 194, 57, 179)));
            list.Add(new ColorItem(Color.FromArgb(255, 177, 70, 194)));
            list.Add(new ColorItem(Color.FromArgb(255, 0, 204, 106)));
            list.Add(new ColorItem(Color.FromArgb(255, 73, 130, 5)));
            list.Add(new ColorItem(Color.FromArgb(255, 132, 117, 69)));
            list.Add(new ColorItem(Color.FromArgb(255, 255, 67, 67)));
            list.Add(new ColorItem(Color.FromArgb(255, 154, 0, 137)));
            list.Add(new ColorItem(Color.FromArgb(255, 136, 23, 152)));
            list.Add(new ColorItem(Color.FromArgb(255, 16, 137, 62)));
            list.Add(new ColorItem(Color.FromArgb(255, 16, 124, 16)));
            list.Add(new ColorItem(Color.FromArgb(255, 126, 115, 95)));

            return list;
        }

        private void MainGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var color = e.ClickedItem as ColorItem;

            if (color != null)
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(color.ColorHexValue);
                Clipboard.SetContent(dataPackage);
            }
        }
    }
}
