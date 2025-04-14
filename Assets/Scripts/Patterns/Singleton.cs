using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T> :  MonoBehaviour where T:MonoBehaviour
{
    protected static T instance;
#if THREAD_SAFE
        // ReSharper disable once StaticMemberInGenericType
        protected static readonly object _lock = new object();
#endif

    public static T Instance
    {
        get
        {
#if THREAD_SAFE
                lock (_lock)
#endif
            {
                return instance ? instance : (instance = Search());
            }
        }
    }

    protected static T Search()
    {
        Object[] objects = FindObjectsOfType(typeof(T));
        if (objects.Length > 1)
        {
            UnityEngine.Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopening the scene might fix it.");
            return null;
        }
        return objects[0] ? (T)objects[0] : null;
    }
    protected virtual void Awake()
    {
#if THREAD_SAFE
            lock (_lock)
#endif
        {
            if (instance && instance != this)
            {
                Debug.LogError("Instance of " + GetType()
                                              + " already existing at " + instance.gameObject.scene.name
                                              + " ... overriding it with " + gameObject.scene.name);
            }
            instance = GetComponent<T>();
        }
    }
}
