using UnityEngine;
using System.Collections.Generic;

namespace Nexus.PoolSystem
{
    public class ObjectPool : MonoBehaviour
    {
        public int Count => inactiveObjects.Count;
        public int ActiveCount => activeObjects.Count;
        public bool IsInitialized { get; private set; }

        private GameObject templatePrefab;
        private List<GameObject> inactiveObjects = new List<GameObject>();
        private List<GameObject> activeObjects = new List<GameObject>();

        public void InitializePool(GameObject prefab, int initialCount = 0)
        {
            if (IsInitialized) return;

            IsInitialized = true;
            templatePrefab = prefab;

            for (int i = 0; i < initialCount; ++i)
            {
                var instance = Instantiate(templatePrefab, transform);
                instance.SetActive(false);
                inactiveObjects.Add(instance);
            }
        }

        public GameObject GetFromPool()
        {
            GameObject instance;

            if (inactiveObjects.Count > 0)
            {
                instance = inactiveObjects[0];
                inactiveObjects.RemoveAt(0);
            }
            else
            {
                instance = Instantiate(templatePrefab, transform);
            }

            activeObjects.Add(instance);
            instance.SetActive(true);
            return instance;
        }

        public void ReturnToPool(GameObject instance)
        {
            if (!activeObjects.Contains(instance))
            {
                Debug.LogWarning("Trying to return an object to the pool that is not active.");
                return;
            }

            instance.SetActive(false);
            activeObjects.Remove(instance);
            inactiveObjects.Add(instance);
        }

        public bool Contains(GameObject instance) => inactiveObjects.Contains(instance) || activeObjects.Contains(instance);

        public void Clear()
        {
            foreach (var instance in activeObjects)
            {
                Destroy(instance);
            }

            foreach (var instance in inactiveObjects)
            {
                Destroy(instance);
            }

            inactiveObjects.Clear();
            activeObjects.Clear();
        }

        private void OnDestroy() => Clear();
    }
}
