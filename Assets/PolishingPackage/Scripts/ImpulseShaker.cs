using UnityEngine;
using UnityEngine.InputSystem;

/*
 * For more info on shakes:
 * https://roystan.net/articles/camera-shake/
 */

namespace Polishing.Shakes
{
    public class ImpulseShaker : MonoBehaviour
    {
        [Header("Shake Settings")]
        [Range(0.0f, 50.0f)] public float frequency = 25.0f;
        [Space]
        [SerializeField] private ShakeSettings positionShakeSettings;
        [SerializeField] private ShakeSettings rotationShakeSettings;
        [SerializeField] private ShakeSettings scaleShakeSettings;

        [Header("Impulse Settings")]
        [SerializeField] private float impulseFalloffSpeed = 1.0f;
        [SerializeField] private float impulseExponent = 1.0f;
        

        private float seed;
        private float impulse = 1.0f;

        private void Awake()
        {
            seed = Random.value;
        }

        private void Update()
        {
            impulse = Mathf.Clamp01(impulse - Time.deltaTime * impulseFalloffSpeed);
            Shake();

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ApplyImpulse(1.0f);
            }
        }

        public void ApplyImpulse(float impulse)
        {
            this.impulse = Mathf.Clamp01(this.impulse + impulse);
        }

        private void Shake()
        {
            float impulseMultiplier = Mathf.Pow(impulse, impulseExponent);
            transform.localPosition = impulseMultiplier * GetRandomVector(positionShakeSettings, 1);
            transform.localRotation = Quaternion.Euler(impulseMultiplier * GetRandomVector(rotationShakeSettings, 2));
            transform.localScale = impulseMultiplier * Vector3.one + GetRandomVector(scaleShakeSettings, 3);
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
