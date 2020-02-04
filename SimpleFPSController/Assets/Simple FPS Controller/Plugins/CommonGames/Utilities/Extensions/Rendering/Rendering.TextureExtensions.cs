using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JetBrains.Annotations;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Rendering
    {
        [PublicAPI]
        public static T[,] GetDataFromPixels<T>(this Texture2D texture, in Vector2Int size, in Func<Color, int, int, T> func)
        {
            T[,] __values = new T[size.x, size.y];
            Color[] __colors = texture.GetPixels();

            Vector2Int __blockSize = new Vector2Int(x: texture.width / size.x, y: texture.height / size.y);

            for (int __indexX = 0; __indexX < size.x; __indexX++)
            {
                for (int __indexY = 0; __indexY < size.y; __indexY++)
                {
                    int __x = __blockSize.x * __indexX;
                    int __y = __blockSize.y * __indexY;
                    
                    Color __color = __colors[__x + __y * size.x * __blockSize.x];
                    __values[__indexX, __indexY] = func(arg1: __color, arg2: __indexX, arg3: __indexY);
                }
            }

            return __values;
        }
        
        [PublicAPI]
        public static Color GetAverageColor(this Texture2D tex)
        {
            Color[] __pixels = tex.GetPixels();
            
            Vector3 __avg = Vector3.zero;
            for(int __i = 0; __i < __pixels.Length; __i++)
            {
                __avg += new Vector3
                (
                    x: __pixels[__i].r,
                    y: __pixels[__i].g,
                    z: __pixels[__i].b
                );
            }
            
            __avg /= __pixels.Length;
            
            return new Color(r: __avg.x, g: __avg.y, b: __avg.z, 1);
        }
    }
}