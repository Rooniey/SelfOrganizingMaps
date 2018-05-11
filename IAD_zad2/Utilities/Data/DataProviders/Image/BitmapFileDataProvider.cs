using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using IAD_zad2.Exceptions;
using IAD_zad2.Utilities.Data.DataProviders;
using IAD_zad2.Utilities.ImageProcessing;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Data
{
    public class BitmapFileDataProvider : IDataProvider
    {
        public List<Vector<double>> GetData()
        {
            List<Vector<double>> data = new List<Vector<double>>();
            IImageProcessing imageProcessing = new BitmapConverter();
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog1.Filter = "Bitmap|*.bmp|Jpeg|*.jpg|All files|*.*";

                try
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        using (var sourceImage = new Bitmap(openFileDialog1.FileName, true))
                        {
                            using (var destinationImage = new Bitmap(sourceImage.Width, sourceImage.Height))
                            {
                                int x, y;
                                List<Vector<double>> points = new List<Vector<double>>();
                                // Loop through the images pixels to reset color.
                                for (x = 0; x < sourceImage.Width; x++)
                                {
                                    for (y = 0; y < sourceImage.Height; y++)
                                    {
                                        Color pixelColor = sourceImage.GetPixel(x, y);
                                        Color newColor;

                                        newColor = imageProcessing.MakeMeanGray(pixelColor);

                                        destinationImage.SetPixel(x, y, newColor);
                                    }
                                }

                                using (FileStream fs = new FileStream(
                                    System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//data",
                                    FileMode.Create
                                ))
                                {
                                    destinationImage.Save(fs, ImageFormat.Bmp);
                                }
                            }
                        }
                    }
                }
                catch (ArgumentException)
                {
                    throw;
                }

                return data;
            }
        }
    }
}
