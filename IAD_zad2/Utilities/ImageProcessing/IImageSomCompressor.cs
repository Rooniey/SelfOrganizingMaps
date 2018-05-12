using System.Collections.Generic;
using IAD_zad2.Model;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.ImageProcessing
{
    public interface IImageSomCompressor
    {
        void Compress(SelfOrganizingMap som, List<Vector<double>> dataToCompress, string pathToSave);
        void Decompress(string fileName, string pathToSave, SelfOrganizingMap som);
    }
}
