using UnityEngine;
using System.Collections.Generic;
using Nexus.Singleton;

namespace Nexus.PoolSystem
{
    public class PoolManager : Singleton<PoolManager>
    {
        private Dictionary<string, ObjectPool> poolDictionary = new Dictionary<string, ObjectPool>();
        
        public void CreatePool(string prefabPath, int initialCount = 0)
        {
            if (poolDictionary.ContainsKey(prefabPath))
            {
                Debug.LogWarning($"Pool for {prefabPath} already exists.");
                return;
            }

            var prefab = Resources.Load<GameObject>(prefabPath);
            var poolObject = new GameObject($"{prefab.name}_Pool");
            var objectPool = poolObject.AddComponent<ObjectPool>();
            objectPool.InitializePool(prefab, initialCount);

            poolDictionary[prefabPath] = objectPool;
        }

        public GameObject GetObjectFromPool(string prefabPath)
        {
            if (!poolDictionary.TryGetValue(prefabPath, out var objectPool))
            {
                Debug.LogWarning($"Pool for {prefabPath} doesn't exist.");
                return null;
            }

            return objectPool.GetFromPool();
        }

        public void ReturnObjectToPool(string prefabPath, GameObject instance)
        {
            if (!poolDictionary.TryGetValue(prefabPath, out var objectPool))
            {
                Debug.LogWarning($"Pool for {prefabPath} doesn't exist.");
                return;
            }

            objectPool.ReturnToPool(instance);
        }

        public void ClearPool(string prefabPath)
        {
            if (!poolDictionary.TryGetValue(prefabPath, out var objectPool))
            {
                Debug.LogWarning($"Pool for {prefabPath} doesn't exist.");
                return;
            }

            objectPool.Clear();
            poolDictionary.Remove(prefabPath);
            Object.DestroyImmediate(objectPool.gameObject);
        }

        public void ClearAllPools()
        {
            foreach( var pool in poolDictionary )
            {
                pool.Value.Clear();
                poolDictionary.Remove(pool.Key);
                Object.DestroyImmediate(pool.Value.gameObject);
            }
        }
        
        public bool HasPool(string key)
        {
            return poolDictionary.ContainsKey(key);
        }
    }
}
