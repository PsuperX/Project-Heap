using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/Mono Actions/Delta Time Manager")]
    public class DeltaTimeManager : Action
    {
        public SO.FloatVariable variable;
        public bool isFixed;

        public override void Execute()
        {
            variable.value = (isFixed) ? Time.fixedDeltaTime : Time.deltaTime;
        }
    }
}