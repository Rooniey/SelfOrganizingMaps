using System.Collections.Generic;
using System.Drawing;
using IAD_zad2.Model;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.ImageProcessing
{
    public interface IImageSomCompressor
    {
        void Compress(NeuralNetwork som, List<Vector<double>> dataToCompress);
        Bitmap Decompress(NeuralNetwork som);
    }
}
