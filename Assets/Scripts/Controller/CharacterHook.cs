using UnityEngine;

namespace SA
{
    public class CharacterHook : MonoBehaviour
    {
        public BodyPart body;
        [Tooltip("Optional")] public BodyPart joints;

        public void Init(ClothItem cloth)
        {
            body.meshRenderer.sharedMesh = cloth.mesh;
            body.meshRenderer.sharedMaterial = cloth.material;

            joints.meshRenderer.sharedMesh = cloth.secMesh;
            joints.meshRenderer.sharedMaterial = cloth.secMaterial;
        }
    }

    [System.Serializable]
    public struct BodyPart
    {
        public SkinnedMeshRenderer meshRenderer;
    }
}