using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance;

    private static readonly object _threadLock = new object();

    private bool _persistance = true;

    public static T Instance
    {
        get
        {
            if(_instance != null)
            {
                return _instance;
            }
            lock (_threadLock)
            {
                T[] instances = FindObjectsOfType<T>();
                if(instances.Length > 0)
                {
                    _instance = instances[0];
                    for(int i = 1; i < instances.Length; i++)
                    {
                        Destroy(instances[i]);
                    }
                }
                else
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).ToString();
                    _instance = go.AddComponent<T>();
                }
                DontDestroyOnLoad(_instance);
                return _instance;
            }
        }
    }
}
