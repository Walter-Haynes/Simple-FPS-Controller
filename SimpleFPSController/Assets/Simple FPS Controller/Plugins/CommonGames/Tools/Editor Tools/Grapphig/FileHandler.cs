using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CommonGames.Tools
{
    public static class FileHandler
    {

        public static string writePath = Application.temporaryCachePath;
        public static string delimiter = ",";

        public static void AppendSamplesToCsv(List<Sample> samples, string filename)
        {
            string __output = "";
            foreach (Sample __s in samples)
            {
                __output += __s.t + delimiter + __s.d + Environment.NewLine;
            }
            File.AppendAllText(Path.Combine(writePath, filename), __output);
        }

        public static List<Sample> LoadSamplesFromCsv(string filename)
        {
            List<Sample> __sampleList = new List<Sample>();

            string __fileData = File.ReadAllText(Path.Combine(writePath, filename));
            string[] __lines = __fileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int __i = 1; __i < __lines.Length - 1; __i++)
            {
                string[] __vs = __lines[__i].Trim().Split(',');
                try
                {
                    Sample __gs = new Sample(float.Parse(__vs[1]), float.Parse(__vs[0]));
                    __sampleList.Add(__gs);
                }
                catch { }
            }
            return __sampleList;
        }

        public static string LoadHeaderFromCsv(string filename)
        {
            string __fileData = File.ReadAllText(Path.Combine(writePath, filename));
            string[] __lines = __fileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (__lines.Length > 0)
            {
                return __lines[0];
            }
            else
            {
                Debug.LogWarning("File you are trying to open is empty.");
                return null;
            }
        }

        public static void AppendStringToCsv(string input, string filename)
        {
            File.AppendAllText(Path.Combine(writePath, filename), input);
        }

        public static void WriteStringToCsv(string input, string filename)
        {
            File.WriteAllText(Path.Combine(writePath, filename), input);
        }

        public static List<string> BrowserOpenFiles()
        {
            string __path = EditorUtility.OpenFilePanel("Open previous recording(s)", writePath, "");
            if (__path == null || __path == "")
            {
                return null;
            }
            else
            {
                if (Path.GetExtension(__path) == ".csv")
                {
                    // Fetch single file
                    string[] __arr = new string[1];
                    __arr[0] = __path;
                    Debug.Log("Loaded " + __path);
                    return __arr.ToList();
                }
                else if (Path.GetExtension(__path) == ".ses")
                {
                    // Fetch all files for the recording
                    string __fileData = File.ReadAllText(__path);
                    List<string> __filenameList = __fileData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

                    foreach (string __s in __filenameList)
                    {
                        Debug.Log("Loaded " + __s);
                    }

                    return __filenameList;
                }
                else
                {
                    // Unknown extension
                    Debug.LogError("Unknown file format. Only .csv and .ses files are supported");
                    return null;
                }
            }
        }

        public static string CleanFilename(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty)).Replace(" ", "");
        }
    }
}