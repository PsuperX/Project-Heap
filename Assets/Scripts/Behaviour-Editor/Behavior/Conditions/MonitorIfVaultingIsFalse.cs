using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Conditions/Monitor If Vaulting Is False")]
    public class MonitorIfVaultingIsFalse : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return !state.isVaulting;
        }
    }
}