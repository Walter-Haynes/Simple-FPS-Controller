using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
//using NUnit.Framework;
using UnityEngine;

namespace CommonGames.Utilities.Extensions
{
    public static partial class Threading
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        {
            return Task.Delay(timeSpan).GetAwaiter();
        }

        /*
        private const int TIME_OUT = 25;
    
        /// <summary>
        /// Make a Task yieldable, but there is a time out so it is suitable for running tests.
        /// </summary>
        public static IEnumerator YieldWaitTest(this Task task)
        {
            float timeTaken = 0;
            while (task.IsCompleted == false)
            {
                if(task.IsCanceled)
                {
                    Assert.Fail("Task canceled!");
                }
                yield return null;
                timeTaken += Time.deltaTime;
                if(timeTaken > TIME_OUT)
                {
                    Assert.Fail("Time out!");
                }
            }
            Assert.That(task.IsFaulted, Is.Not.True, task.Exception?.ToString());
            Debug.Log("Task time taken : " + timeTaken);
        }
        */
        
    }
}