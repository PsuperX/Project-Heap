using UnityEngine;

namespace SA
{
    public interface IHittable
    {
        void OnHit(StateManager shooter, Weapon w, Vector3 dir, Vector3 pos, Vector3 normal);
    }
}