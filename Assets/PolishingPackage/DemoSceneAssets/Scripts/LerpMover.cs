using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LerpMover : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    
    [Header("Move Settings")]
    [Tooltip("If enabled, does a simple lerp. If disabled, use an animation curve")]
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private bool useAnimCurve = true;
    [SerializeField] private AnimationCurve animationCurve;

    

    private Coroutine moveCoroutine;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Mouse.current.position.ReadValue(), null, out Vector2 mousePos);
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCoroutine(mousePos));
        }
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.localPosition;
        
        float timer = 0.0f;
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / moveDuration);
            if (useAnimCurve) t = animationCurve.Evaluate(t);

            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
            transform.localPosition = newPosition;
            yield return null;
        }
    }
}
