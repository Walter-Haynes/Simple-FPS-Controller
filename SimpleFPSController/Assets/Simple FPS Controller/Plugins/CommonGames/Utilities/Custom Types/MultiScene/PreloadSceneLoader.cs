using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadSceneLoader : MonoBehaviour
{
    #if ODIN_INSPECTOR && UNITY_EDITOR
    [Sirenix.OdinInspector.RequiredAttribute]
    [Sirenix.OdinInspector.InlineEditorAttribute]
    #endif
    public MultiScene MultiScene = null;
    
    private void Start()
    {
        if(MultiScene == null) return;
        
        MultiSceneLoader.LoadMultiScene(multiScene: MultiScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
