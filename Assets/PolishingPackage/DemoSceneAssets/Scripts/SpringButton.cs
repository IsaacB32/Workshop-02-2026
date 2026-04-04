using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Polishing.Demo
{
    [RequireComponent(typeof(Button))]
    public class SpringButton : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private AudioClip hoverSound;
        
        [Header("Hover Rotation Spring")]
        [SerializeField] private float rotationAmount = 10.0f;
        [SerializeField, Range(0.0f, 1000.0f)] private float rotationStiffness = 100.0f;
        [SerializeField, Range(0.0f, 1.0f)] private float rotationDamping = 10.0f;
        [SerializeField, Range(0.0f, 100.0f)] private float hoverNudgePower = 10.0f;
        private Spring rotationSpring;
        
        [Header("Click Scale Spring")]
        [SerializeField] private float scaleAmount = 1.0f;
        [SerializeField, Range(0.0f, 1000.0f)] private float scaleStiffness = 100.0f;
        [SerializeField, Range(0.0f, 1.0f)] private float scaleDamping = 10.0f;
        [SerializeField, Range(0.0f, 100.0f)] private float clickNudgePower = 10.0f;
        private Spring scaleSpring;
        
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            rotationSpring = new(rotationStiffness, rotationDamping, 0.0f);
            scaleSpring = new(scaleStiffness, scaleDamping, 1.0f);
        }

        private void OnValidate()
        {
            if (rotationSpring == null || scaleSpring == null) return;
            rotationSpring.stiffness = rotationStiffness;
            rotationSpring.damping = rotationDamping;
            scaleSpring.stiffness = scaleStiffness;
            scaleSpring.damping = scaleDamping;
        }

        private void OnEnable() => button.onClick.AddListener(OnButtonClicked);
        private void OnDisable() => button.onClick.RemoveAllListeners();

        private void Update()
        {
            rotationSpring.Simulate(Time.deltaTime);
            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, rotationSpring.position * rotationAmount);
            transform.localRotation = rotation;
            
            scaleSpring.Simulate(Time.deltaTime);
            transform.localScale = Vector3.one * (scaleSpring.position * scaleAmount);
        }

        private void OnButtonClicked()
        {
            scaleSpring.NudgeSpring(clickNudgePower);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            rotationSpring.NudgeSpring(hoverNudgePower);
            if (Camera.main != null) AudioSource.PlayClipAtPoint(hoverSound, Camera.main.transform.position);
        }
    }
}
