
// Credits for this class belong to Unity3D Wiki developers: 
// http://wiki.unity3d.com/index.php/OpenInFileBrowser

using JetBrains.Annotations;

namespace CommonGames.Utilities
{
    public static class OpenInFileBrowser
    {
        [PublicAPI]
        public static bool IsInMacOs 
            => UnityEngine.SystemInfo.operatingSystem.IndexOf("Mac OS", System.StringComparison.Ordinal) != -1;

        [PublicAPI]
        public static bool IsInWinOs 
            => UnityEngine.SystemInfo.operatingSystem.IndexOf("Windows", System.StringComparison.Ordinal) != -1;

        [PublicAPI]
        public static void Test()
        {
            Open(UnityEngine.Application.dataPath);
        }

        [PublicAPI]
        public static void OpenInMac(string path)
        {
            bool __openInsidesOfFolder = false;

            // try mac
            string __macPath = path.Replace("\\", "/"); // mac finder doesn't like backward slashes

            if (System.IO.Directory.Exists(__macPath)) // if path requested is a folder, automatically open insides of that folder
            {
                __openInsidesOfFolder = true;
            }

            if (!__macPath.StartsWith("\""))
            {
                __macPath = "\"" + __macPath;
            }

            if (!__macPath.EndsWith("\""))
            {
                __macPath += "\"";
            }

            string __arguments = (__openInsidesOfFolder ? "" : "-R ") + __macPath;

            try
            {
                System.Diagnostics.Process.Start("open", __arguments);
            }
            catch (System.ComponentModel.Win32Exception __e)
            {
                // tried to open mac finder in windows
                // just silently skip error
                // we currently have no platform define for the current OS we are in, so we resort to this
                __e.HelpLink = ""; // do anything with this variable to silence warning about not using it
            }
        }

        [PublicAPI]
        public static void OpenInWin(string path)
        {
            bool __openInsidesOfFolder = false;

            // try windows
            string __winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

            if (System.IO.Directory.Exists(__winPath)) // if path requested is a folder, automatically open insides of that folder
            {
                __openInsidesOfFolder = true;
            }

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", (__openInsidesOfFolder ? "/root," : "/select,") + __winPath);
            }
            catch (System.ComponentModel.Win32Exception __e)
            {
                // tried to open win explorer in mac
                // just silently skip error
                // we currently have no platform define for the current OS we are in, so we resort to this
                __e.HelpLink = ""; // do anything with this variable to silence warning about not using it
            }
        }

        [PublicAPI]
        public static void Open(string path)
        {
            if (IsInWinOs)
            {
                OpenInWin(path);
            }
            else if (IsInMacOs)
            {
                OpenInMac(path);
            }
            else // couldn't determine OS
            {
                OpenInWin(path);
                OpenInMac(path);
            }
        }
    }
}
