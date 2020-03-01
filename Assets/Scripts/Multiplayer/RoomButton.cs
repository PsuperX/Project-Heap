using Photon.Realtime;
using UnityEngine;

namespace SA
{
    public class RoomButton : MonoBehaviour
    {
        public bool isRoomCreated;
        public Room Room;
        public string scene = "Testing Site";
        public RoomInfo roomInfo;
        public bool isValid;

        public void OnClick()
        {
            GameManagers.GetResourcesManager().curRoom.SetRoomButton(this);
        }
    }
}