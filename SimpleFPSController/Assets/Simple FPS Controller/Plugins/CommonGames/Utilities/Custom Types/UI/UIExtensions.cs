using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace Utilities.UI
{
    public static class UIExtensions
    {
        public enum Direction { Up, Right, Down, Left }

        public struct SmoothArgs<T>
        {
            public T item;
            public float duration;
            public AnimationCurve curve;

            public SmoothArgs(T item, float duration, AnimationCurve curve)
            {
                this.item = item;
                this.duration = duration;
                this.curve = curve;
            }

            public static implicit operator SmoothArgs<T>((T, float, AnimationCurve) args) =>
                new SmoothArgs<T>(args.Item1, args.Item2, args.Item3);
        }

        public static IEnumerator SimpleMove(this SmoothArgs<RectTransform> args, Direction direction)
        {
            Vector3 point = args.item.position;

            switch (direction)
            {
                case Direction.Up:
                    point += Vector3.up * Screen.height;
                    break;
                case Direction.Right:
                    point += Vector3.right * Screen.width;
                    break;
                case Direction.Down:
                    point += Vector3.down * Screen.height;
                    break;
                case Direction.Left:
                    point += Vector3.left * Screen.width;
                    break;
            }

            return args.Move(point);
        }

        public static IEnumerator SimpleScale(this SmoothArgs<RectTransform> args, bool up) => args.Scale(up ? Vector3.one : Vector3.zero);

        #region Shortcuts
        public static IEnumerator Fade(this SmoothArgs<Image> args, bool toOpaque = true)
        {
            void OnSmooth(float lerp)
            {
                Color color = args.item.color;
                if (!toOpaque)
                    lerp = 1 - lerp;

                color.a = lerp;
                args.item.color = color;
            }

            return args.DoSmooth(OnSmooth);
        }

        public static IEnumerator Scale(this SmoothArgs<RectTransform> args, Vector3 targetScale)
        {
            Transform trans = args.item.transform;
            Vector3 startingScale = trans.localScale;

            void OnSmooth(float lerp) => trans.localScale = Vector3.Lerp(startingScale, targetScale, lerp);

            return args.DoSmooth(OnSmooth);
        }

        public static IEnumerator Move(this SmoothArgs<RectTransform> args, Vector3 to)
        {
            Transform trans = args.item.transform;
            Vector3 startPosition = trans.position;

            void OnSmooth(float lerp) => Vector3.Lerp(startPosition, to, lerp);

            return args.DoSmooth(OnSmooth);
        }
        #endregion

        public static IEnumerator DoSmooth<T>(this SmoothArgs<T> args, Action<float> onSmooth)
        {
            float remaining = args.duration;

            void Lerp()
            {
                float lerp = Mathf.InverseLerp(0, args.duration, remaining);
                if (args.curve != null)
                    lerp = args.curve.Evaluate(lerp);
                onSmooth(lerp);
            }

            while ((remaining -= Time.deltaTime) > 0)
            {
                Lerp();
                yield return null;
            }
        }
    }
}