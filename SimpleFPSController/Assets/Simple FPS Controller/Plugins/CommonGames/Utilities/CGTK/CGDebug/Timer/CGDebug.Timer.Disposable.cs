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
            public Timer(in string title, in bool useMilliseconds = true)
            {
                _disposableTimingDebug = title;
                
                TimerDebugs[key: _disposableTimingDebug] = new TimerDebugData(timingDebugTitle: title, precise: useMilliseconds);
            }

            [PublicAPI]
            public void Dispose()
            {
                TimerDebugs[key: _disposableTimingDebug].EndTimingDebug();
                TimerDebugs.Remove(key: _disposableTimingDebug);
            }
        }
    }
}