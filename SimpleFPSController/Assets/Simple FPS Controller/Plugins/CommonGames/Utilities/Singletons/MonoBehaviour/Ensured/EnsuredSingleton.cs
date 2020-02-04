/*
namespace CommonGames.Utilities
{       
    //Why don't virtual statics exist?....
    
    using UnityEngine;

    #if ODIN_INSPECTOR
    using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour; 
    #endif

    /// <typeparam name="T">Type of the Singleton</typeparam>
    public abstract class EnsuredSingleton<T> : MonoBehaviour where T : EnsuredSingleton<T>
    {
        private static readonly object obj = new object();
        
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting) { return null; }

                lock (obj)
                {
                    if (_instance != null) { return _instance; }
                    
                    
                    //#if UNITY_EDITOR
                    //_instance = UnityEditor.EditorUtility.CreateGameObjectWithHideFlags($"{typeof(T)} singleton", HideFlags.HideAndDontSave, typeof(T)).GetComponent<T>();
                    //#else
                    //GameObject singletonGameObject = new GameObject {name = $"{typeof(T)} singleton"};
                    //_instance = singletonGameObject.AddComponent<T>();
                    //#endif
                    
                    GameObject __singletonGameObject = new GameObject {name = $"{typeof(T)} singleton"};
                    //__singletonGameObject.hideFlags = HideFlags.HideInHierarchy;
                    _instance = __singletonGameObject.AddComponent<T>();
                    
                    return _instance;
                }
            }
                    
            protected set => _instance = value;
        }
        
        private static bool _applicationIsQuitting = false;

        protected virtual void OnEnable()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
            }
        }
        
        private void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
}
*/

namespace CommonGames.Utilities
{      
    //Why don't virtual statics exist?....
    
    using UnityEngine;

    #if ODIN_INSPECTOR
    using MonoBehaviour = Sirenix.OdinInspector.SerializedMonoBehaviour; 
    #endif
    
    using JetBrains.Annotations;

    /// <typeparam name="T">Type of the Singleton</typeparam>
    public abstract class EnsuredSingleton<T> : MonoBehaviour where T : EnsuredSingleton<T>
    {
        private static readonly object obj = new object();
        
        private static T _instance = null;
        
        [PublicAPI]
        public static T Instance
        {
            get
            {
                if (_beingDestroyed) { return null; }
                    
                if (_instance != null) { return _instance; }

                GameObject __singletonGameObject = new GameObject {name = $"{typeof(T)} singleton"};
                _instance = __singletonGameObject.AddComponent<T>();

                return _instance;
                
                /*
                //If we're in Edit-Mode.
                if(Application.isEditor && !Application.isPlaying)
                {
                    if (_beingDestroyed) { return null; }
                    
                    if (_instance != null)
                    {
                        Destroy(_instance.gameObject);
                    }
                    else
                    {
                        goto CreateSingletonObject;
                    }
                }
                else
                {
                    if (_beingDestroyed) { return null; }
                    
                    if (_instance != null) { return _instance; }
                
                    //CreateSingletonObject:
                    
                    GameObject __singletonGameObject = new GameObject {name = $"{typeof(T)} singleton"};
                    _instance = __singletonGameObject.AddComponent<T>();   
                }

                return _instance;
                */
            }
                    
            protected set => _instance = value;
        }
        
        // ReSharper disable once StaticMemberInGenericType
        private static bool _beingDestroyed = false;

        protected virtual void OnEnable()
        {
            _beingDestroyed = default;
            
            if (_instance != null)
            {
                Destroy(obj: gameObject);
            }
            else
            {
                Instance = (T)this;
            }
        }
        
        private void OnDestroy()
        {
            _beingDestroyed = true;
        }
    }
}