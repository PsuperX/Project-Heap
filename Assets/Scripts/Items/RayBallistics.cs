using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Ballistics/Ray")]
    public class RayBallistics : Ballistics
    {
        public override void Execute(StateManager states, Weapon w)
        {
            Vector3 origin = w.runtimeW.modelInstance.transform.position;
            Vector3 dir = states.movementValues.aimPosition - origin;
            Ray ray = new Ray(origin, dir);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100);
            if (hits == null) return;
            if (hits.Length == 0) return;

            RaycastHit closestHit = GetClosestHit(origin, hits, states.photonID);
            IHittable isHittable = closestHit.transform.GetComponentInParent<IHittable>();

            if (isHittable == null)
            {
                // Do default hit behaviour
                GameObject hitParticle = GameManagers.GetObjectPooler().RequestObject("bullet_hit");
                Quaternion rot = Quaternion.LookRotation(closestHit.normal);
                hitParticle.transform.position = closestHit.point;
                hitParticle.transform.rotation = rot;
            }
            else
            {
                // Execute IHittable custom behaviour
                isHittable.OnHit(states, w, dir, closestHit.point, closestHit.normal);
            }


            MultiplayerManager mm = MultiplayerManager.singleton;
            if (mm != null) // MultiplayerManager is null if is not on multiplayer
            {
                mm.BroadcastShootWeapon(states, dir, origin);
            }
        }

        RaycastHit GetClosestHit(Vector3 origin, RaycastHit[] hits, int shooterID)
        {
            int closestIndex = 0;

            float minDst = float.MaxValue;
            for (int i = 0; i < hits.Length; i++)
            {
                float tempDst = (hits[i].point - origin).sqrMagnitude;
                if (tempDst < minDst)
                {
                    // Found a closer one

                    // Filter the shooter
                    StateManager states = hits[i].transform.GetComponentInParent<StateManager>();
                    if (states != null)
                    {
                        if (states.photonID == shooterID)
                            continue;
                    }
                    
                    minDst = tempDst;
                    closestIndex = i;
                }
            }
            
            return hits[closestIndex];
        }

        public void ClientShoot(StateManager states, Vector3 dir, Vector3 origin)
        {
            Debug.Log("Client Shoot");
            Ray ray = new Ray(origin, dir);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, 100);
            if (hits == null) return;
            if (hits.Length == 0) return;

            RaycastHit closestHit = GetClosestHit(origin, hits, states.photonID);
            IHittable isHittable = closestHit.transform.GetComponentInParent<IHittable>();

            if (isHittable == null)
            {
                // Do default hit behaviour
                GameObject hitParticle = GameManagers.GetObjectPooler().RequestObject("bullet_hit");
                Quaternion rot = Quaternion.LookRotation(closestHit.normal);
                hitParticle.transform.position = closestHit.point;
                hitParticle.transform.rotation = rot;
            }
            else
            {
                // Execute IHittable custom behaviour
                isHittable.OnHit(states, states.inventory.curWeapon, dir, closestHit.point, closestHit.normal);
            }
        }
    }
}