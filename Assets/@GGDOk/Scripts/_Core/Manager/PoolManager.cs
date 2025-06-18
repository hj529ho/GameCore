using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Core.Manager
{
    class Pool
    {
        private readonly GameObject _prefab;
        private readonly IObjectPool<GameObject> _pool;
        private Transform _root;
        Transform Root
        {
            get
            {
                if (_root == null)
                {
                    GameObject go = new GameObject() { name = $"{_prefab.name}Root" };
                    _root = go.transform;
                }

                return _root;
            }

        }
        public Pool(GameObject prefab)
        {
            _prefab = prefab;
            _pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
        }

        private GameObject OnCreate()
        {
            GameObject go = Object.Instantiate(_prefab, Root, true);
            go.name = _prefab.name;
            return go;
        }

        private void OnGet(GameObject go)
        {
            go.SetActive(true);
        }

        private void OnRelease(GameObject go)
        {
            go.SetActive(false);
        }

        private void OnDestroy(GameObject go)
        {
            Object.Destroy(go);
        }

        public GameObject Pop()
        {
            return _pool.Get();
        }

        public void Push(GameObject go)
        {
            _pool.Release(go);
        }
    }

    // [Manager("Pool","Core")]
    public class PoolManager
    {
        private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

        public GameObject Pop(GameObject prefab)
        {
            if (_pools.ContainsKey(prefab.name) == false)
                CreatePool(prefab);
            return _pools[prefab.name].Pop();
        }

        private void CreatePool(GameObject prefab)
        {
            Pool pool = new Pool(prefab);
            _pools.Add(prefab.name, pool);
        }

        public bool Push(GameObject go)
        {
            if (_pools.ContainsKey(go.name) == false)
                return false;
        
            _pools[go.name].Push(go);
            return true;
        }
        
        public void Clear()
        {
            _pools.Clear();
        }
    }
}