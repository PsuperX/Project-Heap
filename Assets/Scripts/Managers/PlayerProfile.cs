using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Managers/Player Profile")]
    public class PlayerProfile : ScriptableObject
    {
        public string modelID = "Soldier";
        public string[] itemIds;
    }
}