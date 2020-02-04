using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Rendering
    {
        public static void SetEnabled(this LODGroup lodGroup, bool enabled)
        {
            if (lodGroup.enabled == enabled) { return; }
            
            lodGroup.enabled = enabled;
            lodGroup.SetRenderersEnabled(enabled);
        }

        public static void SetRenderersEnabled(this LODGroup lodGroup, bool enabled)
        {
            LOD[] lods = lodGroup.GetLODs();
            SetRenderersEnabled(lods, enabled);
        }

        private static void SetRenderersEnabled(LOD[] lods, bool enabled)
        {
            foreach (LOD lod in lods)
            {
                Renderer[] renderers = lod.renderers;
                foreach (Renderer r in renderers)
                {
                    if (r)
                        r.enabled = enabled;
                }
            }
        }

        public static int GetMaxLOD(this LODGroup lodGroup)
        {
            return lodGroup.lodCount - 1;
        }

        public static int GetCurrentLOD(this LODGroup lodGroup, Camera camera = null)
        {
            LOD[] lods = lodGroup.GetLODs();
            float relativeHeight = lodGroup.GetRelativeHeight(camera ?? Camera.current);

            int lodIndex = GetCurrentLOD(lods, lodGroup.GetMaxLOD(), relativeHeight, camera);

            return lodIndex;
        }

        public static float GetWorldSpaceSize(this LODGroup lodGroup)
        {
            return GetWorldSpaceScale(lodGroup.transform) * lodGroup.size;
        }

        private static int GetCurrentLOD(LOD[] lods, int maxLOD, float relativeHeight, Camera camera = null)
        {
            int lodIndex = maxLOD;

            for (int i = 0; i < lods.Length; i++)
            {
                LOD lod = lods[i];

                if (!(relativeHeight >= lod.screenRelativeTransitionHeight)) { continue; }
                
                lodIndex = i;
                break;
            }

            return lodIndex;
        }

        private static float GetWorldSpaceScale(Transform t)
        {
            Vector3 scale = t.lossyScale;
            float largestAxis = Mathf.Abs(scale.x);
            largestAxis = Mathf.Max(largestAxis, Mathf.Abs(scale.y));
            largestAxis = Mathf.Max(largestAxis, Mathf.Abs(scale.z));
            return largestAxis;
        }

        private static float DistanceToRelativeHeight(Camera camera, float distance, float size)
        {
            if (camera.orthographic){ return size * 0.5F / camera.orthographicSize; }

            float halfAngle = Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView * 0.5F);
            float relativeHeight = size * 0.5F / (distance * halfAngle);
            return relativeHeight;
        }

        private static float GetRelativeHeight(this LODGroup lodGroup, Camera camera)
        {
            float distance = (lodGroup.transform.TransformPoint(lodGroup.localReferencePoint) - camera.transform.position).magnitude;
            return DistanceToRelativeHeight(camera, distance, lodGroup.GetWorldSpaceSize());
        }
    }
}