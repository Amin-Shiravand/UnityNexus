using UnityEngine;
using System;
#if UNITY_EDITOR
using System.Reflection;
#endif

namespace Nexus.Singleton
{
    public class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object initLock = new object();
        private static bool applicationIsQuitting = false;
        private static T instance = null;

        public static T Instance
        {

            get
            {

                if (instance == null)
                {
                    lock (initLock)
                    {
                        instance = CreateInstance();
                    }

                }
                return instance;
            }
        }




        public static T CreateInstance()
        {

            if (!applicationIsQuitting)
            {
                // Ensure there are no public constructors and there is one instance in the scene
#if UNITY_EDITOR
                Type type = typeof(T);
                ConstructorInfo[] constructorInfo = type.GetConstructors(BindingFlags.Public);
                Debug.Assert(constructorInfo.Length <= 0, "has at least one accesible ctor making it impossible to enforce singleton behaviour" + type.Name.ToString());
                Debug.Assert(UnityEngine.Object.FindObjectsOfType<T>().Length <= 1, "has at least more than one instance  in the scene making it impossible to enforce singleton behaviour please find and remove extra instances" + type.Name.ToString());
                Debug.Assert(!applicationIsQuitting, " already destroyed on application quit.Won't create again - returning null");
#endif
                T obj = null;
                obj = UnityEngine.Object.FindObjectOfType<T>() as T;
                GameObject Object = null;
                if (obj != null)
                {
                    Object = obj.gameObject;
                    obj.name = typeof(T).Name;
                }
                else
                {
                    Object = new GameObject(typeof(T).Name);
                    obj = Object.AddComponent<T>() as T;
                    (obj as MonoBehaviorSingleton<T>)?.Init();
                }
                Debug.Log("Instance type of" + obj + " Created");
                return obj;
            }

            return null;
        }

        protected virtual void Init()
        {
            
        }

        protected virtual void OnDestroy()
        {
            //if (applicationIsQuitting)
            //    return;

            if (instance != null)
            {
                if (instance.gameObject != null)
                   Destroy(instance.gameObject);
                Destroy(instance);

            }
            instance = null;
            // applicationIsQuitting = true;

        }
    }
}