using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using UnityEngine;

using JetBrains.Annotations;

namespace CommonGames.Utilities.CGTK
{
    public static partial class CGDebug
    {

        public partial class Timer : IDisposable
        {
            #region Variables

            private static readonly Dictionary<string, TimerDebugData> TimerDebugs = new Dictionary<string, TimerDebugData>();

            private readonly string _disposableTimingDebug;

            private static string _lastStaticTimingDebug = string.Empty;

            #endregion

            #region Custom Types

            private struct TimerDebugData
            {
                private readonly string _timingDebugTitle;
                private readonly bool _precise;
                public readonly Stopwatch Stopwatch;

                private static readonly StringBuilder StringBuilder = new StringBuilder();

                public TimerDebugData(string timingDebugTitle, bool precise)
                {
                    _timingDebugTitle = timingDebugTitle;
                    _precise = precise;
                    Stopwatch = Stopwatch.StartNew();
                }

                public void EndTimingDebug()
                {
                    long __ms = Stopwatch.ElapsedMilliseconds;
                    float __elapsedVal = _precise ? __ms : __ms / 1000f;
                    string __valMark = _precise ? "ms" : "s";

                    StringBuilder.Length = 0;

                    StringBuilder.Append(value: "Timer <color=red>")
                        .Append(value: _timingDebugTitle)
                        .Append(value: "</color>: ")
                        .Append(value: __elapsedVal)
                        .Append(value: __valMark);

                    LogWarning(message: StringBuilder);
                }
            }

            #endregion
        }
    }
}