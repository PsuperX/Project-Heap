using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Items/Cloth Item")]
    public class ClothItem : Item
    {
        public Mesh mesh;
        public Material material;

        public Mesh secMesh;
        public Material secMaterial;
    }
}