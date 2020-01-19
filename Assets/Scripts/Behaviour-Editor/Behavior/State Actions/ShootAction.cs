using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Shoot Action")]
    public class ShootAction : StateActions
    {
        public override void Execute(StateManager states)
        {
            #region Reloading
            if (states.inventory.curWeapon.curBullets < states.inventory.curWeapon.magazineBullets
                && states.inventory.curWeapon.ammoType.carryingAmount > 0)
            {
                if (states.isReloading)
                {
                    if (!states.isInteracting)
                    {
                        states.reloadingFlag = true;
                        states.isInteracting = true;
                        states.PlayAnimation("rifle_reload");
                        states.anim.SetBool("isInteracting", true);
                    }
                    else
                    {
                        // Wait for the animation to end
                        if (!states.anim.GetBool("isInteracting"))
                        {
                            states.isReloading = false;
                            states.isInteracting = false;
                            ReloadCurrentWeapon(states.inventory.curWeapon);
                        }
                    }

                    return;
                }
            }
            else
            {
                states.isReloading = false;
            }
            #endregion

            #region Shooting
            if (states.isShooting)
            {
                states.shootingFlag = true;
                states.isShooting = false;
                Weapon w = states.inventory.curWeapon;

                if (w.curBullets > 0)
                {
                    if (Time.realtimeSinceStartup - w.runtimeW.weaponHook.lastFired > w.fireRate)
                    {
                        w.runtimeW.weaponHook.lastFired = Time.realtimeSinceStartup;
                        w.runtimeW.weaponHook.Shoot();
                        states.animHook.RecoilAnim();

                            if (states.ballisticsAction)
                                states.ballisticsAction.Execute(states, w);
                            else
                                Debug.Log("No ballistic action assigned in: " + states.mTransform.name);

                        w.curBullets--;
                    }
                }
                else
                {
                    states.isReloading = true;
                }
            }
            #endregion
        }

        public void ReloadCurrentWeapon(Weapon curWeapon)
        {
            int target = curWeapon.magazineBullets;
            if (target > curWeapon.ammoType.carryingAmount)
            {
                target = curWeapon.magazineBullets - curWeapon.ammoType.carryingAmount;
            }
            curWeapon.ammoType.carryingAmount -= target;
            curWeapon.curBullets = target;
        }
    }
}