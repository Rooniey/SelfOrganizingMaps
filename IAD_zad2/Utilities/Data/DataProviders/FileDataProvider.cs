using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using IAD_zad2.Exceptions;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Data.DataProviders
{
    public class FileDataProvider : IDataProvider
    {
        public List<Vector<double>> GetData()
        {
            List<Vector<double>> data = new List<Vector<double>>();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.OpenFile()))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] stringValues = line.Split(new char[] { ',' });
                            List<double> values = new List<double>();
                            foreach (var str in stringValues)
                            {
                                values.Add(double.Parse(str, CultureInfo.InvariantCulture));
                            }

                            data.Add(Vector<double>.Build.DenseOfEnumerable(values));
                        }
                    }
                }
            }
            catch (FormatException e)
            {
                throw new DataProviderException($"Something went wrong with parsing, check format of file. Original message: {e.Message}");
            }
            catch (Exception e)
            {
                throw new DataProviderException($"File could not be read. Original message: {e.Message}.");
            }

            return data;
        }
    }
}