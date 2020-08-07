using ClientUtilities.ResourceManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ClientUtilities.Pool
{
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        public int Count
        {
            get { return Pool.Count; }
        }

        public bool IsInitilized
        {
            get;
            set;
        }

        private string TemplatePrefabPath;
        private Stack<T> Pool = null;

        public void InitiliazePool(string Path, int Count = 0)
        {
            //if (Pool != null)
            //    return;
            if (IsInitilized)
                return;
            IsInitilized = true;
            Debug.Assert(TemplatePrefabPath != string.Empty, "Path is Empty");
            TemplatePrefabPath = Path;
            Pool = new Stack<T>(Count);

            for (int i = 0; i < Count; ++i)
            {

                GameObject go = Instantiate(GameResourceManager.Instance.LoadPrefab(Path),
                                           Vector3.zero, Quaternion.identity);


                if ((go.GetComponent<T>()) == null)
                    go.AddComponent<T>();
                SendToPool(go.GetComponent<T>());
                //Component[] co = go.GetComponents(typeof(T));

                //  SendToPool((T)co);

            }
        }

        public void SendToPool(T Item)
        {
            Debug.Assert(!Contains(Item), "Item exist in the pool");
            if (Item == null)
                return;

            Item.gameObject.SetActive(false);
            Pool.Push(Item);
        }

        public T GetFromPool(string Path)
        {
            if (Pool.Count == 0)
            {
                GameObject go = Instantiate(GameResourceManager.Instance.LoadPrefab(Path),
                                      Vector3.zero, Quaternion.identity);
                if ((go.GetComponent<T>()) == null)
                    go.AddComponent<T>();
                SendToPool(go.GetComponent<T>());
            }
            Pool.Peek().gameObject.SetActive(true);
            return Pool.Pop();
        }

        public T GetFromPool()
        {
            Debug.Assert(TemplatePrefabPath != string.Empty, "First of all intilize the pool");
            if (Pool.Count == 0)
            {
                GameObject go = Instantiate(GameResourceManager.Instance.LoadPrefab(TemplatePrefabPath),
                                      Vector3.zero, Quaternion.identity);
                if ((go.GetComponent<T>()) == null)
                    go.AddComponent<T>();
                SendToPool(go.GetComponent<T>());
            }
            Pool.Peek().gameObject.SetActive(true);
            return Pool.Pop();
        }

        public bool Contains(T Item)
        {
            if (Item == null)
                return false;

            return Pool.Contains(Item);
        }

        public void Clear()
        {
            Pool.Clear();
        }

        public T GetItemTypeOfPoolObject()
        {
            if (Pool.Count == 0)
                return null;

            return Pool.Peek();
        }
        private void OnDestroy()
        {
            Pool = null;
            IsInitilized = false;
        }
    }
}
