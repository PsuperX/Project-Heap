using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MultiplayerReferences
    {
        public SpawnPosition[] spawnPositions;
        List<PlayerHolder> players = new List<PlayerHolder>();

        public PlayerHolder localPlayer;
        public Transform referencesParent;

        public MultiplayerReferences()
        {
            referencesParent = new GameObject("References").transform;
        }

        public int GetPlayerCount()
        {
            return players.Count;
        }

        public List<PlayerHolder> GetPlayers()
        {
            return players;
        }

        public PlayerHolder AddNewPlayer(NetworkPrint print)
        {
            if (!IsUniquePlayer(print.photonID))
            {
                Debug.Log("Duplicated id!!!");
                return null;
            }

            PlayerHolder playerHolder = new PlayerHolder
            {
                photonID = print.photonID,
                networkPrint = print,
                health = 100
            };
            players.Add(playerHolder);
            return playerHolder;
        }

        public PlayerHolder GetPlayer(int photonID)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].photonID == photonID)
                    return players[i];
            }

            Debug.Log("No player with photonID: " + photonID + " was found!");
            return null;
        }

        // Check if there are players with the same id
        public bool IsUniquePlayer(int id)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].photonID == id)
                    return false;
            }

            return true;
        }
    }
}