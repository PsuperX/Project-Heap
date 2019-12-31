using UnityEngine;
using UnityEngine.Events;

namespace SA
{
    public class OnEnableExecuteEvent : MonoBehaviour
    {
        public UnityEvent onEnable;

        private void Start()
        {
            onEnable.Invoke();
        }
    }
}