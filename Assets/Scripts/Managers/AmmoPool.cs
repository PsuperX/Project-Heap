using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SA
{
    [CreateAssetMenu(menuName = "Managers/Ammo Pool")]
    public class AmmoPool : ScriptableObject
    {
        public List<Ammo> all_ammo = new List<Ammo>();
        Dictionary<string, Ammo> ammoDictionary = new Dictionary<string, Ammo>();

        public void Init()
        {
            for (int i = 0; i < all_ammo.Count; i++)
            {
                Ammo ammo = Instantiate(all_ammo[i]);
                ammo.name = all_ammo[i].name;
                ammoDictionary.Add(ammo.name,ammo);
            }
        }

        public Ammo GetAmmo(string id)
        {
            if (ammoDictionary.TryGetValue(id, out Ammo a))
            {
                return a;
            }
            else
            {
                Debug.LogError("No ammo type with name: " + id + " was found!");
                return null;
            }
        }
    }
}