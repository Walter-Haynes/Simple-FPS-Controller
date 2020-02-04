using System;
using System.Linq;
using UnityEngine;

namespace CommonGames.Tools.CustomFolderIcons
{
	/// <summary> Defines an icon consisting of several layers. </summary>
	[Serializable]
	public class MultiLayerIcon : IICon
	{
		//TODO: Remove Linq
		
		public LayerIcon[] layers = new LayerIcon[0];

		public bool IsEmpty => (layers == null || layers.Length == 0);

		public void Add(LayerIcon layer)
		{
			if(layers == null)
			{
				layers = new[] {layer};
				return;
			}
			
			int __size = layers.Length;
			Array.Resize(ref layers, __size + 1);
			layers[__size] = layer;
		}

		/// <summary> Render this icon in the specified region. </summary>
		/// <param name="rect"></param>
		public void Draw(Rect rect)
		{
			if(layers == null) return;
			
			foreach(LayerIcon __layer in layers)
			{
				__layer?.Draw(rect);
			}
		}

		public MultiLayerIcon Clone()
			=> new MultiLayerIcon
				{
					layers = layers.Select(layer => layer.Clone()).ToArray()
				};
	}
}
