using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Action/Load Weapon")]
    public class LoadWeapon : StateActions
    {
        public override void Execute(StateManager states)
        {
            ResourcesManager resourcesManager = GameManagers.GetResourcesManager();
            Weapon targetWeapon = (Weapon)resourcesManager.GetItemInstance(states.inventory.weaponID);
            states.inventory.curWeapon = targetWeapon;
            targetWeapon.Init();

            Transform rightHand = states.anim.GetBoneTransform(HumanBodyBones.RightHand);
            targetWeapon.runtimeW.modelInstance.transform.parent = rightHand;
            targetWeapon.runtimeW.modelInstance.transform.localPosition = Vector3.zero;
            targetWeapon.runtimeW.modelInstance.transform.localEulerAngles = Vector3.zero;
            targetWeapon.runtimeW.modelInstance.transform.localScale = Vector3.one;

            states.animHook.LoadWeapon(targetWeapon);
        }
    }
}