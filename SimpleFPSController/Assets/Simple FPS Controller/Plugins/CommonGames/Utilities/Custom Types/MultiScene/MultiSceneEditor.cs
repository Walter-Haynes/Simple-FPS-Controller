//      
//   ^\.-
//  c====ɔ   Crafted with <3 by Nate Tessman
//   L__J    nate@madgvox.com
// 

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Callbacks;
using UnityEditorInternal;

using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if ODIN_INSPECTOR
using Editor = Sirenix.OdinInspector.Editor.OdinEditor;
#endif

//using static MultiScene;

[CustomEditor(typeof(MultiScene))]
public class MultiSceneEditor : Editor
{
	private static class Styles
	{
		public static readonly GUIStyle DragInfoStyle;

		static Styles()
		{
			DragInfoStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel) {wordWrap = true};
		}
	}

	[MenuItem("Assets/Create/Multi-Scene", false, 201)]
	private static void CreateMultiScene()
	{
		MultiScene __multi = CreateInstance<MultiScene>();
		__multi.name = "New Multi-Scene";

		UnityEngine.Object __parent = Selection.activeObject;

		string __directory = "Assets";
		if(__parent != null)
		{
			__directory = AssetDatabase.GetAssetPath(__parent.GetInstanceID());
			if(!Directory.Exists(__directory))
			{
				__directory = Path.GetDirectoryName(__directory);
			}
		}

		ProjectWindowUtil.CreateAsset(__multi, $"{__directory}/{__multi.name}.asset");
	}

	[MenuItem("Edit/Multi-Scene From Open Scenes %#&s", false, 0)]
	private static void CreatePresetFromOpenScenes()
	{
		MultiScene __multi = CreateInstance<MultiScene>();
		__multi.name = "New Multi-Scene";

		Scene __activeScene = SceneManager.GetActiveScene();
		int __sceneCount = SceneManager.sceneCount;

		for(int __i = 0; __i < __sceneCount; __i++)
		{
			Scene __scene = SceneManager.GetSceneAt(__i);

			SceneAsset __sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(__scene.path);
			
			if(__activeScene == __scene)
			{
				__multi.ActiveScene = __sceneAsset;
			}

			__multi.SceneAssets.Add(new MultiScene.SceneInfo(__sceneAsset, __scene.isLoaded));
		}

		string __directory = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
		bool __isDirectory = Directory.Exists(__directory);
		if(!__isDirectory)
		{
			__directory = Path.GetDirectoryName(__directory);
		}

		ProjectWindowUtil.CreateAsset(__multi, $"{__directory}/{__multi.name}.asset");
	}

	[OnOpenAsset(1)]
	private static bool OnOpenAsset(int id, int line)
	{
		UnityEngine.Object __obj = EditorUtility.InstanceIDToObject(id);
		if(__obj is MultiScene __scene)
		{
			OpenMultiScene(__scene, Event.current.alt);
			return true;
		}

		if(!(__obj is SceneAsset)) { return false; }
		if(!Event.current.alt) { return false; }
		
		EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(__obj.GetInstanceID()), OpenSceneMode.Additive);
		
		return true;

	}

	private new MultiScene _target;
	private ScenePresetList _list;

	#if ODIN_INSPECTOR
	protected override void OnEnable()
	{
		base.OnEnable();
	#else
	protected void OnEnable()
	{
	#endif
		
		_target = (MultiScene)base.target;
		_list = new ScenePresetList(_target, _target.SceneAssets, typeof(SceneAsset));
	}

	private static void OpenMultiScene(MultiScene obj, bool additive)
	{
		Scene __activeScene = default;
		
		if(additive || EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			List<string> __firstUnloadedScenes = new List<string>();
			bool __inFirstUnloadedScenes = true;
			Scene __firstLoadedScene = default;
			
			foreach(MultiScene.SceneInfo __info in obj.SceneAssets)
			{
				if(__info.Asset == null) { continue; }
				
				string __path = AssetDatabase.GetAssetPath(((SceneAsset)__info.Asset).GetInstanceID());
				OpenSceneMode __mode = OpenSceneMode.Single;
				bool __isActiveScene =(SceneAsset)__info.Asset == obj.ActiveScene;

				bool __exitedFirstUnloadedScenes = false;
				if(__inFirstUnloadedScenes)
				{
					if(!__isActiveScene && !__info.LoadScene)
					{
						__firstUnloadedScenes.Add(__path);
						continue;
					}

					__inFirstUnloadedScenes = false;
					__exitedFirstUnloadedScenes = true;
				}

				if((!__exitedFirstUnloadedScenes) || additive)
				{
					__mode = ((!additive && __isActiveScene) || __info.LoadScene)
						? OpenSceneMode.Additive
						: OpenSceneMode.AdditiveWithoutLoading; 
				}

				Scene __scene = EditorSceneManager.OpenScene(__path, __mode);

				if(__isActiveScene) __activeScene = __scene;
				if(__exitedFirstUnloadedScenes) __firstLoadedScene = __scene;
			}

			foreach(string __path in __firstUnloadedScenes)
			{
				Scene __scene = EditorSceneManager.OpenScene(__path, OpenSceneMode.AdditiveWithoutLoading);
				
				if(__firstLoadedScene.IsValid())
				{
					EditorSceneManager.MoveSceneBefore(__scene, __firstLoadedScene);
				}
			}
			
		}
		if(!additive && __activeScene.IsValid())
		{
			SceneManager.SetActiveScene(__activeScene);
		}
	}

	private static Scene SceneAssetToScene(UnityEngine.Object asset)
	{
		return SceneManager.GetSceneByPath(AssetDatabase.GetAssetPath(asset));
	}

	protected override void OnHeaderGUI()
	{
		if(_target.SceneAssets == null) { return; }
		base.OnHeaderGUI();	
	}

	public override void OnInspectorGUI()
	{
		if(_target.SceneAssets == null) { return; }
		EditorGUI.BeginChangeCheck();
		_list.DoLayoutList();

		Event __evt = Event.current;

		switch(__evt.type)
		{
			case EventType.DragPerform:
			case EventType.DragUpdated:
			{
				UnityEngine.Object[] __objects = DragAndDrop.objectReferences;
				bool __canDrag = false;
				foreach(UnityEngine.Object __obj in __objects)
				{
					if(!(__obj is SceneAsset))
					{
						__canDrag = false;
						break;
					}

					__canDrag = true;
				}

				if(__canDrag)
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Move;

					if(__evt.type == EventType.DragPerform)
					{
						Undo.RecordObject(_target, "Add Scenes");
						GUI.changed = true;
						foreach(UnityEngine.Object __obj in __objects)
						{
							SceneAsset __scene =(SceneAsset)__obj;
							int __index = _target.SceneAssets.FindIndex(i => i.Asset == __scene);
							if(__index > -1)
							{
								_target.SceneAssets.RemoveAt(__index);
							}
							
							_target.SceneAssets.Add(new MultiScene.SceneInfo(__scene));
						}
					}
				}

				if(__canDrag)
				{
					DragAndDrop.AcceptDrag();
					__evt.Use();
				}

				break;
			}
			
			case EventType.DragExited:
				Repaint();
				break;
			case EventType.MouseDown:
				break;
			case EventType.MouseUp:
				break;
			case EventType.MouseMove:
				break;
			case EventType.MouseDrag:
				break;
			case EventType.KeyDown:
				break;
			case EventType.KeyUp:
				break;
			case EventType.ScrollWheel:
				break;
			case EventType.Repaint:
				break;
			case EventType.Layout:
				break;
			case EventType.Ignore:
				break;
			case EventType.Used:
				break;
			case EventType.ValidateCommand:
				break;
			case EventType.ExecuteCommand:
				break;
			case EventType.ContextClick:
				break;
			case EventType.MouseEnterWindow:
				break;
			case EventType.MouseLeaveWindow:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if(EditorGUI.EndChangeCheck()){ EditorUtility.SetDirty(_target); }

		//EditorGUILayout.Space();
		//GUILayout.Label("Drag and drop scenes into the inspector window to append them to the list.", Styles.dragInfoStyle);
	}

	private sealed class ScenePresetList : ReorderableList
	{
		private static readonly GUIContent LoadSceneContent = new GUIContent(string.Empty, "Load Scene");
		private static readonly GUIContent ActiveSceneContent = new GUIContent(string.Empty, "Set Active Scene");

		private readonly MultiScene _target;
		private new readonly List<MultiScene.SceneInfo> _list;

		public ScenePresetList(MultiScene target, List<MultiScene.SceneInfo> elements, Type elementType) 
			: base(elements, elementType, true, false, true, true)
		{
			this._target = target;
			_list = elements;

			drawElementCallback = DrawElement;
			drawHeaderCallback = DrawHeader;
			onRemoveCallback = OnRemove;
			onAddCallback = OnAdd;
		}

		private static void DrawHeader(Rect rect)
		{
			const int __TOGGLE_WIDTH = 17;

			Rect __loadSceneRect = new Rect(rect.x + rect.width - __TOGGLE_WIDTH * 2, rect.y, __TOGGLE_WIDTH, rect.height);
			Rect __activeSceneRect = new Rect(rect.x + rect.width - __TOGGLE_WIDTH, rect.y, __TOGGLE_WIDTH, rect.height);
			Rect __labelRect = new Rect(rect.x, rect.y, rect.width -(__TOGGLE_WIDTH * 2) - 5, EditorGUIUtility.singleLineHeight);

			GUI.Label(__labelRect, "Scenes");
			GUI.Label(__loadSceneRect, "L");
			GUI.Label(__activeSceneRect, "A");
		}

		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.y += 2;

			const int __TOGGLE_WIDTH = 17;

			Rect __loadSceneRect = new Rect(rect.x + rect.width - __TOGGLE_WIDTH * 2, rect.y, __TOGGLE_WIDTH, rect.height);
			Rect __activeSceneRect = new Rect(rect.x + rect.width - __TOGGLE_WIDTH, rect.y, __TOGGLE_WIDTH, rect.height);
			Rect __labelRect = new Rect(rect.x, rect.y, rect.width -(__TOGGLE_WIDTH * 2) - 5, EditorGUIUtility.singleLineHeight);

			MultiScene.SceneInfo __info = _list[index];
			 EditorGUI.BeginChangeCheck();
			SceneAsset __scene =(SceneAsset)EditorGUI.ObjectField(__labelRect, __info.Asset, typeof(SceneAsset), false);
			
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_target, "Change Scene");
				__info.Asset = __scene;
				_target.SceneAssets[index] = __info;
			}

			bool __active = __info.Asset == _target.ActiveScene;
			GUI.enabled = !__active;
			EditorGUI.BeginChangeCheck();
			bool __loadScene = GUI.Toggle(__loadSceneRect, __info.LoadScene, LoadSceneContent);
			
			if(EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_target, "Change Load Scene");
				__info.LoadScene = __loadScene;
				_target.SceneAssets[index] = __info;
			}
			GUI.enabled = true;

			EditorGUI.BeginChangeCheck();
			bool __setActive = GUI.Toggle(__activeSceneRect, __active, ActiveSceneContent);
			
			if(!EditorGUI.EndChangeCheck()) { return; }
			if(!__setActive) { return; }
			
			Undo.RecordObject(_target, "Change Active Scene");
			_target.ActiveScene = __info.Asset;
		}

		private void OnRemove(ReorderableList l)
		{
			Undo.RecordObject(_target, "Remove Scene");
			MultiScene.SceneInfo __removed = _target.SceneAssets[ index ];
			if(__removed.Asset == _target.ActiveScene)
			{
				_target.ActiveScene = null;
			}
			_target.SceneAssets.RemoveAt(index);
		}

		private void OnAdd(ReorderableList l)
		{
			index = _list.Count;
			Undo.RecordObject(_target, "Add Scene");
			_list.Add(default);
		}
	}
}

#endif