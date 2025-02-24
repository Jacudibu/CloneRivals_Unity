﻿using UnityEngine;

namespace Utility
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T: SingletonBehaviour<T>
    {
        public static T instance { get; protected set; }
 
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                instance = (T)this;
            }
        }
    }
}