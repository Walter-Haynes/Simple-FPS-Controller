using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

namespace CommonGames.Tools
{
    [ExecuteInEditMode]
public partial class Grapphig : EditorWindow
{
    private static int _frameCounter = 0;
    private static bool _wasPlayingOrPaused = false;
    //private static string _consoleString = "";
    public static float sharedVerticalResolution = 10;
    public static bool beingManuallyAdjusted = false;

    public static string grapphigPath = "";
    public static bool validGrapphigPath = true;

    private enum MouseState
    {
        Up, Down
    }

    private Vector3 _mousePosition;
    private MouseState _mouseState;
    private bool _mouseInside;

    private static List<Channel> _channels = new List<Channel>();

    public static Grapphig grapphigWindow;

    [MenuItem("Window/Grapphig %g")]
    public static void Init()
    {
        grapphigWindow = (Grapphig)GetWindow(typeof(Grapphig));
        grapphigWindow.Show();
    }

    private void OnEnable()
    {
        SetupStyles();

        // Add additional update functions
        EditorApplication.update += Update;
        EditorApplication.update += UpdateReplay;
        EditorApplication.update += TimeKeeper.Update;
    }

    private void OnDisable()
    {
        // Remove additional update functions
        EditorApplication.update -= Update;
        EditorApplication.update -= UpdateReplay;
        EditorApplication.update -= TimeKeeper.Update;
    }

    private void Update()
    {
        _frameCounter++;

        // Detect when application stops playing
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            _wasPlayingOrPaused = true;
        }
        else
        {
            if (_wasPlayingOrPaused) OnStopped();
            _wasPlayingOrPaused = false;
            SetupStyles();
        }

        // Find max auto scale value and check for manual adjustment
        float __max = 0;
        foreach (Channel __ch in _channels)
        {
            if (__ch.rangeSlider != 0)
            {
                __ch.AutoScale = false;
            }

            if (__ch.AutoScale && __ch.autoScaleResolution > __max && __ch.Show)
            {
                __max = __ch.autoScaleResolution;
            }
        }

        // Adjust scale values
        foreach (Channel __ch in _channels)
        {
            if (!__ch.AutoScale) continue;
            __ch.VerticalResolution = GraphSettings.SharedVerticalResolution == 0 ? __ch.autoScaleResolution : __max;
        }
    }

    private void OnGUI()
    {
        // Style for toggle button
        _toggleButtonStyle = new GUIStyle(GUI.skin.button)
        {
            margin = new RectOffset(4, 4, 2, 2), 
            padding = new RectOffset(2, 2, 2, 2)
        };

        // Get mouse state and position
        _mousePosition = Event.current.mousePosition;

        // Determine LMB click state
        if (Event.current.type == EventType.MouseUp || Input.GetMouseButtonUp(0))
        {
            _mouseState = MouseState.Up;
        }
        else if (Event.current.type == EventType.MouseDown || Input.GetMouseButtonDown(0))
        {
            _mouseState = MouseState.Down;
        }

        // Check if mouse inside graph
        _mouseInside = _graphRect.Contains(_mousePosition) && _mousePosition.x < _graphRect.width;

        // Draw GUI
        Handles.BeginGUI();

        DrawStatic();
        
        // Avoid double calculations during layout and repaint
        if (Event.current.type == EventType.Repaint)
        {
            DrawGraph();
        }

        DrawRules();
        
        DrawBottomControls();

        Handles.EndGUI();

        // Force GUI repaint every frame
        Repaint();
    }

    /// <summary>
    /// Convert world position to graph position.
    /// </summary>
    private static Vector2 WorldToGraphPosition(Vector2 pos)
    {
        Vector2 __r = new Vector3();
        __r.x = GraphSettings.graphMargins.x + _graphRect.width - pos.x;
        __r.y = GraphSettings.graphMargins.y + _graphRect.height - pos.y;
        RectClamp(_graphRect, __r);
        return __r;
    }

    /// <summary>
    /// Convert graph position to world position.
    /// </summary>
    private static Vector2 GraphToWorldPosition(Vector2 pos)
    {
        Vector2 __r = new Vector3();
        __r.x = -(pos.x - GraphSettings.graphMargins.x - _graphRect.width);
        __r.y = -(pos.y - GraphSettings.graphMargins.y - _graphRect.height);
        return __r;
    }

    /// <summary>
    /// Clamp a point to inside of a rect.
    /// </summary>
    private static Vector2 RectClamp(Rect rect, Vector2 point)
    {
        point.x = Mathf.Clamp(point.x, rect.x, rect.x + rect.width);
        point.y = Mathf.Clamp(point.y, rect.y, rect.y + rect.height);
        return new Vector2(point.x, point.y);
    }

    /// <summary>
    /// Reset Grapphig.
    /// </summary>
    private void Reset()
    {
        try
        {
            _channels.Clear();
            replayControl = ReplayControls.Stop;
            _replaySampleQueues.Clear();
            _replayFiles.Clear();
            TimeKeeper.Reset();
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    /// Add new channel to graph.
    /// </summary>
    private static Channel AddChannel()
    {
        Channel __ch;
        _channels.Add(__ch = new Channel(_channels.Count));
        __ch.Init();

        return __ch;
    }

    /// <summary>
    /// Main Log function.
    /// </summary>
    public static void Log(object obj, string name, Color color, float time)
    {
        // Check for vectors
        Type __type = obj.GetType();

        if(__type == typeof(Vector2))
        {
            Vector2 __v = (Vector2)obj;
            Log(__v.x, name + " X", color, time);
            Log(__v.y, name + " Y", color, time);
            return;
        }

        if (__type == typeof(Vector3))
        {
            Vector3 __v = (Vector3)obj;
            Log(__v.x, name + " X", color, time);
            Log(__v.y, name + " Y", color, time);
            Log(__v.z, name + " Z", color, time);
            return;
        }
        if (__type == typeof(Vector4))
        {
            Vector3 __v = (Vector3)obj;
            Log(__v.x, name + " X", color, time);
            Log(__v.y, name + " Y", color, time);
            Log(__v.z, name + " Z", color, time);
            return;
        }
        if (typeof(IEnumerable).IsAssignableFrom(__type))
        {
            IEnumerable __enumerable = (IEnumerable)obj;
            int __n = 0;
            foreach(object __item in __enumerable)
            {
                Log(__item, name + "[" + __n + "]", color, time);
                __n++;
            }
            return;
        }

        float __d = ToFloat(obj);

        Channel __ch = null;
        if ((__ch = _channels.Find(i => i.name == name)) == null)
        {
            __ch = AddChannel();
            __ch.name = name;
            __ch.color = color;
            SetChannel(color, name);
            __ch.TimeScale = GraphSettings.HorizontalResolution;

            // Self get
            __ch.VerticalResolution = __ch.VerticalResolution;
            __ch.LogToFile = __ch.LogToFile;
            __ch.LogToConsole = __ch.LogToConsole;
        }

        if (!EditorApplication.isPlayingOrWillChangePlaymode && replayControl != ReplayControls.Play) return;
        
        if (__ch.lastFrame == _frameCounter && EditorApplication.isPlaying)
        {
            //Debug.LogWarning("Grapphig received 2 values in the same frame. You might be logging to the same channel name twice in a frame. Only the first value has been accepted.");
        }
        else
        {
            __ch.newestObj = obj;
            __ch.Enqueue(__d, time);
        }
        __ch.lastFrame = _frameCounter;
    }
    

    public static void Log(object obj, string name, float time)
    {
        Color __chColor = GetChannelColor(name);
        SetChannel(__chColor, name);
        Log(obj, name, __chColor, time);
    }

    public static void Log(object obj, string name, Color color = default)
    {
        Log(obj, name, color, TimeKeeper.Time);
    }

    public static void Log(object value, string name)
    {
        Color __color = GetChannelColor(name);
        SetChannel(__color, name);
        Log(value, name, __color);
    }

    public static void Log(object value, int id)
    {
        string __name = "Var " + id;
        Log(value, __name);
    }

    /// <summary>
    /// Called when Editor Application is stopped.
    /// </summary>
    private static void OnStopped()
    {
        // Generate session filename
        string __sessionFilename = "";

        // Make names unique or keep the same name
        if (GraphSettings.OverwriteFiles == 0)
        {
            IncrementRecordingSessionId();
            __sessionFilename = "S" + GetRecordingSessionId() + "_" + GetFilenameTimestamp() + ".ses";
        }
        else
        {
            __sessionFilename = "S" + GetRecordingSessionId() + ".ses";
        }

        // Log to file
        string __sessionList = "";

        for (int __i = 0; __i < _channels.Count; __i++)
        {
            Channel __ch = _channels[__i];

            if (!__ch.LogToFile) continue;
            // Generate filename
            string __filename = "";
            if (GraphSettings.OverwriteFiles == 1)
            {
                __filename = FileHandler.CleanFilename(__ch.name) + ".csv";
            }
            else
            {
                __filename = "S" + GetRecordingSessionId() + "_" + FileHandler.CleanFilename(__ch.name) + "_" + GetFilenameTimestamp() + ".csv";
            }

            // Write header
            string __header = "";
            __header += __ch.name + "," + __ch.VerticalResolution + "," + __ch.color.r + "," + __ch.color.g + "," + __ch.color.b + Environment.NewLine;
            FileHandler.WriteStringToCsv(__header, __filename);

            // Append samples
            FileHandler.AppendSamplesToCsv(__ch.rawSampleList, __filename);

            // Append to session
            __sessionList += __filename;
            if (__i != _channels.Count - 1) __sessionList += Environment.NewLine;
        }

        // Add channel filename to session filename list
        if (__sessionList != "") FileHandler.WriteStringToCsv(__sessionList, __sessionFilename);
    }
    
    #region Helpers
    
    private static float ToFloat(object d)
    {
        Type __type = d.GetType();
        float __x = 0f;

        if (__type == typeof(float))
        {
            __x = (float)d;
        }
        else if (__type == typeof(int))
        {
            __x = (float)(int)d;
        }
        else if (__type.IsEnum)
        {
            __x = (float)(int)d;
        }
        else
        {
            try
            {
                __x = (float)d;
            }
            catch
            {
                Debug.LogWarning("Grapphig: Variable type you are trying to graph is not recognized.");
                return __x;
            }
        }

        return __x;
    }

    private static bool IsNullOrValue<T>(T value)
    {
        return object.Equals(value, default(T));
    }

    private static string GetFilenameTimestamp()
    {
        return "_" + DateTime.Now.ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("HHmmss");
    }

    private static int GetRecordingSessionId()
    {
        string __key = "GrapphigSessionID";
        if (EditorPrefs.HasKey(__key))
        {
            return EditorPrefs.GetInt(__key);
        }
        else
        {
            EditorPrefs.SetInt(__key, 0);
            return 0;
        }
    }

    private static int IncrementRecordingSessionId()
    {
        const string __KEY = "GrapphigSessionID";
        if (EditorPrefs.HasKey(__KEY))
        {
            int __id = EditorPrefs.GetInt(__KEY) + 1;
            EditorPrefs.SetInt(__KEY, __id);
            return __id;
        }
        else
        {
            EditorPrefs.SetInt(__KEY, 0);
            return 0;
        }
    }
    
    #endregion
    }
}