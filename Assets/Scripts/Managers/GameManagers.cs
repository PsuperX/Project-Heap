using UnityEngine;

namespace SA
{
    public static class GameManagers
    {
        static ObjectPooler objectPooler;
        public static ObjectPooler GetObjectPooler()
        {
            if (!objectPooler)
            {
                objectPooler = Resources.Load("Object Pooler") as ObjectPooler;
                objectPooler.Init();
            }

            return objectPooler;
        }

        static ResourcesManager resourcesManager;
        public static ResourcesManager GetResourcesManager()
        {
            if (!resourcesManager)
            {
                resourcesManager = Resources.Load("Resources Manager") as ResourcesManager;
                resourcesManager.Init();
            }

            return resourcesManager;
        }

        static AmmoPool ammoPool;
        public static AmmoPool GetAmmoPool()
        {
            if (!ammoPool)
            {
                ammoPool = Resources.Load("Ammo Pool") as AmmoPool;
                ammoPool.Init();
            }

            return ammoPool;
        }
    }
}