using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Items/Ammo")]
    public class Ammo : ScriptableObject
    {
        public int carryingAmount;
        public int damageValue;

        public virtual void OnHit()
        {

        }
    }
}