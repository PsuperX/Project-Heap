using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Shoot Action Client")]
    public class ShootActionClient : StateActions
    {
        public override void Execute(StateManager states)
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
                    }
                }
            }

            if (states.isShooting && !states.isReloading)
            {
                states.isShooting = false;
                Weapon w = states.inventory.curWeapon;

                w.runtimeW.weaponHook.Shoot();
                states.animHook.RecoilAnim();
            }
        }
    }
}