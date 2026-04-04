using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Polishing.Demo
{
    public class BarInterpolation : MonoBehaviour
    {
        [SerializeField] private Image healthBarFill;
        [SerializeField] private float lerpSpeed = 1.0f;
        [SerializeField] private float healthChangeAmount = 25.0f;

        private float maxHealth = 100.0f;
        private float currentHealth;

        private float targetFillValue = 0.0f;
        private float currentFillValue = 0.0f;

        private void Awake()
        {
            SetHealth(maxHealth);
        }

        private void Update()
        {
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                SetHealth(currentHealth + healthChangeAmount);
            }

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                SetHealth(currentHealth - healthChangeAmount);
            }
            
            currentFillValue = Mathf.Lerp(currentFillValue, targetFillValue, lerpSpeed * Time.deltaTime);
            healthBarFill.fillAmount = currentFillValue;
        }

        private void SetHealth(float value)
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            targetFillValue = currentHealth/maxHealth;
        }
    }
}
