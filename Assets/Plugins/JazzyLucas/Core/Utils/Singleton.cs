using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null || _instance == this || _instance == this as T)
            {
                _instance = this as T;
                Init();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        public abstract void Init();
    }
}