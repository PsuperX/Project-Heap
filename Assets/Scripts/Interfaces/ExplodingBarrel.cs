using UnityEngine;

namespace SA
{
    public class ExplodingBarrel : MonoBehaviour, IHittable
    {
        public string targetParticle = "buller_hit";

        public void OnHit(StateManager shooter, Weapon w, Vector3 dir, Vector3 pos, Vector3 normal)
        {
            GameObject hitParticle = GameManagers.GetObjectPooler().RequestObject(targetParticle);
            hitParticle.transform.position = pos;
        }
    }
}