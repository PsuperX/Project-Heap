using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/Mono Actions/Delta Time Manager")]
    public class DeltaTimeManager : Action
    {
        public SO.FloatVariable variable;

        public override void Execute()
        {
            variable.value = Time.deltaTime;
        }
    }
}