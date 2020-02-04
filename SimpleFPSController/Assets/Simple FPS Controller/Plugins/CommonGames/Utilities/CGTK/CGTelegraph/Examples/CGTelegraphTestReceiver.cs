using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if  ODIN_INSPECTOR
    
using Sirenix.OdinInspector;

#endif

namespace CommonGames.Utilities.CGTK.Examples
{
    using Extensions;

    public class CGTelegraphTestReceiver : MonoBehaviour, IReceiver<TestTelegraph>
    {
        //TODO: -Walter-, make OnEnable and OnDisable a requirement..
        
        private void OnEnable() => CGTelegraph.Instance.Add(this);
        private void OnDisable() => CGTelegraph.Instance.Remove(this);
        
        public void HandleSignal(TestTelegraph damageTelegraph) => ($"{damageTelegraph.GameObject.name} zegt: {damageTelegraph.Message}!").Log();
    }
}