using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace SimpleColorPicker
{
    public class ColorItem
    {
        public Color Color { get; private set; }
        public SolidColorBrush ColorBrush
        {
            get
            {
                return new SolidColorBrush(Color);
            }
        }
        public String ColorHexValue
        {
            get
            {
                return Color.ToString().Substring(3);
            }
        }

        public ColorItem(Color color)
        {
            Color = color;
        }
    }
}
