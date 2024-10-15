using UnityEngine;
using DG.Tweening; // Make sure you have DOTween installed

public class ObjectMover : MonoBehaviour
{
    public Transform targetObject; // Optional target object to move towards
    public Vector3 targetPosition; // Fallback position to move to if no target object is set
    public Vector3 targetRotation; // Fallback rotation to move to if no target object is set
    public float moveDuration = 2f; // Base time it takes to move to the target
    public Ease moveEase = Ease.Linear; // Type of easing for the movement animation
    public float animationSpeed = 1f; // Speed of the animation (1x is normal speed)

    private Vector3 initialPosition; // Store the starting position of the object
    private Vector3 initialRotation; // Store the starting rotation of the object
    private Tween currentTween; // Reference to the current animation

    void Start()
    {
        initialPosition = transform.position; // Capture the initial position when the game starts
        initialRotation = transform.eulerAngles; // Capture the initial rotation when the game starts
    }

    // Call this method to move the object to the target position and rotation
    public void MoveToTarget()
    {
        // If there is an active tween, kill it before starting a new one
        if (currentTween != null && currentTween.IsPlaying())
        {
            currentTween.Kill();
        }

        // Determine the target position (either from the targetObject or the fallback targetPosition)
        Vector3 targetPos = targetObject != null ? targetObject.position : targetPosition;
        Vector3 targetRot = targetObject != null ? targetObject.eulerAngles : targetRotation;

        float adjustedDuration = moveDuration / animationSpeed; // Adjust the duration based on speed

        // Move towards the determined target position and rotate
        currentTween = transform.DOMove(targetPos, adjustedDuration).SetEase(moveEase);
        transform.DORotate(targetRot, adjustedDuration).SetEase(moveEase);
    }

    // Call this method to reverse the movement and rotation back to the initial position and rotation
    public void Reverse()
    {
        // If there is an active tween, kill it before starting a new one
        if (currentTween != null && currentTween.IsPlaying())
        {
            currentTween.Kill();
        }

        float adjustedDuration = moveDuration / animationSpeed; // Adjust the duration based on speed

        // Move back to the initial position and rotation
        currentTween = transform.DOMove(initialPosition, adjustedDuration).SetEase(moveEase);
        transform.DORotate(initialRotation, adjustedDuration).SetEase(moveEase);
    }

    // Call this method to continuously follow the target position and rotation in real-time
    public void FollowTarget()
    {
        if (targetObject == null) return; // Do nothing if no targetObject is assigned

        // Use DOFollow for continuous following of position and rotation
        currentTween = transform.DOMove(targetObject.position, moveDuration)
            .SetEase(moveEase)
            .SetSpeedBased(true)
            .SetLoops(-1, LoopType.Yoyo); // Optional: loops forever in yoyo mode

        // Continuous rotation following
        transform.DORotate(targetObject.eulerAngles, moveDuration)
            .SetEase(moveEase)
            .SetSpeedBased(true)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
