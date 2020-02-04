using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelayedInvoker
{
    private readonly Dictionary<int, IEnumerator> actionIdToCoroutine = new Dictionary<int, IEnumerator>();
    private MonoBehaviour CoroutineHost { get; }

    /// <remarks> 
    /// Execution order in the first frame after specified delay time depends on Script Execution Order of this `coroutineHost`.
    /// </remarks>
    public DelayedInvoker(MonoBehaviour coroutineHost)
    {
        CoroutineHost = coroutineHost;
    }

    /// <param name="actionId"></param>
    /// <param name="hitMethod"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator DelayedActionRoutine(int actionId, Action hitMethod, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hitMethod.Invoke();
        actionIdToCoroutine.Remove(actionId);
    }

    public void CancelAction(int actionId)
    {
        if (!actionIdToCoroutine.TryGetValue(actionId, out IEnumerator coroutine)){ return;}
        
        CoroutineHost.StopCoroutine(coroutine);
        actionIdToCoroutine.Remove(actionId);
    }

    public void CancelAllActions()
    {
        if (actionIdToCoroutine.Count == 0) return;
        
        foreach (IEnumerator coroutine in actionIdToCoroutine.Values)
        {
            CoroutineHost.StopCoroutine(coroutine);
        }
        actionIdToCoroutine.Clear();
    }

    /// <remarks> 
    /// Throws an exception if the same action Id is already scheduled. Use <see cref="IsSchedulingAction"/> to check.
    /// </remarks>
    public void ScheduleAction(int actionId, Action action, float invokeAfterDelay)
    {
        if (actionIdToCoroutine.ContainsKey(actionId) == false)
        {
            IEnumerator routine = DelayedActionRoutine(actionId, action, invokeAfterDelay);
            CoroutineHost.StartCoroutine(routine);
            actionIdToCoroutine.Add(actionId, routine);
        }
        else
        {
            throw new ArgumentException($"Action ID {actionId} already scheduled!");
        }
    }
    
    public bool IsSchedulingAction(int actionId) => actionIdToCoroutine.ContainsKey(actionId);
}
