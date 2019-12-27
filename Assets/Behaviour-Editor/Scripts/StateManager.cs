using UnityEngine;

namespace SA
{
    public class StateManager : MonoBehaviour
    {
        public MovementValues movementValues;

        [System.Serializable]
        public struct MovementValues
        {
            public float horizontal;
            public float vertical;
            public float moveAmount;
            public Vector3 moveDirection;
            public Vector3 lookDirection;
        }

        public bool isAiming;

        public State currentState;

        [HideInInspector]
        public Animator anim;

        [HideInInspector]
        public float delta;
        [HideInInspector]
        public Transform mTransform;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public LayerMask ignoreLayers;

        private void Start()
        {
            mTransform = this.transform;

            rigid = GetComponent<Rigidbody>();
            rigid.drag = 4;
            rigid.angularDrag = 999;
            rigid.constraints = RigidbodyConstraints.FreezeRotation;

            ignoreLayers = ~(1 << 9 | 1 << 3);
            anim = GetComponentInChildren<Animator>();
        }

        private void FixedUpdate()
        {
            delta = Time.deltaTime;

            if (currentState != null)
            {
                currentState.FixedTick(this);
            }
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.Tick(this);
            }
        }
    }
}
