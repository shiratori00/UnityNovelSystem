﻿using UnityEngine;
using System.Collections;

public class SingletonMonoBefaviour<T> :MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
                {
                    Debug.Log(typeof(T) + "is nothing");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (this == Instance)
        {
            return true;
        }

        Destroy(this);

        return false;
    }
}