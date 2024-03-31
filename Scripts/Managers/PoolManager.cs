using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    private GameObject _prefab;
    private IObjectPool<GameObject> _pool;
    private Transform root;
    private Transform Root
    {
        get
        {
            if (root == null)
            {
                GameObject obj = new() { name = $"[Pool_Root] {_prefab.name}" };
                root = obj.transform;
            }
            return root;
        }
    }

    public Pool(GameObject prefab)
    {
        this._prefab = prefab;
        this._pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    public void Push(GameObject obj)
    {
        _pool.Release(obj);
    }


    private GameObject OnCreate()
    {
        GameObject obj = GameObject.Instantiate(_prefab);
       // obj.transform.SetParent(Root);
        obj.name = _prefab.name;
        return obj;
    }
    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }
    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void OnDestroy(GameObject obj)
    {
        GameObject.Destroy(obj);
    }
}

public class PoolManager
{
    public Dictionary<string, Pool> _pools = new();
   // private Dictionary<string, Pool> _pools = new();

    public GameObject Pop(GameObject prefab)
    {
        if (_pools.ContainsKey(prefab.name) == false) 
            CreatePool(prefab);

        return _pools[prefab.name].Pop();
    }

    public bool Push(GameObject obj)
    {
        if (_pools.ContainsKey(obj.name) == false) 
            return false;

        _pools[obj.name].Push(obj);

        return true;
    }

    private void CreatePool(GameObject prefab)
    {
        Pool pool = new(prefab);
        _pools.Add(prefab.name, pool);
    }

    public void Clear()
    {
        _pools.Clear();
    }

}