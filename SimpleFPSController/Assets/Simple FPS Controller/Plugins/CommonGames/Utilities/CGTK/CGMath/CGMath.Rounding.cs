namespace CommonGames.Utilities.CGTK
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Utilities.Extensions;
    using static System.Math;

    public static partial class CGMath
    {
        public enum RoundingMode
        {
            RoundDown,
            RoundUp,
            ToEven,
            ToOdd
        };

        public static float Round(float value, RoundingMode mode = RoundingMode.ToEven)
        {
            switch (mode)
            {
                case RoundingMode.ToEven:
                    return value.RoundToMultipleOf(2);
                case RoundingMode.ToOdd:
                    return value.RoundToMultipleOf(1);
                case RoundingMode.RoundUp:
                    return value.Ceil();
                case RoundingMode.RoundDown:
                    return value.Floor();
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static int RoundToInt(float value, RoundingMode mode = RoundingMode.ToEven)
        {
            switch (mode)
            {
                case RoundingMode.ToEven:
                    return value.RoundToMultipleIntOf(2);
                case RoundingMode.ToOdd:
                    return value.RoundToMultipleIntOf(1);
                case RoundingMode.RoundUp:
                    return value.CeilToInt();
                case RoundingMode.RoundDown:
                    return value.FloorToInt();
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static float RoundToMultipleOf(float value, float multiple)
            => multiple.Approximately(0) ? value : (value / multiple).Round() * multiple;

        public static int NearestMultipleOf(int value, int multiple)
        {
            int mod = value % multiple;
            float midPoint = multiple / 2.0f;

            if (mod > midPoint)
            {
                return value + (multiple - mod);
            }
            else
            {
                return value - mod;
            }
        }

        public static int RoundToMultipleIntOf(float value, int multiple)
            => (multiple == 0) ? value.RoundToInt() : (value / multiple).RoundToInt() * multiple;

        //return (value % 0.5f).Approximately(0) ? Mathf.Ceil(value) : Mathf.Floor(value);

        /*
        public static float Round(float value, System.MidpointRounding mode = System.MidpointRounding.ToEven)
        {
            CGMath.Round(.5f, System.MidpointRounding.ToEven);
    
            return 0f;
        }
        */
    }
    
}