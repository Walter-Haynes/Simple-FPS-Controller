using UnityEngine;
using System;

namespace CommonGames.Utilities.Helpers.IO
{
    public static class CSVReader
    {
        public static string[][] Load(string fileName)
        {
            TextAsset file = Resources.Load(fileName) as TextAsset;
            return Read(file.text);
        }

        private static string[][] Read(string all)
        {
            string[] splitString = all.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string[][] database = new string[splitString.Length][];

            for (int i = 0; i < database.Length; i++)
                database[i] = splitString[i].Split(new string[] { "," }, StringSplitOptions.None);

            return database;
        }
    }
}