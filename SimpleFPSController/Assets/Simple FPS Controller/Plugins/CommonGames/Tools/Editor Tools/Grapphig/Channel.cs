using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CommonGames.Tools
{
    public class Channel
    {
        public Grapphig g;
        public List<Sample> rawSampleList;

        public int id;
        public string name;
        public Color color;
        public float tagY;
        public float tagX;

        private float _timeScale = 5;
        private float _yMax;
        private float _yMin;
        public int sampleNo = 0;
        public bool beingManuallyAdjusted = false;
        public float autoScaleResolution = Grapphig.GraphSettings.DefaultVerticalResolution;

        public int lastFrame = 0;

        // Sliders
        public float timeScaleSlider;
        public float rangeSlider;

        // Marker
        public Vector2 pointAtMousePosition;

        public int lastVisiblePointIndex;
        public int firstVisiblePointIndex;

        public Sample newestSample;
        public object newestObj;

        // Scale
        public float xScale;
        public float yScale;

        public float xOffset;
        public float yOffset;

        // Replay
        public bool replay = false;
        public bool replayEnded = false;

        public void Init()
        {
            sampleNo = 0;
            rawSampleList = new List<Sample>();
        }

        public bool Show
        {
            get
            {
                string __key = $"Grapphig{name}Show";
                return !EditorPrefs.HasKey(__key) || EditorPrefs.GetBool(__key, true);
            }
            set
            {
                string __key = $"Grapphig{name}Show";
                EditorPrefs.SetBool(__key, value);
            }
        }

        public bool LogToFile
        {
            get
            {
                string __key = $"Grapphig{name}LogToFile";
                
                return EditorPrefs.HasKey(__key) 
                    ? EditorPrefs.GetBool(__key, true) 
                    : Grapphig.GraphSettings.DefaultLogToFile == 1 ? true : false;
            }
            set
            {
                string __key = $"Grapphig{name}LogToFile";
                EditorPrefs.SetBool(__key, value);
            }
        }

        public bool LogToConsole
        {
            get
            {
                string __key = $"Grapphig{name}LogToConsole";
                return EditorPrefs.HasKey(__key) ? EditorPrefs.GetBool(__key, true) 
                    : Grapphig.GraphSettings.DefaultLogToConsole == 1 ? true : false;
            }
            set
            {
                string __key = $"Grapphig{name}LogToConsole";
                EditorPrefs.SetBool(__key, value);
            }
        }

        public bool AutoScale
        {
            get
            {
                string __key = $"Grapphig{name}AutoScale";
                return !EditorPrefs.HasKey(__key) || EditorPrefs.GetBool(__key, true);
            }
            set
            {
                string __key = "Grapphig" + name + "AutoScale";
                EditorPrefs.SetBool(__key, value);
            }
        }

        public float TimeScale
        {
            get => _timeScale;
            set => _timeScale = Mathf.Clamp(value, 0.5f, 3600f);
        }

        public float VerticalResolution
        {
            get
            {
                string __key = "Grapphig" + name + "verticalResolution";
                return EditorPrefs.HasKey(__key) ? EditorPrefs.GetFloat(__key) : Grapphig.GraphSettings.DefaultVerticalResolution;
            }
            set
            {
                string __key = "Grapphig" + name + "verticalResolution";
                float __range = Mathf.Clamp(value, 0.01f, 100000000f);
                EditorPrefs.SetFloat(__key, __range);
            }
        }

        public float YMin => _yMin;
        public float YMax => _yMax;

        public Channel(int id)
        {
            this.id = id;
        }

        public void Enqueue(float x, float t)
        {
            Sample __sample = new Sample(x, t);

            if (rawSampleList == null) rawSampleList = new List<Sample>();
            rawSampleList.Add(__sample);
            sampleNo++;

            // Determine max and min
            if (sampleNo <= 2f)
            {
                _yMax = x;
                _yMin = x;
            }
            else if (x > _yMax)
            {
                _yMax = x;
            }
            else if (x < _yMin)
            {
                _yMin = x;
            }

            // Get auto range
            autoScaleResolution = Mathf.Max(Mathf.Abs(_yMin), Mathf.Abs(_yMax)) * 2f;
        }

        public Sample[] GetSamples()
        {
            if (rawSampleList != null)
            {
                return rawSampleList.ToArray();
            }
            else
            {
                return null;
            }
        }

        public void ResetSamples()
        {
            rawSampleList.Clear();
            sampleNo = 0;
            _timeScale = Grapphig.GraphSettings.HorizontalResolution;
            VerticalResolution = Grapphig.GraphSettings.DefaultVerticalResolution;
        }
    }
}