using UnityEngine;
	
using JetBrains.Annotations;

#if ODIN_INSPECTOR
using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour; 
#endif

namespace CommonGames.Utilities
{

	/// <typeparam name="T"> Type of the Singleton. </typeparam>
	public abstract class Singleton<T> : MonoBehaviour 
		where T : Singleton<T>
	{
        #region Variables

        private static T _instance = null;

        /// <summary> The static reference to the Instance </summary>
        [PublicAPI]
        public static T Instance
        {
            get => _instance = _instance ? _instance : FindObjectOfType<T>();
            protected set => _instance = value;
        }

        /// <summary> Gets whether an Instance of this singleton exists </summary>
        [PublicAPI]
        public static bool InstanceExists => (_instance != null);

        #endregion

        #region Methods

        /// <summary> OnEnable method to associate Singleton with Instance </summary>
        protected virtual void OnEnable()
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

		#endregion
	}
}