#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
 using Enumerable = System.Linq.Enumerable;

// This causes the class' static constructor to be called on load and on starting playmode
[InitializeOnLoad]
internal class PhysicsPlacement
{
    // only ever register once
    private static bool registered = false;

    // are we actively settling physics in our scene
    private static bool active = false;

    // the work list of rigid bodies we can find loaded up
    private static Rigidbody[] workList;

    // we need to disable auto simulation to manually tick physics
    private static bool cachedAutoSimulation;

    // how long do we run physics for before we give up getting things to sleep
    private const float timeToSettle = 10f;

    // how long have we been running
    private static float activeTime = 0f;

    // this is the static constructor called by [InitializeOnLoad]
    static PhysicsPlacement()
    {
        if (!registered)
        {
            // hook into the editor update
            EditorApplication.update += Update;

            // and the scene view OnGui
            SceneView.duringSceneGui += OnSceneGUI;
            registered = true;
        }
    }

    // let users turn on 
    [MenuItem("Tools/Common-Games/Settle Physics")]
    private static void Activate()
    {
        if (active) return;
        
        active = true;

        // Normally avoid Find functions, but this is editor time and only happens once
        workList = Object.FindObjectsOfType<Rigidbody>();

        // we will need to ensure autoSimulation is off to manually tick physics
        cachedAutoSimulation = Physics.autoSimulation;
        activeTime = 0f;

        // make sure that all rigidbodies are awake so they will actively settle against changed geometry.
        foreach( Rigidbody body in workList )
        {
            body.WakeUp();
        }
    }

    // grey out the menu item while we are settling physics
    [MenuItem("Tools/Common-Games/Settle Physics", true)]
    private static bool CheckMenu()
    {
        return !active;
    }

    private static void Update()
    {
        if (!active) { return; }
        
        activeTime += Time.deltaTime;

        // make sure we are not autosimulating
        Physics.autoSimulation = false;

        // see if all our 
        bool allSleeping = Enumerable.Aggregate(Enumerable.Where(workList, body => body != null), true, (current, body) => current & body.IsSleeping());

        if( allSleeping || activeTime >= timeToSettle)
        {
            Physics.autoSimulation = cachedAutoSimulation;
            active = false;
        }
        else
        {
            Physics.Simulate(Time.deltaTime);
        }
    }

    private static void OnSceneGUI(SceneView sceneView) 
    {
        if (!active) { return; }
        
        Handles.BeginGUI();
        Color cacheColor = GUI.color;
        GUI.color = Color.red;
        GUILayout.Label("Simulating Physics.", GUI.skin.box, GUILayout.Width(200));
        GUILayout.Label($"Time Remaining: {(timeToSettle - activeTime):F2}", GUI.skin.box, GUILayout.Width(200));
        Handles.EndGUI();

        foreach( Rigidbody body in workList )
        {
            if (body == null) { continue; }
            bool isSleeping = body.IsSleeping();
                    
            if (isSleeping) continue;
                    
            GUI.color = Color.green;
            Handles.Label(body.transform.position, "SIMULATING");
        }
        GUI.color = cacheColor;
    }
}

#endif