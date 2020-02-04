using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using CommonGames.Utilities.Extensions;

public static class MultiSceneLoader
{
	public static void LoadMultiScene(MultiScene multiScene, LoadSceneMode mode = LoadSceneMode.Single)
		=> multiScene.SceneAssets.For(sceneInfo => SceneManager.LoadScene(sceneInfo.Asset, mode));
}