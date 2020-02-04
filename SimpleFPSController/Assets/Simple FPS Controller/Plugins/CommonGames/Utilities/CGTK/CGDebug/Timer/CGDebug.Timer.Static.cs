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
            [PublicAPI]
            public static void Start(string title, bool useMilliseconds = false)
            {
                if(TimerDebugs.ContainsKey(key: title))
                {
                    TimerDebugs[key: title].Stopwatch.Start();
                }
                else
                {
                    _lastStaticTimingDebug = title;
                    TimerDebugs[key: _lastStaticTimingDebug] =
                        new TimerDebugData(timingDebugTitle: title, precise: useMilliseconds);
                }
            }

            [PublicAPI]
            public static void Pause()
            {
                if(!TimerDebugs.ContainsKey(key: _lastStaticTimingDebug))
                {
                    LogWarning(
                        message: "TimingDebug caused: TimingDebug.Pause() call. There was no TimingDebug.Start()");
                    return;
                }

                TimerDebugs[key: _lastStaticTimingDebug].Stopwatch.Stop();
            }

            [PublicAPI]
            public static void Pause(in string title)
            {
                if(!TimerDebugs.ContainsKey(key: title))
                {
                    LogWarning(message: $"TimingDebug caused: TimingDebug Paused but not Started — {title}");
                    return;
                }

                TimerDebugs[key: title].Stopwatch.Stop();
            }

            [PublicAPI]
            public static void End()
            {
                if(!TimerDebugs.ContainsKey(key: _lastStaticTimingDebug))
                {
                    LogWarning(
                        message: "TimingDebug caused: TimingDebug.End() call. There was no TimingDebug.Start()");
                    return;
                }

                End(title: _lastStaticTimingDebug);
            }

            [PublicAPI]
            public static void End(in string title)
            {
                if(!TimerDebugs.ContainsKey(key: title))
                {
                    LogWarning(message: $"TimingDebug caused: TimingDebug not found — {title}");
                    return;
                }

                TimerDebugs[key: title].EndTimingDebug();
                TimerDebugs.Remove(key: title);

                if(title != _lastStaticTimingDebug)
                {
                    _lastStaticTimingDebug = string.Empty;
                }
            }
        }
    }
}