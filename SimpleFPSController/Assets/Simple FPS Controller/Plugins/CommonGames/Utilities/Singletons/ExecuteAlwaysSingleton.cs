using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

namespace CommonGames.Utilities
{
    //Made by Wybren van den Akker
    //11/12/2019
    [ExecuteAlways]
    public class ExecuteAlwaysSingleton<T> : MonoBehaviour where T : ExecuteAlwaysSingleton<T>
    {
        /// <summary> The static reference to the Instance </summary>
		[PublicAPI]
        public static T Instance { get; protected set; }

        /// <summary> Gets whether an Instance of this singleton exists </summary>
        [PublicAPI]
        public static bool InstanceExists => (Instance != null);

        /// <summary> OnEnable method to associate Singleton with Instance </summary>
        protected virtual void Awake()
        {
            if (InstanceExists)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
            }
        }

        /// <summary> OnDestroy method to clear Singleton association </summary>
        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
