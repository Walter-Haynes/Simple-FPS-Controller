using System;
using UnityEngine;

using Sirenix.OdinInspector;

namespace CommonGames.Tools.CustomFolderIcons
{
	/// <summary> Represents a single layer of a multi layered icon. </summary>
	[Serializable]
	public class LayerIcon : IICon
	{
		#region Variables
		
		//[HideLabel, PreviewField(CustomFolderIcons.LARGE_ICON_SIZE)]
		public Texture2D icon;
		
		public Color color = Color.white;
		
		/// <summary>
		/// The offset relative the render region.
		/// (0-1) Top-Left to Bottom-Right
		/// </summary>
		public Vector2 offset;
		
		public Vector2 scale = Vector2.one;
		
		#endregion

		#region Methods

		public LayerIcon(){ }

		public LayerIcon(Texture2D texture)
		{
			icon = texture;
		}

		/// <summary> Render this icon in the specified region. </summary>
		/// <param name="rect"></param>
		public void Draw(Rect rect)
		{
			if(icon == null) return;
			
			Color __color = GUI.color;
			GUI.color = color;
			
			Vector2 __pos = rect.position + offset * rect.width; // width == height
			Vector2 __size = new Vector2(rect.width * scale.x, rect.height * scale.y);
			
			GUI.DrawTexture(new Rect(__pos, __size), icon);
			GUI.color = __color;
		}

		public LayerIcon Clone()
			=> new LayerIcon
			{
				icon = icon,
				color = color,
				offset = offset,
				scale = scale
			};
		
		#endregion
	}
}
