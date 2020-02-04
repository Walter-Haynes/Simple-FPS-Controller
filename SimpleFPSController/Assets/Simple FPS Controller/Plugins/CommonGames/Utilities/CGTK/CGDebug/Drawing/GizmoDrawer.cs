namespace CommonGames.Utilities.CGTK
{
    using CommonGames.Utilities.Extensions;
    using UnityEngine;
    using System.Collections.Generic;
    using CommonGames.Utilities;
    using static CommonGames.Utilities.CGTK.CGDebug;

    using JetBrains.Annotations;

    public static partial class CGDebug
    {
        public sealed class GizmoDrawer : EnsuredSingleton<GizmoDrawer>
        {
            private readonly List<Gizmo> _gizmos = new List<CGDebug.Gizmo>();

            private void Update()
                => _gizmos.For(gizmo => gizmo.durationLeft -= Time.deltaTime);

            private void OnDrawGizmos()
            {
                foreach(Gizmo _gizmo in _gizmos)
                {
                    Color __prevColor = Gizmos.color;
                    Matrix4x4 __prevMatrix = Gizmos.matrix;
                    Gizmos.color = _gizmo.color;

                    if(_gizmo.matrix != default)
                    {
                        Gizmos.matrix = _gizmo.matrix;
                    }

                    _gizmo.action();

                    Gizmos.color = __prevColor;
                    Gizmos.matrix = __prevMatrix;
                }

                _gizmos.CGRemoveAll(gizmo => gizmo.durationLeft <= 0);
            }

            [PublicAPI]
            public static void Draw(in Gizmo drawing)
                => Instance._gizmos.Add(drawing);

            [PublicAPI]
            public static void Draw(params Gizmo[] drawings)
                => Instance._gizmos.AddRange(drawings);
        }
    }
}