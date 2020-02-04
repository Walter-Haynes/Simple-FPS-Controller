using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using CommonGames.Tools;

namespace CommonGames.Tools
{
// Replay
    public partial class Grapphig : EditorWindow
    {
        private static List<string> _replayFiles = new List<string>();
        private static List<Queue<Sample>> _replaySampleQueues = new List<Queue<Sample>>();

        public static ReplayControls prevControl = ReplayControls.Stop;
        public static ReplayControls replayControl = ReplayControls.Stop;

        public enum ReplayControls
        {
            Play,
            Pause,
            Stop,
            Reverse,
            Forward,
            Replay
        }

        private void ReplayInit()
        {
            // No replay files, ask user to add some
            if (_replayFiles.Count == 0 && _channels.Count == 0)
            {
                Debug.LogWarning("No replay files selected.");
            }

            // If files have been added, get samples
            if (_replayFiles.Count > 0)
            {
                for (int __i = 0; __i < _replayFiles.Count; __i++)
                {
                    List<Sample> __gs = FileHandler.LoadSamplesFromCsv(_replayFiles[__i]);
                    string __header = FileHandler.LoadHeaderFromCsv(_replayFiles[__i]);

                    // If replay file valid
                    if (__header != null)
                    {
                        string[] __hs = __header.Split(',');
                        if (__hs.Length == 5)
                        {
                            Channel __ch = null;
                            string __name = __hs[0] + " [Re]";

                            if ((__ch = _channels.Find(x => x.name == __name)) == null)
                            {
                                __ch = AddChannel();
                                __ch.name = __name;
                                __ch.color = new Color(float.Parse(__hs[2]), float.Parse(__hs[3]), float.Parse(__hs[4]),
                                    1f);
                                __ch.TimeScale = GraphSettings.HorizontalResolution;

                                // Self get
                                __ch.VerticalResolution = __ch.VerticalResolution;
                                __ch.LogToFile = false;
                                __ch.LogToConsole = false;

                                foreach (Sample __g in __gs)
                                {
                                    __ch.Enqueue(__g.d, __g.t);
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Invalid header size. Skipping.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Replay file is missing header. Skipping.");
                    }
                }
            }
        }

        private void UpdateReplay()
        {
            if (prevControl == ReplayControls.Stop && replayControl != ReplayControls.Stop)
            {
                TimeKeeper.Reset();
                foreach (Channel __ch in _channels)
                {
                    __ch.firstVisiblePointIndex = 0;
                    __ch.lastVisiblePointIndex = 0;
                }
            }

            prevControl = replayControl;
        }

        private void OpenFiles()
        {
            List<string> __files = FileHandler.BrowserOpenFiles();

            // Check if user has completed the action
            if (__files != null)
            {
                _replayFiles.AddRange(__files);
            }
            else
            {
                replayControl = ReplayControls.Stop;
            }
        }
    }
}