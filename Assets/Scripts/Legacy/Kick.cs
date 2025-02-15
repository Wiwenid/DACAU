using UnityEngine;

public class PinballFlipper : MonoBehaviour
{
    [Header("Flipper Settings")]
    public KeyCode activateKey = KeyCode.LeftShift; // Key to activate the flipper
    public float hitAngle = 45f; // Maximum rotation angle in degrees
    public float flipperSpeed = 10f; // Speed of flipper movement
    public float returnSpeed = 5f; // Speed at which the flipper resets
    public int flipDirection = 1; // 1 for right flipper, -1 for left flipper

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isFlipping = false;

    void Start()
    {
        // Store the original rotation of the flipper
        initialRotation = transform.localRotation;
        targetRotation = Quaternion.Euler(transform.localEulerAngles + new Vector3(0, 0, flipDirection * hitAngle));
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            isFlipping = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFlipping = false;
        }

        // Rotate the flipper based on input
        if (isFlipping)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * flipperSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, Time.deltaTime * returnSpeed);
        }
    }

    // Function to adjust flipper settings dynamically
    public void SetFlipperProperties(float newHitAngle, float newFlipperSpeed, float newReturnSpeed)
    {
        hitAngle = newHitAngle;
        flipperSpeed = newFlipperSpeed;
        returnSpeed = newReturnSpeed;
        targetRotation = Quaternion.Euler(transform.localEulerAngles + new Vector3(0, 0, flipDirection * hitAngle));
    }
}