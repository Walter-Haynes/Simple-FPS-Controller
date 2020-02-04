using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Sound
    {
        public static void GetDataFromTime(this AudioClip clip, float[] data, float time) => clip.GetData(data, clip.TimeToPoint(time));

        public static int TimeToPoint(this AudioClip clip, float time)
        {
            int sampleIndex = (int)System.Math.Ceiling(clip.samples * (time / clip.length));
            return Mathf.Clamp(sampleIndex, 0, clip.samples);
        }

        public static float PointToTime(this AudioClip clip, int point) => (float)point / clip.samples * clip.length;
    }
}