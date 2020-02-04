using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace CommonGames.Utilities.CGTK
{
    public sealed partial class CGTelegraph : IDisposable
    {
        //TODO: -Walter- Add Summaries.
        
        public static readonly CGTelegraph Instance = new CGTelegraph();
        
        private readonly Dictionary<int, List<IReceiver>> Signals = new Dictionary<int, List<IReceiver>>();
        
        public void Dispose() => Signals.Clear();
    }
}