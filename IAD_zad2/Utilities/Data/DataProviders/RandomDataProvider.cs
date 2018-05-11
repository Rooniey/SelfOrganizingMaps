using System.Collections.Generic;
using IAD_zad2.Utilities.Generators;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Data.DataProviders
{
    public class RandomDataProvider : IDataProvider
    {
        public int NumberOfPoints { get; set; }

        public struct Point
        {
            public double x;
            public double y;

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public RandomDataProvider(int numberOfPoints)
        {
            NumberOfPoints = numberOfPoints;
        }

        public List<Vector<double>> GetData()
        {
            Point A = new Point(RandomNumberProvider.GetRandomNumber(-10, 10), RandomNumberProvider.GetRandomNumber(-10, 10));
            Point B = new Point(RandomNumberProvider.GetRandomNumber(-10, 10), RandomNumberProvider.GetRandomNumber(-10, 10));
            Point C = new Point(RandomNumberProvider.GetRandomNumber(-10, 10), RandomNumberProvider.GetRandomNumber(-10, 10));
            List<Vector<double>> data = new List<Vector<double>>();

            for (int i = 0; i < NumberOfPoints; i++)
            {
                var a = RandomNumberProvider.GetRandomNumber(0, 1);
                var b = RandomNumberProvider.GetRandomNumber(0, 1);

                if (a + b > 1)
                {
                    a = 1 - a;
                    b = 1 - b;
                }

                var c = 1 - a - b;

                var rndX = (a * A.x) + (b * B.x) + (c * C.x);
                var rndY = (a * A.y) + (b * B.y) + (c * C.y);

                data.Add(Vector<double>.Build.DenseOfArray(new[] { rndX, rndY }));
            }

            return data;
        }
    }
}