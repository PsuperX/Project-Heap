using UnityEngine;

namespace SA
{
    public class AnimHashes
    {
        public readonly int vertical = Animator.StringToHash("vertical");
        public readonly int horizontal = Animator.StringToHash("horizontal");
        public readonly int isInteracting = Animator.StringToHash("isInteracting");
        public readonly int isAiming = Animator.StringToHash("aiming");
        public readonly int vaultWalk = Animator.StringToHash("Vault Walk");
    }
}