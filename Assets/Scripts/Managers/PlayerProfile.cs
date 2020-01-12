using UnityEngine;
using System.Collections;

namespace SA
{
    [CreateAssetMenu(menuName ="Managers/Player Profile")]
    public class PlayerProfile : ScriptableObject
    {
        public string[] itemIds; 
    }
}