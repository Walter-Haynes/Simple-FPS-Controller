//      
//   ^\.-
//  c====ɔ   Crafted with <3 by Nate Tessman
//   L__J    nate@madgvox.com
// 
//				 Edited with <3 by Walter Haynes
//

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
    
#if ODIN_INSPECTOR
    
using ScriptableObject = Sirenix.OdinInspector.SerializedScriptableObject;
    
#endif

using CommonGames.Utilities.CGTK;

public class MultiScene : ScriptableObject
{
	[Serializable]
	public struct SceneInfo
	{
		public SceneReference Asset;
		public bool LoadScene;

		public SceneInfo(SceneReference asset, bool loadScene = true)
		{
			this.Asset = asset;
			this.LoadScene = loadScene;
		}
	}
	
	public SceneReference ActiveScene;
	public List<SceneInfo> SceneAssets = new List<SceneInfo>();
	
	public void Load()//(LoadSceneMode mode = LoadSceneMode.Additive)
	{
		//sceneAssets.For(sceneInfo => SceneManager.LoadScene(sceneInfo.asset, LoadSceneMode.Additive));

		for (int __index = 0; __index < SceneAssets.Count; __index++)
		{
			SceneInfo __sceneInfo = SceneAssets[__index];
			
			SceneManager.LoadSceneAsync(__sceneInfo.Asset, (__index == 0)? LoadSceneMode.Single : LoadSceneMode.Additive);
		}
	}
}