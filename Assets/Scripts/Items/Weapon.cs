using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Items/Weapon")]
    public class Weapon : Item
    {
        [Header("Gun Settings")]
        public int curBullets = 30;
        public int magazineBullets = 30;
        public float fireRate = .2f;
        public Ammo ammoType;

        [Header("Visuals")]
        public SO.Vector3Variable rightHandPosition;
        public SO.Vector3Variable rightHandEulers;
        public GameObject modelPrefab;

        public RuntimeWeapon runtimeW;

        [Header("Recoil Settings")]
        public AnimationCurve recoilY;
        public AnimationCurve recoilZ;

        public void Init()
        {
            runtimeW = new RuntimeWeapon();
            runtimeW.modelInstance = Instantiate(modelPrefab);
            runtimeW.weaponHook = runtimeW.modelInstance.GetComponent<WeaponHook>();
            runtimeW.weaponHook.Init();

            ammoType = GameManagers.GetAmmoPool().GetAmmo(ammoType.name);
        }

        [SerializeField]
        public class RuntimeWeapon
        {
            public GameObject modelInstance;
            public WeaponHook weaponHook;
        }
    }
}