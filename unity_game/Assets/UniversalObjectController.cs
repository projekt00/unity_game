using UnityEngine;
using UnityEngine.Events;

public class UniversalObjectsController : MonoBehaviour
{
    public enum InteractionType
    {
        Clickable,
        Holdable
    }

    public InteractionType interactionType; // Type of interaction

    // Events for click interactions
    [Header("Click Events")]
    public UnityEvent onClick; // Event triggered on click

    // Events for hold interactions
    [Header("Hold Events")]
    public UnityEvent onHoldStart; // Event triggered when holding starts
    public UnityEvent onHold;       // Event triggered while holding
    public UnityEvent onHoldEnd;    // Event triggered when holding ends

    public float holdDuration = 2f; // Duration for holding in seconds

    private bool isHolding = false;
    private float holdTime = 0f;

    void Update()
    {
        if (isHolding && interactionType == InteractionType.Holdable)
        {
            holdTime += Time.deltaTime;
            onHold.Invoke(); // Trigger event during holding

            if (holdTime >= holdDuration)
            {
                Debug.Log("Hold duration reached!");
                holdTime = 0f; // Reset hold time if needed
            }
        }
    }

    void OnMouseDown()
    {
        if (interactionType == InteractionType.Clickable)
        {
            onClick.Invoke(); // Trigger event on click
        }
        else if (interactionType == InteractionType.Holdable)
        {
            isHolding = true;
            holdTime = 0f; // Reset hold time
            onHoldStart.Invoke(); // Trigger event when holding starts
        }
    }

    void OnMouseUp()
    {
        if (interactionType == InteractionType.Holdable)
        {
            isHolding = false;
            onHoldEnd.Invoke(); // Trigger event when holding ends
        }
    }

    private void OnValidate()
    {
        // Set event visibility based on the selected interaction type
        if (interactionType == InteractionType.Clickable)
        {
            // Additional conditions or actions can be added if needed
            onHoldStart = null;
            onHold = null;
            onHoldEnd = null;
        }
    }
}
