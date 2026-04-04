using Polishing;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpringJump : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float jumpForce;

    [Header("Stretch Spring")]
    [SerializeField] private Transform stretchSpringTransform;
    [SerializeField, Range(1.0f, 1000.0f)] private float stretchSpringStiffness;
    [SerializeField, Range(0.0f, 1.0f)] private float stretchSpringDamping;
    [SerializeField, Range(0.0f, 100.0f)] private float jumpNudge;
    [SerializeField, Range(0.0f, 100.0f)] private float landNudge;
    
    private Spring stretchSpring;
    
    private void Awake()
    {
        stretchSpring = new Spring(stretchSpringStiffness, stretchSpringDamping, 0.0f);
    }

    private void OnValidate()
    {
        if (stretchSpring == null) return;
        stretchSpring.stiffness = stretchSpringStiffness;
        stretchSpring.damping = stretchSpringDamping;
    }

    private void Update()
    {
        stretchSpring.Simulate(Time.deltaTime);
        stretchSpringTransform.localScale = Vector3.one + stretchSpring.position * new Vector3(-1.0f, 1.0f, -1.0f);
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rigidBody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            stretchSpring.NudgeSpring(jumpNudge);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rigidBody.linearVelocity.y < 0.2f)
        {
            stretchSpring.NudgeSpring(-landNudge);
        }
    }
}
