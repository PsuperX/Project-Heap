using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Shoot Action")]
    public class ShootAction : StateActions
    {
        public override void Execute(StateManager states)
        {
            if (states.isShooting)
            {
                states.isShooting = false;
                Weapon w = states.inventory.curWeapon;

                if (w.curBullets > 0)
                {
                    if (Time.realtimeSinceStartup - w.runtimeW.weaponHook.lastFired > w.fireRate)
                    {
                        w.runtimeW.weaponHook.lastFired = Time.realtimeSinceStartup;
                        w.runtimeW.weaponHook.Shoot();
                        states.animHook.RecoilAnim();

                        w.curBullets--;
                    }
                }
            }
        }
    }
}