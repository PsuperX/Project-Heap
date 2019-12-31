using UnityEngine;

namespace SA
{
    public class RoomButton : MonoBehaviour
    {
        public bool isRoomCreated;
        public Room Room;
        public string scene = "Testing Site";

        public void OnClick()
        {
            GameManagers.GetResourcesManager().curRoom.SetRoomButton(this);
        }
    }
}