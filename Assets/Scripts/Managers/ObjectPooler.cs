﻿using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Managers/Object Pooler")]
    public class ObjectPooler : ScriptableObject
    {
        public List<Pool> pool = new List<Pool>();
        Dictionary<string, int> obj_dict = new Dictionary<string, int>();
        GameObject poolParent;

        public void Init()
        {
            if (poolParent)
                Destroy(poolParent);

            poolParent = new GameObject();
            poolParent.name = "Object Pool";
            obj_dict.Clear();

            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].budget < 1)
                    pool[i].budget = 1;

                pool[i].cur = 0;
                pool[i].createdObjects.Clear();

                if (obj_dict.ContainsKey(pool[i].prefab.name))
                {
                    Debug.Log("Entry with id " + pool[i].prefab.name + " is a duplicate");
                    continue;
                }
                else
                {
                    obj_dict.Add(pool[i].prefab.name, i);
                }
            }
        }

        public GameObject RequestObject(string id)
        {
            GameObject retVal = null;
            int index = 0;
            if(obj_dict.TryGetValue(id,out index))
            {
                Pool p = pool[index];
                if(p.createdObjects.Count - 1 < p.budget)
                {
                    retVal = Instantiate(p.prefab);
                    retVal.transform.parent = poolParent.transform;
                    p.createdObjects.Add(retVal);
                }
                else
                {
                    p.cur = (p.cur < p.createdObjects.Count - 1) ? p.cur + 1 : 0;
                    retVal = p.createdObjects[p.cur];
                    retVal.SetActive(false);
                    retVal.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Object with id " + id + " not found");
            }

            return retVal;
        }
    }

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int budget = 15;
        [HideInInspector]
        public int cur = 0;
        [HideInInspector]
        public List<GameObject> createdObjects = new List<GameObject>();
    }
}