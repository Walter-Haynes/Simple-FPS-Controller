using System;
using UnityEditor;
using UnityEngine;

using static UnityEditor.AssetDatabase;

namespace CommonGames.Tools.CustomFolderIcons.Overlays
{
	/// <summary> Preview for Preset-Scriptable objects. </summary>
	public static class PreviewOverlay
	{
		
		/// <summary> Draw the preview of the folder. </summary>
		public static void Draw(string guid, Rect rect)
		{
			string __path = GUIDToAssetPath(guid);
			
			if(IsValidFolder(__path)) return; // ignore folders
			
			Type __type = GetMainAssetTypeAtPath(__path);
			
			if(!typeof(Style).IsAssignableFrom(__type)) return; // asset is not a preset
			
			Style __style = LoadAssetAtPath<Style>(__path);
			
			if(__style == null) return; // file was not a preset
			
			CustomFolderIcons.SquarifyIconRect(ref rect);
			__style.DrawPreview(rect);
		}
	}
}
