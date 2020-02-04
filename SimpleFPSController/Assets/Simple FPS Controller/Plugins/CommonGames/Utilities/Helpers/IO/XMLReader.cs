using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace CommonGames.Utilities.Helpers.IO
{
    public static class XMLReader
    {
        public static T Load<T>(this string path) where T : class
        {
            if (!File.Exists(path))
            {
                Debug.LogError("No Save Data has been found!");
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(path, FileMode.Open);
            T ret = (T)serializer.Deserialize(stream) as T;
            stream.Close();
            return ret;
        }

        public static void Save<T>(this T saveData, List<string> path, string fileName) where T : class
        {
            Save(saveData, path.GenerateFilePath(fileName));
        }

        public static void Save<T>(this T saveData, string path) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, saveData);
            stream.Close();
        }

        public static string GenerateFilePath(this List<string> folderPath, string fileName)
        {
            char s = Path.DirectorySeparatorChar;
            string path;

#if UNITY_ANDROID || UNITY_IOS
        path = Application.persistentDataPath;
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
            path = Application.dataPath;
#endif

            foreach (string f in folderPath)
                path += s + f;

            path += fileName + ".xml";
            return path;
        }
    }
}