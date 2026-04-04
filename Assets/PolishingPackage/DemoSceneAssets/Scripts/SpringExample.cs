using UnityEngine;
using UnityEngine.InputSystem;

namespace Polishing.Demo
{
    public class SpringExample : MonoBehaviour
    {
        [Header("Spring Settings")]
        [SerializeField, Range(0.0f, 1000.0f)] private float springStiffness;
        [SerializeField, Range(0.0f, 1.0f)] private float springDamping;
        [SerializeField, Range(-1.0f, 1.0f)] private float goalPosition;
        [SerializeField, Range(0.0f, 100.0f)] private float nudgePower;

        [Header("Spring Mapping")]
        [SerializeField] private Vector3 positionMapping;
        [SerializeField] private Vector3 rotationMapping;
        [SerializeField] private Vector3 scaleMapping;

        private Spring spring;

        private void OnValidate()
        {
            if (spring != null)
            {
                spring.stiffness = springStiffness;
                spring.damping = springDamping;
                spring.goalPosition = goalPosition;
            }
        }

        private void Awake()
        {
            spring = new(springStiffness, springDamping, 0.0f);
        }

        private void Update()
        {
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame) spring.NudgeSpring(nudgePower);
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame) spring.NudgeSpring(-nudgePower);
            
            spring.Simulate(Time.deltaTime);
            
            transform.localPosition = positionMapping * spring.position;
            transform.localRotation = Quaternion.Euler(rotationMapping * spring.position);
            transform.localScale = Vector3.one + scaleMapping * spring.position;
        }
    }
}
