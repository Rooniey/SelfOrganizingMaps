using System;
using System.Collections.Generic;

namespace IAD_zad2.Utilities.ExtensionMethods
{
    public static class ListUtility
    {
        private static readonly Random _rand = new Random();

        public static List<T> Shuffle<T>(this List<T> source)
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                var i = _rand.Next(list.Count + 1);
                if (i == list.Count)
                {
                    list.Add(item);
                }
                else
                {
                    var temp = list[i];
                    list[i] = item;
                    list.Add(temp);
                }
            }

            return list;
        }
    }
}