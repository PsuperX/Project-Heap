using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Shoot Action")]
    public class ShootAction : StateActions
    {
        public override void Execute(StateManager states)
        {
            #region Reloading
            if (states.inventory.curWeapon.curBullets < states.inventory.curWeapon.magazineBullets)
            {
                if (states.isReloading)
                {
                    if (!states.isInteracting)
                    {
                        states.isInteracting = true;
                        states.PlayAnimation("rifle_reload");
                        states.anim.SetBool("isInteracting", true);
                    }
                    else
                    {
                        if (!states.anim.GetBool("isInteracting"))
                        {
                            states.isReloading = false;
                            states.isInteracting = false;
                            states.inventory.ReloadCurrentWeapon();
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
                else
                {
                    states.isReloading = true;
                }
            } 
            #endregion
        }
    }
}