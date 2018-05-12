using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IAD_zad2.Model;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.ImageProcessing
{
    public class ImageProcessing : IImageSomCompressor
    {
        private int width = 85;

        public void Compress(SelfOrganizingMap som, List<Vector<double>> dataToCompress, string pathToSave)
        {
            List<Neuron> neurons = som.Neurons;

            using (FileStream fs = new FileStream(pathToSave, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("{");
                    for (int i = 0; i < neurons.Count; i++)
                    {
                        sw.Write($"{i}:");
                        foreach (var weight in neurons[i].CurrentWeights.AsArray())
                        {
                            sw.Write($"{Math.Round(weight)} ");
                        }
                        sw.WriteLine();
                    }
                    sw.WriteLine("}");


                    for (int j = 0; j < dataToCompress.Count; j += 1)
                    {
                        sw.Write($"{som.GetWinnerIndexForInput(dataToCompress[j])} ");
                    }
                }

            }
        }

        public void Decompress(string fileName, string pathToSave, SelfOrganizingMap som)
        {
            int size = 360;
            int scale = size / 3;
            int stepsize = 3;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                  
                    string data = sr.ReadToEnd();

                    Regex rgx = new Regex(@"(?<=\{)[^()]*(?=\})");
                    string neuronsInfo = rgx.Match(data).Value;
                    string[] neurons = neuronsInfo.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                    List<Neuron> list = new List<Neuron>();
                    foreach (var neuron in neurons)
                    {
                        string[] weights = neuron.Split(':')[1].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        List<double> values = new List<double>();
                        foreach (var weight in weights)
                        {
                            values.Add(double.Parse(weight));
                        }
                        Neuron n = new Neuron() { CurrentWeights = Vector<double>.Build.DenseOfEnumerable(values) };
                        list.Add(n);
                    }

                    som.Neurons = list;

                    var compressed = data.Split('}')[1].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    Bitmap bm = new Bitmap(size, size);
                    List<List<Vector<double>>> cos = new List<List<Vector<double>>>();
                    for (int i = 0; i < scale; i++)
                    {
                        List<Vector<double>> it = new List<Vector<double>>();
                        for (int j = 0; j < scale; j++)
                        {
                            int position = Int32.Parse(compressed.ElementAt(i * scale + j));
                            it.Add(som.Neurons[position].CurrentWeights);

                        }
                        cos.Add(it);
                            
                    }

                    for (int x = 0; x < size; x+=stepsize)
                    {
                        for (int y = 0; y < size; y+=stepsize)
                        {
                            for (int i = 0; i < stepsize; i++)
                            {
                                for (int j = 0; j < stepsize; j++)
                                {
                                    var value = (int)cos[x/stepsize][y/stepsize].At(i * stepsize + j);

                                    Color newColor = Color.FromArgb(value, value, value);

                                    bm.SetPixel(y+i, x+j, newColor);
                                }
                            }
                        }
                    }


                        bm.Save(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/foto.bmp");
                        bm.Dispose();

                    }


                }

            }
        }
    }

