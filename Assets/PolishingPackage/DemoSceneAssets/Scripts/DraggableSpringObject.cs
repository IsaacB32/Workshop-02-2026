using Polishing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableSpringObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Spring Settings")]
    [SerializeField, Range(0.0f, 1000.0f)] private float springStiffness;
    [SerializeField, Range(0.0f, 1.0f)] private float springDamping;
    [SerializeField, Range(-1.0f, 1.0f)] private float goalPosition;
    [SerializeField, Range(0.0f, 100.0f)] private float nudgePower;
    
    [Header("Transforms")]
    [SerializeField] private Transform rotateTransform;
    [SerializeField] private float rotationMapping;
    [SerializeField] private Transform scaleTransform;
    [SerializeField] private Transform squashTransform;
    [SerializeField] private float scaleMapping;
    
    [Header("References")]
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private TextMeshProUGUI positionLabel;

    private Spring spring;
    private bool isDragging = false;

    private void Awake()
    {
        spring = new Spring(springStiffness, springDamping, goalPosition);
    }

    private void OnValidate()
    {
        if (spring == null) return;
        spring.stiffness = springStiffness;
        spring.damping = springDamping;
        spring.goalPosition = goalPosition;   
    }
    
    private void Update()
    {
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Mouse.current.position.ReadValue(), null, out Vector2 mousePos);
            transform.localPosition = new Vector3(mousePos.x, transform.localPosition.y, transform.localPosition.z);
            spring.position = mousePos.x / 700.0f;
        }
        else
        {
            HandleInput();
            spring.Simulate(Time.deltaTime);
            transform.localPosition = new Vector3(spring.position * 700.0f, transform.localPosition.y, transform.localPosition.z);
        }

        positionLabel.text = spring.position.ToString("F2");
        
        rotateTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, spring.position * rotationMapping);
        scaleTransform.localScale = Vector3.one * (1.0f + spring.position * scaleMapping);
        squashTransform.localScale = Vector3.one + new Vector3(-1.0f, 1.0f, -1.0f) * (spring.position * scaleMapping);
    }

    private void HandleInput()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame) spring.NudgeSpring(-nudgePower);
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) spring.NudgeSpring(nudgePower);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        spring.velocity = 0.0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
