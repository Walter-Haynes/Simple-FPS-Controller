using System;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Enumerable = System.Linq.Enumerable;

using JetBrains.Annotations;

namespace CommonGames.Utilities.Extensions
{
    //Most of the Extension Methods in these MeshExtensions are based on the MIT licensed Daniil Basmanov library:
    //"ProceduralToolkit" - https://github.com/Syomus/ProceduralToolkit
    //Edited and improved upon by Walter Haynes of Common Games
    
    public static partial class Rendering
    {
        /// <summary>
        /// Moves mesh vertices by <paramref name="vector"/>
        /// </summary>
        [PublicAPI] 
        public static void Move(this Mesh mesh, in Vector3 vector)
        {
            if(mesh == null)
            {
                throw new System.ArgumentNullException(nameof(mesh));
            }

            Vector3[] __vertices = mesh.vertices;
            for(int __i = 0; __i < __vertices.Length; __i++)
            {
                __vertices[__i] += vector;
            }

            mesh.vertices = __vertices;
        }

        /// <summary>
        /// Rotates mesh vertices by <paramref name="rotation"/>
        /// </summary>
        [PublicAPI] 
        public static void Rotate(this Mesh mesh, in Quaternion rotation)
        {
            if(mesh == null)
            {
                throw new System.ArgumentNullException(nameof(mesh));
            }

            Vector3[] __vertices = mesh.vertices;
            Vector3[] __normals = mesh.normals;
            for(int __i = 0; __i < __vertices.Length; __i++)
            {
                __vertices[__i] = rotation * __vertices[__i];
                __normals[__i] = rotation * __normals[__i];
            }

            mesh.vertices = __vertices;
            mesh.normals = __normals;
        }

        /// <summary>
        /// Scales mesh vertices uniformly by <paramref name="scale"/>
        /// </summary>
        [PublicAPI] 
        public static void Scale(this Mesh mesh, in float scale)
        {
            if(mesh == null)
            {
                throw new System.ArgumentNullException(nameof(mesh));
            }

            Vector3[] __vertices = mesh.vertices;
            for(int __i = 0; __i < __vertices.Length; __i++)
            {
                __vertices[__i] *= scale;
            }

            mesh.vertices = __vertices;
        }

        /// <summary>
        /// Scales mesh vertices non-uniformly by <paramref name="scale"/>
        /// </summary>
        [PublicAPI] 
        public static void Scale(this Mesh mesh, in Vector3 scale)
        {
            if(mesh == null)
            {
                throw new System.ArgumentNullException(nameof(mesh));
            }

            Vector3[] __vertices = mesh.vertices;
            Vector3[] __normals = mesh.normals;
            for(int __i = 0; __i < __vertices.Length; __i++)
            {
                __vertices[__i] = Vector3.Scale(__vertices[__i], scale);
                __normals[__i] = Vector3.Scale(__normals[__i], scale).normalized;
            }

            mesh.vertices = __vertices;
            mesh.normals = __normals;
        }

        /// <summary>
        /// Paints mesh vertices with <paramref name="color"/>
        /// </summary>
        [PublicAPI] 
        public static void Paint(this Mesh mesh, in Color color)
        {
            if(mesh == null)
            {
                #if UNITY_EDITOR || DEBUGBUILD
                throw new ArgumentNullException(nameof(mesh));
                #endif
            }

            Color[] __colors = new Color[mesh.vertexCount];
            for(int __i = 0; __i < mesh.vertexCount; __i++)
            {
                __colors[__i] = color;
            }

            mesh.colors = __colors;
        }

        /// <summary>
        /// Flips mesh faces
        /// </summary>
        [PublicAPI] 
        public static void FlipFaces(this Mesh mesh)
        {
            if(mesh == null)
            {
                #if UNITY_EDITOR || DEBUGBUILD
                throw new ArgumentNullException(nameof(mesh));
                #endif
            }

            mesh.FlipTriangles();
            mesh.FlipNormals();
        }

        /// <summary>
        /// Reverses the winding order of mesh triangles
        /// </summary>
        [PublicAPI]
        public static void FlipTriangles(this Mesh mesh)
        {
            if(mesh == null)
            {
                #if UNITY_EDITOR || DEBUGBUILD
                throw new ArgumentNullException(nameof(mesh));
                #endif
            }

            for(int __i = 0; __i < mesh.subMeshCount; __i++)
            {
                int[] __triangles = mesh.GetTriangles(__i);
                
                for(int __j = 0; __j < __triangles.Length; __j += 3)
                {
                    Swap(ref __triangles[__j], ref __triangles[__j + 1]);
                }

                mesh.SetTriangles(__triangles, __i);
            }
        }

        /// <summary>
        /// Reverses the direction of mesh normals
        /// </summary>
        [PublicAPI] 
        public static void FlipNormals(this Mesh mesh)
        {
            if(mesh == null)
            {
                #if UNITY_EDITOR || DEBUGBUILD
                throw new ArgumentNullException(nameof(mesh));
                #endif
            }

            Vector3[] __normals = mesh.normals;
            for(int __i = 0; __i < __normals.Length; __i++)
            {
                __normals[__i] = -__normals[__i];
            }

            mesh.normals = __normals;
        }

        /// <summary>
        /// Flips the UV map horizontally in the selected <paramref name="channel"/>
        /// </summary>
        [PublicAPI] 
        public static void FlipUvHorizontally(this Mesh mesh, in int channel = 0)
        {
            if(mesh == null)
            {
                #if UNITY_EDITOR || DEBUGBUILD
                throw new ArgumentNullException(nameof(mesh));
                #endif
            }

            List<Vector2> __list = new List<Vector2>();
            mesh.GetUVs(channel, __list);
            for(int __i = 0; __i < __list.Count; __i++)
            {
                __list[__i] = new Vector2(1 - __list[__i].x, __list[__i].y);
            }

            mesh.SetUVs(channel, __list);
        }

        /// <summary>
        /// Flips the UV map vertically in the selected <paramref name="channel"/>
        /// </summary>
        [PublicAPI] 
        public static void FlipUvVertically(this Mesh mesh, in int channel = 0)
        {
            if(mesh == null)
            {
                #if UNITY_EDITOR || DEBUGBUILD
                throw new ArgumentNullException(nameof(mesh));
                #endif
            }

            List<Vector2> __list = new List<Vector2>();
            mesh.GetUVs(channel, __list);
            for(int __i = 0; __i < __list.Count; __i++)
            {
                __list[__i] = new Vector2(__list[__i].x, 1 - __list[__i].y);
            }

            mesh.SetUVs(channel, __list);
        }

        /// <summary>
        /// Projects vertices on a sphere with the given <paramref name="radius"/> and <paramref name="center"/>, recalculates normals
        /// </summary>
        [PublicAPI] 
        public static void Spherify(this Mesh mesh, in float radius = 1f, in Vector3 center = default(Vector3))
        {
            if(mesh == null)
            {
                #if UNITY_EDITOR || DEBUGBUILD
                throw new ArgumentNullException(nameof(mesh));
                #endif
            }

            Vector3[] __vertices = mesh.vertices;
            Vector3[] __normals = mesh.normals;
            for(int __i = 0; __i < __vertices.Length; __i++)
            {
                __normals[__i] = (__vertices[__i] - center).normalized;
                __vertices[__i] = __normals[__i] * radius;
            }

            mesh.vertices = __vertices;
            mesh.normals = __normals;
        }
    }
}