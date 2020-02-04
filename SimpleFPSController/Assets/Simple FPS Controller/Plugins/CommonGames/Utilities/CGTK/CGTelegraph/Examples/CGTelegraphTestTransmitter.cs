using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if  ODIN_INSPECTOR
    
using Sirenix.OdinInspector;

#endif

namespace CommonGames.Utilities.CGTK.Examples
{

    public struct TestTelegraph
    {
        public GameObject GameObject;
        public string Message;
    }
    
    public class CGTelegraphTestTransmitter : MonoBehaviour
    {
        private void Update()
        {
            if(!Input.GetKeyDown(KeyCode.Space)) return;
            
            SendTelegraph();
        }

        #if ODIN_INSPECTOR
        [Button(name: "Send Telegraph")]
        #endif
        private void SendTelegraph()
        {
            TestTelegraph __signalExample = new TestTelegraph
            {
                GameObject = gameObject,
                Message = $"Ik heb {CGRandom.RandomRange(50f,200f).ToString()} neusharen in mijn neus", 
            };

            CGTelegraph.Instance.Transmit(__signalExample);
        }
    }
}