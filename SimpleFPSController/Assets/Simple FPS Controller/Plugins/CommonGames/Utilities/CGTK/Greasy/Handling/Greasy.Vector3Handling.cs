using UnityEngine;

namespace CommonGames.Utilities.CGTK.Greasy
{
    public static partial class Greasy
    {
        public static Coroutine To(Vector3 from, Vector3 to, in float duration, in EaseType ease, Setter<Vector3> setter)
            => CreateInterpolater(duration, ease, t => setter (Vector3.LerpUnclamped(from, to, t)));
        
        public static Coroutine To(Vector3 from, Vector3 to, in float duration, in EaseMethod ease, Setter<Vector3> setter)
            => CreateInterpolater(duration, ease, t => setter (Vector3.LerpUnclamped(from, to, t)));

        /*
        public static Coroutine Shake(Vector3 target, float duration, EaseMethod ease, Setter<Vector3> setter)
        {
            float timer = 0f;
            Vector3 lastMovement = Vector3.zero;
            
            while (timer < duration)
            {
                target -= lastMovement; //move back
				
                Vector3 myStrength = new Vector3(
                    curve.Evaluate(timer / duration) * multiplier * effectIntensity,
                    curve.Evaluate(timer / duration) * multiplier * effectIntensity,
                    curve.Evaluate(timer / duration) * multiplier * effectIntensity);

                if (useLocalSpace)
                {
                    lastMovement = MyTransform.localRotation * new Vector3(Random.Range(-myStrength.x, myStrength.x),
                                       Random.Range(-myStrength.y, myStrength.y),
                                       Random.Range(-myStrength.z, myStrength.z));
                }
                else
                {
                    lastMovement = new Vector3(Random.Range(-myStrength.x, myStrength.x),
                        Random.Range(-myStrength.y, myStrength.y),
                        Random.Range(-myStrength.z, myStrength.z));
                }

                if (decimals != 7)
                {
                    lastMovement.x = (float) System.Math.Round(lastMovement.x, decimals);
                    lastMovement.y = (float) System.Math.Round(lastMovement.y, decimals);
                    lastMovement.z = (float) System.Math.Round(lastMovement.z, decimals);
                }

                MyTransform.localPosition += lastMovement;

                timer += Time.unscaledDeltaTime; //count up using unscaled time.
                yield return null;
            }

            target -= lastMovement; //move back
            lastMovement = Vector3.zero;
        }
        */
		
    }
}