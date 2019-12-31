using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Room Variable")]
    public class RoomVariable : ScriptableObject
    {
        public Room value;

        public void Set(Room room)
        {
            value = room;
        }
    }
}