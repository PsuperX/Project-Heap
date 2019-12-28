using UnityEngine;

namespace SA
{
    public static class GameManagers
    {
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
    }
}