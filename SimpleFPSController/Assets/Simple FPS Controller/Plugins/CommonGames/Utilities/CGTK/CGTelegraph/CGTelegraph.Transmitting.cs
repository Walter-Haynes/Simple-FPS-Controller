using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JetBrains.Annotations;

namespace CommonGames.Utilities.CGTK
{
    public sealed partial class CGTelegraph
    {
        [PublicAPI]
        public void Transmit<T>(in T message)
        {
            if(!Signals.TryGetValue(typeof(T).GetHashCode(), out List<IReceiver> __cachedSignals)) return;
            
            foreach(IReceiver __signal in __cachedSignals)
            {
                (__signal as IReceiver<T>)?.HandleSignal(arg: message);
            }
        }
        
        [PublicAPI]
        public void Send<T>(in T message) => Transmit(message);
        
        [PublicAPI]
        public void Broadcast<T>(in T message) => Transmit(message);
        
    }
}