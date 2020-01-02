using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MultiplayerReferences
    {
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

        public List<PlayerHolder> GetPlayer()
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
                networkPrint = print
            };
            players.Add(playerHolder);
            return playerHolder;
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