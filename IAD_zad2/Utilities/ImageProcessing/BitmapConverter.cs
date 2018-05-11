using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAD_zad2.Utilities.ImageProcessing
{
    public class BitmapConverter : IImageProcessing
    {

        public Color MakeMeanGray(Color color)
        {
            int mean = MeanFromColor(color);
            var newColor = Color.FromArgb(mean, mean, mean);
            return newColor;
        }

        private int MeanFromColor(Color color)
        {
            int sum = color.R + color.G + color.B;
            return sum / 3;
        }

    }
}
