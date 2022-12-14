using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    private List<GameObject> _pooledObjects = new List<GameObject>();

    [SerializeField] private GameObject _objectToPool;

    [SerializeField] private int _amountToPool = 1;

    public GameObject this[int i] => _pooledObjects[i];

    private void Awake()
    {
        SharedInstance = this;
    }

    public void InitializePool()
    {
        GameObject tmp;

        for (int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_objectToPool);
            tmp.SetActive(false);

            _pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        if (_pooledObjects.Count != 0)
        {
            for (int i = 0; i < _amountToPool; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }
        }

        return null;
    }
}