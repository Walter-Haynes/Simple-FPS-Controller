using System;
using System.Collections.Generic;
using System.Linq;

//Edited by Wybren
//Credit goes to Walter Hulsebos
public abstract class UUIDMultiton<T> : UUID where T : UUIDMultiton<T>
{
    protected static int Length;

    ///<summary> 
    ///Static reference to all the Instances of T
    ///</summary>
    private static Dictionary<Type, object> InstancesDictionary { get; set; } = new Dictionary<Type, object>();

    public static List<T> Instances
    {
        get
        {
            InstancesDictionary.TryGetValue(typeof(T), out object __instances);
            return ((Dictionary<string, T>)__instances).Values.ToList(); ;
        }
        set
        {
            InstancesDictionary.TryGetValue(typeof(T), out object __instances);
            __instances = value;
        }
    }
        
    public static int IndexFromInstance(UUIDMultiton<T> instance) => InstancesDictionary.Values.ToList().IndexOf(instance);

    ///<summary> 
    ///Gets whether Instances of this Multiton exist.
    ///</summary>
    public static bool InstanceExists
    {
        get
        {
            bool __typeExists = InstancesDictionary.TryGetValue(typeof(T), out object __instances);

            if (!__typeExists) return false;
                
            return ((List<T>)__instances).Count > 0;
        }
    }

    /// <summary>
    /// Get the instance belonging to _ID
    /// </summary>
    /// <param name="_ID">The UUID of the instance you want</param>
    /// <returns>Returns the instance belonging to the supplied UUID</returns>
    public static T GetInstanceFromID(string _ID)
    {
        InstancesDictionary.TryGetValue(typeof(T), out object __instances);
        return ((Dictionary<string, T>)__instances)[_ID];
    }

    ///<summary> 
    ///OnEnable method to associate Multiton to their Instance.
    ///</summary>
    protected virtual void OnEnable()
    {
        bool __typeExists = InstancesDictionary.TryGetValue(typeof(T), out object instances);
            
        if (!__typeExists)
        {
            instances = new Dictionary<string,T>(Length);
            InstancesDictionary.Add(typeof(T), instances);
        }

        ((Dictionary<string, T>)instances)?.Add(ID, (T)this);
    }

    ///<summary> 
    ///OnDisable method to clear Multiton association.
    ///</summary>
    protected virtual void OnDisable()
    {
        InstancesDictionary.TryGetValue(typeof(T), out object __instances);
        ((Dictionary<string, T>)__instances)?.Remove(ID);
    }
}
