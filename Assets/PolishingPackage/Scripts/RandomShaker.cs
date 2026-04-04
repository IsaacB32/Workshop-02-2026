using System;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * For more info on shakes:
 * https://roystan.net/articles/camera-shake/
 */

namespace Polishing.Shakes
{
    public class RandomShaker : MonoBehaviour
    {
        [Range(0.0f, 200.0f)] public float frequency = 100.0f;
        [Space]
        [SerializeField] private ShakeSettings positionShakeSettings;
        [SerializeField] private ShakeSettings rotationShakeSettings;
        [SerializeField] private ShakeSettings scaleShakeSettings;
        
        private float shakeTimer = 0.0f;
        private bool isShaking = true;

        private void Update()
        {
            if (isShaking) RunShakeTimer();
        }

        private void RunShakeTimer()
        {
            if (shakeTimer < 1.0f)
            {
                shakeTimer += frequency * Time.deltaTime;
            }
            else
            {
                shakeTimer -= 1.0f;
                Shake();
            }
        }
        
        public void StartShaking()
        {
            isShaking = true;
            shakeTimer = 0.0f;
            Shake();
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
            transform.localPosition = GetRandomVector(positionShakeSettings);
            transform.localRotation = Quaternion.Euler(GetRandomVector(rotationShakeSettings));
            transform.localScale = Vector3.one + GetRandomVector(scaleShakeSettings);
        }
        
        private Vector3 GetRandomVector(ShakeSettings shakeSettings)
        {
            float randomX = shakeSettings.randomizeX ? Random.Range(-shakeSettings.amplitude, shakeSettings.amplitude) : 0.0f;
            float randomY = shakeSettings.randomizeY ? Random.Range(-shakeSettings.amplitude, shakeSettings.amplitude) : 0.0f;
            float randomZ = shakeSettings.randomizeZ ? Random.Range(-shakeSettings.amplitude, shakeSettings.amplitude) : 0.0f;
            return new Vector3(randomX, randomY, randomZ);
        }
    }
}
