using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Actions/State Actions/Handle Predictions")]
    public class HandlePrediction : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.multiplayerListener.Prediction();
        }
    }
}