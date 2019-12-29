using UnityEngine;

namespace SA
{
    public class WeaponHook : MonoBehaviour
    {
        public Transform leftHandIK;

        [HideInInspector]
        public float lastFired;

        ParticleSystem[] particles;
        AudioSource audioSource;

        [Header("Slider Settings")]
        public Transform slider;
        public AnimationCurve sliderCurve;
        public float multiplier = 1;
        public float sliderSpeed = 9;
        Vector3 startPos;
        float sliderT;

        public bool isShooting;
        bool initSliderLerp;

        public void Init()
        {
            GameObject go = new GameObject();
            go.name = "audio holder";
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector3.zero;
            audioSource = go.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;

            particles = GetComponentsInChildren<ParticleSystem>();

            if(slider)
            startPos = slider.localPosition;
        }

        public void Shoot()
        {
            isShooting = true;

            if (particles != null)
            {
                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Play();
                }
            }
        }

        private void Update()
        {
            if (isShooting)
            {
                if (!initSliderLerp)
                {
                    initSliderLerp = true;
                    sliderT = 0;
                }

                sliderT += Time.deltaTime * sliderSpeed;
                if(sliderT > 1)
                {
                    sliderT = 0;
                    initSliderLerp = false;
                    isShooting = false;
                }

                float targetZ = sliderCurve.Evaluate(sliderT) * multiplier;
                Vector3 targetPos = startPos;
                targetPos.z -= targetZ;
                slider.transform.localPosition = targetPos;
            }
        }
    }
}