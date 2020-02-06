using UnityEngine;

using CommonGames.Utilities;
using CommonGames.Utilities.CGTK;

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoadAttribute]
public class TimeStartup
{
    static TimeStartup()
    {
        CGDebug.Log(message: $"CGTime Started at: {CGTime.Instance.name}");
    }
}
#endif

public class CGTime : EnsuredSingleton<CGTime>
{
    public static float deltaTime => Time.deltaTime;
    public static float fixedDeltaTime => Time.fixedDeltaTime;
    public static float smoothDeltaTime => Time.smoothDeltaTime;
    
    public static float time => Time.time;
    public static float fixedTime => Time.fixedTime;
    public static float unscaledTime => Time.unscaledTime;
    public static float fixedUnscaledTime => Time.fixedUnscaledTime;

    public static float unscaledDeltaTime => Time.unscaledDeltaTime;
    public static float fixedUnscaledDeltaTime => Time.fixedUnscaledDeltaTime;

    public static int   FrameCount => Time.frameCount;
    public static int   FixedFrameCount => _fixedFrameCount;
    
    public static bool  inFixedTimeStep => Time.inFixedTimeStep;
    public static bool  calledFromFixedTimeStep => Time.inFixedTimeStep;
    
    public static float maximumDeltaTime => Time.maximumDeltaTime;
    public static float maximumParticleDeltaTime => Time.maximumParticleDeltaTime;

    public static float realtimeSinceStartup => Time.realtimeSinceStartup;
    public static float timeSinceLevelLoad => Time.timeSinceLevelLoad;

    public static float timeScale => Time.timeScale;


    private static int _fixedFrameCount = 0;
    private void FixedUpdate()
    {
        _fixedFrameCount++;
    }
}