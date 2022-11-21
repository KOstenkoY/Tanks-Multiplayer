using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

    private void Init(GameObject prefab, int indexObject)
    {
        if (prefab != null && pools.ContainsKey(indexObject) == false)
        {
            pools[indexObject] = new Pool(prefab);
        }
    }

    public GameObject Spawn(GameObject prefab, int indexObject, Vector3 position, Quaternion rotation)
    {
        Init(prefab, indexObject);
        return pools[indexObject].Spawn(position, rotation);
    }

    public void Despawn(GameObject obj, int indexObject)
    {
        if (pools.ContainsKey(indexObject))
        {
            pools[indexObject].Despawn(obj);
        }
        else
        {
            Destroy(obj);
        }
    }

    private class Pool
    {
        private List<GameObject> inactive = new List<GameObject>();

        private GameObject prefab;

        public GameObject Prefab => prefab;

        public Pool(GameObject prefab)
        {
            this.prefab = prefab;
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject obj;

            if (inactive.Count == 0)
            {
                obj = Instantiate(prefab, position, rotation);
            }
            else
            {
                obj = inactive[inactive.Count - 1];
                inactive.RemoveAt(inactive.Count - 1);
            }

            obj.transform.position = position;
            obj.transform.rotation = rotation;

            obj.SetActive(true);

            return obj;
        }

        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            inactive.Add(obj);
        }
    }
}
