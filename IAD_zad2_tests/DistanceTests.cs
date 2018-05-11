using IAD_zad2.Utilities.Distance;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IAD_zad2_tests
{
    [TestClass]
    public class DistanceTests
    {
        private EuclideanDistance ed;
        private ManhattanDistance md;

        [TestInitialize]
        public void Setup()
        {
            ed = new EuclideanDistance();
            md = new ManhattanDistance();
        }

        [TestMethod]
        public void EuclideanDistance_ShouldReturnProperValueForTwoDimensionalVectors()
        {
            Vector<double> v1 = Vector<double>.Build.DenseOfArray(new[] { -3d, 2.5d });
            Vector<double> v2 = Vector<double>.Build.DenseOfArray(new[] { -4d, 2d });
            Assert.AreEqual(ed.CalculateDistance(v1, v2), 1.118d, 0.001);
        }

        [TestMethod]
        public void EuclideanDistance_ShouldReturnProperValueForThreeDimensionalVectors()
        {
            Vector<double> v1 = Vector<double>.Build.DenseOfArray(new[] { -3d, 2.5d, -0.5d });
            Vector<double> v2 = Vector<double>.Build.DenseOfArray(new[] { -4d, 2d, 0.15d });
            Assert.AreEqual(ed.CalculateDistance(v1, v2), 1.293d, 0.001);
        }

        [TestMethod]
        public void ManhattanDistance_ShouldReturnProperValueForTwoDimensionalVectors()
        {
            Vector<double> v1 = Vector<double>.Build.DenseOfArray(new[] { -3d, 2.5d });
            Vector<double> v2 = Vector<double>.Build.DenseOfArray(new[] { -4d, 2d });
            Assert.AreEqual(md.CalculateDistance(v1, v2), 1.5d, 0.001);
        }

        [TestMethod]
        public void ManhattanDistance_ShouldReturnProperValueForThreeDimensionalVectors()
        {
            Vector<double> v1 = Vector<double>.Build.DenseOfArray(new[] { -3d, 2.5d, -0.5d });
            Vector<double> v2 = Vector<double>.Build.DenseOfArray(new[] { -4d, 2d, 0.15d });
            Assert.AreEqual(md.CalculateDistance(v1, v2), 2.15d, 0.001);
        }
    }
}