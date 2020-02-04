using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

using CommonGames.Tools;
using CommonGames.Utilities;

namespace CommonGames.Tools
{
public partial class Grapphig : EditorWindow
{
    private static Rect _graphRect;
    private Rect _borderRect;
    private Rect _toolbarRect;

    private GUIStyle _toggleButtonStyle;

    private static Texture2D _showTexture;
    //private static Texture2D _autoScaleTexture;
    private static Texture2D _logTexture;
    private static Texture2D _consoleTexture;

    private static void SetupStyles()
    {
        // Get textures
        _showTexture = GraphSettings.GetTextureFrom64(GraphSettings.showIcon64);
        //_autoScaleTexture = GraphSettings.GetTextureFrom64(GraphSettings.scaleIcon64);
        _logTexture = GraphSettings.GetTextureFrom64(GraphSettings.logIcon64);
        _consoleTexture = GraphSettings.GetTextureFrom64(GraphSettings.consoleIcon64);

        // Button Background
        GUIStyleState __bg = new GUIStyleState
        {
            background = GenerateMonotoneTexture(new Vector2(32, 32), GraphSettings.buttonBackgroundColor)
        };

        // Button Hover
        GUIStyleState __hover = new GUIStyleState
        {
            background = GenerateMonotoneTexture(new Vector2(32, 32), GraphSettings.buttonHoverColor)
        };

        // Button Active
        GUIStyleState __active = new GUIStyleState
        {
            background = GenerateMonotoneTexture(new Vector2(32, 32), GraphSettings.buttonActiveColor)
        };
    }

    /// <summary>
    /// Draw horizontal and vertical rule lines
    /// </summary>
    private void DrawRules()
    {
        // Draw rules
        if (!_graphRect.Contains(_mousePosition)) return;
        
        Handles.color = GraphSettings.ruleLineColor;
        
        Vector2 __horizontalRuleStart = RectClamp(_graphRect, new Vector2(0, _mousePosition.y));
        Vector2 __horizontalRuleEnd = RectClamp(_graphRect, new Vector2(position.width, _mousePosition.y));
        Handles.DrawLine(new Vector3(__horizontalRuleStart.x, __horizontalRuleStart.y), new Vector3(__horizontalRuleEnd.x, __horizontalRuleEnd.y));
        
        Vector2 __verticalRuleStart = RectClamp(_graphRect, new Vector2(_mousePosition.x, 0));
        Vector2 __verticalRuleEnd = RectClamp(_graphRect, new Vector2(_mousePosition.x, position.height));
        Handles.DrawLine(new Vector3(__verticalRuleStart.x, __verticalRuleStart.y), new Vector3(__verticalRuleEnd.x, __verticalRuleEnd.y));
    }

    /// <summary>
    /// Draw graph lines / points and tags.
    /// </summary>
    private void DrawGraph()
    {
        bool __stopReplay = true;

        // Draw graph
        foreach (Channel __ch in _channels)
        {
            if (__ch.Show)
            {
                // Update time scale
                __ch.TimeScale = GraphSettings.HorizontalResolution;

                Vector3 __graphSpaceMousePos = _mousePosition;
                __ch.pointAtMousePosition = Vector3.zero;
                Sample __sampleAtMousePosition = null;

                if (__ch.sampleNo > 0)
                {
                    Sample[] __samples = __ch.GetSamples();

                    List<Vector3> __points = new List<Vector3>();

                    float __newestSampleTime = Mathf.Max(0f, __samples[__samples.Length - 1].t);
                    float __oldestSampleTime = Mathf.Max(0f, __samples[0].t);
                    float __timeSpan = __newestSampleTime - __oldestSampleTime;
                    __timeSpan = Mathf.Clamp(__timeSpan, 0f, __ch.TimeScale);

                    // Determine scale
                    //TODO: Scaling
                    float __xScale = _graphRect.width / __ch.TimeScale;
                    float __yScale = (__ch.YMax / (__ch.VerticalResolution / 2f)) *
                                     ((_graphRect.height / 2f) / __ch.YMax); // * GraphSettings.autoScalePercent;

                    // Signal offset
                    float __xOffset = 0f;
                    float __yOffset = _graphRect.height / 2f;

                    float __graphXEnd = GraphSettings.graphMargins.x + _graphRect.width;
                    float __graphYEnd = GraphSettings.graphMargins.y + _graphRect.height;

                    float __minTime = TimeKeeper.Time - __ch.TimeScale;

                    int __pointCount = 0;

                    bool __newestVisibleSampleFound = false;

                    for (int __i = __ch.lastVisiblePointIndex; __i < __ch.sampleNo; __i++)
                    {
                        float __value = __samples[__i].d;
                        float __st = __samples[__i].t;
                        if (__st > TimeKeeper.Time)
                        {
                            if (__newestVisibleSampleFound == false)
                            {
                                __ch.newestSample = __samples[__i];
                                __ch.firstVisiblePointIndex = __i;
                                __newestVisibleSampleFound = true;

                                if(__i == __ch.sampleNo - 1)
                                {
                                    __ch.replayEnded = true;
                                }
                            }
                            continue;
                        }
                        else if (__st <= __minTime)
                        {
                            __ch.lastVisiblePointIndex = __i;
                            continue;
                        }
                        else
                        {
                            __ch.newestSample = __samples[__ch.sampleNo - 1];
                        }

                        float __t = TimeKeeper.Time - __samples[__i].t;                

                        // Convert to graph space (faster WorldToGraphPosition)
                        float __x = __graphXEnd - ((__t * __xScale) + __xOffset);
                        float __y = __graphYEnd - ((__value * __yScale) + __yOffset);

                        // Clamp without function calls
                        __x = __x < _graphRect.x ? _graphRect.x : __x > (_graphRect.x + _graphRect.width) ? (_graphRect.x + _graphRect.width) : __x;
                        __y = __y < _graphRect.y ? _graphRect.y : __y > (_graphRect.y + _graphRect.height) ? (_graphRect.y + _graphRect.height) : __y;

                        Vector2 __point = new Vector2(__x, __y);
                        __points.Add(__point);
                        __pointCount++;

                        // Check for mouse position
                        if (__pointCount <= 1 || !(__points[__pointCount - 1].x > __graphSpaceMousePos.x) ||
                            !(__points[__pointCount - 2].x < __graphSpaceMousePos.x)) continue;
                        
                        __ch.pointAtMousePosition = new Vector2(__x, __y);
                        __sampleAtMousePosition = __samples[__i];
                    }

                    if (__pointCount > 0)
                    {
                        // Right-side indicator
                        Handles.color = __ch.color;
                        Handles.DrawLine(WorldToGraphPosition(new Vector2(0, GraphToWorldPosition(__points[__pointCount - 1]).y)), WorldToGraphPosition(new Vector2(-50, GraphToWorldPosition(__points[__pointCount - 1]).y)));

                        // Right side label
                        if(__ch.newestSample != null)
                            DrawHorizontalLabel(WorldToGraphPosition(new Vector2(4, GraphToWorldPosition(__points[__pointCount - 1]).y + 8)), FloatToCompact(__ch.newestSample.d), __ch.color);

                        switch (GraphSettings.GraphLineStyle)
                        {
                            // Draw polyline (fastest)
                            // Draw dots
                            case 0:
                                Handles.DrawAAPolyLine(__points.ToArray());
                                break;
                            case 1:
                            {
                                if (__points.Count > 0)
                                {
                                    for (int __i = 1; __i < __points.Count - 1; __i++)
                                    {
                                        Handles.DrawSolidDisc(__points[__i], Vector3.forward, 1f);
                                    }
                                }

                                break;
                            }
                        }

                        // Intersection marker and labels at mouse position
                        if (_mouseInside && __sampleAtMousePosition != null)
                        {
                            __ch.tagY = __sampleAtMousePosition.d;
                            __ch.tagX = __sampleAtMousePosition.t;

                            // Draw tag at the mouse position with graph value at that point
                            Handles.DrawSolidDisc(__ch.pointAtMousePosition, Vector3.forward, 3f);
                            DrawHorizontalTag(__ch.pointAtMousePosition, " " + __ch.name + " = " + FloatToCompact(__ch.tagY), __ch.color);

                            // Draw time indicator below graph
                            int __textWidth = 80;
                            int __outOfBoundsOfset = 0;
                            
                            if (_mousePosition.x < __textWidth / 2f)
                            {
                                __outOfBoundsOfset += (__textWidth / 2) - (int)_mousePosition.x;
                            }
                            
                            Vector2 __timeIndicatorPosition = new Vector2(_mousePosition.x - __textWidth / 2f + __outOfBoundsOfset, _graphRect.height + 10);
                            string __timeAtPointer = __ch.tagX.ToString("0.00") + "s";
                            string __timeBehind = " (t" + (__ch.tagX - TimeKeeper.Time).ToString("0.0") + "s)";
                            DrawHorizontalLabel(__timeIndicatorPosition, __timeAtPointer + __timeBehind, Color.black);
                        }
                    }
                }
            }

            if (!__ch.replayEnded) __stopReplay = false;
        }

        if (__stopReplay)
        {
            replayControl = ReplayControls.Stop;
        }
        
        // Draw time marker when mouse outside of graph
        if (_mouseInside) return;
        
        string __label = $"{TimeKeeper.Time.ToString("0.0") + "s",7}";
        DrawHorizontalLabel(new Vector2(_graphRect.width - 25, _graphRect.height + 10), __label, Color.black);
    }

    /// <summary>
    /// Draw backgrounds and channel side panels
    /// </summary>
    private void DrawStatic()
    {
        Vector2 __labelOffset = new Vector2(-4f, -7f);

        // Draw window background
        Rect __bgRect = new Rect(0, 0, position.width, position.height);
        Handles.color = GraphSettings.windowBackgroundColor;
        Handles.DrawSolidRectangleWithOutline(__bgRect, GraphSettings.windowBackgroundColor, GraphSettings.windowBackgroundColor);
        
        // Draw graph border
        Handles.color = GraphSettings.borderBackgroundColor;
        _borderRect = new Rect(
            GraphSettings.graphMargins.x, GraphSettings.graphMargins.y,
            position.width - GraphSettings.graphMargins.z + 49f, position.height - GraphSettings.graphMargins.w + 20f
            );

        Handles.DrawSolidRectangleWithOutline(_borderRect, GraphSettings.borderBackgroundColor, GraphSettings.borderBackgroundColor);

        // Draw graph background
        Handles.color = Color.white;
        
        _graphRect = new Rect(
            x: GraphSettings.graphMargins.x, 
            y: GraphSettings.graphMargins.y, 
            width: position.width - GraphSettings.graphMargins.z, 
            height: position.height - GraphSettings.graphMargins.w
        );
        Handles.DrawSolidRectangleWithOutline(_graphRect, GraphSettings.checkerDarkColor, Color.clear);
        
        //TODO: Replace __GRID_SIZE const with a scaling number

        const float __GRID_SIZE = 20f;
        
        Rect __graphOverlayRect = new Rect(
            x: GraphSettings.graphMargins.x, 
            y: GraphSettings.graphMargins.y, 
            width: (position.width - GraphSettings.graphMargins.z)/__GRID_SIZE, 
            height: (position.height - GraphSettings.graphMargins.w)/__GRID_SIZE
        );
        
        for(int x = 0; x < __GRID_SIZE; x++)
        {
            for (int y = 0; y < __GRID_SIZE; y++)
            {
                if((x + y) % 2 == 0)
                {
                    Handles.DrawSolidRectangleWithOutline(
                        __graphOverlayRect, 
                        GraphSettings.checkerLightColor, 
                        Color.clear);
                }

                __graphOverlayRect.y += ((position.height - GraphSettings.graphMargins.w) / __GRID_SIZE); //((position.height - GraphSettings.graphMargins.w) / 10f);
            }

            __graphOverlayRect.y = GraphSettings.graphMargins.y;
            __graphOverlayRect.x += ((position.width - GraphSettings.graphMargins.z) / __GRID_SIZE);
        }

        // Draw bottom toolbar
        Handles.color = GraphSettings.panelBackgroundColor;
        _toolbarRect = new Rect(
            new Vector2(0, _borderRect.height),
            new Vector2(_borderRect.width, position.height - _borderRect.height)
            );
        
        Handles.DrawSolidRectangleWithOutline(_toolbarRect, GraphSettings.panelBackgroundColor, GraphSettings.panelBackgroundColor);

        // Draw grid
        
        Handles.color = GraphSettings.gridLineColor;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if(GraphSettings.SHOW_VERTICAL_RULER)
            #pragma warning disable 162
        {
            Handles.DrawLine(
                new Vector3(GraphSettings.graphMargins.x, GraphSettings.graphMargins.y + _graphRect.height / 2f),
                new Vector3(GraphSettings.graphMargins.x + _graphRect.width,
                    GraphSettings.graphMargins.y + _graphRect.height / 2f));
        }
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if(GraphSettings.SHOW_HORIZONTAL_RULER)
        {
            Handles.DrawLine(
                new Vector3(GraphSettings.graphMargins.x + _graphRect.width / 2f, GraphSettings.graphMargins.y),
                new Vector3(GraphSettings.graphMargins.x + _graphRect.width / 2f,
                    GraphSettings.graphMargins.y + _graphRect.height));
        }
        #pragma warning restore 162

        // Draw scales
        Handles.color = Color.black;
        Handles.Label(WorldToGraphPosition(new Vector2(-5f, _graphRect.height / 2f)) + __labelOffset, "0"); // Left zero

        for (int __i = 0; __i < _channels.Count; __i++)
        {
           DrawChannelSidebar(__i);
        }
    }

    /// <summary>
    /// Draw buttons on the bottom of the Grapher window.
    /// </summary>
    private void DrawBottomControls()
    {
        GUILayout.BeginArea(_toolbarRect);
        GUILayout.BeginHorizontal();

        GUI.enabled = !EditorApplication.isPlaying && (_channels.Count > 0 || _replayFiles.Count > 0);

        // PLAY PAUSE BUTTON
        string __pp = replayControl == ReplayControls.Play ? "Pause" : "Play  ";

        if (GUILayout.Button(__pp))
        {
            foreach (Channel __ch in _channels) __ch.replayEnded = false;
            replayControl = replayControl == ReplayControls.Play ? ReplayControls.Pause : ReplayControls.Play;
        }

        GUI.enabled = false || !EditorApplication.isPlaying && _channels.Count > 0
                                                            && (replayControl == ReplayControls.Play || replayControl == ReplayControls.Pause);

        // STOP BUTTON
        if (GUILayout.Button("Stop"))
        {
            replayControl = ReplayControls.Stop;
        }

        GUI.enabled = !EditorApplication.isPlaying && (replayControl != ReplayControls.Play || replayControl != ReplayControls.Pause);

        // OPEN BUTTON
        if (GUILayout.Button("Open"))
        {
            OpenFiles();
            ReplayInit();
        }

        GUI.enabled = !EditorApplication.isPlaying && _replayFiles.Count > 0;

        // CLEAR REPLAY FILES BUTTON
        if (GUILayout.Button("Clear"))
        {
            replayControl = ReplayControls.Stop;
            _channels.Clear();
            _replayFiles.Clear();
            _replaySampleQueues.Clear();
            replayControl = ReplayControls.Stop;
        }

        GUI.enabled = !EditorApplication.isPlaying;

        // SHOW IN EXPLORER BUTTON
        if (GUILayout.Button("Explorer"))
        {
            OpenInFileBrowser.Open(FileHandler.writePath);
        }

        // SETTINGS BUTTON
        GUI.enabled = true;
        if (GUILayout.Button("Settings"))
        {
            SettingsWindow.Init();
        }

        // RESET BUTTON
        if (GUILayout.Button("Reset"))
        {
            Reset();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawChannelSidebar(int chId)
    {
        Channel __ch = _channels[chId];

        // Determine panel position
        float __x0 = GraphSettings.graphMargins.x + _graphRect.width + 50f;
        Color __color = _channels[chId].color;

        float __segmentHeight = 50f;
        float __verticalOffset = __segmentHeight * chId;

        // Draw panel
        Handles.color = GraphSettings.panelBackgroundColor;
        Rect __panelRect = new Rect(GraphSettings.graphMargins.x + _graphRect.width + 50f, __verticalOffset, position.width - __x0, __segmentHeight);
        Handles.DrawSolidRectangleWithOutline(__panelRect, GraphSettings.panelBackgroundColor, Color.grey);

        // Draw color header
        float __headerHeight = 22;
        Handles.color = GraphSettings.panelHeaderColor;
        Rect __statRect = new Rect(__panelRect.x, __panelRect.y, __panelRect.width, __headerHeight);
        Handles.DrawSolidRectangleWithOutline(__statRect, GraphSettings.panelHeaderColor, Color.grey);

        // Draw marker
        Handles.color = __color;
        Rect __markerRect = new Rect(__panelRect.x + 4, __panelRect.y + 4, __headerHeight - 8, __headerHeight - 8);
        Handles.DrawSolidRectangleWithOutline(__markerRect, __color, Color.black);

        // Draw name
        GUIStyle __titleStyle = new GUIStyle();
        __titleStyle.fontStyle = FontStyle.Bold;
        // With type
        //Handles.Label(new Vector2(panelRect.x + headerHeight + 3, panelRect.y + 5f), name + " (" + ch.TypeString + ")", titleStyle);
        Handles.Label(new Vector2(__panelRect.x + __headerHeight + 3, __panelRect.y + 5f), __ch.name, __titleStyle);

        // Draw buttons
        float __buttonsWidth = GraphSettings.BUTTON_SIZE * 4f;
        GUILayout.BeginArea(new Rect(__panelRect.x + __panelRect.width - __buttonsWidth * 1.27f, __panelRect.y + 2f, __panelRect.width - __buttonsWidth, GraphSettings.BUTTON_SIZE + 5f));
        GUILayout.BeginHorizontal();
        __ch.Show = DrawToggleButton(__ch.Show, __ch.name + "Show", _showTexture);
        //__ch.AutoScale = DrawToggleButton(__ch.AutoScale, __ch.name + "AutoScale", _autoScaleTexture);

        if (!__ch.replay)
        {
            __ch.LogToFile = DrawToggleButton(__ch.LogToFile, __ch.name + "LogToFile", _logTexture);
            //__ch.LogToConsole = DrawToggleButton(__ch.LogToConsole, __ch.name + "LogToConsole", _consoleTexture);
        }
        else
        {
            GUI.enabled = false;
            DrawToggleButton(false, __ch.name + "LogToFile", _logTexture);
            //DrawToggleButton(false, __ch.name + "LogToConsole", _consoleTexture);
            GUI.enabled = true;

            __ch.LogToFile = false;
            __ch.LogToConsole = false;
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(__panelRect.x + 5f, __panelRect.y + 26, __panelRect.width, __panelRect.height - 20));

        // Vertical Resolution
        GUILayout.BeginHorizontal();
        GUILayout.Label("Vert. Res.:", GUILayout.Width(70));

        __ch.rangeSlider = GUILayout.HorizontalSlider(__ch.rangeSlider, -GraphSettings.SLIDER_SENSITIVITY, GraphSettings.SLIDER_SENSITIVITY, GUILayout.Width(95));

        __ch.beingManuallyAdjusted = false;

        try
        {
            float __rangeInput = float.Parse(GUILayout.TextField(__ch.VerticalResolution.ToString("0.000"), 10, GUILayout.Width(70)));
            if (Mathf.Abs(__rangeInput) > __ch.VerticalResolution + 0.01f)
            {
                __ch.AutoScale = false;
                __ch.VerticalResolution = __rangeInput;
                __ch.beingManuallyAdjusted = true;
            }
        }
        catch { Debug.LogWarning("Input is not a number."); }

        // Check for mouse up
        if (_mouseState == MouseState.Up)
            __ch.rangeSlider = 0;

        __ch.VerticalResolution += __ch.VerticalResolution * __ch.rangeSlider * TimeKeeper.systemDeltaTime;
        __ch.VerticalResolution = Mathf.Max(0.1f, __ch.VerticalResolution);
        

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    /// <summary>
    /// Generates persistent button with toggle functionality.
    /// </summary>
    private bool DrawToggleButton(bool toggle, string key, Texture2D tex)
    {
        Color __def = GUI.color;
        if (toggle)
            GUI.color = GraphSettings.buttonActiveColor;
        
        // Draw button with supplied style
        if (GUILayout.Button(tex, _toggleButtonStyle, GUILayout.Width(GraphSettings.BUTTON_SIZE), GUILayout.Height(GraphSettings.BUTTON_SIZE)))
        {
            toggle = !toggle;
        }
        GUI.color = __def;

        return toggle;
    }

    private static void DrawHorizontalTag(Vector2 p, string text, Color color)
    {
        // Prevent jitter by drawing tags only on repaint (avoid layout)
        if (Event.current.type != EventType.Repaint) return;
        
        Handles.color = color;
        float __charWidth = 6f;
        Rect __tagRect = new Rect(p.x + 3, p.y - 7, __charWidth * text.Length + 5, 15);
        Handles.DrawSolidRectangleWithOutline(__tagRect, Color.white, color);
        Handles.Label(new Vector2(p.x + 4, p.y - 7), text);
    }

    private static void DrawHorizontalLabel(Vector2 p, string text, Color color)
    {
        if (Event.current.type != EventType.Repaint) return;
        
        Handles.color = color;
        Handles.Label(new Vector2(p.x + 4, p.y - 7), text);
    }

    /// <summary>
    /// Returns single color texture.
    /// </summary>
    private static Texture2D GenerateMonotoneTexture(Vector2 size, Color32 color)
    {
        Texture2D __tex = new Texture2D(32, 32);
        
        Color[] __px = __tex.GetPixels();
        for (int __i = 0; __i < __px.Length; __i++)
        {
            __px[__i] = GraphSettings.buttonHoverColor;
        }
        
        __tex.SetPixels(__px);
        __tex.Apply();
        return __tex;
    }

    /// <summary>
    /// Converts large values to more readable format.
    /// </summary>
    private static string FloatToCompact(float x)
    {
        string __first = " ";
        if(x < 0)
        {
            x = Mathf.Abs(x);
            __first = "-";
        }
        string __appendix = " ";
        float __xAbs = Mathf.Abs(x);

        if (__xAbs >= 1000000f)
        {
            x /= 1000000f;
            __appendix = "M";
        }
        else if (__xAbs >= 1000000000f)
        {
            x /= 1000000000f;
            __appendix = "G";
        }
        else if (__xAbs >= 1000f)
        {
            x /= 1000f;
            __appendix = "k";
        }

        return __first + x.ToString("0.000") + __appendix;
    }

    private static string OutOfScreenFormat(string s, bool outside)
    {
        if (!outside)
            return s;
        else
            return "-.-- ";
    }

    /// <summary>
    /// Saves channel color to EditorPrefs.
    /// </summary>
    private static void SetChannel(Color32 color, string name)
    {
        EditorPrefs.SetInt("GrapherCH" + name + "R", color.r);
        EditorPrefs.SetInt("GrapherCH" + name + "G", color.g);
        EditorPrefs.SetInt("GrapherCH" + name + "B", color.b);
    }

    /// <summary>
    /// Tries to get channel color from EditorPrefs by chanel name. If chanel hasn't been previously
    /// used it generates a random color.
    /// </summary>
    private static Color32 GetChannelColor(string name)
    {
        Color32 __res = new Color32();

        // Check for existing key
        if (EditorPrefs.HasKey("GrapherCH" + name + "R"))
        {
            __res.r = (byte)EditorPrefs.GetInt("GrapherCH" + name + "R");
            __res.g = (byte)EditorPrefs.GetInt("GrapherCH" + name + "G");
            __res.b = (byte)EditorPrefs.GetInt("GrapherCH" + name + "B");
        }
        // Key does not exist
        else
        {
            int __sum = 0;

            // Get random color, avoid too dark for visibility
            while(__sum < 300)
            {
                __sum = 0;
                __res.r = (byte)UnityEngine.Random.Range(40, 255);
                __res.g = (byte)UnityEngine.Random.Range(40, 255);
                __res.b = (byte)UnityEngine.Random.Range(40, 255);
                __sum = __res.r + __res.g + __res.b;
            }

            SetChannel(new Color32(__res.r, __res.g, __res.b, 255), name);
        }
        __res.a = 255;

        return __res;
    }
}
}