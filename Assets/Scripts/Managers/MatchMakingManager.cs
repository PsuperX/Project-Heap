using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MatchMakingManager : MonoBehaviour
    {
        public Transform spawnParent;
        public Transform matchesParent;
        public GameObject matchPrefab;

        List<MatchSpawnPosition> spawnPos = new List<MatchSpawnPosition>();

        public static MatchMakingManager singleton;

        private void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            Transform[] p = spawnParent.GetComponentsInChildren<Transform>();

            foreach (Transform t in p)
            {
                if (t != spawnParent)
                {
                    MatchSpawnPosition m = new MatchSpawnPosition
                    {
                        pos = t
                    };
                    spawnPos.Add(m);
                }
            }
        }

        public void AddMatch()
        {
            GameObject go = Instantiate(matchPrefab);
            go.transform.SetParent(matchesParent);

            MatchSpawnPosition spawnPosition = GetSpawnPos();

            spawnPosition.isUsed = true;
            go.transform.position = spawnPosition.pos.position;
            go.transform.localScale = Vector3.one;
        }

        MatchSpawnPosition GetSpawnPos()
        {
            List<MatchSpawnPosition> list = GetUnused();

            int ran = Random.Range(0, list.Count);
            return list[ran];
        }

        List<MatchSpawnPosition> GetUnused()
        {
            List<MatchSpawnPosition> r = new List<MatchSpawnPosition>();

            for (int i = 0; i < spawnPos.Count; i++)
            {
                if (!spawnPos[i].isUsed)
                    r.Add(spawnPos[i]);
            }

            return r;
        }
    }

    class MatchSpawnPosition
    {
        public Transform pos;
        public bool isUsed;
    }
}