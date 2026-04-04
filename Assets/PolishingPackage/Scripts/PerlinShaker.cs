using UnityEngine;

/*
 * For more info on shakes:
 * https://roystan.net/articles/camera-shake/
 */

namespace Polishing.Shakes
{
    public class PerlinShaker : MonoBehaviour
    {
        [Range(0.0f, 50.0f)] public float frequency = 25.0f;
        [Space]
        [SerializeField] private ShakeSettings positionShakeSettings;
        [SerializeField] private ShakeSettings rotationShakeSettings;
        [SerializeField] private ShakeSettings scaleShakeSettings;

        private float seed;
        private bool isShaking = true;

        private void Awake()
        {
            seed = Random.value;
        }

        private void Update()
        {
            if (isShaking) Shake();
        }
        
        public void StartShaking()
        {
            isShaking = true;
        }

        public void StopShaking()
        {
            isShaking = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        private void Shake()
        {
            transform.localPosition = GetRandomVector(positionShakeSettings, 1);
            transform.localRotation = Quaternion.Euler(GetRandomVector(rotationShakeSettings, 2));
            transform.localScale = Vector3.one + GetRandomVector(scaleShakeSettings, 3);
        }

        private Vector3 GetRandomVector(ShakeSettings shakeSettings, float seed2)
        {
            float randomX = shakeSettings.randomizeX ? shakeSettings.amplitude * GetPerlinNoiseVal(seed*seed2) : 0.0f;
            float randomY = shakeSettings.randomizeY ? shakeSettings.amplitude * GetPerlinNoiseVal(seed*seed2+1) : 0.0f;
            float randomZ = shakeSettings.randomizeZ ? shakeSettings.amplitude * GetPerlinNoiseVal(seed*seed2+2) : 0.0f;
            return new Vector3(randomX, randomY, randomZ);
        }

        // Perlin noise modified to return between -1.0f and 1.0f
        private float GetPerlinNoiseVal(float seed)
        {
            return 2.0f * Mathf.PerlinNoise1D(seed * 9999 + Time.time * frequency) - 1.0f;
        }
    }
}
