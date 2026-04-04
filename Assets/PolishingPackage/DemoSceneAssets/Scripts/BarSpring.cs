using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Polishing.Demo
{
    public class BarSpring : MonoBehaviour
    {
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TextMeshProUGUI barText;
        [SerializeField] private float healthChangeAmount = 25.0f;

        [Header("Fill Spring")]
        [SerializeField, Range(1.0f, 1000.0f)] private float fillSpringStiffness;
        [SerializeField, Range(0.0f, 1.0f)] private float fillSpringDamping;
        
        [Header("Impulse Spring")]
        [SerializeField, Range(1.0f, 1000.0f)] private float impulseSpringStiffness;
        [SerializeField, Range(0.0f, 1.0f)] private float impulseSpringDamping;
        [SerializeField, Range(0.0f, 100.0f)] private float impulseSpringNudgePower;
        [SerializeField] private float rotationFactor = 1.0f;
        [SerializeField] private Transform scaleTransform;
        [SerializeField] private Transform rotationTransform;

        private float maxHealth = 100.0f;
        private float currentHealth;

        private Spring fillSpring;
        private Spring impulseSpring;

        private void Awake()
        {
            fillSpring = new Spring(fillSpringStiffness, fillSpringDamping, 1.0f);
            impulseSpring = new Spring(impulseSpringStiffness, impulseSpringDamping);
            
            SetHealth(maxHealth);
        }

        private void OnValidate()
        {
            if (fillSpring == null || impulseSpring == null) return;
            fillSpring.stiffness =  fillSpringStiffness;
            fillSpring.damping = fillSpringDamping;
            
            impulseSpring.stiffness = impulseSpringStiffness;
            impulseSpring.damping = impulseSpringDamping;
        }

        private void Update()
        {
            fillSpring.Simulate(Time.deltaTime);
            healthBarFill.fillAmount = fillSpring.position;
            
            impulseSpring.Simulate(Time.deltaTime);
            scaleTransform.localScale = (1.0f + impulseSpring.position) * Vector3.one;
            rotationTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotationFactor * impulseSpring.position);
            
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                SetHealth(currentHealth + healthChangeAmount);
                impulseSpring.NudgeSpring(impulseSpringNudgePower);
            }

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                SetHealth(currentHealth - healthChangeAmount);
                impulseSpring.NudgeSpring(impulseSpringNudgePower);
            }
        }

        private void SetHealth(float value)
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            fillSpring.goalPosition = currentHealth / maxHealth;
            barText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}