using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public static T GetInstance()
    {
        if(Instance == null)
        {
            GameObject tempObject = new GameObject(typeof(T).ToString() + " EDITOR");
            Instance = tempObject.AddComponent<T>();
        }
        return Instance;
    }

    protected virtual void Awake()
    {
        Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}