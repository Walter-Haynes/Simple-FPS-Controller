using System;
using UnityEditor;

using JetBrains.Annotations;

namespace CommonGames.Tools
{
    public static class TimeKeeper
    {
        [PublicAPI]
        public static double systemTime;
        private static double _prevSystemTime;
        
        [PublicAPI]
        public static float systemDeltaTime;

        [PublicAPI]
        public static float Time { get; private set; } = 0;

        [PublicAPI]
        public static void Update()
        {
            systemTime = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
            if (_prevSystemTime == 0) _prevSystemTime = systemTime;
            systemDeltaTime = (float)(systemTime - _prevSystemTime);

            // Check if time should run
            if ((EditorApplication.isPaused || Grapphig.replayControl == Grapphig.ReplayControls.Pause)
                || (Grapphig.replayControl == Grapphig.ReplayControls.Stop && (!EditorApplication.isPaused && !EditorApplication.isPlaying)))
            {
                Time = EditorPrefs.GetFloat("GrapherTime", 0);
            }
            else
            {
                Time += systemDeltaTime;
                EditorPrefs.SetFloat("GrapherTime", Time);
            }

            _prevSystemTime = systemTime;
        }

        public static void Reset()
        {
            Time = 0;
            EditorPrefs.SetFloat("GrapherTime", 0);
        }

    }
}
