using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using CommonGames.Utilities.Extensions;

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CommonGames.Utilities.Helpers
{

    /// <summary>
    /// All SUVAT Equation Helpers.
    /// 
    /// S = Displacement (metres)
    /// U = Initial Velocity (metres per second)
    /// V = Final Velocity (metres per second)
    /// A = Acceleration (metres per second per second)
    /// T = Time (seconds)
    ///
    /// </summary>
    [SuppressMessageAttribute("ReSharper", "InconsistentNaming")]
    public static partial class SuvatHelper
    {
        //NOTE: All the unnecessary parenthesis are because of my preferences.
        //I think it makes it more readable.
        //But feel free to change it, if you can't handle it - you little bitch. 

        #region Displacement (S)

        /// <summary> Gets Displacement (S) Without knowing Initial Velocity (U)</summary>
        /// <param name="v_finalVelocity"> V (metres per second)</param>
        /// <param name="a_acceleration"> A (metres per second per second)</param>
        /// <param name="t_time"> T (seconds)</param>
        /// <returns> S </returns>
        [PublicAPI]
        public static float GetDisplacementNoU(in float v_finalVelocity, in float a_acceleration, in float t_time)
            => (v_finalVelocity * t_time) - ((a_acceleration * t_time.Pow(2f)) / 2f);


        /// <summary> Gets Displacement (S) Without knowing Final Velocity (V)</summary>
        /// <param name="u_initialVelocity"> U (metres per second)</param>
        /// <param name="a_acceleration"> A (metres per second per second)</param>
        /// <param name="t_time"> T (seconds)</param>
        /// <returns> S </returns>
        [PublicAPI]
        public static float GetDisplacementNoV(in float u_initialVelocity, in float a_acceleration, in float t_time)
            => (u_initialVelocity * t_time) + ((a_acceleration * t_time.Pow(2f)) / 2f);


        /// <summary> Gets Displacement (S) Without knowing Acceleration (A)</summary>
        /// <param name="u_initialVelocity"> U (metres per second)</param>
        /// <param name="v_finalVelocity"> V (metres per second)</param>
        /// <param name="t_time"> T (seconds)</param>
        /// 
        /// <returns> S </returns>
        [PublicAPI]
        public static float GetDisplacementNoA(in float u_initialVelocity, in float v_finalVelocity, in float t_time)
            => ((u_initialVelocity + v_finalVelocity) / 2f) * t_time;


        /// <summary> Gets Displacement (S) Without knowing Acceleration (T)</summary>
        /// <param name="u_initialVelocity"> U (metres per second)</param>
        /// <param name="v_finalVelocity"> V (metres per second)</param>
        /// <param name="a_acceleration"> A (metres per second per second)</param>
        /// 
        /// <returns> S </returns>
        [PublicAPI]
        public static float GetDisplacementNoT(
            in float u_initialVelocity,
            in float v_finalVelocity,
            in float a_acceleration)
            => (u_initialVelocity.Pow(2f) + v_finalVelocity.Pow(2f)) / (2f * a_acceleration);


        #endregion

        #region Initial Velocity (U)

        ///<summary> Gets InitialVelocity (U) Without knowing Displacement (S)</summary>
        /// <param name="v_finalVelocity"> V (metres per second)</param>
        /// <param name="a_acceleration"> A (metres per second per second)</param>
        /// <param name="t_time"> T (seconds)</param>
        /// <returns></returns>
        [PublicAPI]
        public static float GetInitialVelocityNoS(in float v_finalVelocity, in float a_acceleration, in float t_time)
            => v_finalVelocity - (a_acceleration * t_time);


        ///<summary> Gets InitialVelocity (U) Without knowing Final Velocity (V)</summary>
        /// <param name="s_displacement"> S (metres)</param>
        /// <param name="a_acceleration"> A (metres per second per second)</param>
        /// <param name="t_time"> T (seconds)</param>
        /// <returns></returns>
        [PublicAPI]
        public static float GetInitialVelocityNoV(in float s_displacement, in float a_acceleration, in float t_time)
            => (s_displacement - (a_acceleration * t_time.Pow(2f))) / (2f * t_time);


        ///<summary> Gets InitialVelocity (U) Without knowing Acceleration (A)</summary>
        /// <param name="s_displacement"> S (metres)</param>
        /// <param name="v_finalVelocity"> V (metres per second)</param>
        /// <param name="t_time"> T (seconds)</param>
        /// <returns></returns>
        [PublicAPI]
        public static float GetInitialVelocityNoA(in float s_displacement, in float v_finalVelocity, in float t_time)
            => ((2f * s_displacement) / t_time) + v_finalVelocity;


        ///<summary> Gets InitialVelocity (U) Without knowing Time (T)</summary>
        /// <param name="s_displacement"> S (metres)</param>
        /// <param name="v_finalVelocity"> V (metres per second)</param>
        /// <param name="a_acceleration"> A (metres per second per second)</param>
        /// <returns></returns>
        [PublicAPI]
        public static float GetInitialVelocityNoT(
            in float s_displacement,
            in float v_finalVelocity,
            in float a_acceleration)
            => ((2f * a_acceleration * s_displacement) - v_finalVelocity.Pow(2f)).Sqrt();

        #endregion

        #region Final Velocity (V)

        /// <summary> Gets Final Velocity (V) Without knowing Displacement (S)</summary>
        /// <param name="u_initialVelocity"> U </param>
        /// <param name="a_acceleration"> A (metres per second per second)</param>
        /// <param name="t_time"> T </param>
        /// <returns> V </returns>
        [PublicAPI]
        public static float GetFinalVelocityNoS(in float u_initialVelocity, in float a_acceleration, in float t_time)
            => (u_initialVelocity + (a_acceleration * t_time));

        #endregion

        #region Acceleration (A)



        #endregion

        #region Time (S)



        #endregion

    }
    
}