using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks.DataGenerators;

namespace IAD_zad2_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ShapeFileGenerator sh = new ShapeFileGenerator();
            sh.GenerateData(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//image.bmp",
                System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//random_data.txt");
        }
    }
}
