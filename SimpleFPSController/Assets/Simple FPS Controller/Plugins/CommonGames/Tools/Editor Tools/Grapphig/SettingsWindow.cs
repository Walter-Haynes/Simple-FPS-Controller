using UnityEditor;
using UnityEngine;

namespace CommonGames.Tools
{
    public class SettingsWindow : EditorWindow
    {
        [MenuItem("Window/Grapphig Settings")]
        public static void Init()
        {
            SettingsWindow __window = (SettingsWindow)EditorWindow.GetWindow(typeof(SettingsWindow));
            __window.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));

            GUILayout.Space(3);
            GUILayout.Label("Graph", EditorStyles.boldLabel);
            GUILayout.Space(3);

            // Time window
            Grapphig.GraphSettings.HorizontalResolution = FloatField("Horizontal resolution (time)", Grapphig.GraphSettings.HorizontalResolution, 0.5f, 30);

            // Shared Y Range
            Grapphig.GraphSettings.SharedVerticalResolution = (int)FloatField("Share vertical resolution", Grapphig.GraphSettings.SharedVerticalResolution, 0, 1);

            // Line style selection
            Grapphig.GraphSettings.GraphLineStyle = (int)FloatField("Line style", Grapphig.GraphSettings.GraphLineStyle, 0, 1);

            GUILayout.Space(3);
            GUILayout.Label("Logging", EditorStyles.boldLabel);
            GUILayout.Space(3);

            // Overwrite existing files
            Grapphig.GraphSettings.OverwriteFiles = (int)FloatField("Overwrite existing files", Grapphig.GraphSettings.OverwriteFiles, 0, 1);

            GUILayout.Space(3);
            GUILayout.Label("Defaults", EditorStyles.boldLabel);
            GUILayout.Space(3);

            // Default Y Range
            Grapphig.GraphSettings.DefaultVerticalResolution = FloatField("Vertical resolution", Grapphig.GraphSettings.DefaultVerticalResolution, 1, Mathf.Infinity);

            // Default log to file
            Grapphig.GraphSettings.DefaultLogToFile = (int)FloatField("Log To File", Grapphig.GraphSettings.DefaultLogToFile, 0, 1);

            // Default log to console
            Grapphig.GraphSettings.DefaultLogToConsole = (int)FloatField("Log To Console", Grapphig.GraphSettings.DefaultLogToConsole, 0, 1);

            GUILayout.Space(10);

            GUILayout.EndArea();
        }

        public float FloatField(string label, float value, float min, float max)
        {
            float __result;
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label(label, GUILayout.Width(160));
            __result = float.Parse(GUILayout.TextField(value.ToString(), 10, GUILayout.Width(100)));
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                __result -= 1;
            }
            else if (GUILayout.Button("+", GUILayout.Width(20)))
            {
                __result += 1;
            }
            GUILayout.EndHorizontal();
            return Mathf.Clamp(__result, min, max);
        }
    }
}
