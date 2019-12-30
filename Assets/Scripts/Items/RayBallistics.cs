using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Ballistics/Ray")]
    public class RayBallistics : Ballistics
    {
        public override void Execute(StateManager states, Weapon w)
        {
            Vector3 origin = w.runtimeW.modelInstance.transform.position;
            Vector3 dir = states.movementValues.aimPosition;

            Ray ray = new Ray(origin, dir);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, states.ignoreLayers))
            {
                IHittable hittable = hit.transform.GetComponentInParent<IHittable>();

                if (hittable == null)
                {
                    GameObject hitParticle = GameManagers.GetObjectPooler().RequestObject("bullet_hit");
                    hitParticle.transform.position = hit.point;
                    hitParticle.transform.rotation = Quaternion.LookRotation(hit.normal);
                }
                else
                {
                    hittable.OnHit(states, w, dir, hit.point, hit.normal);
                }
            }
        }
    }
}