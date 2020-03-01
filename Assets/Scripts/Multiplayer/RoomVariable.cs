using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Room Variable")]
    public class RoomVariable : ScriptableObject
    {
        public Room value;
        public RoomButtonVariable roomButtonVar;

        public void JoinGame() // ITs called by a UI Button
        {
            if (!roomButtonVar.value)
                return;

            SetRoom(roomButtonVar.value);
        }

        public void Set(Room room)
        {
            value = room;
        }

        public void SetRoom(RoomButton b)
        {
            if (b.isRoomCreated)
            {
                Set(b.Room);
                MultiplayerLauncher.singleton.JoinRoom(b.roomInfo);
            }
            else
            {
                MultiplayerLauncher.singleton.CreateRoom(b);
            }
        }

        public void SetRoomButton(RoomButton b)
        {
            roomButtonVar.value = b;
        }
    }
}