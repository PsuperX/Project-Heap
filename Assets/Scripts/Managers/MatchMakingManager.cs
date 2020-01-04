using Photon.Realtime;
using System;
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

        Dictionary<string, RoomButton> roomsDict = new Dictionary<string, RoomButton>();

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

        RoomButton GetRoomFromDict(string id)
        {
            roomsDict.TryGetValue(id, out RoomButton result);
            return result;
        }

        public void AddMatches(RoomInfo[] roomInfos)
        {
            List<RoomButton> allRooms = new List<RoomButton>();
            allRooms.AddRange(roomsDict.Values);

            SetDirtyRooms(allRooms);

            for (int i = 0; i < roomInfos.Length; i++)
            {
                RoomInfo r = roomInfos[i];

                RoomButton createdRoom = GetRoomFromDict(r.Name);
                if (createdRoom == null) // Create a new room button
                {
                    AddMatch(r);
                }
                else // The room still exists so mark it has valid
                {
                    createdRoom.isValid = true;
                }
            }

            ClearNonValidRooms(allRooms);
        }

        void SetDirtyRooms(List<RoomButton> allRooms)
        {
            foreach (RoomButton r in allRooms)
            {
                r.isValid = false;
            }
        }

        private void ClearNonValidRooms(List<RoomButton> allRooms)
        {
            foreach (RoomButton r in allRooms)
            {
                if (!r.isValid)
                {
                    roomsDict.Remove(r.roomInfo.Name);
                    Destroy(r.gameObject);
                }
            }
        }

        public void AddMatch(RoomInfo roomInfo)
        {
            MatchSpawnPosition spawnPosition = GetSpawnPos();
            GameObject go = Instantiate(matchPrefab, spawnPosition.pos.position, Quaternion.identity, matchesParent);

            spawnPosition.isUsed = true;
            go.transform.localScale = Vector3.one;

            RoomButton roomButton = go.GetComponent<RoomButton>();
            roomButton.roomInfo = roomInfo;
            roomButton.isRoomCreated = true;
            roomButton.isValid = true;

            roomInfo.CustomProperties.TryGetValue("scene", out object sceneObj);
            Room room = new Room
            {
                sceneName = (string)sceneObj,
                roomName = roomInfo.Name
            };

            roomsDict.Add(roomInfo.Name, roomButton);
        }

        MatchSpawnPosition GetSpawnPos()
        {
            List<MatchSpawnPosition> list = GetUnused();

            int ran = UnityEngine.Random.Range(0, list.Count);
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