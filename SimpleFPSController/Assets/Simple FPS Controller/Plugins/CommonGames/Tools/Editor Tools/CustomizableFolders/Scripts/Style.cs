using System.Linq;
using UnityEngine;

using Sirenix.OdinInspector;

namespace CommonGames.Tools.CustomFolderIcons
{
	/// <summary> A Multi-Layer icon preset </summary>
	[CreateAssetMenu(menuName = "CustomFolderIcons/Style")]
	public class Style : ScriptableObject
	{
		#region Variables
		
		[TabGroup("Size", "Large")]
		[FoldoutGroup("Size/Large/Closed")]
		public MultiLayerIcon largeIconClosed = new MultiLayerIcon();
		[FoldoutGroup("Size/Large/Open")]
		public MultiLayerIcon largeIconOpen = new MultiLayerIcon();
		
		[TabGroup("Size", "Small")]
		[FoldoutGroup("Size/Small/Closed")]
		public MultiLayerIcon smallIconClosed = new MultiLayerIcon();
		[FoldoutGroup("Size/Small/Open")]
		public MultiLayerIcon smallIconOpen = new MultiLayerIcon();
		
		#endregion

		#region Methods

		public bool Drawable()
			=> (largeIconClosed?.layers != null) && 
			   largeIconClosed.layers.Any(iconLayer => (((iconLayer.color.a > 0) 
			                                             && iconLayer.icon != null) 
															&& iconLayer.scale.magnitude > 0));

		public void Draw(Rect rect)
		{
			if (rect.height > CustomFolderIcons.SMALL_ICON_SIZE || smallIconClosed.IsEmpty)
			{
				largeIconClosed.Draw(rect);
			}
			else
			{
				smallIconClosed.Draw(rect);
			}
		}

		public void DrawPreview(Rect rect)
		{
			CustomFolderIcons.DrawBackgroundRect(rect);
			
			largeIconClosed.Draw(rect);
		}
		
		#endregion
		
	}
}
