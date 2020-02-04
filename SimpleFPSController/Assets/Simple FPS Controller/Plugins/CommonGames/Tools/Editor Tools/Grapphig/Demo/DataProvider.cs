using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CommonGames.Tools;

#if UNITY_EDITOR
public class DataProvider : MonoBehaviour
{

    // Ignore this line.
    public float t = 0;

    //public AnimationCurve animationCurve;

    public float updateRateSeconds = 4.0F;

    private int frameCount = 0;
    private float dt = 0.0F;
    private float fps = 0.0F;

    private void Update()
    {
        t += Time.deltaTime;
        float __cos1 = Mathf.Cos(t);
        float __cos2 = Mathf.Cos(t);
        float __tan = Mathf.Tan((t % 2) / 10f);
        
        Grapphig.Log(__cos1, "Cos", Color.red);
        
        Grapphig.Log(__tan, "Tan", Color.green);
        /*

        // Feeling lazy version.
        Grapphig.Log(__cos2, "Cos2", Color.red);

        // Don't like the provided time for some reason? Use your own.
        Grapphig.Log(__tan, "Tan", Color.green);

        // Alternative with defined color.
        Grapphig.Log(__cos1 + __cos2, "Cos1 + Cos2", Color.cyan);

        // Different type examples

        // ********** List **********
        List<int> __list = new List<int>();
        __list.Add(1);
        __list.Add(2);
        //Grapher.Log(list, "List", Color.white);


        // ********** List **********
        LinkedList<int> __linkedList = new LinkedList<int>();
        __linkedList.AddLast(1);
        __linkedList.AddLast(2);
        //Grapher.Log(linkedList, "LinkedList", Color.white);


        // ********** Array **********
        //Grapher.Log(new int[3] { 1, 2, 3 }, "Array", Color.white);


        // ********** Queue **********
        Queue<int> __queue = new Queue<int>();
        __queue.Enqueue(1);
        __queue.Enqueue(2);
        //Grapher.Log(queue, "Queue", Color.white);


        // ********** ArrayList **********
        ArrayList __arrList = new ArrayList();
        __arrList.Add(1);
        __arrList.Add(5f);
        //Grapher.Log(arrList, "ArrayList", Color.white);
        */
        
        //TestEnum __tEnum = (int)t % 2 == 0 ? TestEnum.Bird : TestEnum.Alien;
        //Grapphig.Log(__tEnum, "Enum", Color.white);
        
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0 / updateRateSeconds)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0F / updateRateSeconds;
        }

        Grapphig.Log(fps, "FPS", Color.yellow);
    }
    

    public enum TestEnum
    {
        Bird, Horse, Alien
    }
}

#endif