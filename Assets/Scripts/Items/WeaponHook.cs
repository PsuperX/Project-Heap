using UnityEngine;

namespace SA
{
    public class WeaponHook : MonoBehaviour
    {
        public Transform leftHandIK;

        ParticleSystem[] particles;
        AudioSource audioSource;

        public void Init()
        {
            GameObject go = new GameObject();
            go.name = "audio holder";
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector3.zero;
            audioSource = go.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;

            particles = GetComponentsInChildren<ParticleSystem>();
        }

        void Shoot()
        {
            if (particles != null)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Play();
                }
            }
        }
    }
}