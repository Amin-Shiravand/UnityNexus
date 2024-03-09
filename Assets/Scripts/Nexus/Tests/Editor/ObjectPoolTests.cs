using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Nexus.PoolSystem;

public class ObjectPoolTests
{
    private PoolManager poolManager;
    private string testPrefabPath = "TestPrefabs/TestPrefab"; 

    [SetUp]
    public void Setup()
    {
        poolManager = PoolManager.Instance;
    }

    [TearDown]
    public void Teardown()
    {
        // poolManager.ClearAllPools(); 
         PoolManager.DestroyInstance();
    }

    [Test]
    public void PoolInitializesCorrectly()
    {
        poolManager.CreatePool(testPrefabPath, 5);
        Assert.IsTrue(poolManager.HasPool(testPrefabPath), "Pool was not created.");
    }

    [Test]
    public void ObjectRetrievalFromPool()
    {
        poolManager.CreatePool(testPrefabPath, 5);
        var obj = poolManager.GetObjectFromPool(testPrefabPath);
        Assert.IsNotNull(obj, "Failed to retrieve object from pool.");
        poolManager.ReturnObjectToPool(testPrefabPath, obj); 
    }

    [Test]
    public void ReturnObjectToPool()
    {
        poolManager.CreatePool(testPrefabPath, 5);
        var obj = poolManager.GetObjectFromPool(testPrefabPath);
        poolManager.ReturnObjectToPool(testPrefabPath, obj);
    }

    // [Test]
    // public void ClearPoolEmptiesThePool()
    // {
    //     poolManager.CreatePool(testPrefabPath, 10);
    //     var obj = poolManager.GetObjectFromPool(testPrefabPath);
    //     Assert.IsNotNull(obj, "An object should be retrieved from the pool before clearing.");
    //
    //     poolManager.ClearPool(testPrefabPath);
    //     Assert.IsFalse(poolManager.HasPool(testPrefabPath), "Pool should not exist after clearing.");
    // }
}
